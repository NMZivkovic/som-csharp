using Moq;
using SOM.NeuronNamespace;
using SOM.VectorNamespace;
using System;
using Xunit;

namespace SOMTests
{
    public class NeuronTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(9)]
        [InlineData(11)]
        public void Constructor_CreateNeuron_FiveWeightsAdded(int numOfWeights)
        {
            var neuron = new Neuron(numOfWeights);
            Assert.Equal(numOfWeights, neuron.Weights.Count);
        }

        [Fact]
        public void Distance_TwoNeurons_CorrectCallculation()
        {
            var neuron1 = new Neuron(11) { X = 3, Y = 3 };
            var neuron2 = new Neuron(3) { X = 1, Y = 1 };

            var distance = neuron1.Distance(neuron2);
            Assert.Equal(8, distance);
        }

        [Fact]
        public void SetWeight_WightValueFive_SuccessflySet()
        {
            var neuron = new Neuron(3);
            neuron.SetWeight(1, 5);
            Assert.Equal(5.0, neuron.Weights[1]);
        }

        [Fact]
        public void SetWeight_IndexOutOfRange_ExceptionThrown()
        {
            var neuron = new Neuron(3);
            Assert.Throws<ArgumentException>(() => neuron.SetWeight(11, 5));
        }

        [Fact]
        public void GetWeight_FirstWeight_Success()
        {
            var neuron = new Neuron(3);
            neuron.SetWeight(1, 5);
            var returnValue = neuron.GetWeight(1);
            Assert.Equal(5.0, returnValue);
        }

        [Fact]
        public void GetWeight_IndexOutOfRange_ExceptionThrown()
        {
            var neuron = new Neuron(3);
            Assert.Throws<ArgumentException>(() => neuron.GetWeight(11));
        }

        [Fact]
        public void UpdateWeights_InputMocked_CorrectResult()
        {
            var inputMock = new Mock<IVector>();
            inputMock.SetupGet(x => x[0]).Returns(1);
            inputMock.Setup(x => x.Count).Returns(1);

            var neuron = new Neuron(1);
            var initialWeightValue = neuron.GetWeight(0);

            neuron.UpdateWeights(inputMock.Object, 0.5, 0.5);

            Assert.Equal(initialWeightValue + (0.5 * 0.5 * (1 - initialWeightValue)), neuron.GetWeight(0));
        }
    }
}
