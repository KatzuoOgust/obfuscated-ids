using KatzuoOgust.ObfuscatedIds;

namespace GettingStarted.Models;

/// <summary>
/// Outer (API/DTO) model — exposes the identifier as an ObfuscatedId&lt;int&gt;
/// so that raw database IDs are never leaked to callers.
/// </summary>
public sealed class OrderResponse
{
	public required ObfuscatedId<int> Id { get; set; }
	public string CustomerName { get; set; } = string.Empty;
	public decimal Total { get; set; }
}
