using GettingStarted.Mapping;
using GettingStarted.Models;
using KatzuoOgust.ObfuscatedIds;

// --- Configure the obfuscator -------------------------------------------
// In a real application you would load the key from configuration/secrets.
var options = new IdObfuscatorOptions();
options.SetXorKey("example-secret-key");

IIdObfuscator obfuscator = new IdObfuscator(options);

// --- Internal model (as it lives in the database) ------------------------
var internalOrder = new InternalOrder
{
	Id = 42,
	CustomerName = "Alice",
	Total = 99.99m,
};

// --- Map to the external API response ------------------------------------
var mapper = new OrderMapper(obfuscator);
OrderResponse response = mapper.ToResponse(internalOrder);

Console.WriteLine($"Internal ID : {internalOrder.Id}");
Console.WriteLine($"Obfuscated  : {response.Id}");   // URL-safe token

// --- Round-trip: map back to the internal model --------------------------
InternalOrder roundTripped = mapper.FromResponse(response);

Console.WriteLine($"Round-trip  : {roundTripped.Id}");
Console.WriteLine($"Match       : {internalOrder.Id == roundTripped.Id}");
