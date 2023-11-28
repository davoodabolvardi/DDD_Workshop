using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;

namespace DomainTests;

public class MoneySpecs
{
    static T A<T>(Func<T, T>? customization = null)
    {
        var t = new Fixture().Create<T>();
        if (null != customization)
            t = customization(t);
        return t;
    }

    Money aValidMoney() => A<Money>(with => new Money(Math.Abs(with.Value)));

    // class MoneyDto
    // {
    //     public decimal Amount { get; set; }
    //     public string Currency1 { get; set; }
    //     public string Currency2 { get; set; }
    //     public string Currency3 { get; set; }
    //     public string Currency4 { get; set; }
    //     public string Currency5 { get; set; }
    //     public string Currency6 { get; set; }
    //     public string Currency7 { get; set; }
    //     public string Currency8 { get; set; }
    // }

    void x()
    {
        // var money = A<MoneyDto>.But(with => new MoneyDto
        // {
        //     Amount = Math.Abs(with.Amount)
        // });
    }

    [Theory, AutoData]
    public void Money_cannot_be_negative(decimal amount)
    => new Action(() =>               //Arrange
       new Money(-Math.Abs(amount))   //Act
       ).Should().Throw<Exception>(); //Assert

    [Theory, AutoData]
    public void Supports_subtraction(uint five)
    {
        //Arrange
        var smallerNumber = aValidMoney();
        var biggerNumber = new Money(smallerNumber.Value + five);

        //Act
        (biggerNumber - smallerNumber)

        //Assert
        .Value.Should().Be(five);
    }

    [Theory, AutoData]
    public void Supports_plural(uint randomValue)
    {
        var firstMoney = aValidMoney();
        var biggerMoney = PluralFactory(randomValue, firstMoney.Value);
        var secondMoney = new Money(biggerMoney.Value - firstMoney.Value);

        (firstMoney + secondMoney)
            
        .Value.Should().Be(biggerMoney.Value);
    }

    [Theory, AutoData]
    public void Support_is_less_than_operator(uint random)
    {
        var smallerMoney = aValidMoney();
        var biggerMoney = new Money(smallerMoney.Value + random);

        (smallerMoney < biggerMoney)
            
        .Should().BeTrue();
    }

    [Theory, AutoData]
    public void Support_is_greater_than_operator(uint randomValue)
    {
        var smallerMoney = aValidMoney();
        var biggerMoney = new Money(smallerMoney.Value + randomValue);

        (biggerMoney > smallerMoney)
            
        .Should().BeTrue();
    }

    [Fact]
    public void Support_is_less_than_or_equal_to_operator()
    {
        var firstMoney = aValidMoney();
        var secondMoney = firstMoney;

        (firstMoney <= secondMoney)
            
        .Should().BeTrue();
    }

    [Fact]
    public void Support_is_greater_than_or_equal_to_operator()
    {
        var firstMoney = aValidMoney();
        var secondMoney = firstMoney;

        (firstMoney >= secondMoney)
            
        .Should().BeTrue();
    }

    private Money PluralFactory(uint randomValue, decimal tempMoneyValue)
    {
        if (randomValue < tempMoneyValue)
        {
            randomValue += Convert.ToUInt32(tempMoneyValue);
        }

        return new Money(randomValue);
    }
}