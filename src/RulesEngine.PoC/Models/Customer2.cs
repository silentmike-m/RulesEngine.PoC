namespace RulesEngine.PoC.Models;

internal sealed record Customer2(int Age, Guid Id, bool IsSupplier, decimal Money, string FirstName, string LastName, Country Country);
