using System;
using SOM.NeuronNamespace;
using SOM.VectorNamespace;

namespace SOM
{
    public class SOMap
    {
        internal INeuron[,] _matrix;
        internal int _height;
        internal int _width;
        internal double _matrixRadius;
        internal double _numberOfIterations;
        internal double _timeConstant;
        internal double _learningRate;

        public SOMap(int width, int height, int inputDimension, int numberOfIterations, double learningRate)
        {
            _width = width;
            _height = height;
            _matrix = new INeuron[_width, _height];
            _numberOfIterations = numberOfIterations;
            _learningRate = learningRate;

            _matrixRadius = Math.Max(_width, _height) / 2;
            _timeConstant = _numberOfIterations / Math.Log(_matrixRadius);

            InitializeConnections(inputDimension);
        }

        public void Train(Vector[] input)
        {
            int iteration = 0;
            var learningRate = _learningRate;

            while (iteration < _numberOfIterations)
            {
                var currentRadius = CalculateNeighborhoodRadius(iteration);

                for (int i = 0; i < input.Length; i++)
                {
                    var currentInput = input[i];
                    var bmu = CalculateBMU(currentInput);

                    (int xStart, int xEnd, int yStart, int yEnd) = GetRadiusIndexes(bmu, currentRadius);

                    for (int x = xStart; x < xEnd; x++)
                    {
                        for (int y = yStart; y < yEnd; y++)
                        {
                            var processingNeuron = GetNeuron(x, y);
                            var distance = bmu.Distance(processingNeuron);
                            if (distance <= Math.Pow(currentRadius, 2.0))
                            {
                                var distanceDrop = GetDistanceDrop(distance, currentRadius);
                                processingNeuron.UpdateWeights(currentInput, learningRate, distanceDrop);
                            }
                        }
                    }
                }
                iteration++;
                learningRate = _learningRate * Math.Exp(-(double)iteration / _numberOfIterations);
            }
        }

        internal (int xStart, int xEnd, int yStart, int yEnd) GetRadiusIndexes(INeuron bmu, double currentRadius)
        {
            var xStart = (int)(bmu.X - currentRadius - 1);
            xStart = (xStart < 0) ? 0 : xStart;

            var xEnd = (int)(xStart + (currentRadius * 2) + 1);
            if (xEnd > _width) xEnd = _width;

            var yStart = (int)(bmu.Y - currentRadius - 1);
            yStart = (yStart < 0) ? 0 : yStart;

            var yEnd = (int)(yStart + (currentRadius * 2) + 1);
            if (yEnd > _height) yEnd = _height;

            return (xStart, xEnd, yStart, yEnd);
        }

        internal INeuron GetNeuron(int indexX, int indexY)
        {
            if (indexX > _width || indexY > _height)
                throw new ArgumentException("Wrong index!");

            return _matrix[indexX, indexY];
        }

        internal double CalculateNeighborhoodRadius(double itteration)
        {
            return _matrixRadius * Math.Exp(-itteration/_timeConstant);
        }

        internal double GetDistanceDrop(double distance, double radius)
        {
            return Math.Exp(-(Math.Pow(distance, 2.0) / Math.Pow(radius, 2.0)));
        }

        internal INeuron CalculateBMU(IVector input)
        {
            INeuron bmu = _matrix[0, 0];
            double bestDist = input.EuclidianDistance(bmu.Weights);

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    var distance = input.EuclidianDistance(_matrix[i, j].Weights);
                    if( distance < bestDist)
                    {
                        bmu = _matrix[i, j];
                        bestDist = distance;
                    }
                }
            }

            return bmu;
        }

        private void InitializeConnections(int inputDimension)
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _matrix[i, j] = new SOM.NeuronNamespace.Neuron(inputDimension) { X = i, Y = j };
                }
            }
        }
    }
}
