namespace RulesEngine.PoC.Filter.Validators;

using global::RulesEngine.PoC.Filter.Interfaces;

internal sealed class InFilterValidator : IFilterValidator
{
    public bool Validate(string? value, string expectedValue, bool ignoreCase)
    {
        var expectedValues = expectedValue.Split(',').Select(s => s.Trim());

        var result = expectedValues.Contains(value, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

        return result;
    }
}
