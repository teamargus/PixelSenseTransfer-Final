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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setWallet(int val) { this.wallet = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setID(String val) { this.id = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setScore(int val) { this.score = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void setHand(List<Card> val) { this.hand = val; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getWallet() { return this.wallet; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getID() { return this.id; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getScore() { return this.score; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Card> getHand() { return this.hand; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String handString()
        {
            String result = "";

            for (int i = 0; i < this.hand.Count; i++)
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
            return this.id + ":" +
                            "\n\tWallet: " + this.wallet +
                            "\n\tHand: " + handString() +
                            "\n\tCurrent Score: " + this.score;
        }
    }
}
