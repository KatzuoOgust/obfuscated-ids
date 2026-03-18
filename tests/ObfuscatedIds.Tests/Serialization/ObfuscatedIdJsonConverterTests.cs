using System.Text.Json;
using FluentAssertions;
using KatzuoOgust.ObfuscatedIds.Serialization;

namespace KatzuoOgust.ObfuscatedIds.Serialization;

public class ObfuscatedIdJsonConverterTests
{
	private static readonly JsonSerializerOptions Options = new()
	{
		Converters = { new ObfuscatedIdJsonConverterFactory() }
	};

	// ── Serialize ─────────────────────────────────────────────────────────────

	[Fact]
	public void Serialize_WritesExternalToken_IfSingleInt()
	{
		var id = new ObfuscatedId<int>(42);
		var json = JsonSerializer.Serialize(id, Options);
		json.Should().Be($"\"{id.External}\"");
	}

	[Fact]
	public void Serialize_WritesExternalToken_IfTuple2()
	{
		var id = new ObfuscatedId<int, string>(1, "hello");
		var json = JsonSerializer.Serialize(id, Options);
		json.Should().Be($"\"{id.External}\"");
	}

	// ── Deserialize ───────────────────────────────────────────────────────────

	[Fact]
	public void Deserialize_ReturnsOriginalValue_IfSingleInt()
	{
		var id = new ObfuscatedId<int>(42);
		var json = JsonSerializer.Serialize(id, Options);
		JsonSerializer.Deserialize<ObfuscatedId<int>>(json, Options)!.Value.Should().Be(42);
	}

	[Fact]
	public void Deserialize_ReturnsOriginalValue_IfSingleGuid()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<Guid>(guid);
		var json = JsonSerializer.Serialize(id, Options);
		JsonSerializer.Deserialize<ObfuscatedId<Guid>>(json, Options)!.Value.Should().Be(guid);
	}

	[Fact]
	public void Deserialize_ReturnsOriginalValues_IfTuple2()
	{
		var id = new ObfuscatedId<int, string>(7, "world");
		var json = JsonSerializer.Serialize(id, Options);
		var decoded = JsonSerializer.Deserialize<ObfuscatedId<int, string>>(json, Options)!;
		decoded.Value1.Should().Be(7);
		decoded.Value2.Should().Be("world");
	}

	[Fact]
	public void Deserialize_ReturnsOriginalValues_IfTuple3()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<int, Guid, string>(1, guid, "tag");
		var json = JsonSerializer.Serialize(id, Options);
		var decoded = JsonSerializer.Deserialize<ObfuscatedId<int, Guid, string>>(json, Options)!;
		decoded.Value1.Should().Be(1);
		decoded.Value2.Should().Be(guid);
		decoded.Value3.Should().Be("tag");
	}

	[Fact]
	public void Deserialize_ReturnsOriginalValues_IfTuple4()
	{
		var id = new ObfuscatedId<int, int, int, int>(1, 2, 3, 4);
		var json = JsonSerializer.Serialize(id, Options);
		var decoded = JsonSerializer.Deserialize<ObfuscatedId<int, int, int, int>>(json, Options)!;
		decoded.Value1.Should().Be(1);
		decoded.Value2.Should().Be(2);
		decoded.Value3.Should().Be(3);
		decoded.Value4.Should().Be(4);
	}

	[Fact]
	public void Deserialize_ReturnsOriginalValues_IfTuple5()
	{
		var id = new ObfuscatedId<int, int, int, int, int>(1, 2, 3, 4, 5);
		var json = JsonSerializer.Serialize(id, Options);
		var decoded = JsonSerializer.Deserialize<ObfuscatedId<int, int, int, int, int>>(json, Options)!;
		decoded.Value1.Should().Be(1);
		decoded.Value2.Should().Be(2);
		decoded.Value3.Should().Be(3);
		decoded.Value4.Should().Be(4);
		decoded.Value5.Should().Be(5);
	}

	// ── Round-trip as property ────────────────────────────────────────────────

	[Fact]
	public void RoundTrip_PreservesValue_IfUsedAsJsonProperty()
	{
		var payload = new { Id = new ObfuscatedId<long>(9_876_543_210L) };
		var json = JsonSerializer.Serialize(payload, Options);
		var decoded = JsonSerializer.Deserialize<Wrapper>(json, Options)!;
		decoded.Id.Value.Should().Be(9_876_543_210L);
	}

	private sealed class Wrapper
	{
		public ObfuscatedId<long> Id { get; set; } = null!;
	}
}
