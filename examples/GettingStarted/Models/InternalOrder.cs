namespace GettingStarted.Models;

/// <summary>
/// Internal domain model — uses a plain int as the identifier.
/// This is what lives in your database and business logic.
/// </summary>
public sealed class InternalOrder
{
	public int Id { get; set; }
	public string CustomerName { get; set; } = string.Empty;
	public decimal Total { get; set; }
}
