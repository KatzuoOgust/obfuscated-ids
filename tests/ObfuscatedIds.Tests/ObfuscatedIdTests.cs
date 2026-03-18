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
		var ob = new IdObfuscator<int>(new IdObfuscator(new IdObfuscatorOptions { PaddedBytes = 32 }));
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int>(42, ob).External)
			.ToList();

		tokens.Distinct().Should().HaveCount(1);
	}

	// ── Custom key ───────────────────────────────────────────────────────────

	[Fact]
	public void XorKey_ChangesExternalToken_IfKeyChanged()
	{
		var customOb = new IdObfuscator<int>(new IdObfuscator(new IdObfuscatorOptions { XorKey = [0x01, 0x02, 0x03, 0x04] }));
		var externalWithCustomKey = new ObfuscatedId<int>(42, customOb).External;
		var externalWithDefaultKey = new ObfuscatedId<int>(42).External;

		externalWithCustomKey.Should().NotBe(externalWithDefaultKey);
		ObfuscatedId<int>.FromExternal(externalWithCustomKey, customOb).Value.Should().Be(42);
	}

	[Fact]
	public void XorKey_StringAndBytesOptions_ProduceSameToken()
	{
		var key = "my-secret-key";
		var optsFromString = new IdObfuscatorOptions();
		optsFromString.SetXorKey(key);
		var optsFromBytes = new IdObfuscatorOptions { XorKey = System.Text.Encoding.UTF8.GetBytes(key) };

		var tokenFromString = new ObfuscatedId<int>(42, new IdObfuscator<int>(new IdObfuscator(optsFromString))).External;
		var tokenFromBytes = new ObfuscatedId<int>(42, new IdObfuscator<int>(new IdObfuscator(optsFromBytes))).External;

		tokenFromString.Should().Be(tokenFromBytes);
	}

	[Fact]
	public void XorKey_StringOption_RoundTrips()
	{
		var opts = new IdObfuscatorOptions();
		opts.SetXorKey("my-secret-key");
		var ob = new IdObfuscator<int>(new IdObfuscator(opts));
		var id = new ObfuscatedId<int>(42, ob);
		ObfuscatedId<int>.FromExternal(id.External, ob).Value.Should().Be(42);
	}

	[Fact]
	public void XorKey_ThrowsArgumentException_IfKeyIsEmpty()
	{
		var act = () => new IdObfuscatorOptions { XorKey = [] };
		act.Should().Throw<ArgumentException>();
	}

	// ── PaddedBytes alignment ────────────────────────────────────────────────

	[Fact]
	public void PaddedBytes_ThrowsArgumentOutOfRangeException_IfValueIsNegative()
	{
		var act = () => new IdObfuscatorOptions { PaddedBytes = -1 };
		act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("PaddedBytes");
	}

	[Fact]
	public void PaddedBytes_ThrowsArgumentOutOfRangeException_IfValueExceedsMaxPaddedBytes()
	{
		var act = () => new IdObfuscatorOptions { PaddedBytes = IdObfuscatorOptions.MaxPaddedBytes + 1 };
		act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("PaddedBytes");
	}

	[Fact]
	public void PaddedBytes_Succeeds_IfValueIsMaxPaddedBytes()
	{
		var act = () => new IdObfuscatorOptions { PaddedBytes = IdObfuscatorOptions.MaxPaddedBytes };
		act.Should().NotThrow();
	}

	[Fact]
	public void External_AllSameLength_IfPaddedBytesSet()
	{
		var ob = new IdObfuscator(new IdObfuscatorOptions { PaddedBytes = 32 });
		var tokens = new[]
		{
			new ObfuscatedId<int>(1, new IdObfuscator<int>(ob)).External,
			new ObfuscatedId<int>(999_999, new IdObfuscator<int>(ob)).External,
			new ObfuscatedId<long>(long.MaxValue, new IdObfuscator<long>(ob)).External,
			new ObfuscatedId<int, int>(1, 2, new IdObfuscator<int, int>(ob)).External,
		};

		tokens.Should().OnlyHaveUniqueItems();
		tokens.Select(t => t.Length).Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValues_IfEncodedWithPaddedBytes()
	{
		var ob = new IdObfuscator<int, Guid>(new IdObfuscator(new IdObfuscatorOptions { PaddedBytes = 24 }));
		var id = new ObfuscatedId<int, Guid>(7, Guid.NewGuid(), ob);
		var decoded = ObfuscatedId<int, Guid>.FromExternal(id.External, ob);
		decoded.Value1.Should().Be(id.Value1);
		decoded.Value2.Should().Be(id.Value2);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValue_IfPaddedBytesDisabledAtDecodeTime()
	{
		// encode with padding on
		var obWithPadding = new IdObfuscator<int>(new IdObfuscator(new IdObfuscatorOptions { PaddedBytes = 32 }));
		var token = new ObfuscatedId<int>(42, obWithPadding).External;

		// decode with padding off — the embedded length header handles it
		ObfuscatedId<int>.FromExternal(token).Value.Should().Be(42);
	}

	[Fact]
	public void FromExternal_ReturnsOriginalValue_IfPaddedBytesSmallerThanNaturalSize()
	{
		var ob = new IdObfuscator<long>(new IdObfuscator(new IdObfuscatorOptions { PaddedBytes = 1 }));
		var id = new ObfuscatedId<long>(long.MaxValue, ob);
		ObfuscatedId<long>.FromExternal(id.External, ob).Value.Should().Be(long.MaxValue);
	}

	// ── Decode error handling ────────────────────────────────────────────────

	[Theory]
	[InlineData("not-base64!!!")]
	[InlineData("")]
	[InlineData("AAAA")]
	public void Decode_ThrowsFormatException_IfTokenIsInvalid(string token)
	{
		var act = () => IdObfuscator.Default.Decode(token);
		act.Should().Throw<FormatException>().WithMessage($"*{token}*");
	}

	// ── Max length validation ────────────────────────────────────────────────

	[Fact]
	public void Encode_Succeeds_IfPlaintextIsExactlyMaxBytes()
	{
		var plaintext = new string('a', IdObfuscatorOptions.MaxPlaintextBytes);
		var act = () => IdObfuscator.Default.Encode(plaintext);
		act.Should().NotThrow();
	}

	[Fact]
	public void Encode_ThrowsArgumentException_IfPlaintextExceedsMaxBytes()
	{
		var plaintext = new string('a', IdObfuscatorOptions.MaxPlaintextBytes + 1);
		var act = () => IdObfuscator.Default.Encode(plaintext);
		act.Should().Throw<ArgumentException>()
			.WithParameterName("plaintext")
			.WithMessage($"*{IdObfuscatorOptions.MaxPlaintextBytes}*");
	}

	[Fact]
	public void Encode_ThrowsArgumentException_IfMultibyteCharsExceedMaxBytes()
	{
		// Each '€' is 3 UTF-8 bytes, so 86 × 3 = 258 bytes > 255
		var plaintext = new string('€', 86);
		var act = () => IdObfuscator.Default.Encode(plaintext);
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

	// ── Permutation ───────────────────────────────────────────────────────────

	[Fact]
	public void PermutationSeed_ChangesExternalToken_IfSeedSet()
	{
		var tokenWithout = new ObfuscatedId<int>(42).External;

		var opts = new IdObfuscatorOptions();
		opts.SetPermutationSeed("my-perm-seed");
		var ob = new IdObfuscator<int>(new IdObfuscator(opts));
		var tokenWith = new ObfuscatedId<int>(42, ob).External;
		tokenWith.Should().NotBe(tokenWithout);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(42)]
	[InlineData(int.MaxValue)]
	public void External_AlwaysSameToken_IfSingleIntEncodedRepeatedlyWithPermutation(int value)
	{
		var opts = new IdObfuscatorOptions();
		opts.SetPermutationSeed("perm-seed");
		var ob = new IdObfuscator<int>(new IdObfuscator(opts));
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int>(value, ob).External)
			.ToList();
		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfTuple2EncodedRepeatedlyWithPermutation()
	{
		var opts = new IdObfuscatorOptions();
		opts.SetPermutationSeed("perm-seed");
		var ob = new IdObfuscator<int, long>(new IdObfuscator(opts));
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int, long>(7, 123_456_789L, ob).External)
			.ToList();
		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfTuple3EncodedRepeatedlyWithPermutation()
	{
		var guid = Guid.NewGuid();
		var opts = new IdObfuscatorOptions();
		opts.SetPermutationSeed("perm-seed");
		var ob = new IdObfuscator<int, long, Guid>(new IdObfuscator(opts));
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int, long, Guid>(1, 2L, guid, ob).External)
			.ToList();
		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void External_AlwaysSameToken_IfEncodedRepeatedlyWithPermutationAndPaddedBytes()
	{
		var opts = new IdObfuscatorOptions { PaddedBytes = 24 };
		opts.SetPermutationSeed("perm-seed");
		var ob = new IdObfuscator<int>(new IdObfuscator(opts));
		var tokens = Enumerable.Range(0, 10)
			.Select(_ => new ObfuscatedId<int>(42, ob).External)
			.ToList();
		tokens.Distinct().Should().HaveCount(1);
	}

	[Fact]
	public void PermutationSeed_RoundTrips_IfSingleInt()
	{
		var opts = new IdObfuscatorOptions();
		opts.SetPermutationSeed("my-perm-seed");
		var ob = new IdObfuscator<int>(new IdObfuscator(opts));
		var id = new ObfuscatedId<int>(42, ob);
		ObfuscatedId<int>.FromExternal(id.External, ob).Value.Should().Be(42);
	}

	[Fact]
	public void PermutationSeed_RoundTrips_IfTuple3()
	{
		var guid = Guid.NewGuid();
		var ob = new IdObfuscator<int, Guid, string>(new IdObfuscator(new IdObfuscatorOptions { PermutationSeed = [0xDE, 0xAD, 0xBE, 0xEF] }));
		var id = new ObfuscatedId<int, Guid, string>(7, guid, "tag", ob);
		var decoded = ObfuscatedId<int, Guid, string>.FromExternal(id.External, ob);
		decoded.Value1.Should().Be(7);
		decoded.Value2.Should().Be(guid);
		decoded.Value3.Should().Be("tag");
	}

	[Fact]
	public void PermutationSeed_StringAndBytesOptions_ProduceSameToken()
	{
		var seed = "perm-seed";
		var optsStr = new IdObfuscatorOptions();
		optsStr.SetPermutationSeed(seed);
		var optsBytes = new IdObfuscatorOptions { PermutationSeed = System.Text.Encoding.UTF8.GetBytes(seed) };

		var tokenFromString = new ObfuscatedId<int>(99, new IdObfuscator<int>(new IdObfuscator(optsStr))).External;
		var tokenFromBytes = new ObfuscatedId<int>(99, new IdObfuscator<int>(new IdObfuscator(optsBytes))).External;

		tokenFromString.Should().Be(tokenFromBytes);
	}

	[Fact]
	public void PermutationSeed_NullResetsToOriginalToken()
	{
		var tokenWithout = new ObfuscatedId<int>(42).External;
		var opts = new IdObfuscatorOptions();
		opts.SetPermutationSeed("seed");
		var tokenWith = new ObfuscatedId<int>(42, new IdObfuscator<int>(new IdObfuscator(opts))).External;
		tokenWith.Should().NotBe(tokenWithout);

		// Default (no permutation) still produces original token
		new ObfuscatedId<int>(42).External.Should().Be(tokenWithout);
	}

	[Fact]
	public void PermutationSeed_ThrowsArgumentException_IfSeedIsEmpty()
	{
		var act1 = () => new IdObfuscatorOptions { PermutationSeed = Array.Empty<byte>() };
		act1.Should().Throw<ArgumentException>();

		var opts = new IdObfuscatorOptions();
		var act2 = () => opts.SetPermutationSeed(string.Empty);
		act2.Should().Throw<ArgumentException>();
	}

	// ── Typed IdObfuscator<T> ────────────────────────────────────────────────

	[Fact]
	public void TypedObfuscator_RoundTrips_IfSingleInt()
	{
		IIdObfuscator<int> ob = new IdObfuscator<int>();
		var token = ob.Obfuscate(42);
		ob.Deobfuscate(token).Should().Be(42);
	}

	[Fact]
	public void TypedObfuscator_RoundTrips_IfTuple2()
	{
		IIdObfuscator<(int, string)> ob = new IdObfuscator<int, string>();
		var token = ob.Obfuscate((7, "hello"));
		ob.Deobfuscate(token).Should().Be((7, "hello"));
	}

	[Fact]
	public void TypedObfuscator_RoundTrips_IfTuple3()
	{
		var guid = Guid.NewGuid();
		IIdObfuscator<(int, Guid, string)> ob = new IdObfuscator<int, Guid, string>();
		var token = ob.Obfuscate((1, guid, "tag"));
		ob.Deobfuscate(token).Should().Be((1, guid, "tag"));
	}

	[Fact]
	public void TypedObfuscator_UsesCustomBaseObfuscator_IfProvided()
	{
		var baseOb = new IdObfuscator(new IdObfuscatorOptions { XorKey = [0x11, 0x22, 0x33] });
		IIdObfuscator<int> typed = new IdObfuscator<int>(baseOb);
		IIdObfuscator<int> defaultTyped = new IdObfuscator<int>();

		var tokenCustom = typed.Obfuscate(99);
		var tokenDefault = defaultTyped.Obfuscate(99);

		tokenCustom.Should().NotBe(tokenDefault);
		typed.Deobfuscate(tokenCustom).Should().Be(99);
	}
}

