namespace RulesEngine.PoC.Services;

using System.Collections;
using System.Linq.Dynamic.Core;
using System.Reflection;
using global::RulesEngine.Models;
using global::RulesEngine.PoC.Interfaces;
using global::RulesEngine.PoC.Models;

internal sealed class RulesServiceWithReflection : IFilterService
{
    private const string EQUAL_OPERATOR = "equal";
    private const string GREATER_THAN_OPERATOR = ">";
    private const string LESS_THAN_OPERATOR = "<";
    private const string OR_OPERATOR = "Or";

    public bool Validate(Message message, IEnumerable<Filter> filters)
    {
        var result = filters.All(filter => Validate(message.Value, filter.FieldName, filter.FieldValue, filter.LogicOperator));

        return result;
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

    private static bool Validate(object value, string fieldName, string expectedValue, FilterLogicOperator logicOperator)
    {
        var fieldNames = fieldName.Split('.');

        if (fieldNames.Length == 1)
        {
            return ValidateValue(value, fieldName, expectedValue, logicOperator);
        }

        var p = value.GetType().GetProperties();

        var propertyInfo = value.GetType().GetProperty(fieldNames[0], BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo is null)
        {
            return false;
        }

        var propertyValue = propertyInfo.GetValue(value);

        if (propertyValue is null)
        {
            return false;
        }

        fieldName = string.Join(".", fieldNames.Skip(1));

        if (propertyValue is IEnumerable listPropertyValues)
        {
            return listPropertyValues.ToDynamicList().All(listPropertyValue => Validate(listPropertyValue, fieldName, expectedValue, logicOperator));
        }

        return Validate(propertyValue, fieldName, expectedValue, logicOperator);
    }

    private static bool ValidateValue(object value, string fieldName, string expectedValue, FilterLogicOperator logicOperator)
    {
        var rules = CreateRules(new List<Filter> { new(fieldName, expectedValue, logicOperator) });

        var workFlow = new Workflow
        {
            WorkflowName = "Test workflow",
            Rules = rules,
        };

        var engine = new RulesEngine(new[] { workFlow });

        if (value is IEnumerable enumerableValues)
        {
            foreach (var enumerableValue in enumerableValues)
            {
                var result = engine.ExecuteAllRulesAsync(workFlow.WorkflowName, enumerableValue)
                    .Result;

                if (result.TrueForAll(ruleResult => ruleResult.IsSuccess) is false)
                {
                    return false;
                }
            }
        }
        else
        {
            var result = engine.ExecuteAllRulesAsync(workFlow.WorkflowName, value)
                .Result;

            return result.TrueForAll(resultTree => resultTree.IsSuccess);
        }

        return true;
    }
}
