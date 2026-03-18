using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace KatzuoOgust.ObfuscatedIds;

/// <summary>
/// Provides XOR-based obfuscation of ID values using base64url encoding.
/// </summary>
/// <remarks>
/// <para>Encoding pipeline: UTF-8 bytes → frame with 2-byte length header → optional zero-padding
/// → XOR with repeating key → base64url (no <c>+</c>, <c>/</c>, or <c>=</c> characters).</para>
/// <para>Decoding is the same pipeline in reverse; XOR is its own inverse.
/// The 2-byte header allows the decoder to strip padding without knowing the
/// <see cref="PaddedBytes"/> setting that was active at encode time.</para>
/// <para>Configure the key and padding once at application startup via
/// <see cref="ConfigureXorKey(byte[])"/> and <see cref="PaddedBytes"/>.</para>
/// </remarks>
public static partial class IdObfuscator
{
	private static byte[] _key = [0x4A,
		0x7E,
		0x2B,
		0x9F,
		0xD3,
		0x61,
		0xA8,
		0x15,
		0xC7,
		0x53,
		0xE9,
		0x3D,
		0x86,
		0xF2,
		0x0E,
		0xB4];
	private static int _paddedBytes;

	/// <summary>
	/// Maximum number of UTF-8 bytes the plaintext passed to <see cref="Encode"/> may occupy.
	/// Inputs that exceed this limit cause <see cref="Encode"/> to throw an
	/// <see cref="ArgumentException"/>.
	/// </summary>
	public const int MaxPlaintextBytes = 255;

	/// <summary>
	/// Maximum allowed value for <see cref="PaddedBytes"/>.
	/// Equal to the largest possible frame size: 2-byte length header + <see cref="MaxPlaintextBytes"/> data bytes.
	/// Padding beyond this adds only zero noise with no additional alignment benefit.
	/// </summary>
	public const int MaxPaddedBytes = MaxPlaintextBytes + 2;

	/// <summary>
	/// Minimum number of bytes in the pre-base64 buffer.
	/// When the framed payload (2-byte header + UTF-8 data) is smaller than this value,
	/// zero bytes are appended to reach exactly <see cref="PaddedBytes"/> bytes,
	/// making every token the same character length.
	/// Set to <c>0</c> (the default) to disable padding.
	/// </summary>
	/// <remarks>
	/// Every 3 input bytes produce 4 base64url characters, so the resulting token length is
	/// <c>ceil(PaddedBytes / 3) * 4</c> characters (without the stripped <c>=</c> padding).
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when set to a negative value or a value greater than <see cref="MaxPaddedBytes"/>.</exception>
	public static int PaddedBytes
	{
		get => _paddedBytes;
		set
		{
			if (value < 0 || value > MaxPaddedBytes) throw Error.PaddedBytesOutOfRange(value);
			_paddedBytes = value;
		}
	}

	/// <summary>
	/// Replaces the global XOR key used for all obfuscation operations.
	/// Call this once at application startup before encoding or decoding any IDs.
	/// </summary>
	/// <param name="key">A non-empty byte array; longer keys provide better diffusion.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty.</exception>
	public static void ConfigureXorKey(byte[] key)
	{
		ArgumentNullException.ThrowIfNull(key);

		if (key.Length == 0) throw Error.KeyEmpty();
		_key = key;
	}

	/// <summary>
	/// Replaces the global XOR key using the UTF-8 encoding of <paramref name="key"/>.
	/// Call this once at application startup before encoding or decoding any IDs.
	/// </summary>
	/// <param name="key">A non-empty string; its UTF-8 bytes are used as the key.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty.</exception>
	public static void ConfigureXorKey(string key)
	{
		ArgumentNullException.ThrowIfNull(key);

		if (key.Length == 0) throw Error.KeyEmpty();
		_key = Encoding.UTF8.GetBytes(key);
	}

	/// <summary>
	/// Encodes a plaintext string to a URL-safe, base64url-encoded, XOR-obfuscated token.
	/// </summary>
	/// <param name="plaintext">The string to obfuscate.</param>
	/// <returns>
	/// A URL-safe base64url string with no padding characters.
	/// When <see cref="PaddedBytes"/> is set, all tokens share the same character length.
	/// </returns>
	/// <exception cref="ArgumentException">
	/// Thrown when the UTF-8 byte length of <paramref name="plaintext"/> exceeds <see cref="MaxPlaintextBytes"/>.
	/// </exception>
	public static string Encode(string plaintext)
	{
		ArgumentNullException.ThrowIfNull(plaintext);

		var data = Encoding.UTF8.GetBytes(plaintext);
		if (data.Length > MaxPlaintextBytes)
			throw Error.PlaintextTooLong(data.Length);

		// Frame: [lo byte of length][hi byte of length][data bytes][zero padding]
		var totalBytes = Math.Max(2 + data.Length, _paddedBytes);
		var buffer = new byte[totalBytes];
		buffer[0] = (byte)(data.Length & 0xFF);
		buffer[1] = (byte)((data.Length >> 8) & 0xFF);
		data.CopyTo(buffer, 2);
		// remaining bytes are already zero (zero padding)

		var xored = Xor(buffer, _key);
		return Convert.ToBase64String(xored)
			.Replace('+', '-')
			.Replace('/', '_')
			.TrimEnd('=');
	}

	/// <summary>
	/// Decodes a token previously produced by <see cref="Encode"/> back to its plaintext form.
	/// The 2-byte length header embedded in the token is used to strip any zero padding,
	/// so this method works regardless of the <see cref="PaddedBytes"/> setting.
	/// </summary>
	/// <param name="token">A base64url token returned by <see cref="Encode"/>.</param>
	/// <returns>The original plaintext string.</returns>
	/// <exception cref="FormatException">Thrown when <paramref name="token"/> is not a valid obfuscated ID token.</exception>
	public static string Decode(string token)
	{
		ArgumentNullException.ThrowIfNull(token);

		try
		{
			var padded = token.Replace('-', '+').Replace('_', '/');
			var remainder = padded.Length % 4;
			if (remainder != 0) padded += new string('=', 4 - remainder);

			var buffer = Xor(Convert.FromBase64String(padded), _key);

			// Read the 2-byte LE length header to recover exact data length
			var dataLength = buffer[0] | (buffer[1] << 8);
			return Encoding.UTF8.GetString(buffer, 2, dataLength);
		}
		catch (Exception ex)
		{
			throw Error.InvalidToken(token, ex);
		}
	}

	private static byte[] Xor(byte[] data, byte[] key)
	{
		var result = new byte[data.Length];
		for (var i = 0; i < data.Length; i++)
			result[i] = (byte)(data[i] ^ key[i % key.Length]);
		return result;
	}

	private const char Sep = '|';
	private const char Esc = '\\';

	internal static string FormatValue<T>(T? value) =>
		value is IFormattable f
			? f.ToString(null, CultureInfo.InvariantCulture)
			: value?.ToString() ?? string.Empty;

	internal static T ParseValue<T>(string s)
	{
		if (typeof(T) == typeof(string)) return (T)(object)s;
		var converter = TypeDescriptor.GetConverter(typeof(T));
		return (T)converter.ConvertFromInvariantString(s)!;
	}

	/// <summary>
	/// Escapes each component string, then joins them with <see cref="Sep"/>, producing the
	/// plaintext passed to <see cref="Encode"/>.
	/// </summary>
	internal static string JoinComponents(params string[] parts)
	{
		return string.Join(Sep, parts.Select(Escape));

		static string Escape(string s)
		{
			return s.Replace($"{Esc}", $"{Esc}{Esc}").Replace($"{Sep}", $"{Esc}{Sep}");
		}
	}

	/// <summary>
	/// Splits the decoded plaintext on unescaped <see cref="Sep"/> characters into exactly
	/// <paramref name="count"/> parts, unescaping each part in the same pass.
	/// </summary>
	internal static string[] SplitComponents(string plain, int count)
	{
		var parts = new string[count];
		var partIndex = 0;
		var sb = new StringBuilder();
		for (var i = 0; i < plain.Length; i++)
		{
			switch (plain[i])
			{
				case Esc when i + 1 < plain.Length:
					sb.Append(plain[++i]); // consume escape prefix, append the literal char
					break;
				case Sep when partIndex < count - 1:
					parts[partIndex++] = sb.ToString();
					sb.Clear();
					break;
				default:
					sb.Append(plain[i]);
					break;
			}
		}
		parts[partIndex] = sb.ToString();
		return parts;
	}
}
