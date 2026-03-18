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
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
	public ObfuscatedId(T value, IIdObfuscator<T>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(value);

		var ob = obfuscator ?? new IdObfuscator<T>();
		Value = value;
		External = ob.Obfuscate(value);
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
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <returns>An instance whose <see cref="Value"/> holds the decoded internal ID.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T> FromExternal(string external, IIdObfuscator<T>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(external);

		var ob = obfuscator ?? new IdObfuscator<T>();
		var value = ob.Deobfuscate(external);
		return new ObfuscatedId<T>(value, external);
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
	/// <param name="value1">The first component.</param>
	/// <param name="value2">The second component.</param>
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, IIdObfuscator<(T1, T2)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);

		var ob = obfuscator ?? new IdObfuscator<T1, T2>();
		Value1 = value1;
		Value2 = value2;
		External = ob.Obfuscate((value1, value2));
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
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2> FromExternal(string external, IIdObfuscator<(T1, T2)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(external);

		var ob = obfuscator ?? new IdObfuscator<T1, T2>();
		var (v1, v2) = ob.Deobfuscate(external);
		return new ObfuscatedId<T1, T2>(v1, v2, external);
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
	/// <param name="value1">The first component.</param>
	/// <param name="value2">The second component.</param>
	/// <param name="value3">The third component.</param>
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, T3 value3, IIdObfuscator<(T1, T2, T3)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);
		ArgumentNullException.ThrowIfNull(value3);

		var ob = obfuscator ?? new IdObfuscator<T1, T2, T3>();
		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		External = ob.Obfuscate((value1, value2, value3));
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
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2, T3> FromExternal(string external, IIdObfuscator<(T1, T2, T3)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(external);

		var ob = obfuscator ?? new IdObfuscator<T1, T2, T3>();
		var (v1, v2, v3) = ob.Deobfuscate(external);
		return new ObfuscatedId<T1, T2, T3>(v1, v2, v3, external);
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
	/// <param name="value1">The first component.</param>
	/// <param name="value2">The second component.</param>
	/// <param name="value3">The third component.</param>
	/// <param name="value4">The fourth component.</param>
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, T3 value3, T4 value4, IIdObfuscator<(T1, T2, T3, T4)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);
		ArgumentNullException.ThrowIfNull(value3);
		ArgumentNullException.ThrowIfNull(value4);

		var ob = obfuscator ?? new IdObfuscator<T1, T2, T3, T4>();
		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		Value4 = value4;
		External = ob.Obfuscate((value1, value2, value3, value4));
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
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2, T3, T4> FromExternal(string external, IIdObfuscator<(T1, T2, T3, T4)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(external);

		var ob = obfuscator ?? new IdObfuscator<T1, T2, T3, T4>();
		var (v1, v2, v3, v4) = ob.Deobfuscate(external);
		return new ObfuscatedId<T1, T2, T3, T4>(v1, v2, v3, v4, external);
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
	/// <param name="value1">The first component.</param>
	/// <param name="value2">The second component.</param>
	/// <param name="value3">The third component.</param>
	/// <param name="value4">The fourth component.</param>
	/// <param name="value5">The fifth component.</param>
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when any value is null.</exception>
	public ObfuscatedId(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IIdObfuscator<(T1, T2, T3, T4, T5)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(value1);
		ArgumentNullException.ThrowIfNull(value2);
		ArgumentNullException.ThrowIfNull(value3);
		ArgumentNullException.ThrowIfNull(value4);
		ArgumentNullException.ThrowIfNull(value5);

		var ob = obfuscator ?? new IdObfuscator<T1, T2, T3, T4, T5>();
		Value1 = value1;
		Value2 = value2;
		Value3 = value3;
		Value4 = value4;
		Value5 = value5;
		External = ob.Obfuscate((value1, value2, value3, value4, value5));
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
	/// <param name="obfuscator">The obfuscator to use, or <see langword="null"/> to use the default.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="external"/> is null.</exception>
	public static ObfuscatedId<T1, T2, T3, T4, T5> FromExternal(string external, IIdObfuscator<(T1, T2, T3, T4, T5)>? obfuscator = null)
	{
		ArgumentNullException.ThrowIfNull(external);

		var ob = obfuscator ?? new IdObfuscator<T1, T2, T3, T4, T5>();
		var (v1, v2, v3, v4, v5) = ob.Deobfuscate(external);
		return new ObfuscatedId<T1, T2, T3, T4, T5>(v1, v2, v3, v4, v5, external);
	}

	/// <inheritdoc/>
	public override string ToString() => External;
}
