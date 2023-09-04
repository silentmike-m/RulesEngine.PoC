namespace RulesEngine.PoC.Filter.Validators;

using global::RulesEngine.PoC.Filter.Interfaces;

internal sealed class EqualFilterValidator : IFilterValidator
{
    public bool Validate(string? value, string expectedValue, bool ignoreCase)
        => string.Equals(value, expectedValue, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}
