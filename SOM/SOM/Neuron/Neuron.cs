using SOM.VectorNamespace;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SOMTests")]

namespace SOM.NeuronNamespace
{
    public class Neuron : INeuron
    {
        public int X { get; set; }
        public int Y { get; set; }
        public IVector Weights { get; }

        public Neuron(int numOfWeights)
        {
            var random = new Random();
            Weights = new Vector();

            for (int i = 0; i < numOfWeights; i ++)
            {
                Weights.Add(random.NextDouble());
            }
        }

        public double Distance(INeuron neuron)
        {
            return Math.Pow((X - neuron.X), 2) + Math.Pow((Y - neuron.Y), 2);
        }

        public void SetWeight(int index, double value)
        {
            if (index >= Weights.Count)
                throw new ArgumentException("Wrong index!");

            Weights[index] = value;
        }

        public double GetWeight(int index)
        {
            if (index >= Weights.Count)
                throw new ArgumentException("Wrong index!");

            return Weights[index];
        }

        public void UpdateWeights(IVector input, double distanceDecay, double learningRate)
        {
            if(input.Count != Weights.Count)
                throw new ArgumentException("Wrong input!");

            for(int i = 0; i < Weights.Count; i++)
            {
                Weights[i] += distanceDecay * learningRate * (input[i] - Weights[i]);
            }

        }
    }
}
