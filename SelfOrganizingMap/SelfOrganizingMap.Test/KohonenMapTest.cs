using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelfOrganizingMap.Networks;
using SelfOrganizingMap.Math;
using SelfOrganizingMap.Networks.Training;
using SelfOrganizingMap.Maps;
using SelfOrganizingMap.Utils;
using System.Diagnostics;
using System.Collections.Generic;

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
                    var vector = CreateRandom(2);
                    neuronLayer[i, j] = new Neuron(vector);
                };
            }

            _map = new KohonenMap(neuronLayer);
        }

        [TestMethod]
        public void Training()
        {
            KohonenMapConstants constants = new KohonenMapConstants(1.2, 0.4, 100, 100);
            KohonenMapTraining training = new KohonenMapTraining(_map, constants, 2);

            var patterns = new List<Vector<double>>()
            {
                new Vector<double>(new double[] { 1, 1}),
                new Vector<double>(new double[] { 1, 2}),
                new Vector<double>(new double[] { 2, 1}),
                new Vector<double>(new double[] { 4, 1}),
                new Vector<double>(new double[] { 5, 1}),
                new Vector<double>(new double[] { 5, 2}),
                new Vector<double>(new double[] { 4, 5}),
                new Vector<double>(new double[] { 5, 5}),
                new Vector<double>(new double[] { 5, 4}),
                new Vector<double>(new double[] { 1, 5}),
                new Vector<double>(new double[] { 1, 4}),
                new Vector<double>(new double[] { 2, 5}),
             };

            //training.TrainOnSetsNTimes(patterns, 100);
            training.TrainOnSetNTimes(patterns.ToArray(), 100);
            var length = _map.Map.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var point = _map.Map[i, j];
                    Debug.WriteLine($"{point.Weights[0]}-{point.Weights[1]}");
                }
            }
        }

        private Vector<double> CreateRandom(int count)
        {
            var array = new double[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (double)_random.Next(0, 6);
            }

            return new Vector<double>(array);
        }
    }
}
