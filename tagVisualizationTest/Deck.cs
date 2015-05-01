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
        public void setDeck() { this.deckOfCards = getShuffledDeck(); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Card> getDeck() { return this.deckOfCards; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Card> getShuffledDeck()
        {
            List<Card> cards = new List<Card>(52);
            //add 2-10 cards of each suit
            for (int i = 2; i <= 10; i++)
            {
                Card c = new Card(i, i.ToString(), "HEARTS");
                cards.Add(c);
                c = new Card(i, i.ToString(), "DIAMONDS");
                cards.Add(c);
                c = new Card(i, i.ToString(), "CLUBS");
                cards.Add(c);
                c = new Card(i, i.ToString(), "SPADES");
                cards.Add(c);
            }
            //add face cards
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: //JACK
                        Card c = new Card(i, "JACK", "HEARTS");
                        cards.Add(c);
                        c = new Card(10, "JACK", "DIAMONDS");
                        cards.Add(c);
                        c = new Card(10, "JACK", "CLUBS");
                        cards.Add(c);
                        c = new Card(10, "JACK", "SPADES");
                        cards.Add(c);
                        break;
                    case 1: //QUEEN
                        c = new Card(10, "QUEEN", "HEARTS");
                        cards.Add(c);
                        c = new Card(10, "QUEEN", "DIAMONDS");
                        cards.Add(c);
                        c = new Card(10, "QUEEN", "CLUBS");
                        cards.Add(c);
                        c = new Card(10, "QUEEN", "SPADES");
                        cards.Add(c);
                        break;
                    case 2: //KING
                        c = new Card(10, "KING", "HEARTS");
                        cards.Add(c);
                        c = new Card(10, "KING", "DIAMONDS");
                        cards.Add(c);
                        c = new Card(10, "KING", "CLUBS");
                        cards.Add(c);
                        c = new Card(10, "KING", "SPADES");
                        cards.Add(c);
                        break;
                    case 3: //ACE
                        c = new Card(11, "ACE", "HEARTS");
                        cards.Add(c);
                        c = new Card(11, "ACE", "DIAMONDS");
                        cards.Add(c);
                        c = new Card(11, "ACE", "CLUBS");
                        cards.Add(c);
                        c = new Card(11, "ACE", "SPADES");
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
                result += deckOfCards[i].toString();
            }
            return result;
        }
    }
}
