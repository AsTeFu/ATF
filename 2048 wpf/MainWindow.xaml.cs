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
using System.Windows.Media.Animation;

namespace WpfApp1 {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            Start();
        }

        //Game

        private double cellSize = 85;
        private double cellSpawnDuration = 0.75;

        private int[][] field = new int[4][];

        Random rnd = new Random();

        public void Start() {

            field = GetClearField();
            
            NewCell();

            
        }

        private void InputPlayer(object sender, KeyEventArgs e) {

        }

        private void NextStep() {
            //Спавн ячейки
        }

        private void NewCell() {
            var (x, y) = (GetRandomEmptyCell();
            int lower = rnd.Next(0, 100) > 80 ? 4 : 2;
            field[x][y] = lower;

            Rectangle newCell = new Rectangle();
            newCell.Width = 1;
            newCell.Height = 1;
            newCell.Fill = lower == 2 ? Brushes.Yellow : Brushes.DarkRed;
            newCell.RadiusX = 5;
            newCell.RadiusY = 5;
            
            gameField.Children.Add(newCell);
            Grid.SetRow(newCell, x);
            Grid.SetColumn(newCell, y);
            
            AnimateNewCell(newCell);            
        }

        private void AnimateNewCell(Rectangle newCell) {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();

            widthAnimation.From = newCell.ActualWidth;
            widthAnimation.To = cellSize;
            widthAnimation.Duration = TimeSpan.FromSeconds(cellSpawnDuration);

            heightAnimation.From = newCell.ActualHeight;
            heightAnimation.To = cellSize;
            heightAnimation.Duration = TimeSpan.FromSeconds(cellSpawnDuration);

            newCell.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            newCell.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }
        private (int, int) GetRandomEmptyCell() {
            (int, int) coordinate;
            do {
                coordinate = (rnd.Next(4), rnd.Next(4));
            } while (field[coordinate.Item1][coordinate.Item2] != 1);

            return coordinate;
        }

        private int[][] GetClearField() {
            int[][] field = new int[4][];

            for (int i = 0; i < 4; i++) {
                field[i] = new int[4];
                for (int j = 0; j < 4; j++) {
                    field[i][j] = 1;
                }
            }

            return field;
        }
    }
}
