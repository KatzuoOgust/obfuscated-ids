namespace KatzuoOgust.ObfuscatedIds;

public static partial class IdObfuscator
{
	private static class Error
	{
		internal static ArgumentOutOfRangeException PaddedBytesNegative() =>
			new(nameof(PaddedBytes), "Value must be >= 0.");

		internal static ArgumentException KeyEmpty() =>
			new("Key must not be empty.", "key");

		internal static ArgumentException PlaintextTooLong(int length) =>
			new($"Plaintext exceeds the maximum allowed size of {MaxPlaintextBytes} UTF-8 bytes (got {length}).",
				"plaintext");

		internal static FormatException InvalidToken(string token, Exception inner) =>
			new($"'{token}' is not a valid obfuscated ID token.", inner);
	}
}
