using FluentAssertions;

namespace KatzuoOgust.ObfuscatedIds;

public class ObfuscatedIdTests
{
	// ── Single-value ────────────────────────────────────────────────────────

	[Fact]
	public void FromExternal_ReturnsOriginalInt_IfEncodedWithSingleInt()
	{
		var id = new ObfuscatedId<int>(42);
		id.External.Should().NotBeNullOrEmpty();
		id.External.Should().NotBe("42");

		var decoded = ObfuscatedId<int>.FromExternal(id.External);
		decoded.Value.Should().Be(42);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalLong_IfEncodedWithSingleLong()
	{
		var id = new ObfuscatedId<long>(9_876_543_210L);
		var decoded = ObfuscatedId<long>.FromExternal(id.External);
		decoded.Value.Should().Be(9_876_543_210L);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalGuid_IfEncodedWithSingleGuid()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<Guid>(guid);
		var decoded = ObfuscatedId<Guid>.FromExternal(id.External);
		decoded.Value.Should().Be(guid);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalString_IfEncodedWithSingleString()
	{
		var id = new ObfuscatedId<string>("hello-world");
		var decoded = ObfuscatedId<string>.FromExternal(id.External);
		decoded.Value.Should().Be("hello-world");
	}

	// ── Type upgrade ─────────────────────────────────────────────────────────

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(42)]
	[InlineData(int.MaxValue)]
	[InlineData(int.MinValue)]
	public void FromExternal_ReturnsEquivalentValue_IfIntTokenDecodedAsLong(int value)
	{
		var token = new ObfuscatedId<int>(value).External;
		ObfuscatedId<long>.FromExternal(token).Value.Should().Be((long)value);
	}

	// ── Two-value tuple ──────────────────────────────────────────────────────

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple2IntInt()
	{
		var id = new ObfuscatedId<int, int>(10, 20);
		var decoded = ObfuscatedId<int, int>.FromExternal(id.External);
		decoded.Value1.Should().Be(10);
		decoded.Value2.Should().Be(20);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple2IntLong()
	{
		var id = new ObfuscatedId<int, long>(7, 123_456_789_000L);
		var decoded = ObfuscatedId<int, long>.FromExternal(id.External);
		decoded.Value1.Should().Be(7);
		decoded.Value2.Should().Be(123_456_789_000L);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple2IntGuid()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<int, Guid>(99, guid);
		var decoded = ObfuscatedId<int, Guid>.FromExternal(id.External);
		decoded.Value1.Should().Be(99);
		decoded.Value2.Should().Be(guid);
	}

	// ── Three-value tuple ────────────────────────────────────────────────────

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple3IntIntInt()
	{
		var id = new ObfuscatedId<int, int, int>(1, 2, 3);
		var decoded = ObfuscatedId<int, int, int>.FromExternal(id.External);
		decoded.Value1.Should().Be(1);
		decoded.Value2.Should().Be(2);
		decoded.Value3.Should().Be(3);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple3LongGuidInt()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<long, Guid, int>(42L, guid, 7);
		var decoded = ObfuscatedId<long, Guid, int>.FromExternal(id.External);
		decoded.Value1.Should().Be(42L);
		decoded.Value2.Should().Be(guid);
		decoded.Value3.Should().Be(7);
	}

	// ── Four-value tuple ─────────────────────────────────────────────────────

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple4IntIntIntInt()
	{
		var id = new ObfuscatedId<int, int, int, int>(10, 20, 30, 40);
		var decoded = ObfuscatedId<int, int, int, int>.FromExternal(id.External);
		decoded.Value1.Should().Be(10);
		decoded.Value2.Should().Be(20);
		decoded.Value3.Should().Be(30);
		decoded.Value4.Should().Be(40);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple4IntLongGuidString()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<int, long, Guid, string>(5, 100L, guid, "tag");
		var decoded = ObfuscatedId<int, long, Guid, string>.FromExternal(id.External);
		decoded.Value1.Should().Be(5);
		decoded.Value2.Should().Be(100L);
		decoded.Value3.Should().Be(guid);
		decoded.Value4.Should().Be("tag");
	}

	// ── Five-value tuple ─────────────────────────────────────────────────────

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple5IntIntIntIntInt()
	{
		var id = new ObfuscatedId<int, int, int, int, int>(1, 2, 3, 4, 5);
		var decoded = ObfuscatedId<int, int, int, int, int>.FromExternal(id.External);
		decoded.Value1.Should().Be(1);
		decoded.Value2.Should().Be(2);
		decoded.Value3.Should().Be(3);
		decoded.Value4.Should().Be(4);
		decoded.Value5.Should().Be(5);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithTuple5IntLongGuidStringInt()
	{
		var guid = Guid.NewGuid();
		var id = new ObfuscatedId<int, long, Guid, string, int>(1, 2L, guid, "tag", 42);
		var decoded = ObfuscatedId<int, long, Guid, string, int>.FromExternal(id.External);
		decoded.Value1.Should().Be(1);
		decoded.Value2.Should().Be(2L);
		decoded.Value3.Should().Be(guid);
		decoded.Value4.Should().Be("tag");
		decoded.Value5.Should().Be(42);
	}

	[Fact]
	public void External_AlwaysSameToken_IfTuple5EncodedRepeatedly()
	{
		var guid = Guid.NewGuid();
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int, long, Guid, string, int>(1, 2L, guid, "tag", 42).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}


	[Fact]
	public void External_IsUrlSafe_IfValueIsLongMax()
	{
		var id = new ObfuscatedId<long>(long.MaxValue);
		id.External.Should().NotContain("+");
		id.External.Should().NotContain("/");
		id.External.Should().NotContain("=");
	}

	[Fact]
	public void External_DiffersPerValue_IfValuesAreDifferent()
	{
		var a = new ObfuscatedId<int>(1);
		var b = new ObfuscatedId<int>(2);
		a.External.Should().NotBe(b.External);
	}

	[Fact]
	public void External_IsSame_IfSameValueEncodedTwice()
	{
		var a = new ObfuscatedId<int>(42);
		var b = new ObfuscatedId<int>(42);
		a.External.Should().Be(b.External);
	}

	// ── Idempotency ──────────────────────────────────────────────────────────

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(42)]
	[InlineData(int.MaxValue)]
	public void External_AlwaysSameToken_IfSingleIntEncodedRepeatedly(int value)
	{
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int>(value).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfSingleLongEncodedRepeatedly()
	{
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<long>(long.MaxValue).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfSingleGuidEncodedRepeatedly()
	{
		var guid = Guid.NewGuid();
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<Guid>(guid).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfTuple2EncodedRepeatedly()
	{
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int, long>(7, 123_456_789L).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfTuple3EncodedRepeatedly()
	{
		var guid = Guid.NewGuid();
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int, long, Guid>(1, 2L, guid).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfTuple4EncodedRepeatedly()
	{
		var guid = Guid.NewGuid();
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int, long, Guid, string>(5, 100L, guid, "tag").External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfEncodedRepeatedlyWithPaddedBytes()
	{
		IdObfuscator.PaddedBytes = 32;
		try
		{
			var tokens = Enumerable.Range(0, 10)
				.Select(_ => new ObfuscatedId<int>(42).External)
				.ToList();

			tokens.Distinct().Should().HaveCount(1);
		}
		finally { IdObfuscator.PaddedBytes = 0; }
	}

	// ── Custom key ───────────────────────────────────────────────────────────

	[Fact]
	public void ConfigureXorKey_ChangesExternalToken_IfKeyChanged()
	{
		IdObfuscator.ConfigureXorKey([0x01, 0x02, 0x03, 0x04]);
		var externalWithCustomKey = new ObfuscatedId<int>(42).External;

		IdObfuscator.ConfigureXorKey([0x4A,
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
			0xB4]);
		var externalWithDefaultKey = new ObfuscatedId<int>(42).External;

		externalWithCustomKey.Should().NotBe(externalWithDefaultKey);

		// restore custom key and verify round-trip still works
		IdObfuscator.ConfigureXorKey([0x01, 0x02, 0x03, 0x04]);
		var decoded = ObfuscatedId<int>.FromExternal(externalWithCustomKey);
		decoded.Value.Should().Be(42);

		// restore default for subsequent tests
		IdObfuscator.ConfigureXorKey([0x4A,
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
			0xB4]);
	}

	// ── PaddedBytes alignment ────────────────────────────────────────────────

	[Fact]
	public void PaddedBytes_ThrowsArgumentOutOfRangeException_IfValueIsNegative()
	{
		var act = () => IdObfuscator.PaddedBytes = -1;
		act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("PaddedBytes");
	}

	[Fact]
	public void PaddedBytes_ThrowsArgumentOutOfRangeException_IfValueExceedsMaxPaddedBytes()
	{
		var act = () => IdObfuscator.PaddedBytes = IdObfuscator.MaxPaddedBytes + 1;
		act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("PaddedBytes");
	}

	[Fact]
	public void PaddedBytes_Succeeds_IfValueIsMaxPaddedBytes()
	{
		var act = () => IdObfuscator.PaddedBytes = IdObfuscator.MaxPaddedBytes;
		act.Should().NotThrow();
		IdObfuscator.PaddedBytes = 0;
	}

	[Fact]
	public void External_AllSameLength_IfPaddedBytesSet()
	{
		IdObfuscator.PaddedBytes = 32;
		try
		{
			var tokens = new[]
			{
				new ObfuscatedId<int>(1).External,
				new ObfuscatedId<int>(999_999).External,
				new ObfuscatedId<long>(long.MaxValue).External,
				new ObfuscatedId<int, int>(1, 2).External,
			};

			tokens.Should().OnlyHaveUniqueItems();
			tokens.Select(t => t.Length).Distinct().Should().HaveCount(1);
		}
		finally { IdObfuscator.PaddedBytes = 0; }
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithPaddedBytes()
	{
		IdObfuscator.PaddedBytes = 24;
		try
		{
			var id = new ObfuscatedId<int, Guid>(7, Guid.NewGuid());
			var decoded = ObfuscatedId<int, Guid>.FromExternal(id.External);
			decoded.Value1.Should().Be(id.Value1);
			decoded.Value2.Should().Be(id.Value2);
		}
		finally { IdObfuscator.PaddedBytes = 0; }
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValue_IfPaddedBytesDisabledAtDecodeTime()
	{
		// encode with padding on
		IdObfuscator.PaddedBytes = 32;
		var token = new ObfuscatedId<int>(42).External;

		// decode with padding off — the embedded length header handles it
		IdObfuscator.PaddedBytes = 0;
		ObfuscatedId<int>.FromExternal(token).Value.Should().Be(42);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValue_IfPaddedBytesSmallerThanNaturalSize()
	{
		IdObfuscator.PaddedBytes = 1;
		try
		{
			var id = new ObfuscatedId<long>(long.MaxValue);
			ObfuscatedId<long>.FromExternal(id.External).Value.Should().Be(long.MaxValue);
		}
		finally { IdObfuscator.PaddedBytes = 0; }
	}

	// ── Decode error handling ────────────────────────────────────────────────

	[Theory]
	[InlineData("not-base64!!!")]
	[InlineData("")]
	[InlineData("AAAA")]
	public void Decode_ThrowsFormatException_IfTokenIsInvalid(string token)
	{
		var act = () => IdObfuscator.Decode(token);
		act.Should().Throw<FormatException>().WithMessage($"*{token}*");
	}

	// ── Max length validation ────────────────────────────────────────────────

	[Fact]
	public void Encode_Succeeds_IfPlaintextIsExactlyMaxBytes()
	{
		var plaintext = new string('a', IdObfuscator.MaxPlaintextBytes);
		var act = () => IdObfuscator.Encode(plaintext);
		act.Should().NotThrow();
	}

	[Fact]
	public void Encode_ThrowsArgumentException_IfPlaintextExceedsMaxBytes()
	{
		var plaintext = new string('a', IdObfuscator.MaxPlaintextBytes + 1);
		var act = () => IdObfuscator.Encode(plaintext);
		act.Should().Throw<ArgumentException>()
			.WithParameterName("plaintext")
			.WithMessage($"*{IdObfuscator.MaxPlaintextBytes}*");
	}

	[Fact]
	public void Encode_ThrowsArgumentException_IfMultibyteCharsExceedMaxBytes()
	{
		// Each '€' is 3 UTF-8 bytes, so 86 × 3 = 258 bytes > 255
		var plaintext = new string('€', 86);
		var act = () => IdObfuscator.Encode(plaintext);
		act.Should().Throw<ArgumentException>()
			.WithParameterName("plaintext");
	}

	// ── Separator escaping ───────────────────────────────────────────────────

	[Theory]
	[InlineData("a|b")]
	[InlineData("|leading")]
	[InlineData("trailing|")]
	[InlineData("a|b|c")]
	public void FromExternal_ReturnsOriginalString_IfValueContainsSeparator(string value)
	{
		var id = new ObfuscatedId<string>(value);
		ObfuscatedId<string>.FromExternal(id.External).Value.Should().Be(value);
	}

	[Theory]
	[InlineData(@"a\b")]
	[InlineData(@"a\\b")]
	[InlineData(@"a\|b")]
	public void FromExternal_ReturnsOriginalString_IfValueContainsBackslash(string value)
	{
		var id = new ObfuscatedId<string>(value);
		ObfuscatedId<string>.FromExternal(id.External).Value.Should().Be(value);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfTuple2StringComponentContainsSeparator()
	{
		var id = new ObfuscatedId<string, string>("a|b", "c|d");
		var decoded = ObfuscatedId<string, string>.FromExternal(id.External);
		decoded.Value1.Should().Be("a|b");
		decoded.Value2.Should().Be("c|d");
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfTuple2StringComponentContainsBackslash()
	{
		var id = new ObfuscatedId<string, string>(@"a\b", @"c\d");
		var decoded = ObfuscatedId<string, string>.FromExternal(id.External);
		decoded.Value1.Should().Be(@"a\b");
		decoded.Value2.Should().Be(@"c\d");
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfTuple3StringsAllContainSeparatorAndBackslash()
	{
		var id = new ObfuscatedId<string, string, string>(@"a\|b", "c|d", @"e\f");
		var decoded = ObfuscatedId<string, string, string>.FromExternal(id.External);
		decoded.Value1.Should().Be(@"a\|b");
		decoded.Value2.Should().Be("c|d");
		decoded.Value3.Should().Be(@"e\f");
	}
}

