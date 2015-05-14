using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Media.Animation;
using System.Collections;
using System.Windows.Threading;
using System.Windows.Input;
using TransferSystem;

namespace demoSoftware
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        List<string> binaryList = new List<string>();
        List<string> cardList = new List<string>();
        private DispatcherTimer TheTimer = new DispatcherTimer();
        Image[] imgArray = new Image[10];
        Label[] labels = new Label[8];
        Rectangle LynxRect = new Rectangle();
        int pot = 0;
        bool flip = true;
        bool welcomeFlag = false;
        double xAxisSend = 0;
        double yAxisSend = 0;
        double xAxisRecieve = 0;
        double yAxisRecieve = 0;
        double xAxisTag = 0;
        double yAxisTag = 0;
        double x1AxisRecieveUpdated = 0;
        double y1AxisRecieveUpdated = 0;
        double orientation = 0;
        double flashDist = (18 * 2.22) + 39;
        String str = "";
        string command;
        string betAmount;
        List<double> temp = new List<double>();
        List<double> temp1 = new List<double>();
        List<char> binArray = new List<char>();
        List<TouchPoint> AllPoints = new List<TouchPoint>();
        int counter = 0;
        int xLoc = 0;
        private Line[] lineList = new Line[32];
        private Line[] lineList2 = new Line[32];
        GameController controller = new GameController();
        Player player, dealer;
        Label potLabel, chipLabel;

        int cardCounter = 0;
        //TransferManager transferManager;
        /// <summary>
        /// Default constructor.
        /// </summary>

        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
            Grid.SetZIndex(canvas, -1);
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            labels[0] = lbl1;
            labels[1] = lbl2;
            labels[2] = lbl3;
            labels[3] = lbl4;
            labels[4] = lbl5;
            labels[5] = lbl6;
            labels[6] = lbl7;
            labels[7] = lbl8;
            TheTimer.Tick += timer_Tick;
            TheTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            canvas.Background = new ImageBrush() { ImageSource = new BitmapImage((new Uri(@"C:\welcomeScreen.jpg", UriKind.Absolute))) };
            chipLabel = new Label();
            potLabel = new Label();

            Canvas.SetTop(chipLabel,200);
            Canvas.SetRight(chipLabel, 100);
            chipLabel.Height = 50;
            chipLabel.Width = 200;

            Canvas.SetTop(potLabel, 400);
            Canvas.SetRight(potLabel, 100);
            potLabel.Height = 50;
            potLabel.Width = 200;
            canvas.Children.Add(chipLabel);
            canvas.Children.Add(potLabel);
            //transferManager = new TransferManager(myTag, myGrid, 0);
            //transferManager.ReceivedData += onReceivedData;
          
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        #region WPFmethods

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">Event Arguments</param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        #endregion

        #region tagVisualization
        
        /// <summary>
        /// This function recognises tag, and gets relative x and y axis. It also gets orientation of the tag.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">TagVisualizerEvent Arguments</param>
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {

            welcomeFlag = true;
            if (welcomeFlag)
            {
                canvas.Background = new ImageBrush() { ImageSource = new BitmapImage((new Uri(@"C:\background.jpg", UriKind.Absolute))) };
                myGrid.Children.Remove(loadingImg);
            }
            textBox.Clear();

            LynxTagVisualization tag = (LynxTagVisualization)e.TagVisualization;
             
            orientation = tag.Orientation;
            xAxisTag = tag.Center.X;
            yAxisTag = tag.Center.Y;
            xAxisSend = xAxisTag - (flashDist + 10);
            yAxisSend = yAxisTag + ((40 * 2.22));
            x1AxisRecieveUpdated = xAxisTag + (flashDist - 40);
            y1AxisRecieveUpdated = yAxisTag + ((26 * 2.22));

            Canvas.SetLeft(sPanel, xAxisSend);
            Canvas.SetTop(sPanel, yAxisSend);
            LynxRect.Height = 465;
            LynxRect.Width =240;

            LynxRect.Fill = Brushes.Black;
            Canvas.SetZIndex(LynxRect, -1);
            Canvas.SetLeft(LynxRect, xAxisTag - 120);
            Canvas.SetTop(LynxRect, yAxisTag - 70);
            canvas.Children.Add(LynxRect);

            for (int i = 1; i < 9; i++)
            {
                drawboxes(x1AxisRecieveUpdated, y1AxisRecieveUpdated + (i * (18 * 2.22)),orientation);
            }

            Init();
            
        }

        private void Init(){
            pot = 0;
            controller.setNewDeck();
            controller.deal();
       
            player = controller.getPlayer();
            dealer = controller.getDealer();
            potLabel.Content = "Pot: ";
            chipLabel.Content = "Chips: $"+player.getWallet();
            textBox.Text = textBox.Text + player.getWallet();

            for (int i = 0; i < 2; i++)
            {
                string card = "";
               
                Image dealerCard = dealer.getHand()[i].getImage();
                Image playerCard = player.getHand()[i].getImage();

                Canvas.SetTop(dealerCard, 200);
                Canvas.SetLeft(dealerCard, 500+(i*20));

                Canvas.SetTop(playerCard, 700);
                Canvas.SetLeft(playerCard, 500 + (i*20));

                canvas.Children.Add(dealerCard);
                canvas.Children.Add(playerCard);
                xLoc = i;
            }
            player.calculateScore();
            dealer.calculateScore();
            if (player.getScore() == 21)
            {
                textBox.Text = "Player Wins";
                player.setWallet(pot + player.getWallet());
                player.reset();
                dealer.reset();
                ClearCards();
                Init();
            }
            else if (dealer.getScore() == 21)
            {
                textBox.Text = "Dealer Wins";
                player.reset();
                dealer.reset();
                ClearCards();
                Init();
            }
            else { 
            
            }
        }

        private void OnVisualizationMoved(object sender, TagVisualizerEventArgs e)
        {
            
            
            LynxTagVisualization tag = (LynxTagVisualization)e.TagVisualization;
            orientation = tag.Orientation;
            xAxisTag = tag.Center.X;
            yAxisTag = tag.Center.Y;
            xAxisSend = xAxisTag - (flashDist + 10);
            yAxisSend = yAxisTag + ((40 * 2.22));
            x1AxisRecieveUpdated = xAxisTag + (flashDist - 40);
            y1AxisRecieveUpdated = yAxisTag + ((26 * 2.22));

            LynxRect.Fill = Brushes.Black;
            Canvas.SetLeft(LynxRect, xAxisTag - 120);
            Canvas.SetTop(LynxRect, yAxisTag - 70);

            int intTotalChildren = myGrid.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter >= 0; intCounter--)
            {
                if (myGrid.Children[intCounter].GetType() == typeof(Line))
                {
                    Line ucCurrentChild = (Line)myGrid.Children[intCounter];
                    myGrid.Children.Remove(ucCurrentChild);
                }
            }
            Canvas.SetLeft(sPanel,xAxisSend);
            Canvas.SetTop(sPanel, yAxisSend);
            temp.Clear();
            temp1.Clear();
            for (int i = 1; i < 9; i++)
            {
                drawboxes(x1AxisRecieveUpdated, y1AxisRecieveUpdated + (i * (18 * 2.22)),orientation);
                //drawboxes(transferManager.lynx.xRecFirst, transferManager.lynx.yRecFirst + (i * (18 * 2.22)), transferManager.lynx.heading);
            }

            
           
        }

        private void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            canvas.Children.Remove(LynxRect);
            int intTotalChildren = myGrid.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter >= 0; intCounter--)
            {
                if (myGrid.Children[intCounter].GetType() == typeof(Line))
                {
                    Line ucCurrentChild = (Line)myGrid.Children[intCounter];
                    myGrid.Children.Remove(ucCurrentChild);
                }
            }

            ClearCards();


           
        }


        

        #endregion

        #region RecieveStuff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public string constructBin(List<TouchPoint> tp)
        {
            String[] bin = { "0", "0", "0", "0", "0", "0", "0", "0" };

            for (int i = 0; i < tp.Count; i++)
            {
                xAxisRecieve = tp[i].Position.X;
                yAxisRecieve = tp[i].Position.Y;
                if (xAxisRecieve > xAxisTag && xAxisRecieve < (xAxisTag + 200) &&
                yAxisRecieve > yAxisTag && yAxisRecieve < (yAxisTag + 1000))
                {
                    if (xAxisRecieve > lineList[0].X1 && xAxisRecieve < lineList[0].X2 &&
                       yAxisRecieve > temp[0 * 8] - 24 && yAxisRecieve < (temp[0 * 8]))
                    {
                        bin[0] = "1";
                    }

                    if (xAxisRecieve > lineList[4].X1 && xAxisRecieve < lineList[4].X2 &&
                        yAxisRecieve > temp[1 * 8] - 24 && yAxisRecieve < temp[1 * 8])
                    {
                        bin[1] = "1";
                    }

                    if (xAxisRecieve > lineList[8].X1 && xAxisRecieve < lineList[8].X2 &&
                        yAxisRecieve > temp[2 * 8] - 24 && yAxisRecieve < temp[2 * 8])
                    {
                        bin[2] = "1";
                    }

                    if (xAxisRecieve > lineList[12].X1 && xAxisRecieve < lineList[12].X2 &&
                        yAxisRecieve > temp[3 * 8] - 24 && yAxisRecieve < temp[3 * 8])
                    {
                        bin[3] = "1";
                    }

                    if (xAxisRecieve > lineList[16].X1 && xAxisRecieve < lineList[16].X2 &&
                       yAxisRecieve > temp[4 * 8] - 24 && yAxisRecieve < temp[4 * 8])
                    {
                        bin[4] = "1";
                    }

                    if (xAxisRecieve > lineList[20].X1 && xAxisRecieve < lineList[20].X2 &&
                        yAxisRecieve > temp[5 * 8] - 24 && yAxisRecieve < temp[5 * 8])
                    {
                        bin[5] = "1";
                    }

                    if (xAxisRecieve > lineList[24].X1 && xAxisRecieve < lineList[24].X2 &&
                       yAxisRecieve > temp[6 * 8] - 24 && yAxisRecieve < temp[6 * 8])
                    {
                        bin[6] = "1";
                    }

                    if (xAxisRecieve > lineList[28].X1 && xAxisRecieve < lineList[28].X2 &&
                       yAxisRecieve > temp[7 * 8] - 24 && yAxisRecieve < temp[7 * 8])
                    {
                        bin[7] = "1";
                    }

                    if (xAxisRecieve > lineList2[0].X1 && xAxisRecieve < lineList2[0].X2 &&
                       yAxisRecieve > temp1[0 * 8] - 24 && yAxisRecieve < (temp1[0 * 8] + 10))
                    {
                        bin[0] = "1";
                    }

                    if (xAxisRecieve > lineList2[4].X1 && xAxisRecieve < lineList2[4].X2 &&
                        yAxisRecieve > temp1[1 * 8] - 24 && yAxisRecieve < temp1[1 * 8])
                    {
                        binArray.Add('9');
                    }

                    if (xAxisRecieve > lineList2[8].X1 && xAxisRecieve < lineList2[8].X2 &&
                        yAxisRecieve > temp1[2 * 8] - 24 && yAxisRecieve < temp1[2 * 8])
                    {
                        //Console.WriteLine("ace");
                        binArray.Add('a');
                    }

                    if (xAxisRecieve > lineList2[12].X1 && xAxisRecieve < lineList2[12].X2 &&
                        yAxisRecieve > temp1[3 * 8] - 24 && yAxisRecieve < temp1[3 * 8])
                    {
                        //Console.WriteLine("b");
                        binArray.Add('b');
                    }

                    if (xAxisRecieve > lineList2[16].X1 && xAxisRecieve < lineList2[16].X2 &&
                       yAxisRecieve > temp1[4 * 8] - 24 && yAxisRecieve < temp1[4 * 8])
                    {
                        //Console.WriteLine("c");
                        binArray.Add('c');
                    }

                    if (xAxisRecieve > lineList2[20].X1 && xAxisRecieve < lineList2[20].X2 &&
                        yAxisRecieve > temp1[5 * 8] - 24 && yAxisRecieve < temp1[5 * 8])
                    {
                        //Console.WriteLine("d");
                        binArray.Add('d');
                    }

                    if (xAxisRecieve > lineList2[24].X1 && xAxisRecieve < lineList2[24].X2 &&
                       yAxisRecieve > temp1[6 * 8] - 24 && yAxisRecieve < temp1[6 * 8])
                    {
                        //Console.WriteLine("e");
                        binArray.Add('e');
                    }

                    if (xAxisRecieve > lineList2[28].X1 && xAxisRecieve < lineList2[28].X2 &&
                       yAxisRecieve > temp1[7 * 8] - 24 && yAxisRecieve < temp1[7 * 8])
                    {
                        //Console.WriteLine("f");
                        binArray.Add('f');
                    }

                }
            }
            string letter = "";
            for (int q = 0; q < bin.Length; q++)
            {
                letter = letter + bin[q];
            }
            var data = GetBytesFromBinaryString(letter);
            var text = Encoding.ASCII.GetString(data);
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            Console.WriteLine(e.GetTouchPoints(this.myGrid).Count);
           
                foreach (TouchPoint _touchPoint in e.GetTouchPoints(this.myGrid))
                {
                        int id = _touchPoint.TouchDevice.Id;
                        myGrid.InvalidateVisual();
                        if (!_touchPoint.TouchDevice.GetIsTagRecognized() && !_touchPoint.TouchDevice.GetIsFingerRecognized())
                        {

                            bool flag = false;
                            for (int i = 0; i < AllPoints.Count; i++)
                            {
                                if (AllPoints[i].TouchDevice.Id == id)
                                {
                                    flag = true;
                                }
                            }

                            if (!flag)
                            {
                                AllPoints.Add(_touchPoint);
                                flag = false;
                            }
                    }
                       
                
                }

            if (e.GetTouchPoints(this.myGrid).Count == 1 && AllPoints.Count>0)
            {
                //Game stuff recieve
                
            }
            
        }
        void onReceivedData(object sender, LynxReceivedArgs e)
        {
           
        }

        /// <summary>
        /// draws boxes for recieving bits
        /// </summary>
        /// <param name="x1Axis">start of box X Axis</param>
        /// <param name="y1Axis">start of box Y Axis</param>
        /// <param name="angle">Orientation Angle</param>
        public void drawboxes(double x1Axis, double y1Axis, double angle)
        {
            x1Axis = x1Axis - 13;
            y1Axis = y1Axis - 13;

            int boxSize = 26;
            /* -----------------------------------------------Rotational Stuff------------------------------------------
            double radians = angle * (Math.PI / 180);
            double negSin = -Math.Sin(radians);
            double posSin = Math.Sin(radians);
            double negCos = -Math.Cos(radians);
            double posCos = Math.Cos(radians);


            double[,] rotMatrix = new double[,] { { negSin, posCos }, { posCos, posSin } };
            double[,] point = new double[,]{{x1Axis},{y1Axis}};
            


            int rA = rotMatrix.GetLength(0);
            int cA = rotMatrix.GetLength(1);
            int rB = point.GetLength(0);
            int cB = point.GetLength(1);
            double temp = 0;
            double[,] solution = new double[rA, cB];

           
            if (cA != rB)
            {
                Console.WriteLine("matrix can't be multiplied !!");
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += rotMatrix[i, k] * point[k, j];
                        }
                        solution[i, j] = temp;
                    }

                }
            }
            Console.WriteLine("X: "+solution[0,0]);
            Console.WriteLine("Y: "+solution[1,0]);
       
            
            for (int w = 0; w < lineList.Length; w = w + 4)
            {
                lineList[w] = new Line();
                lineList[w].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w].X1 = solution[0, 0];
                lineList[w].X2 = solution[0, 0] + boxSize;
                lineList[w].Y1 = solution[1, 0];
                lineList[w].Y2 = solution[1, 0];
                lineList[w].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w]);

                lineList[w + 1] = new Line();
                lineList[w + 1].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w + 1].X1 = solution[0, 0];
                lineList[w + 1].X2 = solution[0, 0];
                lineList[w + 1].Y1 = solution[1, 0]; ;
                lineList[w + 1].Y2 = solution[1, 0] +boxSize;
                lineList[w + 1].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w + 1]);

                lineList[w + 2] = new Line();
                lineList[w + 2].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w + 2].X1 = solution[0, 0] + boxSize;
                lineList[w + 2].X2 = solution[0, 0] + boxSize;
                lineList[w + 2].Y1 = solution[1, 0]; ;
                lineList[w + 2].Y2 = solution[1, 0] +boxSize;
                lineList[w + 2].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w + 2]);

                lineList[w + 3] = new Line();
                lineList[w + 3].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w + 3].X1 = solution[0, 0];
                lineList[w + 3].X2 = solution[0, 0] + boxSize;
                lineList[w + 3].Y1 = solution[1, 0] +boxSize;
                lineList[w + 3].Y2 = solution[1, 0] +boxSize;
                lineList[w + 3].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w + 3]);
            }
            //-----------------------------------------------------------------------------------------------------*/
            
            for (int w = 0; w < lineList.Length; w=w+4)
            {
                //Horizontal top Line
                lineList[w] = new Line();
                lineList[w].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w].X1 = x1Axis;
                lineList[w].X2 = x1Axis + boxSize;
                lineList[w].Y1 = y1Axis;
                lineList[w].Y2 = y1Axis;
                lineList[w].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w]);


                //Vertical Left line
                lineList[w+1] = new Line();
                lineList[w+1].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w + 1].X1 = x1Axis;
                lineList[w + 1].X2 = x1Axis;
                lineList[w + 1].Y1 = y1Axis;
                lineList[w + 1].Y2 = y1Axis + boxSize;
                lineList[w + 1].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w + 1]);


                //Vertical right
                lineList[w+2] = new Line();
                lineList[w + 2].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w + 2].X1 = x1Axis + boxSize;
                lineList[w + 2].X2 = x1Axis + boxSize;
                lineList[w + 2].Y1 = y1Axis;
                lineList[w + 2].Y2 = y1Axis + boxSize;
                lineList[w + 2].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w + 2]);


                //Horizontal Bottom
                lineList[w+3] = new Line();
                lineList[w + 3].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList[w + 3].X1 = x1Axis;
                lineList[w + 3].X2 = x1Axis + boxSize;
                lineList[w + 3].Y1 = y1Axis + boxSize;
                lineList[w + 3].Y2 = y1Axis + boxSize;
                lineList[w + 3].StrokeThickness = 1;
                myGrid.Children.Add(lineList[w + 3]);

                temp.Add(y1Axis + boxSize);
                //Console.WriteLine(y1Axis + boxSize);

            }
            x1Axis = x1Axis + 39;
            for (int w = 0; w < lineList.Length; w = w + 4)
            {
                lineList2[w] = new Line();
                lineList2[w].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList2[w].X1 = x1Axis;
                lineList2[w].X2 = x1Axis + boxSize;
                lineList2[w].Y1 = y1Axis;
                lineList2[w].Y2 = y1Axis;
                lineList2[w].StrokeThickness = 1;
                myGrid.Children.Add(lineList2[w]);

                lineList2[w + 1] = new Line();
                lineList2[w + 1].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList2[w + 1].X1 = x1Axis;
                lineList2[w + 1].X2 = x1Axis;
                lineList2[w + 1].Y1 = y1Axis;
                lineList2[w + 1].Y2 = y1Axis + boxSize;
                lineList2[w + 1].StrokeThickness = 1;
                myGrid.Children.Add(lineList2[w + 1]);

                lineList2[w + 2] = new Line();
                lineList2[w + 2].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList2[w + 2].X1 = x1Axis + boxSize;
                lineList2[w + 2].X2 = x1Axis + boxSize;
                lineList2[w + 2].Y1 = y1Axis;
                lineList2[w + 2].Y2 = y1Axis + boxSize;
                lineList2[w + 2].StrokeThickness = 1;
                myGrid.Children.Add(lineList2[w + 2]);

                lineList2[w + 3] = new Line();
                lineList2[w + 3].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                lineList2[w + 3].X1 = x1Axis;
                lineList2[w + 3].X2 = x1Axis + boxSize;
                lineList2[w + 3].Y1 = y1Axis + boxSize;
                lineList2[w + 3].Y2 = y1Axis + boxSize;
                lineList2[w + 3].StrokeThickness = 1;
                myGrid.Children.Add(lineList2[w + 3]);

                temp1.Add(y1Axis + boxSize);
            }

        }

        #endregion

        #region SendStuff
        
        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            string transferredData = "";
            if (AllPoints.Count > 0)
            {
                List<TouchPoint> letterList = new List<TouchPoint>();
                letterList.Add(AllPoints[0]);
                for (int i = 1; i < AllPoints.Count; i++)
                {
                    if (AllPoints[i].Position.Y < AllPoints[i - 1].Position.Y)
                    {
                        transferredData = transferredData + constructBin(letterList);
                        letterList.Clear();
                    }
                    letterList.Add(AllPoints[i]);
                }
                transferredData = transferredData + constructBin(letterList);
                Console.WriteLine(transferredData);
                textBox.Text = transferredData;
                if (transferredData.Contains(','))
                {
                    string[] words = transferredData.Split(',');
                    command = words[0];
                    betAmount = words[1];
                    Random rng = new Random();
                    if (command.Equals("B"))
                    {
                        pot = (2 * Convert.ToInt32(betAmount));
                        player.setWallet(player.getWallet() - Convert.ToInt32(betAmount));
                        potLabel.Content = "Pot: $" + pot;
                        for (int i = 0; i < player.getHand().Count(); i++)
                        {
                            string card = "";
                            card = (player.getHand()[i].getSuit()[0]).ToString() + (player.getHand()[i].getFace()[0]).ToString();
                            sendData(card);
                        }
                    }
                    else if (command.Equals("H"))
                    {
                        player.getHand().Add(controller.deck.getDeck()[0]);

                        controller.deck.getDeck().RemoveAt(0);
                        string card = "";
                        card = (player.getHand()[player.getHand().Count - 1].getSuit()[0]).ToString() + (player.getHand()[player.getHand().Count - 1].getFace()[0]).ToString();
                        sendData(card);
                        Image playerCard = player.getHand()[player.getHand().Count - 1].getImage();
                        Canvas.SetTop(playerCard, 700);
                        xLoc = xLoc + 1;
                        Canvas.SetLeft(playerCard, 500 + (xLoc * 20));
                        canvas.Children.Add(playerCard);
                        player.calculateScore();
                        if (player.getScore() > 21)
                        {
                            textBox.Text = "Player Bust!";
                            player.reset();
                            dealer.reset();
                            xLoc = 0;
                            ClearCards();
                            Init();

                        }

                    }
                    else if (command.Equals("S"))
                    {
                        while (dealer.getScore() <= 17)
                        {
                            dealer.getHand().Add(controller.deck.getDeck()[0]);
                            controller.deck.getDeck().RemoveAt(0);
                            dealer.calculateScore();
                            Image dealerCard = dealer.getHand()[dealer.getHand().Count - 1].getImage();
                            Canvas.SetTop(dealerCard, 200);
                            xLoc = xLoc + 1;
                            Canvas.SetLeft(dealerCard, 500 + (xLoc * 20));
                            canvas.Children.Add(dealerCard);
                        }
                        if (dealer.getScore() > 21)
                        {
                            textBox.Text = "Dealer Bust!";
                            player.setWallet(pot + player.getWallet());
                            player.reset();
                            dealer.reset();
                            xLoc = 0;
                            ClearCards();
                            Init();
                        }
                        else
                        {
                            if (player.getScore() > dealer.getScore())
                            {
                                textBox.Text = "Player Wins";
                                player.setWallet(pot + player.getWallet());
                                player.reset();
                                dealer.reset();
                                xLoc = 0;
                                ClearCards();
                                Init();
                            }
                            else
                            {
                                textBox.Text = "Dealer Wins";
                                player.reset();
                                dealer.reset();
                                xLoc = 0;
                                ClearCards();
                                Init();
                            }
                        }
                    }


                }
                else
                {
                    textBox.Text = "Bad Read";
                }
                

                transferredData = "";
                AllPoints.Clear();
                letterList.Clear();
               // temp.Clear();
               // temp1.Clear();
            }
            /*
            counter = 0;
            for (int i = 0; i < AllPoints.Count; i++)
            {
                Console.WriteLine(AllPoints[i].Position.X+", "+AllPoints[i].Position.Y);
            }
            string result = transferStringBuilder("h3");
            string binary;
            char[] binArray;
            string transferString = result;
            binArray = transferString.ToCharArray();
            int location = 0;
            
                while (location < transferString.Length)
                {
                    binary = ConvertToBinary(transferString[location]);
                    binaryList.Add(binary);
                    location++;
                }
                
                TheTimer.Start();*/
        }

        private void sendData(String str)
        {
            counter = 0;
            for (int i = 0; i < AllPoints.Count; i++)
            {
                Console.WriteLine(AllPoints[i].Position.X + ", " + AllPoints[i].Position.Y);
            }
            string result = transferStringBuilder(str);
            string binary;
            char[] binArray;
            string transferString = result;
            binArray = transferString.ToCharArray();
            int location = 0;

            while (location < transferString.Length)
            {
                binary = ConvertToBinary(transferString[location]);
                binaryList.Add(binary);
                location++;
            }

            TheTimer.Start();

        }

        private void ClearCards()
        {

            int intTotalChildren = canvas.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter >= 0; intCounter--)
            {
                if (canvas.Children[intCounter].GetType() == typeof(Image))
                {
                    Image ucCurrentChild = (Image)canvas.Children[intCounter];
                    canvas.Children.Remove(ucCurrentChild);
                }
            }
        }
        /// <summary>
        /// Flashes binary bits
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguements</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            lbl1.Background = Brushes.Black;
            lbl2.Background = Brushes.Black;
            lbl3.Background = Brushes.Black;
            lbl4.Background = Brushes.Black;
            lbl5.Background = Brushes.Black;
            lbl6.Background = Brushes.Black;
            lbl7.Background = Brushes.Black;
            lbl8.Background = Brushes.Black;
            if (counter < binaryList.Count)
            {
                
                str = binaryList[counter];


                if (str[0] == '1')
                {
                    lbl1.Background = Brushes.White;
                }
                if (str[1] == '1')
                {
                    lbl2.Background = Brushes.White;
                }
                if (str[2] == '1')
                {
                    lbl3.Background = Brushes.White;
                }
                if (str[3] == '1')
                {
                    lbl4.Background = Brushes.White;
                }
                if (str[4] == '1')
                {
                    lbl5.Background = Brushes.White;
                }
                if (str[5] == '1')
                {
                    lbl6.Background = Brushes.White;
                }
                if (str[6] == '1')
                {
                    lbl7.Background = Brushes.White;
                }
                if (str[7] == '1')
                {
                    lbl8.Background = Brushes.White;
                }
                lbl1.Background = Brushes.White;
            }
            
            if (counter >= binaryList.Count)
            {
                counter = 0;
                binaryList.Clear();
                lbl1.Background = Brushes.Black;
                lbl2.Background = Brushes.Black;
                lbl3.Background = Brushes.Black;
                lbl4.Background = Brushes.Black;
                lbl5.Background = Brushes.Black;
                lbl6.Background = Brushes.Black;
                lbl7.Background = Brushes.Black;
                lbl8.Background = Brushes.Black;
                TheTimer.Stop();

            }
            if (flip)
            {
                counter++;
                flip = false;
            }
            else
            {
                lbl1.Background = Brushes.Black;
                lbl2.Background = Brushes.Black;
                lbl3.Background = Brushes.Black;
                lbl4.Background = Brushes.Black;
                lbl5.Background = Brushes.Black;
                lbl6.Background = Brushes.Black;
                lbl7.Background = Brushes.Black;
                lbl8.Background = Brushes.Black;
                flip = true;
            }
            
        }

        /// <summary>
        /// This method builds the transfer String with addition null characters between each transfer character.
        /// </summary>
        /// <param name="transferString">string to transfer</param>
        /// <returns>string including null characters in between</returns>
        private string transferStringBuilder(string transferString)
        {
            string BuildString = "";
            for (int i = 0; i < transferString.Length; i++)
            {
               string temp = transferString[i].ToString();

               BuildString = BuildString + (temp); //To keep whitle light stay, decrease no. of nulls

            }
            return BuildString;
        }
   
        /// <summary>
        /// This method converts character to binary string
        /// </summary>
        /// <param name="asciiString">ascii character</param>
        /// <returns>binary string</returns>
        public static string ConvertToBinary(char asciiString)
        {
            string result = string.Empty;

            result += Convert.ToString((int)asciiString, 2);
            string finalBin = "0" + result;
            if (finalBin.Length == 7)
            {
                finalBin = "0" + finalBin;
            }
            Console.WriteLine((char)asciiString + "   "+ finalBin);
            return finalBin;
            
        }


        #endregion

    }
}