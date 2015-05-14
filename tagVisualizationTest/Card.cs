using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using demoSoftware.Properties;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace demoSoftware
{
    /// <summary>
    /// 
    /// </summary>
    class Card
    {
        private int value;
        private String face;
        private String suit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <param name="s"></param>
        public Card(int v, String f, String s)
        {
            setValue(v);
            setFace(f);
            setSuit(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getValue() { return this.value; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getFace() { return this.face; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getSuit() { return this.suit; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setValue(int val) { this.value = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setFace(String val) { this.face = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setSuit(String val) { this.suit = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return this.face + ":" + this.suit + " = " + this.value;
        }
        public Image getImage()
        {
            Image cardImg = new Image();
            String name = this.face+"_of_"+this.suit+".png";
            Object o = Resources.ResourceManager.GetObject("Deck/"+name);
            cardImg = new Image() { Source = new BitmapImage((new Uri(@"C:\Users\surface\Documents\GitHub\DemoSoftware\tagVisualizationTest\Resources\Deck\"+name, UriKind.Absolute))) };
            cardImg.Height = 200;
            cardImg.Width = 120;
            return cardImg;
        }
    }
}
