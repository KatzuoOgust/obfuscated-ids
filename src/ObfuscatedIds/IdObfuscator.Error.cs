namespace KatzuoOgust.ObfuscatedIds;

public sealed partial class IdObfuscator
{
	private static class Error
	{
		internal static ArgumentException PlaintextTooLong(int length) =>
			new($"Plaintext exceeds the maximum allowed size of {IdObfuscatorOptions.MaxPlaintextBytes} UTF-8 bytes (got {length}).",
				"plaintext");

		internal static FormatException InvalidToken(string token, Exception inner) =>
			new($"'{token}' is not a valid obfuscated ID token.", inner);
	}
}
