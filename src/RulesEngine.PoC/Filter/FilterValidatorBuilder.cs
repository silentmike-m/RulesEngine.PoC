namespace RulesEngine.PoC.Filter;

using global::RulesEngine.PoC.Filter.Interfaces;
using global::RulesEngine.PoC.Filter.Validators;
using global::RulesEngine.PoC.Models;

internal sealed class FilterValidatorBuilder : IFilterValidatorBuilder
{
    public IFilterValidator GetValidator(FilterLogicOperator logicOperator)
        => logicOperator switch
        {
            FilterLogicOperator.Equals => new EqualFilterValidator(),
            FilterLogicOperator.GreaterThan => new GreaterThanFilterValidator(),
            FilterLogicOperator.In => new InFilterValidator(),
            FilterLogicOperator.LessThan => new LessThanFilterValidator(),
            _ => throw new ArgumentOutOfRangeException(nameof(logicOperator), logicOperator, message: null),
        };
}
