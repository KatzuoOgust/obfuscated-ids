using System.Text;

namespace KatzuoOgust.ObfuscatedIds;

/// <summary>
/// Provides XOR-based obfuscation of ID values using base64url encoding.
/// </summary>
/// <remarks>
/// <para>Encoding pipeline: UTF-8 bytes → frame with 2-byte length header → optional zero-padding
/// → XOR with repeating key → optional byte-position permutation → base64url (no <c>+</c>, <c>/</c>, or <c>=</c> characters).</para>
/// <para>Decoding is the same pipeline in reverse; XOR is its own inverse and the permutation
/// is inverted by the stored inverse table.
/// The 2-byte header allows the decoder to strip padding without knowing the
/// <see cref="IdObfuscatorOptions.PaddedBytes"/> setting that was active at encode time.</para>
/// <para>Use <see cref="Default"/> for the default configuration, or create an instance with
/// custom <see cref="IdObfuscatorOptions"/>.</para>
/// </remarks>
public sealed partial class IdObfuscator : IIdObfuscator
{
	/// <summary>A shared instance using default <see cref="IdObfuscatorOptions"/>.</summary>
	public static readonly IdObfuscator Default = new(new IdObfuscatorOptions());

	private readonly byte[] _key;
	private readonly int _paddedBytes;
	private readonly byte[]? _permSeed;

	/// <summary>
	/// Initialises a new <see cref="IdObfuscator"/> from the supplied options.
	/// A snapshot of the option values is taken at construction time; subsequent
	/// mutations to <paramref name="options"/> have no effect.
	/// </summary>
	/// <param name="options">Configuration options.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
	public IdObfuscator(IdObfuscatorOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		_key = options.XorKey;
		_paddedBytes = options.PaddedBytes;
		_permSeed = options.PermutationSeed;
	}

	/// <summary>
	/// Obfuscates a plaintext string to a URL-safe base64url token.
	/// </summary>
	/// <param name="plaintext">The string to obfuscate.</param>
	/// <returns>
	/// A URL-safe base64url string with no padding characters.
	/// When <see cref="IdObfuscatorOptions.PaddedBytes"/> is set, all tokens share the same character length.
	/// </returns>
	/// <exception cref="ArgumentException">
	/// Thrown when the UTF-8 byte length of <paramref name="plaintext"/> exceeds <see cref="IdObfuscatorOptions.MaxPlaintextBytes"/>.
	/// </exception>
	public string Encode(string plaintext)
	{
		ArgumentNullException.ThrowIfNull(plaintext);

		var data = Encoding.UTF8.GetBytes(plaintext);
		if (data.Length > IdObfuscatorOptions.MaxPlaintextBytes)
			throw Error.PlaintextTooLong(data.Length);

		// Frame: [lo byte of length][hi byte of length][data bytes][zero padding]
		var totalBytes = Math.Max(2 + data.Length, _paddedBytes);
		var buffer = new byte[totalBytes];
		buffer[0] = (byte)(data.Length & 0xFF);
		buffer[1] = (byte)((data.Length >> 8) & 0xFF);
		data.CopyTo(buffer, 2);
		// remaining bytes are already zero (zero padding)

		var xored = Xor(buffer, _key);
		var permuted = _permSeed is null ? xored : Permute(xored, _permSeed);
		return Convert.ToBase64String(permuted)
			.Replace('+', '-')
			.Replace('/', '_')
			.TrimEnd('=');
	}

	/// <summary>
	/// Decodes a token previously produced by <see cref="Encode"/> back to its plaintext form.
	/// The 2-byte length header embedded in the token is used to strip any zero padding,
	/// so this method works regardless of the <see cref="IdObfuscatorOptions.PaddedBytes"/> setting.
	/// </summary>
	/// <param name="token">A base64url token returned by <see cref="Encode"/>.</param>
	/// <returns>The original plaintext string.</returns>
	/// <exception cref="FormatException">Thrown when <paramref name="token"/> is not a valid obfuscated ID token.</exception>
	public string Decode(string token)
	{
		ArgumentNullException.ThrowIfNull(token);

		try
		{
			var padded = token.Replace('-', '+').Replace('_', '/');
			var remainder = padded.Length % 4;
			if (remainder != 0) padded += new string('=', 4 - remainder);

			var raw = Convert.FromBase64String(padded);
			var unpermuted = _permSeed is null ? raw : Unpermute(raw, _permSeed);
			var buffer = Xor(unpermuted, _key);

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

	/// <summary>
	/// Builds a deterministic Fisher-Yates permutation of length <paramref name="n"/>
	/// seeded by <paramref name="seed"/> XOR'd with the length so each buffer size gets
	/// a distinct shuffle.
	/// </summary>
	private static int[] BuildPermutation(int n, byte[] seed)
	{
		var perm = new int[n];
		for (var i = 0; i < n; i++) perm[i] = i;

		// Derive a stable uint seed by combining the seed bytes with n
		uint s = (uint)n;
		for (var i = 0; i < seed.Length; i++)
			s = s * 1_664_525u + seed[i] + 1_013_904_223u; // LCG mix

		// Fisher-Yates shuffle driven by the LCG
		for (var i = n - 1; i > 0; i--)
		{
			s = s * 1_664_525u + 1_013_904_223u;
			var j = (int)(s % (uint)(i + 1));
			(perm[i], perm[j]) = (perm[j], perm[i]);
		}
		return perm;
	}

	private static byte[] Permute(byte[] data, byte[] seed)
	{
		var perm = BuildPermutation(data.Length, seed);
		var result = new byte[data.Length];
		for (var i = 0; i < data.Length; i++) result[perm[i]] = data[i];
		return result;
	}

	private static byte[] Unpermute(byte[] data, byte[] seed)
	{
		var perm = BuildPermutation(data.Length, seed);
		var result = new byte[data.Length];
		for (var i = 0; i < data.Length; i++) result[i] = data[perm[i]];
		return result;
	}
}
