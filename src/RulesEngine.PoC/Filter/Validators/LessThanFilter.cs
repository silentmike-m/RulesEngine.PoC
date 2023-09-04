namespace RulesEngine.PoC.Filter.Validators;

using global::RulesEngine.PoC.Filter.Interfaces;

internal sealed class LessThanFilterValidator : IFilterValidator
{
    public bool Validate(string? value, string expectedValue, bool ignoreCase)
    {
        var equal = new EqualFilterValidator().Validate(value, expectedValue, ignoreCase);

        if (equal)
        {
            return false;
        }

        if (!decimal.TryParse(value, out var valueDecimal))
        {
            return false;
        }

        if (!decimal.TryParse(expectedValue, out var expectedValueDecimal))
        {
            return false;
        }

        return valueDecimal < expectedValueDecimal;
    }
}
