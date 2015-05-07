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

namespace demoSoftware
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        Image[] imgArray = new Image[10];
        Rectangle[] rectArray = new Rectangle[10];
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
        string temp2 = "";
        List<double> temp = new List<double>();
        List<double> temp1 = new List<double>();
        List<char> binArray = new List<char>();
        List<TouchPoint> AllPoints = new List<TouchPoint>();


        private Line[] lineList = new Line[32];
        private Line[] lineList2 = new Line[32];
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
            Grid.SetZIndex(canvas, -1);
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            imgArray[0] = img1;
            imgArray[1] = img2;
            imgArray[2] = img3;
            imgArray[3] = img4;
            imgArray[4] = img5;
            imgArray[5] = img6;
            imgArray[6] = img7;
            imgArray[7] = img8;
            imgArray[8] = img9;
            imgArray[9] = img10;
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
        #region SurfaceInput


        #endregion

        #region tagVisualization
        /// <summary>
        /// This function recognises tag, and gets relative x and y axis. It also gets orientation of the tag.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">TagVisualizerEvent Arguments</param>
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
           textBox.Clear();
            LynxTagVisualization tag = (LynxTagVisualization)e.TagVisualization;

       //     Console.WriteLine(tag.VisualizedTag.Value);
            orientation = tag.Orientation;
            xAxisTag = tag.Center.X;
            yAxisTag = tag.Center.Y;
            xAxisSend = (tag.Center.X - 960) * 2;
            yAxisSend = (tag.Center.Y - 540) * 2;
            xAxisSend = xAxisSend - (flashDist - 20);
            yAxisSend = yAxisSend + (2.75 * 45 * 2.22);
            x1AxisRecieveUpdated = xAxisTag + (flashDist - 40);
            y1AxisRecieveUpdated = yAxisTag + ((26 * 2.22));
        //    Console.WriteLine(orientation);

            for (int i = 1; i < 9; i++)
            {
                drawboxes(x1AxisRecieveUpdated, y1AxisRecieveUpdated + (i * (18 * 2.22)),orientation);
            }
        }

        private void OnVisualizationMoved(object sender, TagVisualizerEventArgs e)
        {

            LynxTagVisualization tag = (LynxTagVisualization)e.TagVisualization;
            orientation = tag.Orientation;
            xAxisTag = tag.Center.X;
            yAxisTag = tag.Center.Y;
            xAxisSend = (tag.Center.X - 960) * 2;
            yAxisSend = (tag.Center.Y - 540) * 2;
            xAxisSend = xAxisSend - (flashDist - 20);
            yAxisSend = yAxisSend + (2.75 * 45 * 2.22);
            x1AxisRecieveUpdated = xAxisTag + (flashDist - 40);
            y1AxisRecieveUpdated = yAxisTag + ((26 * 2.22));
            int intTotalChildren = myGrid.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter >= 0; intCounter--)
            {
                if (myGrid.Children[intCounter].GetType() == typeof(Line))
                {
                    Line ucCurrentChild = (Line)myGrid.Children[intCounter];
                    myGrid.Children.Remove(ucCurrentChild);
                }
            }
            temp.Clear();
            temp1.Clear();
            for (int i = 1; i < 9; i++)
            {
                drawboxes(x1AxisRecieveUpdated, y1AxisRecieveUpdated + (i * (18 * 2.22)),orientation);

            }
           
        }

        private void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            int intTotalChildren = myGrid.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter >= 0; intCounter--)
            {
                if (myGrid.Children[intCounter].GetType() == typeof(Line))
                {
                    Line ucCurrentChild = (Line)myGrid.Children[intCounter];
                    myGrid.Children.Remove(ucCurrentChild);
                }
            }


            string transferredData = "";
            List<TouchPoint> letterList = new List<TouchPoint>();
            letterList.Add(AllPoints[0]);
            for (int i = 1; i < AllPoints.Count; i++)
            {
              //  Console.WriteLine(AllPoints[i].TouchDevice.Id);
              //  Console.WriteLine(AllPoints[i].Position.X + "   " + AllPoints[i].Position.Y);
                if (AllPoints[i].Position.Y < AllPoints[i - 1].Position.Y)
                {
                    transferredData=transferredData + constructBin(letterList);
                    letterList.Clear();
                }
                letterList.Add(AllPoints[i]);
                
               
            }
            transferredData = transferredData + constructBin(letterList);
            Console.WriteLine(transferredData);
            textBox.Text = transferredData;
            transferredData = "";
            AllPoints.Clear();
            letterList.Clear();
            temp.Clear();
            temp1.Clear();

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
                        //Console.WriteLine("0");
                        //binString[0] = "1";
                        bin[0] = "1";
                    }

                    if (xAxisRecieve > lineList[4].X1 && xAxisRecieve < lineList[4].X2 &&
                        yAxisRecieve > temp[1 * 8] - 24 && yAxisRecieve < temp[1 * 8])
                    {
                        //Console.WriteLine("1");
                        //binString[1] = "1";
                        bin[1] = "1";
                    }

                    if (xAxisRecieve > lineList[8].X1 && xAxisRecieve < lineList[8].X2 &&
                        yAxisRecieve > temp[2 * 8] - 24 && yAxisRecieve < temp[2 * 8])
                    {
                        //Console.WriteLine("2");
                        //binString[2] = "1";
                        bin[2] = "1";
                    }

                    if (xAxisRecieve > lineList[12].X1 && xAxisRecieve < lineList[12].X2 &&
                        yAxisRecieve > temp[3 * 8] - 24 && yAxisRecieve < temp[3 * 8])
                    {
                        //Console.WriteLine("3");
                        //binString[3] = "1";
                        bin[3] = "1";
                    }

                    if (xAxisRecieve > lineList[16].X1 && xAxisRecieve < lineList[16].X2 &&
                       yAxisRecieve > temp[4 * 8] - 24 && yAxisRecieve < temp[4 * 8])
                    {
                        //Console.WriteLine("4");
                        //binString[4] = "1";
                        bin[4] = "1";
                    }

                    if (xAxisRecieve > lineList[20].X1 && xAxisRecieve < lineList[20].X2 &&
                        yAxisRecieve > temp[5 * 8] - 24 && yAxisRecieve < temp[5 * 8])
                    {
                        //Console.WriteLine("5");
                        //binString[5] = "1";
                        bin[5] = "1";
                    }

                    if (xAxisRecieve > lineList[24].X1 && xAxisRecieve < lineList[24].X2 &&
                       yAxisRecieve > temp[6 * 8] - 24 && yAxisRecieve < temp[6 * 8])
                    {
                        //Console.WriteLine("6");
                        //binString[6] = "1";
                        bin[6] = "1";
                    }

                    if (xAxisRecieve > lineList[28].X1 && xAxisRecieve < lineList[28].X2 &&
                       yAxisRecieve > temp[7 * 8] - 24 && yAxisRecieve < temp[7 * 8])
                    {
                        //Console.WriteLine("7");
                        //binString[7] = "1";
                        bin[7] = "1";
                    }

                    if (xAxisRecieve > lineList2[0].X1 && xAxisRecieve < lineList2[0].X2 &&
                       yAxisRecieve > temp1[0 * 8] - 24 && yAxisRecieve < (temp1[0 * 8] + 10))
                    {
                        //Console.WriteLine("8");
                        //binString[8] = "1";
                        bin[0] = "1";
                    }

                    if (xAxisRecieve > lineList2[4].X1 && xAxisRecieve < lineList2[4].X2 &&
                        yAxisRecieve > temp1[1 * 8] - 24 && yAxisRecieve < temp1[1 * 8])
                    {
                        //Console.WriteLine("9");
                        binArray.Add('9');
                    }

                    if (xAxisRecieve > lineList2[8].X1 && xAxisRecieve < lineList2[8].X2 &&
                        yAxisRecieve > temp1[2 * 8] - 24 && yAxisRecieve < temp1[2 * 8])
                    {
                        //Console.WriteLine("a");
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
            foreach (TouchPoint _touchPoint in e.GetTouchPoints(this.myGrid))
            {
                int id = _touchPoint.TouchDevice.Id;
                //Console.WriteLine(_touchPoint.TouchDevice.GetIsTagRecognized());
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
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1Axis"></param>
        /// <param name="y1Axis"></param>
        /// <param name="angle"></param>
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

            for (int i = 0; i < AllPoints.Count; i++)
            {
                Console.WriteLine(AllPoints[i].Position.X+", "+AllPoints[i].Position.Y);
            }
            string result = transferStringBuilder("1");
            string binary;
            char[] binArray;
            string transferString = result;
            binArray = transferString.ToCharArray();
            int location = 0;
            
            while (true)
            {
                while (location < transferString.Length)
                {
                    binary = ConvertToBinary(transferString[location]);
                    blink(binary);
                    location++;
                }
                location = 0;
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

               BuildString = BuildString + (temp + '\0' + '\0' + '\0' ); //To keep whitle light stay, decrease no. of nulls.

            }
            return BuildString;
        }

        /// <summary>
        /// This method actually blinks the binary flashes on table
        /// </summary>
        /// <param name="binary">binary String with null characters</param>
        private void blink(string binary)
        {
           // myGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
           // myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            
            Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                int binLength = binary.Length;
             

                
                //no. miliseconds delay, last point
                TimeSpan fadeInTime = new TimeSpan(0, 0, 0, 0, 60); //keep it similar to ThreadSleep--------Timing
                TimeSpan fadeOutTime = new TimeSpan(0, 0, 0, 0, 60); //keep it similar to ThreadSleep-------Timing
                var fadeInAnimation = new DoubleAnimation(0d, fadeInTime);
                var fadeOutAnimation = new DoubleAnimation(1d, fadeOutTime);

                fadeOutAnimation.Completed += (o, e) =>
                {
                    for (int i = 0; i < binLength; i++)
                    {
                   
                        if (binary[i] == '1')
                        {
                            imgArray[i].Margin = new Thickness(xAxisSend, yAxisSend + (i * flashDist), 0, 0);
                            imgArray[i].BeginAnimation(Image.OpacityProperty, fadeInAnimation);
                        }
                        
                    }
                    
                };
              
                 //keep it similar to FadeIn and FadeOut -----------------------------------Timing
                for (int i = 0; i < binLength; i++)
                {
                    if (binary[i] == '1')
                    {
                        imgArray[i].Margin = new Thickness(xAxisSend, yAxisSend + (i * flashDist), 0, 0);
                        imgArray[i].BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
                    }
                   
                }
                
            }));
            Thread.Sleep(30);
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

           return result;
            
        }

        public static string BinaryToString(string hex)
        {
            // Convert the number expressed in base-16 to an integer. 
            int value = Convert.ToInt32(hex, 16);
            // Get the character corresponding to the integral value. 
            string stringValue = Char.ConvertFromUtf32(value);
            char charValue = (char)value;
            return charValue.ToString();
        }

        #endregion

    }
}