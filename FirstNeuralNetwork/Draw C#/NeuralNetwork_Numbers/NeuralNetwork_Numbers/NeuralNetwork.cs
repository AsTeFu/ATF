using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NumSharp.Core;
using NumSharp.Generic;

namespace NeuralNetwork_Numbers {
    class NeuralNetwork {

        private int inputNodes, hiddenNodes, outputNodes;
        private double learningRate;

        private delegate double[,] ActivationFunc(NDArray x);
        private ActivationFunc activationFunc;
        
        public NDArray wih;
        public NDArray who;
        
        public NeuralNetwork(int inputNodes, int hiddenNodes, int outputNodes, double learningRate) {
            //Колво узлов
            this.inputNodes = inputNodes;
            this.hiddenNodes = hiddenNodes;
            this.outputNodes = outputNodes;

            //Коэффицент обучения
            this.learningRate = learningRate;

            //Матрицы весовых коэффицентов
            wih = np.random.normal(0.0, Math.Pow(hiddenNodes, -0.5), hiddenNodes, inputNodes);
            who = np.random.normal(0.0, Math.Pow(outputNodes, -0.5), outputNodes, hiddenNodes);

            //Функция активация: здесь сигмоида 1 / (1 + e ^ -x)

            activationFunc = x => {
                double[,] arr = new double[x.shape[0], x.shape[1]];
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++) {
                        arr[i, j] = 1.0 / (1 + Math.Exp(-1.0 * (double)x[i, j]));
                    }
                return arr;
            };
        }

        public NDArray Query(double[] inputList) {
            NDArray inputs = np.array(inputList, ndim: 2);

            //Входящие сигналы для скрытого слоя
            NDArray hidden_inputs = np.dot(wih, inputs);
            //Исходящий сигнал из скрытого слоя
            NDArray hidden_outputs = activationFunc(hidden_inputs);

            //Входящий сигнал в выходной слой
            NDArray final_inputs = np.dot(who, hidden_outputs);
            //Выходящий сигнал из выходного слоя
            NDArray final_outputs = activationFunc(final_inputs);
            
            return final_outputs.transpose();
        }
        public void Train(double[] inputsList, double[] targetsList) {
            NDArray inputs = np.asarray(inputsList, ndim: 2);
            NDArray targets = Trans(Trans(np.asarray(targetsList, ndim: 2)));

            //Входящие сигналы для скрытого слоя
            NDArray hidden_inputs = np.dot(wih, inputs);
            //Исходящий сигнал из скрытого слоя
            NDArray hidden_outputs = activationFunc(hidden_inputs);

            //Входящий сигнал в выходной слой
            NDArray final_inputs = np.dot(who, hidden_outputs);
            //Выходящий сигнал из выходного слоя
            NDArray final_outputs = activationFunc(final_inputs);

            //Ошибка = целевое значение - фактическое значение
            NDArray output_errors = targets - final_outputs;
            //Ошибки скрытого слоя
            NDArray hidden_errors = np.dot(Trans(who), output_errors);

            //Обновление весовых коэффицентов между скрытым слоем и выходным
            who += learningRate * np.dot((output_errors * final_outputs * (1.0 - final_outputs)), hidden_outputs.transpose());

            //Обновление весовых коэффицентов между входным слоем и скрытым
            wih += learningRate * np.dot((hidden_errors * hidden_outputs * (1.0 - hidden_outputs)), inputs.transpose());
        }

        private static double[,] NormalDistribution(double mathExpectation, double standardDeviation, (int, int) size) {
            double[,] array = new double[size.Item1, size.Item2];

            Random rnd = new Random();
            for (int i = 0; i < size.Item1; i++)
                for (int j = 0; j < size.Item2; j++) {
                    array[i, j] = normal(mathExpectation, standardDeviation);
                }

            
            double normal(double loc, double scale) {
                double s, x;
                do {
                    x = (rnd.NextDouble() * 2) - 1;
                    double y = (rnd.NextDouble() * 2) - 1;

                    s = x * x + y * y;
                } while (!(s > 0 && s <= 1));

                double z = x * Math.Sqrt((-2.0 * Math.Log(s)) / s);
                
                return loc + scale * z;
            }

            return array;
        }
        private static NDArray Trans(NDArray arr) {
            arr.reshape(arr.shape[0], arr.shape.Length > 1 ? arr.shape[1] : 1);
            double[,] newArr = new double[arr.shape[1], arr.shape[0]];
            
            for (int i = 0; i < arr.shape[1]; i++) {
                for (int j = 0; j < arr.shape[0]; j++) {
                    object a = newArr[i, j];
                    a = arr[j, i];
                    newArr[i, j] = (double)arr[j, i];
                }
            }

            return newArr;
        }
    }
}
