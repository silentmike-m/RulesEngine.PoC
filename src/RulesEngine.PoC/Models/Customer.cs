namespace RulesEngine.PoC.Models;

internal sealed record Customer(int Age, Guid Id, bool IsSupplier, decimal Money, string FirstName, string LastName, IEnumerable<Country> Countries);
