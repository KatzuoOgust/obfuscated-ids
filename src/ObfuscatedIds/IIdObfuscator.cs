namespace KatzuoOgust.ObfuscatedIds;

/// <summary>
/// Encodes and decodes ID values at the raw string level.
/// Implementations typically apply XOR, padding, and base64url encoding.
/// </summary>
/// <remarks>
/// Use <see cref="IIdObfuscator{T}"/> for a type-safe wrapper that handles value formatting and parsing.
/// </remarks>
public interface IIdObfuscator
{
	/// <summary>
	/// Obfuscates a plaintext string to a URL-safe base64url token.
	/// </summary>
	/// <param name="plaintext">The formatted string representation of the value to obfuscate.</param>
	/// <returns>A URL-safe base64url token with no padding characters.</returns>
	string Encode(string plaintext);

	/// <summary>
	/// Decodes a token previously produced by <see cref="Encode"/> back to its plaintext form.
	/// </summary>
	/// <param name="token">A base64url token returned by <see cref="Encode"/>.</param>
	/// <returns>The original plaintext string.</returns>
	/// <exception cref="FormatException">Thrown when <paramref name="token"/> is not a valid token.</exception>
	string Decode(string token);
}

/// <summary>
/// Type-safe obfuscator that converts values of type <typeparamref name="T"/>
/// to and from URL-safe external tokens.
/// </summary>
/// <typeparam name="T">
/// The value type being obfuscated.
/// For composite identifiers use a tuple, e.g. <c>(int, Guid)</c>.
/// </typeparam>
public interface IIdObfuscator<T>
{
	/// <summary>
	/// Converts <paramref name="value"/> to a URL-safe external token.
	/// </summary>
	/// <param name="value">The value to obfuscate.</param>
	/// <returns>A URL-safe base64url token representing <paramref name="value"/>.</returns>
	string Obfuscate(T value);

	/// <summary>
	/// Decodes a token previously produced by <see cref="Obfuscate"/> back to the original value.
	/// </summary>
	/// <param name="token">A token returned by <see cref="Obfuscate"/>.</param>
	/// <returns>The original value.</returns>
	/// <exception cref="FormatException">Thrown when <paramref name="token"/> is not a valid token.</exception>
	T Deobfuscate(string token);
}

