using Moq;
using SOM;
using SOM.NeuronNamespace;
using SOM.VectorNamespace;
using System;
using Xunit;

namespace SOMTests
{
    public class SOMapTests
    {
        [Fact]
        public void Constructor_DimensionsPassed_MatrixInitializerProperly()
        {
            var som = new SOMap(6, 4, 2, 100, 0.5);
            Assert.Equal(6, som._width);
            Assert.Equal(4, som._height);
            Assert.Equal(2, som._matrix.Rank);
            Assert.Equal(24, som._matrix.Length);
        }

        [Fact]
        public void GetNeuron_WidthOutOfRange_ExceptionThrown()
        {
            var som = new SOMap(5, 5, 2, 100, 0.5);
            Assert.Throws<ArgumentException>(() => som.GetNeuron(6, 5));
        }

        [Fact]
        public void GetNeuron_HeightOutOfRange_ExceptionThrown()
        {
            var som = new SOMap(5, 5, 2, 100, 0.5);
            Assert.Throws<ArgumentException>(() => som.GetNeuron(5, 6));
        }

        [Fact]
        public void GetNeuron_CorrectIndexes_Success()
        {
            var som = new SOMap(5, 5, 2, 100, 0.5);
            var neuron = som.GetNeuron(4, 4);
            Assert.NotNull(neuron);
        }

        [Fact]
        public void GetRadiusIndexes_CorrectBMURadiusPassed_Success()
        {
            var som = new SOMap(5, 5, 2, 100, 0.5);

            var bmuMock = new Mock<INeuron>();
            bmuMock.Setup(x => x.X).Returns(2);
            bmuMock.Setup(x => x.Y).Returns(2);

            (int xStart, int xEnd, int yStart, int yEnd) = som.GetRadiusIndexes(bmuMock.Object, 2);

            Assert.Equal(0, xStart);
            Assert.Equal(5, xEnd);
            Assert.Equal(0, yStart);
            Assert.Equal(5, yEnd);
        }

        [Fact]
        public void CalculateNeighborhoodRadius_ItterationTwo_Success()
        {
            var som = new SOMap(5, 5, 2, 100, 0.5);
            var radius = som.CalculateNeighborhoodRadius(2);

            Assert.Equal(1.97, Math.Round(radius, 2));
        }

        [Fact]
        public void GetDistanceDrop_DistanceTwoRadiusTwo_Success()
        {
            var som = new SOMap(5, 5, 2, 100, 0.5);
            var distanceDrop = som.GetDistanceDrop(2, 2);

            Assert.Equal(0.37, Math.Round(distanceDrop, 2));
        }

        [Fact]
        public void CalculateBMU_InputPassed_Success()
        {
            var som = new SOMap(2, 2, 2, 100, 0.5);
            var inputMock = new Mock<IVector>();
            inputMock.SetupSequence(x => x.EuclidianDistance(It.IsAny<IVector>()))
                .Returns(4)
                .Returns(3)
                .Returns(2)
                .Returns(1);

            var bmu = som.CalculateBMU(inputMock.Object);

            Assert.Equal(1, bmu.X);
            Assert.Equal(1, bmu.Y);
        }

        [Fact]
        public void Train_UseCase_Success()
        {
            var inputVector = new Vector();
            inputVector.Add(2);
            inputVector.Add(2);

            var input = new Vector[10]
            {
                inputVector,
                inputVector,
                inputVector,
                inputVector,
                inputVector,
                inputVector,
                inputVector,
                inputVector,
                inputVector,
                inputVector
            };
            

            var som = new SOMap(2, 2, inputVector.Count, 100, 0.5);
            som.Train(input);
        }
    }
}
