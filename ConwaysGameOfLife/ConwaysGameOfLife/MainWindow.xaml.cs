using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ConwaysGameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rectangle[,] rects;
        //private Stopwatch sw = new Stopwatch();
        //private int counter = 0;
        private SolidColorBrush aliveBrush = new SolidColorBrush(Colors.Yellow);
        private SolidColorBrush deadBrush = new SolidColorBrush(Colors.Gray);


        public MainWindow()
        {
            InitializeComponent();

            InitRectangles(200, 200, 1, ref deadBrush, ref aliveBrush);
            var runner = new System.Windows.Threading.DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 100) };
            runner.Tick += new EventHandler(Run);
            //sw.Start();
            runner.Start();
        }

        private void InitRectangles(int countInX, int countInY, int rectSize, ref SolidColorBrush asdf, ref SolidColorBrush hallo)
        {
            rects = new Rectangle[countInX, countInY];
            var rand = new Random();

            GameField.Width = (rectSize + 1) * countInX;
            GameField.Height = (rectSize + 1) * countInY;
            this.SizeToContent = SizeToContent.WidthAndHeight;

            //var test = GetRandomNumbersFromString();
            //var counter = 0;

            for (int x = 0; x < countInX; x++)
            {
                for (int y = 0; y < countInY; y++)
                {
                    GameField.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(rectSize + 1) });
                    GameField.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rectSize + 1) });

                    var rect = new Rectangle
                    {
                        Width = rectSize,
                        Height = rectSize,
                        //Fill = test[counter] == 1 ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Gray)
                        Fill = rand.Next(2) == 1 ? hallo : asdf
                    };
                    rect.MouseEnter += RectClick;


                    Grid.SetColumn(rect, y);
                    Grid.SetRow(rect, x);
                    GameField.Children.Add(rect);
                    rects[x, y] = rect;
                    //counter++;
                }
            }

        }

        private void RectClick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ((Rectangle)sender).Fill = aliveBrush;
        }


        private void Run(object sender, EventArgs e)
        {

            var counts = new short[rects.GetLength(0), rects.GetLength(1)];

            for (int x = 0; x < rects.GetLength(0); x++)
            {
                for (int y = 0; y < rects.GetLength(1); y++)
                {
                    if (rects[x, y].Fill.ToString() == Brushes.Yellow.ToString())
                    {
                        AddToRectangle(counts, x, y);
                    }
                }
            }
            ChangeColors(counts);
        }

        private void ChangeColors(short[,] counts)
        {
            for (int x = 0; x < counts.GetLength(0); x++)
            {
                for (int y = 0; y < counts.GetLength(1); y++)
                {
                    if (counts[x, y] == 3)
                    {
                        rects[x, y].Fill = aliveBrush;
                    }
                    else if (rects[x, y].Fill.ToString() == Brushes.Yellow.ToString() && (counts[x, y] <= 1 || counts[x, y] >= 4))
                    {
                        rects[x, y].Fill = deadBrush;
                    }
                }
            }
        }

        private void AddToRectangle(short[,] counts, int x, int y)
        {
            //              x-1,y-1;    y-1;        x+1,y+1;
            //              x-1;        Cell;       x+1;
            //              x-1,y+1;    y+1;        x+1,y+1;

            if (x - 1 >= 0)
            {
                counts[x - 1, y]++;
                if (y - 1 >= 0)
                    counts[x - 1, y - 1]++;

                if (y + 1 < counts.GetLength(1))
                    counts[x - 1, y + 1]++;
            }

            if (y - 1 >= 0)
                counts[x, y - 1]++;
            //cell
            if (y + 1 < counts.GetLength(1))
                counts[x, y + 1]++;

            if (x + 1 < counts.GetLength(0))
            {
                counts[x + 1, y]++;
                if (y - 1 >= 0)
                    counts[x + 1, y - 1]++;

                if (y + 1 < counts.GetLength(1))
                    counts[x + 1, y + 1]++;
            }
        }
    }
}
