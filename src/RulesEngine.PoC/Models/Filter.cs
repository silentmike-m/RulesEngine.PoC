namespace RulesEngine.PoC.Models;

public sealed record Filter(string FieldName, string FieldValue, FilterLogicOperator LogicOperator);
