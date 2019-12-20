using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//telerik;devExpress
namespace _1712907
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartEngine();
        }
        private Process AI_Process;
        private const int startX = 30;
        private const int startY = 30;
        private static int sizeBoard = 20;
        private static int heightCell = 480 / sizeBoard;
        private Point lastMove;
        public bool isXTurn { set; get; }
        public int nCellsRemain { set; get; }
        private int[,] Board;
        private Image[,] matrixImage;
        private Rectangle lastmoveRect;
        private static int timeout_move = 100; // mili giây

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isXTurn = true;
            lastMove = new Point();
            lastMove.X = -1;
            lastMove.Y = -1;
            // số ô còn lại (0 ô = hòa)
            nCellsRemain = sizeBoard * sizeBoard;
            // Bàn cờ backend (0=null ; 1=X ; 2=O)
            Board = new int[sizeBoard, sizeBoard];
            matrixImage = new Image[sizeBoard, sizeBoard];
            for (int i = 0; i < sizeBoard; ++i)
            {
                for (int j = 0; j < sizeBoard; ++j)
                {
                    Board[i, j] = 0;
                }
            }

            // nước đánh cuối cùng có background màu vàng
            lastmoveRect = new Rectangle();
            lastmoveRect.Width = heightCell - 2;
            lastmoveRect.Height = heightCell - 2;
            lastmoveRect.Fill = Brushes.Yellow;

            // hình chữ nhật bên dưới lines (màu xám tro)
           // Rectangle bgRect = new Rectangle();
            bgRect.Width = sizeBoard * heightCell;
            bgRect.Height = sizeBoard * heightCell;
            //bgRect.Opacity = 0.5;

            bgRect.Fill = Brushes.Beige;
            //canvas.Children.Add(bgRect);
            Canvas.SetLeft(bgRect, startX);
            Canvas.SetTop(bgRect, startY);
            // vẽ các lines
            DrawCaroGrid();

        }

        private void DrawCaroGrid()
        {
            // sizBoard = 6 thì vẽ 5 đường ngang, 5 đường dọc 
            for (int i = 1; i < sizeBoard; ++i)
            {
                // tạo 1 vertical line
                var line1 = new Line();
                line1.StrokeThickness = 1;
                line1.Stroke = new SolidColorBrush(Colors.Black);
                canvas.Children.Add(line1);

                //  tạo 1 horizontal line
                var line2 = new Line();
                line2.StrokeThickness = 1;
                line2.Stroke = new SolidColorBrush(Colors.Black);
                canvas.Children.Add(line2);

                // vẽ 2 line
                DrawLine(line1, i, "Vertical");
                DrawLine(line2, i, "Horizontal");
            }
        }
        private void DrawLine(Line line, int order, String orientation)
        {
            if (orientation.Equals("Vertical"))
            {
                line.X1 = startX + heightCell * order;
                line.Y1 = startY;

                line.X2 = startX + heightCell * order;
                line.Y2 = startY + heightCell * sizeBoard;
            }
            else if (orientation.Equals("Horizontal"))
            {
                line.X1 = startX;
                line.Y1 = startY + heightCell * order;

                line.X2 = startX + heightCell * sizeBoard;
                line.Y2 = startY + heightCell * order;
            }
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
            if (x < 0 || x >= sizeBoard || y < 0 || y >= sizeBoard)
                return false;
            return true;
        }
        private bool isValidPos(double x, double y)
        {
            if (x < startX || x > startX + sizeBoard * heightCell ||
                y < startY || y > startX + sizeBoard * heightCell)
                return false;
            return true;
        }
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / heightCell;
            int j = ((int)position.X - startX) / heightCell;

            if (isValidPos(position.X, position.Y))
            {
                if (ModelProcess(i, j))
                {
                    // lượt ENGINE đi     
                    if (radiobox2.IsChecked == true)
                    {
                        radiobox1.IsEnabled = false;
                        EngineProcess();
                    }
                    else
                    {
                        radiobox2.IsChecked = false;
                        radiobox2.IsEnabled = false;
                    }
                }
            }
        }

        // Làm trung gian giữa Engine và GUI
        private void EngineProcess()
        {
            int i = (int)lastMove.X;
            int j = (int)lastMove.Y;
            // Ghi lên AI_Process đoạn command: turn i,j
            AI_Process.StandardInput.WriteLine($"turn {i},{j}");
            while (true)
            {
                // đọc tọa độ từ AI_Process trả ra
                StringBuilder str = new StringBuilder(AI_Process.StandardOutput.ReadLine());
                if (str[0] != 'M')
                {
                    var tokens = str.ToString().Split(new char[] { ',' }, StringSplitOptions.None);
                    // Tọa độ Engine trả ra: inew, jnew
                    int iNew = int.Parse(tokens[0]);
                    int jNew = int.Parse(tokens[1]);

                    // Xử lí bên UI và model:  check WIN, vẽ nước đi lên bàn cờ, cập nhật Board[,] ...
                    ModelProcess(iNew, jNew);
                    break;
                }
            }
        }

        // X == true thì Draw X, ngược lại Draw O
        private void DrawXO(int i, int j, bool XTurn, bool isLastMove)
        {
            var img = new Image();
            img.Width = (double)heightCell * 2 / 3;
            img.Height = (double)heightCell * 2 / 3;
            if (isLastMove)
            {
                canvas.Children.Add(lastmoveRect);
                Canvas.SetLeft(lastmoveRect, startX + j * heightCell + 1);
                Canvas.SetTop(lastmoveRect, startY + i * heightCell + 1);
            }
            if (XTurn)
            {
                img.Source = new BitmapImage(new Uri("X.png", UriKind.Relative));
            }
            else
            {
                img.Source = new BitmapImage(new Uri("O.png", UriKind.Relative));
            }
            canvas.Children.Add(img);
            matrixImage[i, j] = img;
            Canvas.SetLeft(img, startX + j * heightCell + heightCell / 6);
            Canvas.SetTop(img, startY + i * heightCell + heightCell / 6);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        // -------------
        private bool ModelProcess(int i, int j)
        {
            if (Board[i, j] == 0)
            {
                if (lastMove.X != -1)
                {
                    // xóa nền vàng ở dưới (đánh dấu nước đi cuối cùng)
                    canvas.Children.Remove(lastmoveRect);
                }
                --nCellsRemain;
                lastMove.X = i;
                lastMove.Y = j;

                DrawXO(i, j, isXTurn, true);
                if (isXTurn)
                {
                    isXTurn = false;
                    Board[i, j] = 1;
                }
                else
                {
                    isXTurn = true;
                    Board[i, j] = 2;
                }

                if (checkWin(i, j))
                {
                    MessageBoxResult res;
                    if (isXTurn)
                        res = MessageBox.Show("O Win");
                    else
                        res = MessageBox.Show("X Win");
                    // xóa nền vàng

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
            for (int i = 0; i < sizeBoard; ++i)
            {
                for (int j = 0; j < sizeBoard; ++j)
                {
                    if (Board[i, j] != 0)
                    {
                        Board[i, j] = 0;
                        canvas.Children.Remove(matrixImage[i, j]);
                    }
                }
            }
            canvas.Children.Remove(lastmoveRect);
            if (radiobox2.IsChecked == true)
            {
                radiobox1.IsEnabled = true;
            }
            else
            {
                radiobox2.IsEnabled = true;
            }


            lastMove.X = -1;
            lastMove.Y = -1;
            AI_Process.Kill();
            StartEngine();
        }


        // Run process Engine
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
            AI_Process.StandardInput.WriteLine($"info timeout_turn {timeout_move}");
            AI_Process.StandardInput.WriteLine($"start {sizeBoard}");
            string str = AI_Process.StandardOutput.ReadLine();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            AI_Process.Kill();

        }

        private void Canvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!isXTurn)
            {
                EngineProcess();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";
            var writer = new StreamWriter(filename);
            writer.WriteLine(isXTurn ? "X" : "O");

            for (int i = 0; i < sizeBoard; ++i)
            {
                for (int j = 0; j < sizeBoard; ++j)
                {

                    writer.Write($"{Board[i, j]}");
                    if (j != sizeBoard - 1)
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
                resetBoard();
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

                for (int i = 0; i < sizeBoard; ++i)
                {
                    var tokens = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.None);
                    for (int j = 0; j < tokens.Length; ++j)
                    {
                        if (tokens[j] == "1")
                        {
                            Board[i, j] = 1;
                            DrawXO(i, j, true, false);
                        }
                        else if (tokens[j] == "2")
                        {
                            Board[i, j] = 2;
                            DrawXO(i, j, false, false);
                        }
                        else
                        {
                            Board[i, j] = 0;
                        }
                    }
                }
            }
        }
    }
}
