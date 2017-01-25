using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelfOrganizingMap.Networks;
using SelfOrganizingMap.Math;
using SelfOrganizingMap.Networks.Training;
using SelfOrganizingMap.Maps;
using SelfOrganizingMap.Utils;

namespace SelfOrganizingMap.Test
{
    [TestClass]
    public class KohonenMapTest
    {
        static Random _random = new Random();
        KohonenMap _map;

        [TestInitialize]
        public void Init()
        {
            var count = 2;
            var neuronLayer = new Neuron[count, count];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var vector = CreateRandom(count);
                    neuronLayer[i, j] = new Neuron(vector);
                };
            }

            _map = new KohonenMap(neuronLayer);
        }

        [TestMethod]
        public void Training()
        {
            var count = 2;
            KohonenMapConstants constants = new KohonenMapConstants(1, count, 0.5, 1.25);
            KohonenMapTraining training = new KohonenMapTraining(_map, constants);

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
