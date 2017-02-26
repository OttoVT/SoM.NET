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
            KohonenMapConstants constants = new KohonenMapConstants(2, 0.1, 100, 100);
            KohonenMapTraining training = new KohonenMapTraining(_map, constants, 0.000001, true);

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
            training.FullTrain(patterns, 100);
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


        [TestMethod]
        public void TrainingV2()
        {
            var patterns = new List<List<double>>()
            {
                new List<double>(new List<double>() { 1, 1}),
                new List<double>(new List<double>() { 1, 2}),
                new List<double>(new List<double>() { 2, 1}),
                new List<double>(new List<double>() { 4, 1}),
                new List<double>(new List<double>() { 5, 1}),
                new List<double>(new List<double>() { 5, 2}),
                new List<double>(new List<double>() { 4, 5}),
                new List<double>(new List<double>() { 5, 5}),
                new List<double>(new List<double>() { 5, 4}),
                new List<double>(new List<double>() { 1, 5}),
                new List<double>(new List<double>() { 1, 4}),
                new List<double>(new List<double>() { 2, 5}),
             };


            Map mp = new Map(2, 2, patterns, 0.0001, 100, 100, 0, false, 2);

            double[,] array = mp.GetUMatrixMap();

            var length = array.GetLength(0);
            //for (int i = 0; i < length; i++)
            //{
            //    for (int j = 0; j < length; j++)
            //    {
            //        var point = array[i, j];
            //        Debug.WriteLine($"{point.Weights[0]}-{point.Weights[1]}");
            //    }
            //}
        }

        /*
         Map mp = new Map(Convert.ToInt32(neuron_count.Text), Convert.ToInt32(txt_neuron_count_y.Text), 
                textAnalyzer.document_vectors, Convert.ToDouble(target_error.Text), 
                Convert.ToInt32(target_epoch.Text), cur_err, curr_epoch, radioButton2, 
                richTextBox_log, WinnerSearchMode);
         */

        private Vector<double> CreateRandom(int count)
        {
            var array = new double[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = count * _random.NextDouble();
            }

            return new Vector<double>(array);
        }
    }
}
