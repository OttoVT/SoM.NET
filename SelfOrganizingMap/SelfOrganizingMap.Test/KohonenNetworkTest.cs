using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelfOrganizingMap.Networks;
using SelfOrganizingMap.Math;
using SelfOrganizingMap.Networks.Training;

namespace SelfOrganizingMap.Test
{
    [TestClass]
    public class KohonenTest
    {
        static Random _random = new Random();
        KohonenNetwork<double> _network;

        [TestInitialize]
        public void Init()
        {
            var count = 2;
            var neuronLayer = new Neuron[count];
            for (int i = 0; i < count; i++)
            {
                var vector = CreateRandom(count);
                neuronLayer[i] = new Neuron(vector);
            }

            _network = new KohonenNetwork<double>(neuronLayer, double.MaxValue);
        }

        [TestMethod]
        public void Training()
        {
            KohonenNetworkTraining training = new KohonenNetworkTraining(_network);
            training.MinimalPriority = 0.5;
            training.TrainingSpeed = 0.01;

            var trainingSet = new Vector<double>[]
            {
                new Vector<double>(new double[] { 1, 4}),
                new Vector<double>(new double[] { 2, 3}),
                new Vector<double>(new double[] { 4, 1}),
                new Vector<double>(new double[] { 5, 2}),
            };

            training.TrainOnSetNTimes(trainingSet, 10000);
        }

        private Vector<double> CreateRandom(int count)
        {
            var array = new double[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (double)_random.Next(-100, 100);
            }

            return new Vector<double>(array);
        }
    }
}
