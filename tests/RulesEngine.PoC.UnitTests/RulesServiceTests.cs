namespace RulesEngine.PoC.UnitTests;

using global::RulesEngine.PoC.Interfaces;
using global::RulesEngine.PoC.Models;
using global::RulesEngine.PoC.Services;
using Xunit;

public sealed record RulesServiceTests
{
    private static readonly Message MESSAGE_WITH_CUSTOMER = new
    (
        new Customer(Age: 21, Guid.NewGuid(), IsSupplier: true, Money: 1000, "John", "Wick", new List<Country> { new(Population: 100, "Poland", new CountryCode("PL", Value: 1)) })
    );

    private static readonly Message MESSAGE_WITH_CUSTOMER_2 = new
    (
        new Customer2(Age: 21, Guid.NewGuid(), IsSupplier: true, Money: 1000, "John", "Wick", new Country(Population: 100, "Poland", new CountryCode("PL", Value: 1)))
    );

    public static readonly IEnumerable<object[]> SERVICES = new List<object[]>()
    {
        new object[] { new RulesServiceWithReflection(), MESSAGE_WITH_CUSTOMER },
        new object[] { new RulesServiceWithReflection(), MESSAGE_WITH_CUSTOMER_2 },
        new object[] { new JsonService(), MESSAGE_WITH_CUSTOMER },
        new object[] { new JsonService(), MESSAGE_WITH_CUSTOMER_2 },
        new object[] { new RulesServiceWithoutReflection(), MESSAGE_WITH_CUSTOMER },
        new object[] { new RulesServiceWithoutReflection(), MESSAGE_WITH_CUSTOMER_2 },
    };

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_Equal_Countries_Codes_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = message.Value is Customer
            ? new List<Filter>
            {
                new("countries.code.code", "pl", FilterLogicOperator.Equals),
            }
            : new List<Filter>
            {
                new("country.code.code", "pl", FilterLogicOperator.Equals),
            };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_Equal_Countries_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = message.Value is Customer
            ? new List<Filter>
            {
                new("countries.name", "Poland", FilterLogicOperator.Equals),
            }
            : new List<Filter>
            {
                new("country.name", "Poland", FilterLogicOperator.Equals),
            };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_Equal_Customer_Age_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("age", "21", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_Equal_FirstName_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = message.Value is Customer
            ? new List<Filter>
            {
                new("firstname", "john", FilterLogicOperator.Equals),
            }
            : new List<Filter>
            {
                new(nameof(Customer.FirstName), "john", FilterLogicOperator.Equals),
            };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_Equal_IsSupplier_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("issupplier", "true", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_Equal_Money_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("money", "1000", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_GreaterThan_Customer_Age_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("age", "20", FilterLogicOperator.GreaterThan),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_In_Countries_Code_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = message.Value is Customer
            ? new List<Filter>
            {
                new("countries.code.code", "poland, pl", FilterLogicOperator.In),
            }
            : new List<Filter>
            {
                new("country.code.code", "poland, pl", FilterLogicOperator.In),
            };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_In_Countries_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = message.Value is Customer
            ? new List<Filter>
            {
                new("countries.name", "poland, pl", FilterLogicOperator.In),
            }
            : new List<Filter>
            {
                new("country.name", "poland, pl", FilterLogicOperator.In),
            };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_In_Customer_Age_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("age", "21, 22, 23", FilterLogicOperator.In),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_In_FirstName_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = message.Value is Customer
            ? new List<Filter>
            {
                new("firstname", "john, jan", FilterLogicOperator.In),
            }
            : new List<Filter>
            {
                new(nameof(Customer.FirstName), "john, jan", FilterLogicOperator.In),
            };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_In_IsSupplier_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("issupplier", "true", FilterLogicOperator.In),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_In_Money_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("money", "1000, 2000, 3000", FilterLogicOperator.In),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }

    [Theory, MemberData(nameof(SERVICES))]
    public void Should_Return_True_On_LessThan_Customer_Age_Filters(IFilterService filterService, Message message)
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("age", "22", FilterLogicOperator.LessThan),
        };

        //WHEN
        var result = filterService.Validate(message, filters);

        //THEN
        Assert.True(result);
    }
}
