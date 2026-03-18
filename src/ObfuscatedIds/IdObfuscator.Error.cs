namespace KatzuoOgust.ObfuscatedIds;

public static partial class IdObfuscator
{
	private static class Error
	{
		internal static ArgumentOutOfRangeException PaddedBytesOutOfRange(int value) =>
			new(nameof(PaddedBytes), value, $"Value must be between 0 and {MaxPaddedBytes} (inclusive).");

		internal static ArgumentException KeyEmpty() =>
			new("Key must not be empty.", "key");

		internal static ArgumentException PlaintextTooLong(int length) =>
			new($"Plaintext exceeds the maximum allowed size of {MaxPlaintextBytes} UTF-8 bytes (got {length}).",
				"plaintext");

		internal static FormatException InvalidToken(string token, Exception inner) =>
			new($"'{token}' is not a valid obfuscated ID token.", inner);
	}
}
