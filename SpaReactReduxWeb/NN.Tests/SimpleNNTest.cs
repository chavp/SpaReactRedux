using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NN.Tests
{
    [TestClass]
    public class SimpleNNTest
    {
        double[] weights = new double[] {
            0.3, 0.3, 0.3,
           // 0.3, 0.3, 0.3
        };
        double bias = 0.5;
        double lr = 0.8;// learning rate

        [TestInitialize]
        public void Setup()
        {
            var ran = new Random();
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = ran.NextDouble();
            }
        }

        /// <summary>
        /// basic step
        /// 1 - เอา input คูณกับ w
        /// 2 - เพิ่ม bias
        /// 3 - เอาผลลัพย์ที่ได้ ใส่เข้า activation function
        /// 4 - ส่งผลลัพย์ไปยัง layer ถัดไป
        /// </summary>
        [TestMethod]
        public void TestPerceptron()
        {
            var trainData = new List<(int, int, int)>()
            {
                (0, 0, 0),
                (0, 1, 0),
                (1, 0, 0),
                (1, 1, 1),
            };
            var testData = new List<(int, int, int)>()
            {
                (0, 0, 0),
                (0, 1, 0),
                (1, 0, 0),
                (1, 1, 1),
            };

            int i = 0;
            int limit = 100;
            var errors = new List<double>();
            var activation = Sigmoid;
            while (true)
            {
                foreach (var data in trainData)
                {
                    errors.Add(Train(data.Item1, data.Item2, data.Item3, activation));
                }
                var acc = Evaluate(testData, activation);
                if (acc == 1) break;
                ++i;
                if (i == limit) break;
            }

            // predict
            foreach (var test in testData)
            {
                Console.WriteLine($"{test.Item1} * {test.Item2} = {Predict(test.Item1, test.Item2, activation)}");
            }

            Console.Write($"(trains = {i}, [{string.Join(",", weights)}])");
        }

        decimal Evaluate(List<(int, int, int)> testData, Func<double, double> activation)
        {
            decimal accuracy = 0;
            var correct = 0;
            foreach (var test in testData)
            {
                var actual = Predict(test.Item1, test.Item2, activation);
                if (actual == test.Item3) ++correct;
            }
            accuracy = correct / testData.Count;
            return accuracy;
        }

        /// <summary>
        /// Perceptron
        /// </summary>
        public double Train(
            double input1, double input2,
            double output,
            Func<double, double> activation)
        {
            // net input function
            var outputP = Predict(input1, input2, activation);

            var error = output - outputP;

            weights[0] += error * input1 * lr;
            weights[1] += error * input2 * lr;

            //weights[2] += error * input1 * lr;
            //weights[3] += error * input2 * lr;

            weights[2] += error * bias * lr;
            //weights[5] += error * bias * lr;

            return error;
        }

        Func<double, double> Heaviside = (x) =>
        {
            //if (x > 0) //#activation function (here Heaviside)
            //    x = 1;
            //else
            //    x = 0;
            //return x;
            if (x > 1) return 3;
            if (x > 0) return 2;
            if (x > -1) return 1;
            return 0;
        };

        static double MAXRESULT = 1;
        Func<double, double> Sigmoid = (x) =>
        {
            var result = 1 / (1 + Math.Exp(-x));
            return Math.Round(result * MAXRESULT);
        };

        public double Predict(double input1, double input2,
            Func<double, double> activation)
        {
            //var outputP = input1 * weights[0] + input2 * weights[1] + bias * weights[2];
            //var o1 = outputP * weights[3];
            //var o2 = outputP * weights[4];
            //outputP = outputP + o1 + 02;
            var outputP = input1 * weights[0] + input2 * weights[1] + bias * weights[2];
            //outputP += input1 * weights[2] + input2 * weights[3] + bias * weights[5];

            outputP = activation(outputP);

            return outputP;
        }


        double SumSqrt(List<double> data)
        {
            return Math.Sqrt(data.Select(x => x * x).Sum());
        }

        double MSE(List<double> data)
        {
            return data.Select(x => x * x).Sum() / data.Count;
        }
    }
}
