using SOM.VectorNamespace;
using System;
using Xunit;

namespace SOMTests
{
    public class VectorTests
    {
        [Fact]
        public void EuclidianDistance_WrongSize_ExceptionThrown()
        {
            var vector1 = new Vector();
            var vector2 = new Vector();

            vector1.Add(6.0);
            Assert.Throws<ArgumentException>(() => vector1.EuclidianDistance(vector2));
        }

        [Fact]
        public void EuclidianDistance_GoodSize_GoodResult()
        {
            Vector vector1 = new Vector();
            Vector vector2 = new Vector();

            vector1.Add(3);
            vector1.Add(3);

            vector2.Add(1);
            vector2.Add(1);

            var returnValue = vector1.EuclidianDistance(vector2);

            Assert.Equal(8.0, returnValue);
        }
    }
}
