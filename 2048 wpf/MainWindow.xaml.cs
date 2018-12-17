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

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            margin = 40 * (1.0 / sizeField);

            cellSize = ((gridWidth - margin * 2) - margin * sizeField) / sizeField;
            fontSize = 180 * (1.0 / sizeField);

            CreateGamefield();

            Start();
            
        }

        private Dictionary<int, Color> colors = new Dictionary<int, Color> {
            { 2, Color.FromRgb(239, 229, 219) },
            { 4, Color.FromRgb(239, 227, 207) },
            { 8, Color.FromRgb(244, 179, 127) },
            { 16, Color.FromRgb(247, 151, 106) },
            { 32, Color.FromRgb(248, 127, 104) },
            { 64, Color.FromRgb(248, 97, 69) },
            { 128, Color.FromRgb(239, 214, 142) },
            { 256, Color.FromRgb(238, 199, 78) },
            { 512, Color.FromRgb(238, 195, 55) },
            { 1024, Color.FromRgb(241, 191, 39) },
            { 2048, Color.FromRgb(238, 205, 95) },
        };
        
        private double cellSpawnDuration = 0.2;
        private double moveCellDuration = 0.05;
        private double cellSizeDuration = 0.2;
        private double cellColorDuration = 0.1;
        private double cellSizeDelay = 0.03;

        private double newCellSize = 1.1;

        private int[,] field;
        private Canvas[,] cells;

        Random rnd = new Random();


        private enum Direction {
            Right, Left, Up, Down
        }

        private int canvasWidth = 400;
        private int canvasHeight = 400;
        private SolidColorBrush bakcgroundColor = new SolidColorBrush(Color.FromRgb(165, 148, 130));
        private int radiusBackground = 5;
        private int sizeField = 5;

        private double margin = 10;
        Grid gameField;
        private int gridWidth = 400;
        private int gridHeight = 400;

        private double fontSize = 45;
        private double cellSize = 85;
        private SolidColorBrush cellColor = new SolidColorBrush(Color.FromRgb(190, 176, 160));
        private int radiusCell = 5;

        private Rectangle backgroundField;
        private int score = 0;
        private int Score {
            set {
                score = value;
                scoreLabel.Text = value.ToString();
            } 
            get {
                return score;
            }
        }

        private int[,] lastField;
        private int lastScore;

        public void Start() {
            cells = new Canvas[sizeField, sizeField];
            lastField = GetClearField();

            field = GetClearField();
            
            NewCell();
            NewCell();

            CopyTable(ref lastField, field);

            score = 0;
        }

        private void NewGame(object sender, EventArgs e) {
            gameField.Children.RemoveRange(sizeField * sizeField, gameField.Children.Count - sizeField * sizeField);

            Start();
        }
        private void LastStep(object sender, EventArgs e) {
            gameField.Children.RemoveRange(sizeField * sizeField, gameField.Children.Count - sizeField * sizeField);
            cells = new Canvas[sizeField, sizeField];

            CopyTable(ref field, lastField);
            Score = lastScore;

            for (int i = 0; i < sizeField; i++) {
                for (int j = 0; j < sizeField; j++) {
                    if (field[i, j] != 1) {
                        NewCell(i, j, field[i, j]);
                    }
                }
            }
        }
        
        private void CreateGamefield() {
            KeyUp += InputPlayer;

            Canvas canvas = new Canvas();
            canvas.Width = canvasWidth;
            canvas.Height = canvasHeight;
            
            backgroundField = new Rectangle();
            backgroundField.Height = canvasHeight;
            backgroundField.Width = canvasWidth;
            backgroundField.Fill = bakcgroundColor;
            backgroundField.RadiusX = backgroundField.RadiusY = radiusBackground;

            gameField = new Grid();
            gameField.Width = gridWidth - margin * 2;
            gameField.Height = gridHeight - margin * 2;
            gameField.Margin = new Thickness(margin);
            for (int i = 0; i < sizeField; i++) {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1.0 / sizeField, GridUnitType.Star);

                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1.0 / sizeField, GridUnitType.Star);

                gameField.RowDefinitions.Add(row);
                gameField.ColumnDefinitions.Add(column);
            }
            
            for (int i = 0; i < sizeField; i++) {
                for (int j = 0; j < sizeField; j++) {
                    Rectangle cell = new Rectangle();

                    cell.Width = cell.Height = cellSize;
                    cell.Fill = cellColor;
                    cell.RadiusX = cell.RadiusY = radiusCell;

                    //cell.Margin = new Thickness(j == 0 ? 10 : 5, i == 0 ? 10 : 5, j == sizeField - 1 ? 10 : 5, i == sizeField - 1 ? 10 : 5);

                    gameField.Children.Add(cell);
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);

                    //cells[i, j] = cell;
                }
            }

            main.Children.Add(canvas);
            canvas.Children.Add(backgroundField);
            canvas.Children.Add(gameField);
        }

        private void InputPlayer(object sender, KeyEventArgs e) {
            CopyTable(ref lastField, field);

            if (e.Key == Key.Up || e.Key == Key.W)
                SwipeUp();
            else if (e.Key == Key.Down || e.Key == Key.S)
                SwipeDown();
            else if (e.Key == Key.Left || e.Key == Key.A)
                SwipeLeft();
            else if (e.Key == Key.Right || e.Key == Key.D) 
                SwipeRight();
        }

        private void CopyTable(ref int[,] copyTo, int[,] copyFrom) {
            for (int i = 0; i < sizeField; i++) {
                for (int j = 0; j < sizeField; j++) {
                    copyTo[i, j] = copyFrom[i, j];
                }
            }
        }
        
        private void SwipeDown() {
            bool isMove = false;

            for (int col = 0; col < sizeField; col++) {
                int pivot = sizeField - 1, row = pivot - 1;

                while (row >= 0) {
                    if (field[row, col] == 1) {
                        row--;
                    } else if (field[pivot, col] == 1) {
                        field[pivot, col] = field[row, col];
                        cells[pivot, col] = cells[row, col];

                        TranslateCell(Direction.Down, row, pivot, cells[row, col], false);
                        
                        field[row, col] = 1;
                        cells[row--, col] = null;
                        
                        isMove = true;

                    } else if (field[pivot, col] == field[row, col]) {
                        field[pivot, col] += field[row, col];
                        
                        TranslateCell(Direction.Down, row, pivot, cells[row, col], true);
                        AddingCell(cells[pivot, col], field[pivot--, col]);
                        
                        field[row, col] = 1;
                        cells[row--, col] = null;

                        isMove = true;
                    } else if (--pivot == row)
                        row--;
                }
            }

            if (isMove) NextStep();
        }
        private void SwipeUp() {
            bool isMove = false;

            for(int col = 0; col < sizeField; col++) { 
                int pivot = 0, row = pivot + 1;

                while (row < sizeField) {
                    if (field[row, col] == 1) {
                        row++;
                    } 
                    else if (field[pivot, col] == 1) {
                        field[pivot, col] = field[row, col];
                        cells[pivot, col] = cells[row, col];

                        TranslateCell(Direction.Up, row, pivot, cells[row, col], false);
                        
                        field[row, col] = 1;
                        cells[row++, col] = null;

                        isMove = true;
                    } 
                    else if (field[pivot, col] == field[row, col]) {
                        field[pivot, col] += field[row, col];

                        TranslateCell(Direction.Up, row, pivot, cells[row, col], true);
                        AddingCell(cells[pivot, col], field[pivot++, col]);

                        field[row, col] = 1;
                        cells[row++, col] = null;

                        isMove = true;
                    } 
                    else if (++pivot == row)
                        row++;
                }
            }

            if (isMove) NextStep();
        }
        private void SwipeRight() {
            bool isMove = false;

            for (int row = 0; row < sizeField; row++) {
                int pivot = sizeField - 1, col = pivot - 1;

                while (col >= 0) {
                    if (field[row, col] == 1) {
                        col--;
                    } else if (field[row, pivot] == 1) {
                        field[row, pivot] = field[row, col];
                        cells[row, pivot] = cells[row, col];

                        TranslateCell(Direction.Right, col, pivot, cells[row, col], false);

                        cells[row, col] = null;
                        field[row, col--] = 1;

                        isMove = true;
                    } else if (field[row, pivot] == field[row, col]) {
                        field[row, pivot] += field[row, col];

                        TranslateCell(Direction.Right, col, pivot, cells[row, col], true);
                        AddingCell(cells[row, pivot], field[row, pivot--]);

                        cells[row, col] = null;
                        field[row, col--] = 1;

                        isMove = true;
                    } else if (--pivot == col)
                        col--;
                }
            }

            if (isMove) NextStep();
        }
        private void SwipeLeft() {
            bool isMove = false;

            for (int row = 0; row < sizeField; row++) {
                int pivot = 0, col = pivot + 1;

                while (col < sizeField) {
                    if (field[row, col] == 1) {
                        col++;
                    } else if (field[row, pivot] == 1) {
                        field[row, pivot] = field[row, col];
                        cells[row, pivot] = cells[row, col];

                        TranslateCell(Direction.Left, col, pivot, cells[row, col], false);

                        cells[row, col] = null;
                        field[row, col++] = 1;

                        isMove = true;
                    } else if (field[row, pivot] == field[row, col]) {
                        field[row, pivot] += field[row, col];

                        TranslateCell(Direction.Left, col, pivot, cells[row, col], true);
                        AddingCell(cells[row, pivot], field[row, pivot++]);

                        cells[row, col] = null;
                        field[row, col++] = 1; ;

                        isMove = true;
                    } else if (++pivot == col)
                        col++;
                }
            }

            if (isMove) NextStep();
        }

        private void ShowTable() {
            string tmp = "";
            for (int i = 0; i < sizeField; i++) {
                for (int j = 0; j < sizeField; j++) {
                    tmp += field[i,j] + "\t";
                }
                tmp += "\n";
            }
            MessageBox.Show(tmp);
        }

        private void NextStep() {
            if (!isEndGame()) {
                NewCell();
            }
        }

        private bool isEndGame() {
            for (int i = 0; i < sizeField; i++) {
                for (int j = 0; j < sizeField; j++) {
                    if (field[i, j] == 1)
                        return false;
                }
            }
            return true;
        }

        private void NewCell(int x = -1, int y = -1, int value = -1) {
            if (x == -1 || y == -1)
                (x, y) = GetRandomEmptyCell();

            int lower = value == -1 ? rnd.Next(0, 100) > 80 ? 4 : 2 : value;
            field[x,y] = lower;
            
            Canvas cnv = new Canvas();
            cnv.Width = cnv.Height = 70;

            TextBlock text = new TextBlock();
            text.Text = lower.ToString();
            text.TextAlignment = TextAlignment.Center;
            text.Height = text.Width = cellSize;
            text.FontSize = fontSize;
            text.FontWeight = FontWeights.Bold;
            text.Foreground = new SolidColorBrush(Color.FromRgb(77, 77, 77));
            text.Margin = new Thickness(0, 10, 0, 0);

            Rectangle newCell = new Rectangle();
            newCell.Width = cellSize;
            newCell.Height = cellSize;
            newCell.Fill = new SolidColorBrush(colors[lower]);
            newCell.RadiusX = 5;
            newCell.RadiusY = 5;
            
            cnv.Children.Add(newCell);
            cnv.Children.Add(text);
            gameField.Children.Add(cnv);
            Grid.SetRow(cnv, x);
            Grid.SetColumn(cnv, y);

            cells[x, y] = cnv;
            
            AnimateNewCell(cnv);
        }

        private void ChengeColor(Canvas cell, int newValue) {
            ColorAnimation colorAnimation = new ColorAnimation();
            SolidColorBrush brush = new SolidColorBrush(colors[2]);

            colorAnimation.From = colors[newValue / 2];
            colorAnimation.To = colors[newValue];
            colorAnimation.Duration = TimeSpan.FromSeconds(cellColorDuration);
            colorAnimation.FillBehavior = FillBehavior.HoldEnd;

            ((Rectangle)(cell.Children[0])).Fill = brush;

            //MessageBox.Show("Ну я хз");

            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
        private void TranslateCell(Direction dir, int startPos, int endPos, Canvas cell, bool delete) {
            bool isVertical = dir == Direction.Down || dir == Direction.Up;

            DoubleAnimation anim = new DoubleAnimation();
            TranslateTransform transform = new TranslateTransform(0, 0);
            cell.RenderTransform = transform;

            anim.From = 0;
            anim.To = (cellSize + margin) * (endPos - startPos);
            anim.Duration = TimeSpan.FromSeconds(moveCellDuration);
            anim.FillBehavior = FillBehavior.Stop;

            void AnimationMove_Completed(object sender, EventArgs e) {
                if (delete) {
                    string tmp = cell.Name;
                    gameField.Children.Remove(cell);
                    cell = null;
                }

                else if (!isVertical)
                    Grid.SetColumn(cell, endPos);
                else Grid.SetRow(cell, endPos);

            }
            anim.Completed += AnimationMove_Completed;

            transform.BeginAnimation(isVertical ? TranslateTransform.YProperty : TranslateTransform.XProperty, anim);
        }
        private void AddingCell(Canvas cell, int value) {
            AnimateSizeCell((Rectangle)cell.Children[0], cell.Width * newCellSize, cellSizeDuration, true, cellSizeDelay);
            AnimateSizeCell(cell, cell.Width * newCellSize, cellSizeDuration, true, cellSizeDelay);

            lastScore = Score;
            Score += value;

            ChengeColor(cell, value);

            ((TextBlock)cell.Children[1]).Text = value.ToString();

            if (value > 4) {
                ((TextBlock)cell.Children[1]).Foreground = Brushes.White;
            }
        }


        //Привязать размер канваса к тексту и ячейке
        private void AnimateSizeCell(TextBlock cell, double newSize, double duration, bool reverse, double beginTime) {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();

            widthAnimation.From = cell.ActualWidth;
            widthAnimation.To = newSize;
            widthAnimation.Duration = TimeSpan.FromSeconds(duration);
            widthAnimation.AutoReverse = reverse;
            widthAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);

            heightAnimation.From = cell.ActualHeight;
            heightAnimation.To = newSize;
            heightAnimation.Duration = TimeSpan.FromSeconds(duration);
            heightAnimation.AutoReverse = reverse;
            heightAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);

            cell.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            cell.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }
        private void AnimateSizeCell(Rectangle cell, double newSize, double duration, bool reverse, double beginTime) {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();

            widthAnimation.From = cell.ActualWidth;
            widthAnimation.To = newSize;
            widthAnimation.Duration = TimeSpan.FromSeconds(duration);
            widthAnimation.AutoReverse = reverse;
            widthAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);

            heightAnimation.From = cell.ActualHeight;
            heightAnimation.To = newSize;
            heightAnimation.Duration = TimeSpan.FromSeconds(duration);
            heightAnimation.AutoReverse = reverse;
            heightAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);

            cell.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            cell.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }
        private void AnimateSizeCell(Canvas cell, double newSize, double duration, bool reverse, double beginTime) {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();

            widthAnimation.From = cell.ActualWidth;
            widthAnimation.To = newSize;
            widthAnimation.Duration = TimeSpan.FromSeconds(duration);
            widthAnimation.AutoReverse = reverse;
            widthAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);

            heightAnimation.From = cell.ActualHeight;
            heightAnimation.To = newSize;
            heightAnimation.Duration = TimeSpan.FromSeconds(duration);
            heightAnimation.AutoReverse = reverse;
            heightAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);

            cell.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            cell.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }
        private void AnimateNewCell(Canvas newCell) {
            AnimateSizeCell(newCell, cellSize, cellSpawnDuration, false, 0);
            AnimateSizeCell((Rectangle)newCell.Children[0], cellSize, cellSpawnDuration, false, 0);
            AnimateSizeCell((TextBlock)newCell.Children[1], cellSize, cellSpawnDuration, false, 0);
        }
        private (int, int) GetRandomEmptyCell() {
            (int, int) coordinate;
            do {
                coordinate = (rnd.Next(4), rnd.Next(4));
            } while (field[coordinate.Item1, coordinate.Item2] != 1);

            return coordinate;
        }

        private int[,] GetClearField() {
            int[,] field = new int[sizeField, sizeField];
            
            for (int i = 0; i < sizeField; i++) {
                for (int j = 0; j < sizeField; j++) {
                    field[i, j] = 1;
                }
            }

            return field;
        }


        //UI

        private void mouseEnterATF(object sender, EventArgs e) {
            ATF.Foreground = new SolidColorBrush(Color.FromRgb(250, 170, 170));
        }
        private void mouseLeaveATF(object sender, EventArgs e) {
            ATF.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
