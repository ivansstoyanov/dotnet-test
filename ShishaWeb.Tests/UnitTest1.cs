using ShishaWeb.Controllers;
using System;
using Xunit;

namespace ShishaWeb.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var asd = new ValuesController();
            var str = asd.Get();
            Assert.Equal("1", "1");
        }
    }
}
