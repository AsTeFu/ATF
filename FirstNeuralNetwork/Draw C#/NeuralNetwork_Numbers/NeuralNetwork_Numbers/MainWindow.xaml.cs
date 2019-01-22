using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace NeuralNetwork_Numbers {
    public partial class MainWindow : Window {
        NeuralNetwork n;

        public MainWindow() {
            InitializeComponent();
            
            mouseDown = false;

            CreateTable();

            int input_nodes = 784;
            int hidden_nodes = 200;
            int output_nodes = 10;
            double learning_rate = 0.01;

            n = new NeuralNetwork(input_nodes, hidden_nodes, output_nodes, learning_rate);
            
            using (TextFieldParser parser = new TextFieldParser(@"data\wih.csv")) {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                for (int i = 0; i < 200; i++) {
                    string[] fields = parser.ReadFields();

                    for (int j = 0; j < 784; j++) {
                        n.wih[i, j] = double.Parse(fields[j].Replace('.', ','));
                    }
                }
            }

            using (TextFieldParser parser = new TextFieldParser(@"data\who.csv")) {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                for (int i = 0; i < 10; i++) {
                    string[] fields = parser.ReadFields();

                    for (int j = 0; j < 200; j++) {
                        n.who[i, j] = double.Parse(fields[j].Replace('.', ','));
                    }
                }
            }


            //List<string> test_data_list = new List<string>();
            //using (StreamReader sr = new StreamReader(@"data/" + "test" + ".csv")) {
            //    string line;
            //    while ((line = sr.ReadLine()) != null) {
            //        test_data_list.Add(line);
            //    }
            //}

            //List<int> scorecard = new List<int>();
            //foreach (string record in test_data_list) {
            //    string[] all_values = record.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //    double[] inputs = calc(all_values);
            //    int correct = int.Parse(all_values[0]);

            //    int value = n.Query(inputs).argmax();

            //    if (value == correct)
            //        scorecard.Add(1);
            //    else scorecard.Add(0);
            //}

            //label2.Text = "Эффективность = " + (scorecard.Sum() * 1.0 / scorecard.Count) * 100 + "%";

        }

        private double[] calc(string[] str) {
            double[] data = new double[str.Length - 1];

            for (int i = 1; i < str.Length; i++) {
                data[i - 1] = (double.Parse(str[i]) / 255.0 * 0.99) + 0.01;
            }

            return data;
        }

        private Dictionary<Rectangle, bool> cells;

        private int size = 28;
        private Grid grid;
        private void CreateTable() {
            grid = new Grid();

            MouseDown += (sender, e) => mouseDown = true;
            MouseUp += (sender, e) => mouseDown = false;

            for (int i = 0; i < size; i++) {
                var row = new RowDefinition();
                var column = new ColumnDefinition();
                
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            cells = new Dictionary<Rectangle, bool>();

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    Rectangle cell = new Rectangle();

                    cell.Width = cell.Height = 280.0 / size;
                    cell.Fill = new SolidColorBrush(Colors.White);
                    
                    cell.MouseEnter += MouseDownEnter;
                    
                    cells.Add(cell, false);
                    grid.Children.Add(cell);
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                }
            }

            Table.Children.Add(grid); 
        }

        private bool mouseDown = false;
        private void MouseDownEnter(object sender, EventArgs e) {
            if (mouseDown) {
                Rectangle rectangle = (Rectangle)sender;
                cells[(Rectangle)sender] = true;
                rectangle.Fill = new SolidColorBrush(Colors.Black);
            }
        }
        private void ClearField(object sender, EventArgs e) {
            CreateTable();
        }

        private void GetNumber(object sender, EventArgs e) {
            double[] values = new double[size * size];

            int k = 0;
            foreach (Rectangle rectangle in cells.Keys) {
                values[k] = cells[rectangle] == true ? 0.99 : 0.01;
                k++;
            }
            
            int value = n.Query(values).argmax();

            label.Text = "Я вижу число: " + value;
        }
        
    }
}
