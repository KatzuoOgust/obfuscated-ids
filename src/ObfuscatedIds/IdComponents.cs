using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace KatzuoOgust.ObfuscatedIds;

internal static class IdComponents
{
	private const char Sep = '|';
	private const char Esc = '\\';

	internal static string FormatValue<T>(T? value) =>
		value is IFormattable f
			? f.ToString(null, CultureInfo.InvariantCulture)
			: value?.ToString() ?? string.Empty;

	internal static T? ParseValue<T>(string? s)
	{
		if (string.IsNullOrEmpty(s) && Nullable.GetUnderlyingType(typeof(T)) != null)
			return default;
		if (typeof(T) == typeof(string)) return (T)(object)s!;
		var converter = TypeDescriptor.GetConverter(typeof(T));
		return (T)converter.ConvertFromInvariantString(s!)!;
	}

	/// <summary>
	/// Escapes each component string, then joins them with <see cref="Sep"/>, producing the
	/// plaintext passed to <see cref="IdObfuscator.Encode"/>.
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
		var input = plain.AsSpan();
		Span<char> buffer = stackalloc char[plain.Length];
		var bufLen = 0;

		for (var i = 0; i < input.Length; i++)
		{
			switch (input[i])
			{
				case Esc when i + 1 < input.Length:
					buffer[bufLen++] = input[++i]; // consume escape prefix, append the literal char
					break;
				case Sep when partIndex < count - 1:
					parts[partIndex++] = new string(buffer[..bufLen]);
					bufLen = 0;
					break;
				default:
					buffer[bufLen++] = input[i];
					break;
			}
		}
		parts[partIndex] = new string(buffer[..bufLen]);
		return parts;
	}
}
