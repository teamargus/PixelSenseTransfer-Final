using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public int getValue() { return value; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getFace() { return face; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getSuit() { return suit; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setValue(int val) { value = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setFace(String val) { face = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setSuit(String val) { suit = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return String.Format("{0,2}:{1,2}", face,suit);
            //return face + ":" + suit + " = " + value;
        }
    }
}
