using System.Text.Json;
using System.Text.Json.Serialization;

namespace KatzuoOgust.ObfuscatedIds.Serialization;

/// <summary>
/// A <see cref="JsonConverterFactory"/> that handles all <c>ObfuscatedId&lt;…&gt;</c> variants
/// (single value through five values). Serializes to and from the URL-safe
/// <see cref="ObfuscatedId{T}.External"/> token string.
/// </summary>
/// <remarks>
/// Register once on <see cref="JsonSerializerOptions"/>:
/// <code>options.Converters.Add(new ObfuscatedIdJsonConverterFactory());</code>
/// </remarks>
public sealed class ObfuscatedIdJsonConverterFactory : JsonConverterFactory
{
	/// <inheritdoc/>
	public override bool CanConvert(Type typeToConvert)
	{
		if (!typeToConvert.IsGenericType) return false;
		var def = typeToConvert.GetGenericTypeDefinition();
		return def == typeof(ObfuscatedId<>)
			|| def == typeof(ObfuscatedId<,>)
			|| def == typeof(ObfuscatedId<,,>)
			|| def == typeof(ObfuscatedId<,,,>)
			|| def == typeof(ObfuscatedId<,,,,>);
	}

	/// <inheritdoc/>
	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var args = typeToConvert.GetGenericArguments();
		var converterType = args.Length switch
		{
			1 => typeof(ObfuscatedIdJsonConverter<>).MakeGenericType(args),
			2 => typeof(ObfuscatedIdJsonConverter<,>).MakeGenericType(args),
			3 => typeof(ObfuscatedIdJsonConverter<,,>).MakeGenericType(args),
			4 => typeof(ObfuscatedIdJsonConverter<,,,>).MakeGenericType(args),
			5 => typeof(ObfuscatedIdJsonConverter<,,,,>).MakeGenericType(args),
			_ => throw new NotSupportedException($"Unsupported ObfuscatedId arity: {args.Length}.")
		};
		return (JsonConverter)Activator.CreateInstance(converterType)!;
	}
}
