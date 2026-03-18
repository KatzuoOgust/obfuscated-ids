using System.Text.Json;
using System.Text.Json.Serialization;

namespace KatzuoOgust.ObfuscatedIds.Serialization;

internal sealed class ObfuscatedIdJsonConverter<T> : JsonConverter<ObfuscatedId<T>>
{
	public override ObfuscatedId<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> ObfuscatedId<T>.FromExternal(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, ObfuscatedId<T> value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.External);
}

internal sealed class ObfuscatedIdJsonConverter<T1, T2> : JsonConverter<ObfuscatedId<T1, T2>>
{
	public override ObfuscatedId<T1, T2> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> ObfuscatedId<T1, T2>.FromExternal(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, ObfuscatedId<T1, T2> value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.External);
}

internal sealed class ObfuscatedIdJsonConverter<T1, T2, T3> : JsonConverter<ObfuscatedId<T1, T2, T3>>
{
	public override ObfuscatedId<T1, T2, T3> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> ObfuscatedId<T1, T2, T3>.FromExternal(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, ObfuscatedId<T1, T2, T3> value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.External);
}

internal sealed class ObfuscatedIdJsonConverter<T1, T2, T3, T4> : JsonConverter<ObfuscatedId<T1, T2, T3, T4>>
{
	public override ObfuscatedId<T1, T2, T3, T4> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> ObfuscatedId<T1, T2, T3, T4>.FromExternal(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, ObfuscatedId<T1, T2, T3, T4> value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.External);
}

internal sealed class ObfuscatedIdJsonConverter<T1, T2, T3, T4, T5> : JsonConverter<ObfuscatedId<T1, T2, T3, T4, T5>>
{
	public override ObfuscatedId<T1, T2, T3, T4, T5> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> ObfuscatedId<T1, T2, T3, T4, T5>.FromExternal(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, ObfuscatedId<T1, T2, T3, T4, T5> value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.External);
}
