using GettingStarted.Models;
using KatzuoOgust.ObfuscatedIds;

namespace GettingStarted.Mapping;

/// <summary>
/// Maps between the internal domain model and the external API response.
/// Accepts <see cref="IIdObfuscator"/> so the obfuscation key can be injected
/// (e.g., from configuration or a DI container) rather than hard-coded here.
/// Wraps it in an <see cref="IdObfuscator{T}"/> to get the typed
/// <see cref="IIdObfuscator{T}"/> required by <see cref="ObfuscatedId{T}"/>.
/// </summary>
public sealed class OrderMapper(IIdObfuscator obfuscator)
{
	private readonly IIdObfuscator<int> _typedObfuscator = new IdObfuscator<int>(obfuscator);

	public OrderResponse ToResponse(InternalOrder order) => new()
	{
		Id = new ObfuscatedId<int>(order.Id, _typedObfuscator),
		CustomerName = order.CustomerName,
		Total = order.Total,
	};

	public InternalOrder FromResponse(OrderResponse response) => new()
	{
		Id = response.Id.Value,
		CustomerName = response.CustomerName,
		Total = response.Total,
	};
}
