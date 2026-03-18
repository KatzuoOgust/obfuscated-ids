namespace KatzuoOgust.ObfuscatedIds;

/// <summary>
/// Type-safe obfuscator for a single value of type <typeparamref name="T"/>.
/// Wraps an <see cref="IIdObfuscator"/> to handle formatting and parsing of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the ID value (e.g. <see cref="int"/>, <see cref="Guid"/>, <see cref="string"/>).</typeparam>
public sealed class IdObfuscator<T> : IIdObfuscator<T>
{
	private readonly IIdObfuscator _base;

	/// <summary>
	/// Creates a new <see cref="IdObfuscator{T}"/> wrapping the given base obfuscator.
	/// </summary>
	/// <param name="obfuscator">The base obfuscator to use, or <see langword="null"/> to use <see cref="IdObfuscator.Default"/>.</param>
	public IdObfuscator(IIdObfuscator? obfuscator = null) =>
		_base = obfuscator ?? IdObfuscator.Default;

	/// <inheritdoc/>
	public string Obfuscate(T value) =>
		_base.Encode(IdComponents.FormatValue(value));

	/// <inheritdoc/>
	public T Deobfuscate(string token) =>
		IdComponents.ParseValue<T>(_base.Decode(token));
}

/// <summary>
/// Type-safe obfuscator for a two-component composite identifier encoded as a <c>(T1, T2)</c> tuple.
/// </summary>
public sealed class IdObfuscator<T1, T2> : IIdObfuscator<(T1, T2)>
{
	private readonly IIdObfuscator _base;

	/// <summary>
	/// Creates a new <see cref="IdObfuscator{T1,T2}"/> wrapping the given base obfuscator.
	/// </summary>
	/// <param name="obfuscator">The base obfuscator to use, or <see langword="null"/> to use <see cref="IdObfuscator.Default"/>.</param>
	public IdObfuscator(IIdObfuscator? obfuscator = null) =>
		_base = obfuscator ?? IdObfuscator.Default;

	/// <inheritdoc/>
	public string Obfuscate((T1, T2) value) =>
		_base.Encode(IdComponents.JoinComponents(
			IdComponents.FormatValue(value.Item1),
			IdComponents.FormatValue(value.Item2)
		));

	/// <inheritdoc/>
	public (T1, T2) Deobfuscate(string token)
	{
		var parts = IdComponents.SplitComponents(_base.Decode(token), 2);
		return (IdComponents.ParseValue<T1>(parts[0]), IdComponents.ParseValue<T2>(parts[1]));
	}
}

/// <summary>
/// Type-safe obfuscator for a three-component composite identifier encoded as a <c>(T1, T2, T3)</c> tuple.
/// </summary>
public sealed class IdObfuscator<T1, T2, T3> : IIdObfuscator<(T1, T2, T3)>
{
	private readonly IIdObfuscator _base;

	/// <summary>
	/// Creates a new <see cref="IdObfuscator{T1,T2,T3}"/> wrapping the given base obfuscator.
	/// </summary>
	/// <param name="obfuscator">The base obfuscator to use, or <see langword="null"/> to use <see cref="IdObfuscator.Default"/>.</param>
	public IdObfuscator(IIdObfuscator? obfuscator = null) =>
		_base = obfuscator ?? IdObfuscator.Default;

	/// <inheritdoc/>
	public string Obfuscate((T1, T2, T3) value) =>
		_base.Encode(IdComponents.JoinComponents(
			IdComponents.FormatValue(value.Item1),
			IdComponents.FormatValue(value.Item2),
			IdComponents.FormatValue(value.Item3)
		));

	/// <inheritdoc/>
	public (T1, T2, T3) Deobfuscate(string token)
	{
		var parts = IdComponents.SplitComponents(_base.Decode(token), 3);
		return (
			IdComponents.ParseValue<T1>(parts[0]),
			IdComponents.ParseValue<T2>(parts[1]),
			IdComponents.ParseValue<T3>(parts[2])
		);
	}
}

/// <summary>
/// Type-safe obfuscator for a four-component composite identifier encoded as a <c>(T1, T2, T3, T4)</c> tuple.
/// </summary>
public sealed class IdObfuscator<T1, T2, T3, T4> : IIdObfuscator<(T1, T2, T3, T4)>
{
	private readonly IIdObfuscator _base;

	/// <summary>
	/// Creates a new <see cref="IdObfuscator{T1,T2,T3,T4}"/> wrapping the given base obfuscator.
	/// </summary>
	/// <param name="obfuscator">The base obfuscator to use, or <see langword="null"/> to use <see cref="IdObfuscator.Default"/>.</param>
	public IdObfuscator(IIdObfuscator? obfuscator = null) =>
		_base = obfuscator ?? IdObfuscator.Default;

	/// <inheritdoc/>
	public string Obfuscate((T1, T2, T3, T4) value) =>
		_base.Encode(IdComponents.JoinComponents(
			IdComponents.FormatValue(value.Item1),
			IdComponents.FormatValue(value.Item2),
			IdComponents.FormatValue(value.Item3),
			IdComponents.FormatValue(value.Item4)
		));

	/// <inheritdoc/>
	public (T1, T2, T3, T4) Deobfuscate(string token)
	{
		var parts = IdComponents.SplitComponents(_base.Decode(token), 4);
		return (
			IdComponents.ParseValue<T1>(parts[0]),
			IdComponents.ParseValue<T2>(parts[1]),
			IdComponents.ParseValue<T3>(parts[2]),
			IdComponents.ParseValue<T4>(parts[3])
		);
	}
}

/// <summary>
/// Type-safe obfuscator for a five-component composite identifier encoded as a <c>(T1, T2, T3, T4, T5)</c> tuple.
/// </summary>
public sealed class IdObfuscator<T1, T2, T3, T4, T5> : IIdObfuscator<(T1, T2, T3, T4, T5)>
{
	private readonly IIdObfuscator _base;

	/// <summary>
	/// Creates a new <see cref="IdObfuscator{T1,T2,T3,T4,T5}"/> wrapping the given base obfuscator.
	/// </summary>
	/// <param name="obfuscator">The base obfuscator to use, or <see langword="null"/> to use <see cref="IdObfuscator.Default"/>.</param>
	public IdObfuscator(IIdObfuscator? obfuscator = null) =>
		_base = obfuscator ?? IdObfuscator.Default;

	/// <inheritdoc/>
	public string Obfuscate((T1, T2, T3, T4, T5) value) =>
		_base.Encode(IdComponents.JoinComponents(
			IdComponents.FormatValue(value.Item1),
			IdComponents.FormatValue(value.Item2),
			IdComponents.FormatValue(value.Item3),
			IdComponents.FormatValue(value.Item4),
			IdComponents.FormatValue(value.Item5)
		));

	/// <inheritdoc/>
	public (T1, T2, T3, T4, T5) Deobfuscate(string token)
	{
		var parts = IdComponents.SplitComponents(_base.Decode(token), 5);
		return (
			IdComponents.ParseValue<T1>(parts[0]),
			IdComponents.ParseValue<T2>(parts[1]),
			IdComponents.ParseValue<T3>(parts[2]),
			IdComponents.ParseValue<T4>(parts[3]),
			IdComponents.ParseValue<T5>(parts[4])
		);
	}
}
