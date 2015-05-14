using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Input;


namespace TransferSystem
{
    /// <summary>
    /// A class that holds the internal logic to the Lynx Transfer System for a PixelSense Table.
    /// </summary>
    public class TransferManager
    {
        #region Internal Variables
        /// <summary>
        /// A logical representation of the Lynx device.
        /// </summary>
        public Lynx lynx { get; protected set; }
        private byte[] sendBuffer;
        private byte[] receiveBuffer;
        private int[] lightMatrix;
        // might need a r and s offset: consider setting roffset to 4 (header end point to avoid unecessary padding)
        private int offset;
        private int toSend;
        private int toReceive;
        private int sIndex;
        private int rIndex;

        Boolean hasRead;
        Boolean hasWritten;
        Boolean lynxHasRead;
        Boolean lynxHasWritten;
        #endregion

        private Grid grid;
        private Boolean LynxAvailable;
        private Boolean DataReset;
        //private Boolean tempTestStop;
        private int sendingOffset = 0;
        /*
         * Header Definition: 28 bits (2 complete data flashes)
         * |-id-|fg|type|--le_ngth--|checksum|
         * 
         * id - 4 bits - id of Lynx (bytecode?)
         * fg - 2 bits - flags
         * type - 4 bits - type of data send as enum sendType
         * length - 10 bits - total length of data sent
         * chucksum - 8 bits - check for data integrity:
         *                     1's compliment of the sum of every byte excluding checksum itself
         * 
         * */
        #region Receiving Header Information Variables
        private int currentRID;
        private int flags;
        private sendType type;
        private int length;
        private byte checksum;
        private Boolean headerInProgress;
        #endregion

        /*
         * Event for Returning Data receieved from Lynx, hook into ReceivedData to capture event
         *  - Received event fired once the entire length of a message is received, and checksum verification returns true
         * 
         *  - ReceivedError event fired once the entire length of a message is received, and checsum verification fails
         */
        #region Events
        /// <summary>
        /// Delegate for methods to handle the ReceivedData event
        /// </summary>
        /// <param name="o">object that fired the event</param>
        /// <param name="Data">Custom Args class that supplies the data interpreted along with it's type</param>
        public delegate void ReceivedEventHandler(object o, LynxReceivedArgs Data);
        /// <summary>
        /// Delegate for methods t o handle ReceivedError event
        /// </summary>
        /// <param name="o">object that fired the event</param>
        /// <param name="error">Custom Args class that supplies the computed and received checksums</param>
        public delegate void ReceivedErrorEventHandler(object o, LynxErrorArgs error);
        /// <summary>
        /// Fires upon successful interpretation of a complete piece of data
        /// </summary>
        public event ReceivedEventHandler ReceivedData;
        /// <summary>
        /// Fires upon the iterpretation of a complete piece of data, but the checksum fails
        /// </summary>
        public event ReceivedErrorEventHandler ReceivedError;
        #endregion

        #region Constructors

        /// <summary>
        /// Primary Constructor for a TransferManager. Expected to work with one lynx
        /// </summary>
        /// <param name="tagger">used to initiate event handlers in TransferManager</param>
        /// <param name="FrameReporter">used to initiate event handlers in TransferManager</param>
        /// <param name="anticipatedLynx">the id of the lynx this TransferManager is intended to handle</param>
        public TransferManager(TagVisualizer tagger, Grid FrameReporter, int anticipatedLynx)
        {
            lynx = new Lynx(anticipatedLynx);//TEMP
            sendBuffer = new byte[512];//size is arbitrary
            receiveBuffer = new byte[4098];
            toSend = 0;
            sIndex = 0;
            rIndex = 0;
            toReceive = 0;
            lightMatrix = new int[16];

            length = -1;
            currentRID = -1;
            flags = -1;
            type = sendType.String;
            checksum = 0;

            headerInProgress = false;
            offset = 0;

            hasRead = false;
            hasWritten = false;
            lynxHasRead = false;
            lynxHasWritten = true;

            addTagListener(tagger);
            addFrameReporter(FrameReporter);
            //tempTestStop = true;
            LynxAvailable = false;
            DataReset = false;
            
        }
        #endregion

        #region Adding Event Listeners
        public void addTouchListener(TouchTarget tgt)
        {
            //tgt.TouchDown += onTouchDown;
        }
        /// <summary>
        /// Wrapper method to add appropriate eventhandlers to events
        /// </summary>
        /// <param name="tagger">the TagVisualizer object to listen to</param>
        public void addTagListener(TagVisualizer tagger)
        {
            tagger.VisualizationAdded += onTagAdded;
            tagger.VisualizationMoved += onTagMoved;
            tagger.VisualizationRemoved += onTagRemoved;
        }
        /// <summary>
        /// Wrapper method to add appropriate eventhandlers to events
        /// </summary>
        /// <param name="FrameReporter">The Grid to listen to</param>
        public void addFrameReporter(Grid FrameReporter)
        {
            Touch.FrameReported += onFrameReported;
            grid = FrameReporter;
        }
        #endregion 

        #region Event Handlers
        /* OLD CODE
        private void onTouchDown(object sender, Microsoft.Surface.Core.TouchEventArgs e)
        {
            //Console.Write(string.Format("Touchpoint Coordinates - {0},{1}\n", e.TouchPoint.CenterX, e.TouchPoint.CenterY));
                //Old Code in if statement
                if ((e.TouchPoint.CenterX > lynx.xSendFirst && e.TouchPoint.CenterX < lynx.xSendFirst + lynx.distance * 8) &&
                     (e.TouchPoint.CenterY > lynx.ySendFirst && e.TouchPoint.CenterY < lynx.ySendFirst + lynx.distance * 2)) 
                {
                    isReading = true;
                    Console.Write(string.Format("xFirst = {0}, yFirst = {1}, point received {2},{3} x should be {4}, y should be {5}\n", lynx.xSendFirst, lynx.ySendFirst, e.TouchPoint.CenterX, e.TouchPoint.CenterY, (int)((e.TouchPoint.CenterX - lynx.xSendFirst) / lynx.distance), (int)((e.TouchPoint.CenterY - lynx.ySendFirst) / lynx.distance)));
                    int matrixIndex = (int)((e.TouchPoint.CenterX - lynx.xSendFirst) / lynx.distance) + (8 * ((int)((e.TouchPoint.CenterY - lynx.ySendFirst) / lynx.distance)));
                    lightMatrix[matrixIndex] = 1;
                    Console.Write(string.Format("{0} bit changed to 1\n",matrixIndex));
                }

                if ((e.TouchPoint.CenterX > lynx.xSendFirst && e.TouchPoint.CenterX < lynx.xSendFirst + lynx.distance) &&
                         (e.TouchPoint.CenterY > lynx.ySendFirst && e.TouchPoint.CenterY < lynx.ySendFirst + lynx.distance))
                {
                    Console.Write("Write Bit Detected\n");
                    //Console.Write(string.Format("xFirst = {0}, yFirst = {1}, point received {2},{3} x should be {4}, y should be {5}\n", lynx.xSendFirst, lynx.ySendFirst, e.TouchPoint.CenterX, e.TouchPoint.CenterY, (int)((e.TouchPoint.CenterX - lynx.xSendFirst) / lynx.distance), (int)((e.TouchPoint.CenterY - lynx.ySendFirst) / lynx.distance)));
                    lightMatrix[0] = 1;
                    receive();
                }
                else if ((e.TouchPoint.CenterX < lynx.xSendFirst && e.TouchPoint.CenterX > lynx.xSendFirst - lynx.distance) &&
                         (e.TouchPoint.CenterY > lynx.ySendFirst && e.TouchPoint.CenterY < lynx.ySendFirst + lynx.distance))
                {
                    Console.Write("Read Bit Detected\n");
                    lightMatrix[8] = 1;
                }
            //receive();//test code, comment for normal use
        }*/

        private void onFrameReported(object sender, TouchFrameEventArgs e)
        {
            foreach (System.Windows.Input.TouchPoint _touchPoint in e.GetTouchPoints(grid))
            {
                int id = _touchPoint.TouchDevice.Id;
                if (!_touchPoint.TouchDevice.GetIsTagRecognized() && !_touchPoint.TouchDevice.GetIsFingerRecognized())
                {

                   /* bool flag = false;
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
                    }*/

                    if ((_touchPoint.Position.X > lynx.xSendFirst - (lynx.distance/2) && _touchPoint.Position.X < lynx.xSendFirst + (lynx.distance*2) - (lynx.distance/2)) &&
                         (_touchPoint.Position.Y > lynx.ySendFirst && _touchPoint.Position.Y < lynx.ySendFirst + lynx.distance * 8))
                    {
                        //Console.Write(string.Format("xFirst = {0}, yFirst = {1}, point received {2},{3} x should be {4}, y should be {5}\n", lynx.xSendFirst, lynx.ySendFirst, e.TouchPoint.CenterX, e.TouchPoint.CenterY, (int)((e.TouchPoint.CenterX - lynx.xSendFirst) / lynx.distance), (int)((e.TouchPoint.CenterY - lynx.ySendFirst) / lynx.distance)));
                        int matrixIndex = (int)(((_touchPoint.Position.X - lynx.xSendFirst) / lynx.distance)*-8) + ( ((int)((_touchPoint.Position.Y - lynx.ySendFirst) / lynx.distance)));
                        lightMatrix[matrixIndex] = 1;
                        //Console.Write(string.Format("{0} bit changed to 1\n", matrixIndex));
                    }

                    if (lightMatrix[0] == 1 && lynxHasWritten == true)
                    {
                        receive();
                        lynxHasWritten = false;
                    }
                    else if (lightMatrix[0] == 0)
                        lynxHasWritten = true;

                    if (lightMatrix[8] == 1 && lynxHasRead == true)
                    {
                        DataReset = true;
                        lynxHasRead = false;
                    }
                    else if (lightMatrix[8] == 0)
                    {
                        lynxHasRead = true;
                    }
                        

                }
            }
        
        }

        /// <summary>
        /// This function recognises tag, and gets relative x and y axis. It also gets orientation of the tag. This function is not used by software.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">tag visualizer event arguements</param>
        private void onTagAdded(object sender, TagVisualizerEventArgs e)
        {
            TagVisualization tag = e.TagVisualization;

            //     Console.WriteLine(tag.VisualizedTag.Value);
            lynx.heading = tag.Orientation;
            lynx.tagX = tag.Center.X;
            lynx.tagY = tag.Center.Y;
            
            lynx.xRecFirst = lynx.tagX + 32; 
            lynx.yRecFirst = lynx.tagY + 99;
            lynx.xSendFirst = lynx.xRecFirst - (52*2.22); //X is for border of read send ranger
            lynx.ySendFirst = lynx.tagY + 99;

            LynxAvailable = true;

           
        }

        /// <summary>
        /// Changes the position of Lynx Reciever screen on table upon lynx movement. This function is not used by software.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">tag visualizer event arguements</param>
        private void onTagMoved(object sender, TagVisualizerEventArgs e)
        {
            TagVisualization tag = e.TagVisualization;

            lynx.heading = tag.Orientation;
            lynx.tagX = tag.Center.X;
            lynx.tagY = tag.Center.Y;
            lynx.xSendFirst = lynx.tagX - (((18 * 2.22) + 39) + 10) + ((18 * 2.22) + 39); //X is for border of read send ranger
            lynx.ySendFirst = lynx.tagY + ((40 * 2.22));
            lynx.xRecFirst = lynx.tagX + (((18 * 2.22) + 39) - 40) - (18 * 2.22);
            lynx.yRecFirst = lynx.tagY + ((26 * 2.22));
        }

        /// <summary>
        /// Destroys the transfer screen on removal of the lynx device. This function is not used by software.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">tag visualizer event arguements</param>
        private void onTagRemoved(object sender, TagVisualizerEventArgs e)
        {
            LynxAvailable = false;
        }
        #endregion

        #region Receiving Data

        /// <summary>
        /// Recieves stuff.
        /// </summary>
        public void receive()
        {
            byte h1 = calculateByte(0);
            byte h2 = calculateByte(8);
            if (length < 0 || headerInProgress == true)
            {
                if (headerInProgress == true)
                {
                    length += ((h1 & 126) >> 1) + 4; 
                    checksum = (byte)(((h1 & 1) << 7) + (h2 & 127));
                    headerInProgress = false;
                }
                else
                {
                    currentRID = (h1 & 120) >> 3;
                    flags = (h1 & 6) >> 1;
                    type = (sendType)(((h1 & 1) << 3) + (byte)((h2 & 112) >> 4));
                    length = (h2 & 15) << 6;
                    headerInProgress = true;
                }
            }
            if (length - toReceive >= 0 || headerInProgress == true) // ADD INITIAL CASE!!!!
            {
                //shift h1
                if (offset != 7)
                    receiveBuffer[rIndex] += (byte)((h1 & (127 ^ (127 >> offset))) >> (7 - offset));//add least significant bits to previous byte
                else
                    receiveBuffer[rIndex] += (byte)(h1 & 127);
                if (offset != 0)
                {
                    rIndex++;
                    toReceive++;
                }
                receiveBuffer[rIndex] = (byte)((h1 & (127 >> offset)) << (offset + 1)); //add most significatnt bits to current byte
                offset = (offset + 1) % 8;
                //end shift h1


                //shift h2
                if (offset != 7)
                    receiveBuffer[rIndex] += (byte)((h2 & (127 ^ (127 >> offset))) >> (7 - offset));//add least significant bits to previous byte
                else
                    receiveBuffer[rIndex] += (byte)(h2 & 127);
                if (offset != 0)
                {
                    rIndex++;
                    toReceive++;
                }
                receiveBuffer[rIndex] = (byte)((h2 & (127 >> offset)) << (offset + 1)); //add most significatnt bits to current byte
                offset = (offset + 1) % 8;
                //end shift h2
            }
            else if (length - toReceive < 0)
            {
                toReceive = 0;
                translateBinary(); 
            }

        }

        /// <summary>
        /// calculates byte
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private byte calculateByte(int i)
        {
            byte returnByte = 0;
            int stop = i + 8;

            while (i < stop)
            {
                if (lightMatrix[i] == 1)
                {
                    returnByte = (byte)(returnByte << 1);
                    returnByte = (byte)(returnByte | 1);
                    lightMatrix[i] = 0;// only commented because of testing, other code, needs to be uncommented for later
                }
                else
                    returnByte = (byte)(returnByte << 1);
                i++;
            }
            return returnByte;
        }

        

        private void translateBinary()
        {
            int check;
            switch (type)
            {
                case sendType.String:

                    String returnString = "";
                    check = 0;

                    for (int i = 4; i < length; i++)
                    {
                        returnString = returnString + (char)receiveBuffer[i];
                        check += receiveBuffer[i];
                    }

                    check += receiveBuffer[0] + receiveBuffer[1] + (receiveBuffer[2] & 240);
                    while (check > 255)
                        check = (check & 255) + (check >> 8);
                    for (int i = 0; i < 8; i++) //XOR check not necessary
                        check = check ^ (1 << i);

                    Console.Write("fire\n");
                    if (((byte)check ^ checksum) == 0)
                    {
                        if (ReceivedData != null)
                            ReceivedData(this, new LynxReceivedArgs(returnString, type));
                        Console.Write(returnString);
                    }
                    else
                    {
                        if (ReceivedError != null)
                            ReceivedError(this, new LynxErrorArgs(checksum, (byte)check));
                        Console.Write("Error\n");
                    }     
                    break;
                case sendType.Char:
                    check = receiveBuffer[0] + receiveBuffer[1] + receiveBuffer[2] & 240 + receiveBuffer[4];
                    char returnChar = (char)receiveBuffer[4];

                    while (checksum > 255)
                        check = check & 255 + check >> 8;
                    for (int i = 0; i < 8; i++)
                        check = check ^ (1 << i);

                    if (((byte)check ^ checksum) == 0)
                    { 
                        if(ReceivedData != null)
                            ReceivedData(this,new LynxReceivedArgs(returnChar, type));
                    }
                    else
                        if (ReceivedError != null)
                            ReceivedError(this, new LynxErrorArgs(checksum, (byte)check));
                    break;
            }
            rIndex = 0;
            type = sendType.NULLTYPE;
            flags = -1;
            length = -1;
            currentRID = -1;
            offset = 0;
            sendingOffset = 0;
        }

        #endregion

        #region Sending Data
        public void push(char data)
        {
            while (toSend + 4 > sendBuffer.Length)
                doubleBuffer();

            int checksum = (int)data;

            formHeader(1, sendType.Char, checksum);

            sendBuffer[(sIndex + toSend++) % sendBuffer.Length] = (byte)data;
        }
        public void push(string data)
        {
            
            while (toSend + 4 + data.Length > sendBuffer.Length)
                doubleBuffer();

            //partial checksum over data, add header in formHeader()
            int checksum = 0;
            for (int i = 0; i < data.Length; i++)
                checksum += (byte)data[i];

            formHeader(data.Length, sendType.String, checksum);
            for (int i = 0; i < data.Length; i++)
                sendBuffer[(sIndex + toSend++) % sendBuffer.Length] = (byte)data[i];

        }
        public void push(object obj)
        {
            while (toSend + 4 + obj.ToString().Length > sendBuffer.Length)
                doubleBuffer();

            push(obj.ToString());
        }
        public byte send()
        {
            if (toSend == 0)
                return 0;
            else
            {
                byte sentByte = 0;
                if (sendingOffset != 0)
                {
                    sentByte += (byte)((sendBuffer[sIndex++] & (byte)(255 >> (8 - sendingOffset))) << (byte)(7 - sendingOffset));
                    toSend--;
                    if (toSend != 0)
                        sentByte += (byte)((sendBuffer[sIndex] & (255 ^ ((1 << (sendingOffset + 1)) - 1))) >> (sendingOffset + 1));
                }
                else
                {
                    sentByte += (byte)((sendBuffer[sIndex] & (255 ^ ((1 << (sendingOffset + 1)) - 1))) >> (sendingOffset + 1));
                }
                sendingOffset = (sendingOffset + 1) % 8;
                return sentByte;
            }
        }

        /*
         * IMPORTANT: header generates padding to fit in 8-bit chucks. Remember to restart shift after sending header
         * */
        private void formHeader(int length, sendType t, int partialCheck)
        {
            sendBuffer[(sIndex + toSend) % sendBuffer.Length] = (byte)((lynx.id<<4) + ((int)t >> 2));
            partialCheck += sendBuffer[sIndex + toSend++];
           
            sendBuffer[(sIndex + toSend) % sendBuffer.Length] = (byte)((((int)t & 3)<<6) + (length>>4));
            partialCheck += sendBuffer[sIndex + toSend++];

            sendBuffer[(sIndex + toSend) % sendBuffer.Length] = (byte)((length & 15) << 4);
            partialCheck += sendBuffer[sIndex + toSend++];

            while (partialCheck > 255)
                partialCheck = (partialCheck >> 8) + (partialCheck & 255);
            for (int i = 0; i < 8; i++)
                partialCheck = partialCheck ^ (1 << i);

            sendBuffer[sIndex + toSend - 1] += (byte)(partialCheck >> 4);
            sendBuffer[sIndex + toSend++] = (byte)((partialCheck & 15) << 4);

        }
        #endregion

        #region Internal Maintanence
        private void doubleBuffer()
        {
            byte[] tempBuffer = new byte[sendBuffer.Length*2];

            for (int i = sIndex; i < sIndex + toSend; i++)
                tempBuffer[i - sIndex] = sendBuffer[i % sendBuffer.Length];

            sIndex = 0;
            sendBuffer = tempBuffer;
        }
        public void feedback(int row, byte feedback)
        {
            for (int i = 0; i < 8; i++)
            {
                lightMatrix[i + (8 * row)] = (feedback & (128 >> i)) >> (7 - i);
            }
        }
        public Boolean isLynxAvailable()
        {
            return LynxAvailable;
        }
        public Boolean Blackout()
        {
            return DataReset;
        }
        #endregion
       // [STAThread]
        /*static int Main()
        {
            /* //BASIC TESTING PURPOSES ONLY
             TransferManager tester = new TransferManager();
             //TagTest tagForm = new TagTest();
            
             //Application.Run();
             tester.push("Of course, that praise is essentially saying; as praise goes, it’s somewhat less glowing when you consider that Valve actually created that system in the first place. It was Valve who chose to allow anyone at all to pay $100 and upload whatever they like onto Greenlight; Valve who decided that there should be no pre-screening, no attempt to filter what ends up being available for votes. There’s a fervent ideological belief at work here, one which says that the most open system is the best system; kill the gatekeepers and haul open the portcullis, let everyone flood in and then allow a combination of the marketplace and algorithmic wizardry to sort the wheat from the chaff. Sure, it needs a bit of tending when some of the chaff turns out to be downright poisonous, but by and large the item of faith writ large by Greenlight’s policies is that the cacophonous roar of the community can be filtered through market logic and algorithms to become a clear, pure voice expressing the wisdom of the crowd.");
             int i = 0;
             while (tester.tempTestStop == true)
             {
                 Console.Write(i++);
                 tester.feedback(0, tester.send());
                 tester.feedback(1, tester.send());
                 tester.receive();
             }
            
            
             //TouchTarget tgt = new TouchTarget(form.Handle, EventThreadChoice.OnBackgroundThread, true);
             //tester.addTouchListener(tgt);
            return 0;

        }*/

    }

    #region Custom Args Classes
    public class LynxReceivedArgs : EventArgs 
    {
        private string type;
        dynamic data;

        public LynxReceivedArgs( dynamic receivedData, sendType t)
        {
            switch (t)
            {
                case sendType.Char:
                    type = "char";
                    data = (char)receivedData;
                    break;
                case sendType.Double:
                    type = "double";
                    data = (double)receivedData;
                    break;
                case sendType.Float:
                    type = "float";
                    data = (float)receivedData;
                    break;
                case sendType.Int:
                    type = "int";
                    data = (int)receivedData;
                    break;
                case sendType.NULLTYPE:
                    type = "null";
                    data = null;
                    break;
                case sendType.Object:
                    type = "object";
                    data = receivedData;
                    break;
                case sendType.String:
                    type = "string";
                    data = (string)receivedData;
                    break;
            }
            
        }
        public dynamic Data { get { return data; } }
        public string Type { get { return type; } }

    }
    public class LynxErrorArgs : EventArgs
    {
        private byte rcheck;
        private byte ccheck;
        public LynxErrorArgs(byte RCheck, byte CCheck)
        {
            rcheck = RCheck;
            ccheck = CCheck;
        }
        public byte ReceivedChecksum { get { return rcheck; } }
        public byte CalculatedChecksum { get { return ccheck; } }
    }
    #endregion

}