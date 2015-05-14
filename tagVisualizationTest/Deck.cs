using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demoSoftware
{
    /// <summary>
    /// 
    /// </summary>
    class Deck
    {
        private List<Card> deckOfCards;

        /// <summary>
        /// 
        /// </summary>
        public Deck()
        {
            setDeck();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setDeck() { deckOfCards = getShuffledDeck(); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Card> getDeck() { return deckOfCards; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Card> getShuffledDeck()
        {
            List<Card> cards = new List<Card>(52);
            string heart = "hearts", club = "clubs", spade = "spades", diamond = "diamonds";
            //add 2-10 cards of each suit
            for (int i = 2; i <= 10; i++)
            {

                Card c = new Card(i, i.ToString(), heart);
                cards.Add(c);
                c = new Card(i, i.ToString(), club);
                cards.Add(c);
                c = new Card(i, i.ToString(), spade);
                cards.Add(c);
                c = new Card(i, i.ToString(), diamond);
                cards.Add(c);
            }
            //add face cards
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: //JACK
                        Card c = new Card(10, "jack", heart);
                        cards.Add(c);
                        c = new Card(10, "jack", diamond);
                        cards.Add(c);
                        c = new Card(10, "jack", club);
                        cards.Add(c);
                        c = new Card(10, "jack", spade);
                        cards.Add(c);
                        break;
                    case 1: //QUEEN
                        c = new Card(10, "queen", heart);
                        cards.Add(c);
                        c = new Card(10, "queen", diamond);
                        cards.Add(c);
                        c = new Card(10, "queen", club);
                        cards.Add(c);
                        c = new Card(10, "queen", spade);
                        cards.Add(c);
                        break;
                    case 2: //KING
                        c = new Card(10, "king", heart);
                        cards.Add(c);
                        c = new Card(10, "king", diamond);
                        cards.Add(c);
                        c = new Card(10, "king", club);
                        cards.Add(c);
                        c = new Card(10, "king", spade);
                        cards.Add(c);
                        break;
                    case 3: //ACE
                        c = new Card(11, "ace", heart);
                        cards.Add(c);
                        c = new Card(11, "ace", diamond);
                        cards.Add(c);
                        c = new Card(11, "ace", club);
                        cards.Add(c);
                        c = new Card(11, "ace", spade);
                        cards.Add(c);
                        break;
                }
            }
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }

            return cards;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            String result = "";
            for (int i = 0; i < deckOfCards.Count; i++)
            {
                result += deckOfCards[i].toString() + "\n";
            }
            return result;
        }
    }
}
