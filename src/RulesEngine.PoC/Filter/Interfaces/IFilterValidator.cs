namespace RulesEngine.PoC.Filter.Interfaces;

internal interface IFilterValidator
{
    bool Validate(string? value, string expectedValue, bool ignoreCase);
}
