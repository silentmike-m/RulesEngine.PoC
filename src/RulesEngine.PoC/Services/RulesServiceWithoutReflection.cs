namespace RulesEngine.PoC.Services;

using global::RulesEngine.Models;
using global::RulesEngine.PoC.Interfaces;
using global::RulesEngine.PoC.Models;

internal sealed class RulesServiceWithoutReflection : IFilterService
{
    private const string EQUAL_OPERATOR = "equal";
    private const string GREATER_THAN_OPERATOR = ">";
    private const string LESS_THAN_OPERATOR = "<";
    private const string OR_OPERATOR = "Or";

    public bool Validate(Message message, IEnumerable<Filter> filters)
    {
        var rules = CreateRules(filters);

        var workFlow = new Workflow
        {
            WorkflowName = "Test workflow",
            Rules = rules,
        };

        var engine = new RulesEngine(new[] { workFlow });

        var result = engine.ExecuteAllRulesAsync(workFlow.WorkflowName, message.Value)
            .Result;

        return result.TrueForAll(ruleResult => ruleResult.IsSuccess);
    }

    private static Rule CreateEqualRule(string fieldName, string expectedValue)
        => new()
        {
            Expression = $"input1.{fieldName}.Trim().ToLower() {EQUAL_OPERATOR} \"{expectedValue.Trim().ToLower()}\"",
            RuleName = $"Rule for {fieldName} and {expectedValue}",
        };

    private static Rule CreateGreaterThanRule(string fieldName, string expectedValue)
        => new()
        {
            Expression = $"input1.{fieldName} {GREATER_THAN_OPERATOR} \"{expectedValue}\"",
            RuleName = $"Rule for {fieldName} and {expectedValue}",
        };

    private static Rule CreateInRule(Filter filter)
    {
        var values = filter.FieldValue.Split(",");

        var childRules = values
            .Select(value => CreateEqualRule(filter.FieldName, value))
            .ToList();

        return new Rule
        {
            RuleName = $"Rule for {filter.FieldName}",
            Operator = OR_OPERATOR,
            Rules = childRules,
        };
    }

    private static Rule CreateLessThanRule(string fieldName, string expectedValue)
        => new()
        {
            Expression = $"input1.{fieldName} {LESS_THAN_OPERATOR} \"{expectedValue}\"",
            RuleName = $"Rule for {fieldName} and {expectedValue}",
        };

    private static IEnumerable<Rule> CreateRules(IEnumerable<Filter> filters)
    {
        var rules = new List<Rule>();

        foreach (var filter in filters)
        {
            var rule = filter.LogicOperator switch
            {
                FilterLogicOperator.Equals => CreateEqualRule(filter.FieldName, filter.FieldValue),
                FilterLogicOperator.In => CreateInRule(filter),
                FilterLogicOperator.GreaterThan => CreateGreaterThanRule(filter.FieldName, filter.FieldValue),
                FilterLogicOperator.LessThan => CreateLessThanRule(filter.FieldName, filter.FieldValue),
                _ => throw new ArgumentOutOfRangeException(),
            };

            rules.Add(rule);
        }

        return rules;
    }
}
