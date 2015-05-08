using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace demoSoftware
{
    /// <summary>
    /// 
    /// </summary>
    class GameController
    {
        private Player dealer;
        private Player player;
        private int pot;
        private Deck deck;

        /// <summary>
        /// 
        /// </summary>
        public GameController()
        {
            setDealer(new Player(-1, "Dealer"));
            setDefaultPlayer();
            setPot(0);
            setDeck(new Deck());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        private void setDealer(Player d) { this.dealer = d; }
        /// <summary>
        /// 
        /// </summary>
        private void setDefaultPlayer() { this.player = new Player(10000, "Default Player"); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void setPot(int val) { this.pot = val; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void setDeck(Deck val) { this.deck = val; }

        /// <summary>
        /// method that runs the blackjack game
        /// </summary>
        private void play()
        {
            bool inMenu = true, betting = false, playing = false, gameOver = false;
            while (inMenu)
            {
                //ask user if they want to play
                Console.Write("Hello %s, would you like to play a game? (Y)es or (N)o", player.getID());
                //get user input
                string userInput = "y"; //Console.ReadLine();
                //if yes go to betting
                if (userInput.Equals("y"))
                {
                    betting = true;
                    inMenu = false;
                }
                //else go to game over
                else
                {
                    gameOver = true;
                    inMenu = false;
                }
            }

            while (betting)
            {
                //show user their current wallet balance
                Console.WriteLine(player.toString());
                Console.Write("Enter a valid amount to bet: ");

                //ask user for a valid bet amount
                string userInput = Console.ReadLine();
                int bet = int.Parse(userInput);
                //if valid subtract bet amount from players wallet
                player.setWallet(player.getWallet() - bet);

                //add bet amount to the pot
                setPot(bet);

                //change the gamestat and start playing
                playing = true;
                betting = false;
            }
            
            while (playing)
            {
                //deal initial cards
                deal();
                //ask user to hit or stay

                //if hit, continue until bust or stay
                //if user busts, they lose and go to game over

                //dealer hits until value >= 17

                //determine who won
                //check if anyone busted

                //compare values

                //if user wins, add pot*2 to their wallet

                //go to game over
                gameOver = true;
                playing = false;
            }
            while (gameOver)
            {
                //ask user if they want to play again
                Console.Write("%s, would you like to play again? (Y)es or (N)o", player.getID());
                //get user input
                string userInput = "y"; //Console.ReadLine();
                //if yes go to betting
                if (userInput.Equals("y"))
                {
                    inMenu = true;
                    gameOver = false;
                }
                //else exit program
                else
                {
                    gameOver = false;
                }

            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void hit(Player p)
        {
            //grab next card from deck
            Card c = this.deck.getDeck()[0];

            //add card to players hand
            p.getHand().Add(c);
        }

        /// <summary>
        /// 
        /// </summary>
        private void deal()
        {//deal initial cards to user and dealer
            //grab first 4 cards from deck
            Card c1, c2, c3, c4;
            c1 = this.deck.getDeck()[0];
            this.deck.getDeck().RemoveAt(0);
            c2 = this.deck.getDeck()[1];
            c3 = this.deck.getDeck()[2];
            c4 = this.deck.getDeck()[3];

            //remove those cards from the deck
            for (int i = 0; i < 4; i++)
            {
                this.deck.getDeck().RemoveAt(0);
            }

            //give cards 1 & 3 to user
            player.getHand().Add(c1);
            player.getHand().Add(c3);
            //give cards 2 & 4 to dealer
            dealer.getHand().Add(c2);
            dealer.getHand().Add(c4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int getBet()
        {
            int result = 10;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool startPlaying()
        {
            bool result;

            Console.WriteLine("Would you like to play a game?\n(Y)es\n(N)o");
            string choice = Console.ReadLine();
            choice.ToUpper();
            if (choice.Equals("Y")) { result = true; }
            else { result = false; }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool gameOverState()
        {
            bool result;

            Console.WriteLine("Would you like to play again? \n(Y)es\n(N)o");
            string choice = Console.ReadLine();
            choice.ToUpper();
            if (choice.Equals("Y")) { result = true; }
            else { result = false; }

            return result;
        }
    }
}
