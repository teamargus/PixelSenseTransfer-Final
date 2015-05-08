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
        private static Player dealer;
        private static Player player;
        private static int pot;
        private static Deck deck;

        public GameController()
        {
            setDealer(new Player(-1, "Dealer"));
            setDefaultPlayer();
            setPot(0);
            setDeck(new Deck());
        }

        private void setDealer(Player d) { dealer = d; }
        private void setDefaultPlayer() { player = new Player(10000, "Bilbo Baggins"); }
        private void setPot(int val) { pot = val; }
        private void setDeck(Deck val) { deck = val; }

        /// <summary>
        /// 
        /// </summary>
        public void play()
        {
            //Console.WriteLine(deck.toString());
            bool play = startPlaying();
            while (true)
            {
                pot = 0;
                if (play)
                {
                    //ask user for a valid bet amount
                    int bet = getBet();
                    //if valid subtract bet amount from players wallet
                    player.setWallet(player.getWallet() - bet);

                    //add bet amount to the pot
                    int newPot = bet * 2;
                    setPot(newPot);

                    bool playerBust = false, dealerBust = false, playerWins = false;
                    //deal initial cards
                    deal();
                    player.calculateScore();
                    if (player.getScore() != 21)

                        //ask user to hit or stay
                        hitOrStay();
                    //if user busts, they lose and go to game over
                    playerBust = busted(player);

                    if (!playerBust)
                    {
                        //dealer hits until value >= 17
                        dealerHit();
                        dealerBust = busted(dealer);
                        //determine who won
                        if (dealerBust || (player.getScore() > dealer.getScore()))
                            playerWins = true;
                    }
                    //if user wins, add pot*2 to their wallet
                    if (playerWins)
                    {
                        player.setWallet(player.getWallet() + pot);
                        Console.WriteLine("You won: " + pot);
                    }
                    else { Console.WriteLine("You Lose"); }

                    play = gameOverState();
                }
                else {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        private void dealerHit()
        {
            while (dealer.getScore() < 17)
            {
                Card c = deck.getDeck()[0];
                dealer.calculateScore();
                Console.WriteLine("Dealer hits: " + c.toString());
                Console.WriteLine("Dealer: " + dealer.getScore());
                   
                deck.getDeck().RemoveAt(0);
                dealer.getHand().Add(c);

                if (dealer.getScore() > 21) { Console.WriteLine("Dealer Busts!"); }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void hit(Player p)
        {
            //grab next card from deck
            Card c = deck.getDeck()[0];

            //add card to players hand
            p.getHand().Add(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool busted(Player p)
        {
            bool result = false;

            if (p.getScore() > 21)
                result = true;

            return result;
        }

        /// <summary>
        /// deals the initial cards to the dealer and player
        /// </summary>
        private void deal()
        {//deal initial cards to user and dealer
            //grab first 4 cards from deck
            player.getHand().Add(deck.getDeck()[0]); Console.WriteLine(player.getID() +": " + deck.getDeck()[0].toString());
            deck.getDeck().RemoveAt(0);

            dealer.getHand().Add(deck.getDeck()[0]); Console.WriteLine("Dealer: *:*");
            deck.getDeck().RemoveAt(0);

            player.getHand().Add(deck.getDeck()[0]); Console.WriteLine(player.getID() + ": " + deck.getDeck()[0].toString());
            deck.getDeck().RemoveAt(0);

            dealer.getHand().Add(deck.getDeck()[0]); Console.WriteLine("Dealer: " + deck.getDeck()[0].toString());
            deck.getDeck().RemoveAt(0);

            player.calculateScore();
            Console.WriteLine("\n\n"+ player.getID()+ ": " +player.getScore());
        }

        /// <summary>
        /// asks user for bet
        /// </summary>
        /// <returns>result of the users bet amount</returns>
        private int getBet()
        {
            int result = 1;

            Console.WriteLine("How much are you wagering? ");
            result = Console.Read();

            return result;
        }

        /// <summary>
        /// asks the user to hit or stay
        /// </summary>
        /// <returns>0 if stay, 1 if hit</returns>
        private void hitOrStay()
        {
            string choice = "";
            Console.WriteLine("(H)it or (S)tay: ");
            while (!choice.Equals("S"))
            {
                
                choice = Console.ReadLine().ToUpper();
                if (choice.Equals("H"))
                {
                    Card c = deck.getDeck()[0];
                    player.getHand().Add(c);
                    Console.WriteLine("You Hit: " + c.toString());
                    player.calculateScore(); 
                    Console.WriteLine( player.getID()+": "+player.getScore());
                    deck.getDeck().RemoveAt(0);
                    player.getHand().Add(c);
                }
                if (player.getScore() > 21) {
                    Console.WriteLine("You Busted!");
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool startPlaying()
        {
            bool result = true;

            Console.WriteLine("Would you like to play a game?\n(Y)es\t(N)o: ");
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
            if (choice.Equals("Y")) { 
                result = true;
                deck = new Deck();
                player.setHand(new List<Card>());
                dealer.setHand(new List<Card>());
            }
            else { result = false; }

            return result;
        }

        public static void Main() {
            GameController gc = new GameController();
            gc.play();
        }
    }
}
