using Nensure;
using Xunit;

namespace Njoy.Services
{
    public sealed class GooglePhoneNumberUtilUnitTests
    {
        [Fact]
        public void Can_Be_Constructed()
        {
            new GooglePhoneNumberUtility(PhoneNumbers.PhoneNumberUtil.GetInstance());
        }

        #region IsValid

        [Fact]
        public void IsValid_Throws_If_Null()
        {
            var util = Get();
            Assert.Throws<AssertionException>(() => util.IsValid(null));
        }

        [Fact]
        public void IsValid_Does_Not_Throw_If_Empty_Or_Whitespace()
        {
            Get().IsValid(string.Empty);
            Get().IsValid(" ");
        }

        [Fact]
        public void IsValid_Returns_False_If_Empty_Or_Whitespace()
        {
            Assert.False(Get().IsValid(string.Empty));
            Assert.False(Get().IsValid(" "));
        }

        [Theory]
        [InlineData("532550910", "TR")]
        [InlineData("3124551", "TR")]
        public void IsValid_Returns_False_If_Too_Short(string number, string region)
        {
            Assert.False(Get().IsValid(number, region));
        }

        [Theory]
        [InlineData("532550910123", "TR")]
        [InlineData("3124551001125", "TR")]
        public void IsValid_Returns_False_If_Too_Long(string number, string region)
        {
            Assert.False(Get().IsValid(number, region));
        }

        [Theory]
        [InlineData("+903122556789", "TR")]
        [InlineData("+90 3122556789", "TR")]
        [InlineData("903122556789", "TR")]
        [InlineData("03122556789", "TR")]
        [InlineData("3122556789", "TR")]
        [InlineData("312 255 67 89", "TR")]
        [InlineData("312255 67 89", "TR")]
        public void IsValid_Returns_True_With_Different_Formats(string number, string region)
        {
            Assert.True(Get().IsValid(number, region));
        }

        #endregion IsValid

        #region Format

        [Theory]
        [InlineData((string)null, "TR")]
        [InlineData("", "TR")]
        [InlineData("532550910123", "TR")]
        [InlineData("3124551001125", "TR")]
        [InlineData("25", "TR")]
        public void Format_Throws_If_Number_Is_Not_Valid(string number, string region)
        {
            Assert.Throws<AssertionException>(() => Get().Format(number, region));
        }

        [Theory]
        [InlineData("+903122556789", "TR")]
        [InlineData("+90 3122556789", "TR")]
        [InlineData("903122556789", "TR")]
        [InlineData("03122556789", "TR")]
        [InlineData("3122556789", "TR")]
        [InlineData("312 255 67 89", "TR")]
        [InlineData("312255 67 89", "TR")]
        public void Format_Returns_Full_Formatted_Without_Spaces(string number, string region)
        {
            Assert.Equal("+903122556789", Get().Format(number, region));
        }

        #endregion Format

        private GooglePhoneNumberUtility Get()
        {
            return new GooglePhoneNumberUtility(PhoneNumbers.PhoneNumberUtil.GetInstance());
        }
    }
}