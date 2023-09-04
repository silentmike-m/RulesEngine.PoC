namespace RulesEngine.PoC.Services;

using global::RulesEngine.PoC.Filter;
using global::RulesEngine.PoC.Interfaces;
using global::RulesEngine.PoC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal sealed class JsonService : IFilterService
{
    private readonly FilterValidatorBuilder builder = new();

    public bool Validate(Message message, IEnumerable<Filter> filters)
    {
        var json = JsonConvert.SerializeObject(message);

        var jsonToken = JObject.Parse(json)["Value"];

        if (jsonToken is null)
        {
            return false;
        }

        var result = filters.All(filter => this.ValidateJsonToken(jsonToken, filter.FieldName, filter.FieldValue.Trim(), filter.LogicOperator));

        return result;
    }

    private bool ValidateJsonToken(JToken jsonToken, string fieldName, string expectedValue, FilterLogicOperator logicOperator)
    {
        while (true)
        {
            if (jsonToken is JArray jsonArray)
            {
                return jsonArray.All(jsonArrayItem => this.ValidateJsonToken(jsonArrayItem, fieldName, expectedValue, logicOperator));
            }

            var fieldNames = fieldName.Split('.');

            var jsonObject = (JObject)jsonToken;

            if (!jsonObject.TryGetValue(fieldNames[0], StringComparison.OrdinalIgnoreCase, out var value))
            {
                return false;
            }

            if (fieldNames.Length == 1)
            {
                return this.ValidateValue(value, expectedValue, logicOperator);
            }

            fieldName = string.Join(".", fieldNames.Skip(1));

            jsonToken = value;
        }
    }

    private bool ValidateValue(JToken jsonToken, string expectedValue, FilterLogicOperator logicOperator)
    {
        var value = jsonToken.Value<string>()?.Trim();

        var validator = this.builder.GetValidator(logicOperator);

        var result = validator.Validate(value, expectedValue, ignoreCase: true);

        return result;
    }
}
