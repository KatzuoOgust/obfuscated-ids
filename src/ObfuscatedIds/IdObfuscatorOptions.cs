using System.Text;

namespace KatzuoOgust.ObfuscatedIds;

/// <summary>
/// Configuration options for <see cref="IdObfuscator"/>.
/// </summary>
public sealed class IdObfuscatorOptions
{
	private static readonly byte[] DefaultKey =
	[
		0x4A,
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
		0xB4,
	];

	/// <summary>
	/// Maximum number of UTF-8 bytes the plaintext passed to <see cref="IdObfuscator.Encode"/> may occupy.
	/// </summary>
	public const int MaxPlaintextBytes = 255;

	/// <summary>
	/// Maximum allowed value for <see cref="PaddedBytes"/>.
	/// Equal to the largest possible frame size: 2-byte length header + <see cref="MaxPlaintextBytes"/> data bytes.
	/// </summary>
	public const int MaxPaddedBytes = MaxPlaintextBytes + 2;

	private byte[] _xorKey = DefaultKey;
	private byte[]? _permutationSeed;
	private int _paddedBytes;

	/// <summary>
	/// The XOR key used for obfuscation. Must be non-empty. Defaults to a built-in 16-byte key.
	/// Longer keys provide better diffusion.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when set to null.</exception>
	/// <exception cref="ArgumentException">Thrown when set to an empty array.</exception>
	public byte[] XorKey
	{
		get => _xorKey;
		set
		{
			ArgumentNullException.ThrowIfNull(value);
			if (value.Length == 0)
				throw new ArgumentException("XOR key must not be empty.", nameof(value));
			_xorKey = value;
		}
	}

	/// <summary>
	/// Optional seed for the byte-position permutation step. When set, bytes are shuffled
	/// after XOR (encode) and unshuffled before XOR (decode) using a deterministic,
	/// per-buffer-length Fisher-Yates shuffle derived from this seed.
	/// Set to <see langword="null"/> (the default) to disable permutation.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown when set to an empty array.</exception>
	public byte[]? PermutationSeed
	{
		get => _permutationSeed;
		set
		{
			if (value is { Length: 0 })
				throw new ArgumentException("Permutation seed must not be empty.", nameof(value));
			_permutationSeed = value;
		}
	}

	/// <summary>
	/// Minimum number of bytes in the pre-base64 buffer. When the framed payload is smaller
	/// than this value, zero bytes are appended so all tokens share the same character length.
	/// Set to <c>0</c> (the default) to disable padding.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when set to a negative value or a value greater than <see cref="MaxPaddedBytes"/>.</exception>
	public int PaddedBytes
	{
		get => _paddedBytes;
		set
		{
			if (value < 0 || value > MaxPaddedBytes)
				throw new ArgumentOutOfRangeException(nameof(PaddedBytes), value,
					$"Value must be between 0 and {MaxPaddedBytes} (inclusive).");
			_paddedBytes = value;
		}
	}

	/// <summary>
	/// Sets <see cref="XorKey"/> from the UTF-8 encoding of <paramref name="key"/>.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty.</exception>
	public void SetXorKey(string key)
	{
		ArgumentNullException.ThrowIfNull(key);
		if (key.Length == 0)
			throw new ArgumentException("XOR key must not be empty.", nameof(key));
		_xorKey = Encoding.UTF8.GetBytes(key);
	}

	/// <summary>
	/// Sets <see cref="PermutationSeed"/> from the UTF-8 encoding of <paramref name="seed"/>.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="seed"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="seed"/> is empty.</exception>
	public void SetPermutationSeed(string seed)
	{
		ArgumentNullException.ThrowIfNull(seed);
		if (seed.Length == 0)
			throw new ArgumentException("Permutation seed must not be empty.", nameof(seed));
		_permutationSeed = Encoding.UTF8.GetBytes(seed);
	}
}
