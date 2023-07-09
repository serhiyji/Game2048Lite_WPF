using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;

namespace Game2048Lite_WPF
{
    public class ViewModel : INotifyPropertyChanged
    {

        #region Props
        RelayCommand W;
        RelayCommand A;
        RelayCommand S;
        RelayCommand D;

        public ICommand WCmd => W;
        public ICommand ACmd => A;
        public ICommand SCmd => S;
        public ICommand DCmd => D;

        private int score;
        private int highScore;
        private int size;
        DispatcherTimer timer;
        private UniformGrid grid;
        private Matrix<int> matrix;
        private Matrix<Button> matrix_out;
        Dictionary<string, Color> colors;
        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; this.OnPropertyChanged(); this.RestartGame(value); }
        }

        public int Score
        {
            get { return score; }
            set { score = value; HighScore = Math.Max(HighScore, Score); this.OnPropertyChanged(); }
        }
        public int HighScore
        {
            get { return highScore; }
            set { highScore = value; this.OnPropertyChanged(); }
        }
        #endregion

        #region Ctor
        public ViewModel(int size = 4)
        {
            W = new RelayCommand((e) => W_Clik(), (e) => this.IsPossibleNextCourse());
            A = new RelayCommand((e) => A_Clik(), (e) => this.IsPossibleNextCourse());
            S = new RelayCommand((e) => S_Clik(), (e) => this.IsPossibleNextCourse());
            D = new RelayCommand((e) => D_Clik(), (e) => this.IsPossibleNextCourse());
            this.size = size;

            /*timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0);
            timer.Start();*/
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            this.W_Clik();
            Thread.Sleep(50);
            this.A_Clik();
            Thread.Sleep(50);
            this.S_Clik();
            Thread.Sleep(50);
            this.D_Clik();
            Thread.Sleep(50);
        }

        public void Init()
        {
            this.Score = 0;
            this.HighScore = 0;
            this.matrix = new Matrix<int>(size);
            this.matrix_out = new Matrix<Button>(size);
            this.colors = new Dictionary<string, Color>();
            this.InitAllColors();
            this.GenerateGrid(size);
            this.GenerateNewPoint();
            this.UpdateGridInfo();
        }
        public void SetGrid(ref UniformGrid grid_)
        {
            this.grid = grid_;
        }
        private void InitAllColors()
        {
            this.colors.Add("0", Color.FromArgb(255, 204, 192, 179));
            this.colors.Add("2", Color.FromArgb(255, 238, 228, 218));
            this.colors.Add("4", Color.FromArgb(255, 237, 224, 200));
            this.colors.Add("8", Color.FromArgb(255, 242, 177, 121));
            this.colors.Add("16", Color.FromArgb(255, 245, 149, 99));
            this.colors.Add("32", Color.FromArgb(255, 246, 124, 95));
            this.colors.Add("64", Color.FromArgb(255, 246, 94, 59));
            this.colors.Add("128", Color.FromArgb(255, 237, 207, 114));
            this.colors.Add("256", Color.FromArgb(255, 237, 204, 97));
            this.colors.Add("512", Color.FromArgb(255, 237, 200, 80));
            this.colors.Add("1024", Color.FromArgb(255, 237, 197, 63));
            this.colors.Add("2048", Color.FromArgb(255, 237, 194, 46));
        }
        #endregion

        #region BtnClik
        public void W_Clik()
        {
            this.matrix.TransposeMain();
            // ...
            var res = HendlerSideLeft();
            Score += res.NewScore;
            this.matrix.TransposeMain();
            GenerateNewPoint();
            UpdateGridInfo();
        }
        public void A_Clik()
        {
            // _
            // ...
            var res = HendlerSideLeft();
            Score += res.NewScore;
            // _
            GenerateNewPoint();
            UpdateGridInfo();
        }
        public void S_Clik()
        {
            this.matrix.VerticalFlip();
            this.matrix.TransposeMain();
            // ...
            var res = HendlerSideLeft();
            Score += res.NewScore;
            this.matrix.TransposeMain();
            this.matrix.VerticalFlip();
            GenerateNewPoint();
            UpdateGridInfo();
        }
        public void D_Clik()
        {
            this.matrix.HorizontalFlip();
            // ...
            var res = HendlerSideLeft();
            Score += res.NewScore;
            this.matrix.HorizontalFlip();
            GenerateNewPoint();
            UpdateGridInfo();
        }
        #endregion

        #region HendlerGrids
        public void UpdateGridInfo()
        {
            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    Grid.SetRow(this.grid, i);
                    Grid.SetColumn(this.grid, j);
                    string content = matrix.matrix[i, j].ToString();
                    this.matrix_out.matrix[i, j].Content = matrix.matrix[i, j] == 0 ? "" : content;
                    this.matrix_out.matrix[i, j].Background = 
                        (this.colors.ContainsKey(this.matrix.matrix[i, j].ToString())) ? 
                        new SolidColorBrush(this.colors[this.matrix.matrix[i, j].ToString()]) : 
                        new SolidColorBrush(Color.FromRgb(222, 17, 17));
                    //this.matrix_out.matrix[i, j].LabelT.Content = (matrix.matrix[i, j] == 0) ? " " : matrix.matrix[i, j].ToString();
                    //this.grid.Children.OfType<Label>().First().Content = (matrix.matrix[i, j] == 0) ? " " : matrix.matrix[i, j].ToString();
                    //this.grid.Children.OfType<Label>().First().Background = new SolidColorBrush(this.colors[matrix.matrix[i, j].ToString()]);
                }
            }
        }
        public void GenerateGrid(int row_col)
        {
            this.grid.Rows = row_col;
            this.grid.Columns = row_col;
            this.grid.Children.Clear();
            for (int i = 0; i < row_col; i++)
            {
                for (int j = 0; j < row_col; j++)
                {
                    Grid.SetRow(this.grid, i);
                    Grid.SetColumn(this.grid, j);

                    Button button = new Button()
                    {
                        Content = "0",
                        Background = new SolidColorBrush(this.colors["0"]),
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,

                        FontSize = 20,
                        FontFamily = new FontFamily("Arial"),
                        Margin = new System.Windows.Thickness(5),
                        BorderBrush = new SolidColorBrush(Colors.White),
                        
                        
                    };

                    this.matrix_out.matrix[i, j] = button;
                    this.grid.Children.Add(button);
                }
            }
        }
        #endregion

        #region Logic
        public void RestartGame(int newsize)
        {
            Score = 0;
            this.size = newsize;
            this.matrix = new Matrix<int>(size);
            this.matrix_out = new Matrix<Button>(size);
            this.GenerateGrid(size);
            this.GenerateNewPoint();
            this.UpdateGridInfo();
        }
        public bool GenerateNewPoint()
        {
            PointMatrix pos = IsGenerateNewPoint();
            if (pos != null)
            {
                this.matrix.matrix[pos.i, pos.j] = (new Random().Next(1, 101) < 90) ? 2 : 4;
                return true;
            }
            return false;
        }
        public PointMatrix IsGenerateNewPoint() 
        {
            List<PointMatrix> positions = new List<PointMatrix>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (this.matrix.matrix[i, j] == 0)
                    {
                        positions.Add(new PointMatrix(i, j));
                    }
                }
            }
            return (positions.Count > 0) ? positions[new Random().Next(0, positions.Count)] : null;
        }
        public bool IsHeaderLines(Matrix<int> temp)
        {
            bool total = false;
            for (int i = 0; i < temp.Size; i++)
            {
                var res = this.HendlerLine(temp.matrix.GetRow(i));
                total = res.IsEdited || total;
            }
            return total;
        }
        public bool IsPossibleNextCourse()
        {
            bool total = false;
            total = IsGenerateNewPoint() != null || total;
            if (total) { return true; }
            Matrix<int> temp = new Matrix<int>(this.matrix.matrix, this.matrix.Size);
            total = IsHeaderLines(temp) || total;
            temp.HorizontalFlip();
            total = IsHeaderLines(temp) || total;
            temp.TransposeMain();
            total = IsHeaderLines(temp) || total;
            temp.VerticalFlip();
            total = IsHeaderLines(temp) || total;
            if (total == false)
            {
                MessageBox.Show("Game Over");
                this.RestartGame(size);
            }
            return total;
        }
        public (int NewScore, bool IsEdited) HendlerLine(int[] arr)
        {
            int NewScore = 0;
            bool IsEdited = false;
            arr.BiasArr();
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] == arr[i + 1])
                {
                    arr[i] = arr[i] + arr[i + 1];
                    arr[i + 1] = 0;
                    IsEdited = true;
                    NewScore += arr[i];
                    arr.BiasArr();
                }
            }
            return (NewScore, IsEdited);
        }
        public (int NewScore, bool IsEdited) HendlerSideLeft()
        {
            int NewScore = 0;
            bool IsEdited = false;
            for (int i = 0; i < this.matrix.Size; i++)
            {
                int[] arr = this.matrix.matrix.GetRow(i);
                var res = this.HendlerLine(arr);
                if (res.IsEdited)
                {
                    Score += res.NewScore;
                }
                for (int j = 0; j < arr.Length; j++)
                {
                    this.matrix.matrix[i, j] = arr[j];
                }
            }
            return (NewScore, IsEdited);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
