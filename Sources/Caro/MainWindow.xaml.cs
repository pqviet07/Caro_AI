// 1712907 - Phùng Quốc Việt
// Game Caro

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace Caro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Cols { set; get; }
        public int Rows { set; get; }

        public bool isXTurn { set; get; }
        public int nCellsRemain { set; get; }
        private int[,] Board;
        private Button[,] matrixButton;
        public Point lastMove;

        private Process AI_Process;
        public MainWindow()
        {
            InitializeComponent();
            StartEngine();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isXTurn = true;
            lastMove = new Point();
            lastMove.X = -1;
            lastMove.Y = -1;
            Cols = 20;
            Rows = 20;
            nCellsRemain = Cols * Rows;
            Board = new int[Rows, Cols];
            matrixButton = new Button[Rows, Cols];
            const int btnWidth = 25;
            const int btnHeight = 25;

            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Cols; ++j)
                {
                    Board[i, j] = 0;
                    Button btn = new Button()
                    {                   
                        FontWeight = FontWeights.Bold
                    };

                    btn.Width = btnWidth;
                    btn.Height = btnHeight;
                    btn.Tag = new Tuple<int, int>(i, j);
                    btn.Click += Btn_Click;
                    btn.BorderThickness = new Thickness(0.8);
                    btn.Background = Brushes.Beige;
                    
                    matrixButton[i, j] = btn;
                    mainCanvas.Children.Add(btn);
                    Canvas.SetLeft(btn, j * btnWidth);
                    Canvas.SetTop(btn, i * btnHeight);
                }
            }

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var tuple = btn.Tag as Tuple<int, int>;
            int i = tuple.Item1;
            int j = tuple.Item2;
            bool isValid = ModelProcess(btn, i, j);

            if (radiobox2.IsChecked == true)
            {
                if (isValid)
                {
                    // lượt ENGINE đi
                    AI_Process.StandardInput.WriteLine($"turn {i},{j}");
                    while (true)
                    {
                        StringBuilder str = new StringBuilder(AI_Process.StandardOutput.ReadLine());
                        if (str[0] != 'M')
                        {
                            var tokens = str.ToString().Split(new char[] { ',' }, StringSplitOptions.None);
                            int iNew = int.Parse(tokens[0]);
                            int jNew = int.Parse(tokens[1]);

                            ModelProcess(matrixButton[iNew, jNew], iNew, jNew);
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nước đi ko hợp lệ !");
                }
            }
            else
            {

            }
        }

        // Xử lí thắng / thua / hòa
        // Hiển thị lên UI
        // Thay đổi dữ liệu bên dưới
        private bool ModelProcess(Button btn, int i, int j)
        {
            if (Board[i, j] == 0)
            {
                if (lastMove.X != -1)
                {
                    matrixButton[(int)lastMove.X, (int)lastMove.Y].Background = Brushes.Beige;
                }
                --nCellsRemain;
                lastMove.X = i;
                lastMove.Y = j;
                if (isXTurn)
                {
                    btn.Content = "X";
                    
                    btn.Foreground = Brushes.DarkGreen;
                    isXTurn = false;
                    Board[i, j] = 1;
                }
                else
                {
                    btn.Content = "O";
                    btn.Foreground = Brushes.Red;
                    isXTurn = true;
                    Board[i, j] = 2;
                }
                btn.Background = Brushes.Yellow;
                if (checkWin(i, j))
                {
                    MessageBoxResult res;
                    if (isXTurn)
                        res = MessageBox.Show("O Win");
                    else
                        res = MessageBox.Show("X Win");
                    btn.Background = Brushes.Beige;
                    resetBoard();
                }


                if (nCellsRemain == 0)
                {
                    MessageBox.Show("Draw!");
                    resetBoard();
                }
               
                return true;
            }
            return false;
        }
        private void resetBoard()
        {
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Cols; ++j)
                {
                    Board[i, j] = 0;
                    matrixButton[i, j].Content = "";
                }
            }
            lastMove.X = -1;
            lastMove.Y = -1;
            AI_Process.Kill();
            StartEngine();
        }
        private bool checkWin(int i, int j)
        {
            int valueOfPreviousTurn = 0;
            // Nếu isXTurn==true  ==> nước đi trước đó là O  ==> value =2
            if (isXTurn)
            {
                valueOfPreviousTurn = 2;
            }
            else
            {
                valueOfPreviousTurn = 1;
            }
            int count = 1;
            int x = i;
            int y = j;

            // Thứ tự hướng duyệt:
            // 1. dọc : xuống, lên
            // 2. ngang: xuống, lên 
            // 3. chéo chính: xuống, lên 
            // 4. chéo phụ: xuống , lên
            int[] dX = { 0, 0, 1, -1, 1, -1, -1, 1 };
            int[] dY = { 1, -1, 0, 0, 1, -1, 1, -1 };

            // k= 0,1  --> duyệt dọc
            // k= 2,3  --> duyệt ngang
            // k= 4,5  --> duyệt chéo chính
            // k= 6,7  --> duyệt chéo phụ
            for (int k = 0; k < dX.Length; ++k)
            {
                // k chẵn thì reset biến count
                // ví dụ k= 0; k= 1 thì vẫn là duyệt trên 1 cột nên count giữ nguyên để phía dưới cộng dồn
                if ((k & 1) == 0)
                {
                    count = 1;
                }

                while (isValidCord(x + dX[k], y + dY[k]) && (Board[x += dX[k], y += dY[k]] == valueOfPreviousTurn))
                {
                    ++count;
                    if (count == 5)
                    {
                        return true;
                    }
                }
                // đặt lại giá trị ban đầu để duyệt theo hướng khác
                x = i;
                y = j;
            }
            return false;
        }
        private bool isValidCord(int x, int y)
        {
            if (x < 0 || x >= Rows || y < 0 || y >= Cols)
                return false;
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";
            var writer = new StreamWriter(filename);
            writer.WriteLine(isXTurn ? "X" : "O");

            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Cols; ++j)
                {

                    writer.Write($"{Board[i, j]}");
                    if (j != Cols - 1)
                    {
                        writer.Write(" ");
                    }
                }
                writer.WriteLine("");
            }
            writer.Close();
            MessageBox.Show("Game saved!");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.InitialDirectory = Environment.CurrentDirectory;
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;
                var reader = new StreamReader(filename);
                String turn = reader.ReadLine();
                if (turn == "X")
                {
                    isXTurn = true;
                }
                else
                {
                    isXTurn = false;
                }

                for (int i = 0; i < Rows; ++i)
                {
                    var tokens = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.None);
                    for (int j = 0; j < tokens.Length; ++j)
                    {
                        if (tokens[j] == "1")
                        {
                            Board[i, j] = 1;
                            matrixButton[i, j].Content = "X";
                        }
                        else if (tokens[j] == "2")
                        {
                            Board[i, j] = 2;
                            matrixButton[i, j].Content = "O";
                        }
                        else
                        {
                            Board[i, j] = 0;
                            matrixButton[i, j].Content = "";
                        }
                    }
                }
            }
        }

        // Nhúng Engine vào GUI
        private void StartEngine()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            string filename = Environment.CurrentDirectory + @"\caroAI.exe";
            processInfo.FileName = filename;

            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardInput = true;

            AI_Process = Process.Start(processInfo);
            AI_Process.StandardInput.WriteLine("info timeout_turn 200");
            AI_Process.StandardInput.WriteLine("start 20");
            string str = AI_Process.StandardOutput.ReadLine();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AI_Process.Kill();
        }
    }
}
