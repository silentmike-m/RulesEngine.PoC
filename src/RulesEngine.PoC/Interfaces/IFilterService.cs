namespace RulesEngine.PoC.Interfaces;

using global::RulesEngine.PoC.Models;

public interface IFilterService
{
    bool Validate(Message message, IEnumerable<Filter> filters);
}
