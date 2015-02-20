using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.IntegerSearchTests
{
    [TestFixture]
    internal class LessThanOrEqualTests : IDisposable
    {
        private readonly TestContext context = new TestContext();

        [Test]
        public void LessThanOrEqual_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => this.context.TestModels.Search(x => x.IntegerOne).LessThanOrEqual(10));
        }

        [Test]
        public void LessThanOrEqual_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = this.context.TestModels.Search(x => x.IntegerOne).LessThanOrEqual(50);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void LessThanOrEqual_CallWithValue_AllRecordsHaveLessThanOrEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.IntegerOne).LessThanOrEqual(3);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.IntegerOne <= 3));
        }

        [Test]
        public void LessThanOrEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .LessThanOrEqual(101);

            //Assert
            Assert.IsTrue(result.All(x => x.IntegerOne <= 101 || x.IntegerThree <= 101));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}