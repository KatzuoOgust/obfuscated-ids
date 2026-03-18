namespace KatzuoOgust.ObfuscatedIds;

/// <summary>
/// Represents a single-value obfuscated identifier.
/// </summary>
/// <typeparam name="T">The type of the internal ID value (e.g. <see cref="int"/>, <see cref="long"/>, <see cref="Guid"/>).</typeparam>
public sealed class ObfuscatedId<T>
{
	/// <summary>The original internal ID value.</summary>
	public T Value { get; }

	/// <summary>The URL-safe obfuscated representation of <see cref="Value"/>.</summary>
	public string External { get; }

	/// <summary>
	/// Creates a new <see cref="ObfuscatedId{T}"/> from an internal value and computes its external token.
	/// </summary>
	/// <param name="value">The internal ID to obfuscate.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
	public ObfuscatedId(T value)
	{
		ArgumentNullException.ThrowIfNull(value);

		Value = value;
		External = IdObfuscator.Encode(IdObfuscator.FormatValue(value));
	}

	private ObfuscatedId(T value, string external)
	{
		Value = value;
		External = external;
	}

	/// <summary>
	/// Decodes an external token back into an <see cref="ObfuscatedId{T}"/>.
	/// </summary>
	/// <param name="external">A token previously returned by <see cref="External"/>.</param>
	/// <returns>An instance whose <see cref="Value"/> holds the decoded internal ID.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T> FromExternal(string external)
	{
		ArgumentNullException.ThrowIfNull(external);

		var plain = IdObfuscator.Decode(external);
		return new ObfuscatedId<T>(IdObfuscator.ParseValue<T>(plain), external);
	}

	/// <inheritdoc/>
	public override string ToString() => External;
}

/// <summary>
/// Represents a two-value tuple obfuscated identifier.
/// </summary>
/// <typeparam name="T1">Type of the first component.</typeparam>
/// <typeparam name="T2">Type of the second component.</typeparam>
public sealed class ObfuscatedId<T1, T2>
{
	/// <summary>The first component of the internal composite ID.</summary>
	public T1 Value1 { get; }

	/// <summary>The second component of the internal composite ID.</summary>
	public T2 Value2 { get; }

	/// <summary>The URL-safe obfuscated representation of the composite ID.</summary>
	public string External { get; }

	/// <summary>
	/// Creates a new <see cref="ObfuscatedId{T1,T2}"/> from two internal values.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);

		Value1 = value1;
		Value2 = value2;
		External = IdObfuscator.Encode(
			IdObfuscator.JoinComponents(
				IdObfuscator.FormatValue(value1),
				IdObfuscator.FormatValue(value2)
			)
		);
	}

	private ObfuscatedId(T1 value1, T2 value2, string external)
	{
		Value1 = value1;
		Value2 = value2;
		External = external;
	}

	/// <summary>
	/// Decodes an external token back into an <see cref="ObfuscatedId{T1,T2}"/>.
	/// </summary>
	/// <param name="external">A token previously returned by <see cref="External"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2> FromExternal(string external)
	{
		ArgumentNullException.ThrowIfNull(external);

		var plain = IdObfuscator.Decode(external);
		var parts = IdObfuscator.SplitComponents(plain, 2);
		return new ObfuscatedId<T1, T2>(
			IdObfuscator.ParseValue<T1>(parts[0]),
			IdObfuscator.ParseValue<T2>(parts[1]),
			external
		);
	}

	/// <inheritdoc/>
	public override string ToString() => External;
}

/// <summary>
/// Represents a three-value tuple obfuscated identifier.
/// </summary>
/// <typeparam name="T1">Type of the first component.</typeparam>
/// <typeparam name="T2">Type of the second component.</typeparam>
/// <typeparam name="T3">Type of the third component.</typeparam>
public sealed class ObfuscatedId<T1, T2, T3>
{
	/// <summary>The first component of the internal composite ID.</summary>
	public T1 Value1 { get; }

	/// <summary>The second component of the internal composite ID.</summary>
	public T2 Value2 { get; }

	/// <summary>The third component of the internal composite ID.</summary>
	public T3 Value3 { get; }

	/// <summary>The URL-safe obfuscated representation of the composite ID.</summary>
	public string External { get; }

	/// <summary>
	/// Creates a new <see cref="ObfuscatedId{T1,T2,T3}"/> from three internal values.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, T3 value3)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);
		ArgumentNullException.ThrowIfNull(value3);

		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		External = IdObfuscator.Encode(
			IdObfuscator.JoinComponents(
				IdObfuscator.FormatValue(value1),
				IdObfuscator.FormatValue(value2),
				IdObfuscator.FormatValue(value3)
			)
		);
	}

	private ObfuscatedId(T1 value1, T2 value2, T3 value3, string external)
	{
		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		External = external;
	}

	/// <summary>
	/// Decodes an external token back into an <see cref="ObfuscatedId{T1,T2,T3}"/>.
	/// </summary>
	/// <param name="external">A token previously returned by <see cref="External"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2, T3> FromExternal(string external)
	{
		ArgumentNullException.ThrowIfNull(external);

		var plain = IdObfuscator.Decode(external);
		var parts = IdObfuscator.SplitComponents(plain, 3);
		return new ObfuscatedId<T1, T2, T3>(
			IdObfuscator.ParseValue<T1>(parts[0]),
			IdObfuscator.ParseValue<T2>(parts[1]),
			IdObfuscator.ParseValue<T3>(parts[2]),
			external
		);
	}

	/// <inheritdoc/>
	public override string ToString() => External;
}

/// <summary>
/// Represents a four-value tuple obfuscated identifier.
/// </summary>
/// <typeparam name="T1">Type of the first component.</typeparam>
/// <typeparam name="T2">Type of the second component.</typeparam>
/// <typeparam name="T3">Type of the third component.</typeparam>
/// <typeparam name="T4">Type of the fourth component.</typeparam>
public sealed class ObfuscatedId<T1, T2, T3, T4>
{
	/// <summary>The first component of the internal composite ID.</summary>
	public T1 Value1 { get; }

	/// <summary>The second component of the internal composite ID.</summary>
	public T2 Value2 { get; }

	/// <summary>The third component of the internal composite ID.</summary>
	public T3 Value3 { get; }

	/// <summary>The fourth component of the internal composite ID.</summary>
	public T4 Value4 { get; }

	/// <summary>The URL-safe obfuscated representation of the composite ID.</summary>
	public string External { get; }

	/// <summary>
	/// Creates a new <see cref="ObfuscatedId{T1,T2,T3,T4}"/> from four internal values.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, T3 value3, T4 value4)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);
		ArgumentNullException.ThrowIfNull(value3);
		ArgumentNullException.ThrowIfNull(value4);

		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		Value4 = value4;
		External = IdObfuscator.Encode(
			IdObfuscator.JoinComponents(
				IdObfuscator.FormatValue(value1),
				IdObfuscator.FormatValue(value2),
				IdObfuscator.FormatValue(value3),
				IdObfuscator.FormatValue(value4)
			)
		);
	}

	private ObfuscatedId(T1 value1, T2 value2, T3 value3, T4 value4, string external)
	{
		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		Value4 = value4;
		External = external;
	}

	/// <summary>
	/// Decodes an external token back into an <see cref="ObfuscatedId{T1,T2,T3,T4}"/>.
	/// </summary>
	/// <param name="external">A token previously returned by <see cref="External"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2, T3, T4> FromExternal(string external)
	{
		ArgumentNullException.ThrowIfNull(external);

		var plain = IdObfuscator.Decode(external);
		var parts = IdObfuscator.SplitComponents(plain, 4);
		return new ObfuscatedId<T1, T2, T3, T4>(
			IdObfuscator.ParseValue<T1>(parts[0]),
			IdObfuscator.ParseValue<T2>(parts[1]),
			IdObfuscator.ParseValue<T3>(parts[2]),
			IdObfuscator.ParseValue<T4>(parts[3]),
			external
		);
	}

	/// <inheritdoc/>
	public override string ToString() => External;
}

/// <summary>
/// Represents a five-value tuple obfuscated identifier.
/// </summary>
/// <typeparam name="T1">Type of the first component.</typeparam>
/// <typeparam name="T2">Type of the second component.</typeparam>
/// <typeparam name="T3">Type of the third component.</typeparam>
/// <typeparam name="T4">Type of the fourth component.</typeparam>
/// <typeparam name="T5">Type of the fifth component.</typeparam>
public sealed class ObfuscatedId<T1, T2, T3, T4, T5>
{
	/// <summary>The first component of the internal composite ID.</summary>
	public T1 Value1 { get; }

	/// <summary>The second component of the internal composite ID.</summary>
	public T2 Value2 { get; }

	/// <summary>The third component of the internal composite ID.</summary>
	public T3 Value3 { get; }

	/// <summary>The fourth component of the internal composite ID.</summary>
	public T4 Value4 { get; }

	/// <summary>The fifth component of the internal composite ID.</summary>
	public T5 Value5 { get; }

	/// <summary>The URL-safe obfuscated representation of the composite ID.</summary>
	public string External { get; }

	/// <summary>
	/// Creates a new <see cref="ObfuscatedId{T1,T2,T3,T4,T5}"/> from five internal values.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);
		ArgumentNullException.ThrowIfNull(value3);
		ArgumentNullException.ThrowIfNull(value4);
		ArgumentNullException.ThrowIfNull(value5);

		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		Value4 = value4;
		Value5 = value5;
		External = IdObfuscator.Encode(
			IdObfuscator.JoinComponents(
				IdObfuscator.FormatValue(value1),
				IdObfuscator.FormatValue(value2),
				IdObfuscator.FormatValue(value3),
				IdObfuscator.FormatValue(value4),
				IdObfuscator.FormatValue(value5)
			)
		);
	}

	private ObfuscatedId(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, string external)
	{
		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		Value4 = value4;
		Value5 = value5;
		External = external;
	}

	/// <summary>
	/// Decodes an external token back into an <see cref="ObfuscatedId{T1,T2,T3,T4,T5}"/>.
	/// </summary>
	/// <param name="external">A token previously returned by <see cref="External"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2, T3, T4, T5> FromExternal(string external)
	{
		ArgumentNullException.ThrowIfNull(external);

		var plain = IdObfuscator.Decode(external);
		var parts = IdObfuscator.SplitComponents(plain, 5);
		return new ObfuscatedId<T1, T2, T3, T4, T5>(
			IdObfuscator.ParseValue<T1>(parts[0]),
			IdObfuscator.ParseValue<T2>(parts[1]),
			IdObfuscator.ParseValue<T3>(parts[2]),
			IdObfuscator.ParseValue<T4>(parts[3]),
			IdObfuscator.ParseValue<T5>(parts[4]),
			external
		);
	}

	/// <inheritdoc/>
	public override string ToString() => External;
}
