using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demoSoftware
{
    /// <summary>
    /// 
    /// </summary>
   class Player
    {
        private int wallet;
        private String id;
        private int score;
        private List<Card> hand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="id"></param>
        public Player(int wallet, String id)
        {
            setWallet(wallet);
            setID(id);
            setScore(0);
            setHand( new List<Card>() );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setWallet(int val) { wallet = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setID(String val) { id = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setScore(int val) { score = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setHand(List<Card> val) { hand = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getWallet() { return wallet; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getID() { return id; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getScore() { return score; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Card> getHand() { return hand; }

        public void calculateScore() {
            int result = 0;

            for (int i = 0; i < this.hand.Count; i++) {
                result += this.hand[i].getValue();
            }
            setScore(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String handString()
        {
            String result = "";
            
            for (int i = 0; i < getHand().Count; i++)
            {
                result += hand[i].toString() + ", ";
            }
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return id + ":" +
                            "\n\tWallet: " + wallet +
                            "\n\tHand: " + handString() +
                            "\n\tCurrent Score: " + score;
        }
    }
}
