namespace RulesEngine.PoC.Filter.Interfaces;

using global::RulesEngine.PoC.Models;

internal interface IFilterValidatorBuilder
{
    IFilterValidator GetValidator(FilterLogicOperator logicOperator);
}
