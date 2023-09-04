namespace RulesEngine.PoC.UnitTests;

using global::RulesEngine.PoC.Filter;
using global::RulesEngine.PoC.Filter.Interfaces;
using global::RulesEngine.PoC.Models;
using Xunit;

public sealed class FilterValidatorBuilderTests
{
    public static readonly IEnumerable<object[]> FILTER_LOGIC_OPERATORS
        = Enum.GetValues<FilterLogicOperator>()
            .Select(value => new[]
            {
                (object)value,
            });

    [Theory, MemberData(nameof(FILTER_LOGIC_OPERATORS))]
    public void GetValidator_Should_Return_Validator_For_Each_Filter_Logic_Operator(FilterLogicOperator logicOperator)
    {
        //GIVEN
        var builder = new FilterValidatorBuilder();

        //WHEN
        var validator = builder.GetValidator(logicOperator);

        //THEN
        Assert.IsAssignableFrom<IFilterValidator>(validator);
    }
}
