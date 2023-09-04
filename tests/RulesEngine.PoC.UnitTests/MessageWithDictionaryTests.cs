namespace RulesEngine.PoC.UnitTests;

using global::RulesEngine.PoC.Interfaces;
using global::RulesEngine.PoC.Models;
using global::RulesEngine.PoC.Services;
using Xunit;

public sealed class MessageWithDictionaryTests
{
    private static readonly Message MESSAGE = new
    (
        new Dictionary<string, object>
        {
            {
                "customer",
                new Customer(Age: 21, Guid.NewGuid(), IsSupplier: true, Money: 1000, "John", "Wick", new List<Country> { new(Population: 100, "Poland", new CountryCode("PL", Value: 1)) })
            },
        }
    );

    private readonly IFilterService filterService = new JsonService();

    [Fact]
    public void Should_Return_True_On_Equal_Countries_Codes_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.countries.code.code", "pl", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_Equal_Countries_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.countries.name", "Poland", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_Equal_Customer_Age_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.age", "21", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_Equal_FirstName_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.firstname", "john", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_Equal_IsSupplier_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.issupplier", "true", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_Equal_Money_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.money", "1000", FilterLogicOperator.Equals),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_GreaterThan_Customer_Age_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.age", "20", FilterLogicOperator.GreaterThan),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_In_Countries_Code_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.countries.code.code", "poland, pl", FilterLogicOperator.In),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_In_Countries_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.countries.name", "poland, pl", FilterLogicOperator.In),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_In_Customer_Age_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.age", "21, 22, 23", FilterLogicOperator.In),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_In_FirstName_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.firstname", "john, jan", FilterLogicOperator.In),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_In_IsSupplier_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.issupplier", "true", FilterLogicOperator.In),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_In_Money_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.money", "1000, 2000, 3000", FilterLogicOperator.In),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_True_On_LessThan_Customer_Age_Filters()
    {
        //GIVEN
        var filters = new List<Filter>
        {
            new("customer.age", "22", FilterLogicOperator.LessThan),
        };

        //WHEN
        var result = this.filterService.Validate(MESSAGE, filters);

        //THEN
        Assert.True(result);
    }
}
