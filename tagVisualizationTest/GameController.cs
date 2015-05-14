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
        public static Player dealer;
        public static Player player;
        public static int pot;
        public  Deck deck;

        public GameController()
        {
            setDealer(new Player(100000, "Dealer"));
            setDefaultPlayer();
            setPot(0);
            setDeck(new Deck());
        }

        private void setDealer(Player d) { dealer = d; }
        private void setDefaultPlayer() { player = new Player(10000, "Bilbo Baggins"); }
        private void setPot(int val) { pot = val; }
        private void setDeck(Deck val) { deck = val; }

        public void setNewDeck()
        {
            deck = new Deck();
        }
        public Player getPlayer(){
            return player;
        }

        public Player getDealer()
        {
            return dealer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public void play()
        {
            //Console.WriteLine(deck.toString());
            bool play = true;
            while (true)
            {
                pot = 0;
                if (play)
                {
                    //ask user for a valid bet amount
                    int bet = getBet(); 
                    //if valid subtract bet amount from players allet
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
                        //show the dealers cards and score
                        dealer.calculateScore();
                        sendData(dealer.getScore().ToString());
                        sendData(dealer.dealerToString());
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
                        sendData("You Won: $" + pot);//Console.WriteLine("You won: " + pot);
                    }
                    else { 
                        sendData("You Lose"); 
                    }

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
        public void dealerHit()
        {
            while (dealer.getScore() < 17)
            {
                Card c = deck.getDeck()[0];
                deck.getDeck().RemoveAt(0);
                dealer.getHand().Add(c);
                
                dealer.calculateScore();
                sendData("Dealer hits: " + c.toString());//Console.WriteLine("Dealer hits: " + c.toString());
                sendData("Dealer: " + dealer.getScore());//Console.WriteLine("Dealer: " + dealer.getScore());

                if (dealer.getScore() > 21) {
                    sendData("Dealer Busts");//Console.WriteLine("Dealer Busts!"); 
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
        public void deal()
        {//deal initial cards to user and dealer
            //grab first 4 cards from deck
            player.getHand().Add(deck.getDeck()[0]);
           // sendData(player.getID() + ": " + deck.getDeck()[0].toString());
            //Console.WriteLine(player.getID() +": " + deck.getDeck()[0].toString());
            deck.getDeck().RemoveAt(0);

            dealer.getHand().Add(deck.getDeck()[0]);
            //sendData("Dealer: *:*"); 
            //Console.WriteLine("Dealer: *:*");
            deck.getDeck().RemoveAt(0);

            player.getHand().Add(deck.getDeck()[0]);
            //sendData(player.getID() + ": " + deck.getDeck()[0].toString()); 
            //Console.WriteLine(player.getID() + ": " + deck.getDeck()[0].toString());
            deck.getDeck().RemoveAt(0);

            dealer.getHand().Add(deck.getDeck()[0]);
           // sendData("Dealer: " + deck.getDeck()[0].toString()); 
            //Console.WriteLine("Dealer: " + deck.getDeck()[0].toString());
            deck.getDeck().RemoveAt(0);

            player.calculateScore();
            //sendData("\n\n" + player.getID() + ": " + player.getScore()); 
            //Console.WriteLine("\n\n" + player.getID() + ": " + player.getScore());
        }

        /// <summary>
        /// asks user for bet
        /// </summary>
        /// <returns>result of the users bet amount</returns>
        private int getBet()
        {
            int result = 10;

            sendData("How much are you wagering? ");
            //Console.WriteLine("How much are you wagering? ");
            string temp = getData();//Console.ReadLine();
            result = Convert.ToInt32(temp);

            return result;
        }

        /// <summary>
        /// asks the user to hit or stay
        /// </summary>
        /// <returns>0 if stay, 1 if hit</returns>
        private void hitOrStay()
        {
            string choice = "";
            sendData("(H)it or (S)tay: ");//Console.WriteLine("(H)it or (S)tay: ");
            while (!choice.ToUpper().Equals("S"))
            {

                choice = getData();//Console.ReadLine();
                
                if (choice.ToUpper().Equals("H"))
                {
                    Card c = deck.getDeck()[0];
                    player.getHand().Add(c);
                    sendData("You Hit: " + c.toString());
                    //Console.WriteLine("You Hit: " + c.toString());
                    player.calculateScore();
                    sendData(player.getID() + ": " + player.getScore());
                    //Console.WriteLine( player.getID()+": "+player.getScore());
                    deck.getDeck().RemoveAt(0);
                    player.getHand().Add(c);
                }
                if (player.getScore() > 21) {
                    sendData("You Bust");
                    //Console.WriteLine("You Busted!");
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

            sendData("Hello " + player.getID());
            sendData("Would you like to play a game?\n(Y)es\t(N)o: ");
            //Console.WriteLine("Would you like to play a game?\n(Y)es\t(N)o: ");
            string choice = getData();//Console.ReadLine();
            if (choice.ToUpper().Equals("Y")) { result = true; }
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

            
            sendData("Would you like to play again? \n(Y)es\n(N)o");
            //Console.WriteLine("Would you like to play again? \n(Y)es\n(N)o");
            string choice = getData();//Console.ReadLine();
            if (choice.ToUpper().Equals("Y")) { 
                result = true;
                deck = new Deck();
                player.setHand(new List<Card>());
                dealer.setHand(new List<Card>());
            }
            else { result = false; }

            return result;
        }

        private void test() {
            Player x = new Player(1000,"X");

            for (int i = 0; i < 20; i++)
            {
                x.calculateScore();
                //Console.WriteLine("score: " + x.getScore());
                Console.WriteLine(x.toString());
                x.getHand().Add( new Card(11, "ace", "SOME SUIT") );
            }
            x.calculateScore();
            //Console.WriteLine("score: " + x.getScore());
            Console.WriteLine(x.toString());

            Console.ReadLine();
        }

        public static void sendData(string s)
        {
            Console.WriteLine(s);
        }

        public static string getData()
        {
            string result = null;

            result = Console.ReadLine();

            return result;
        }

        /*
        public static void Main() {
            GameController gc = new GameController();
            gc.play();
            //gc.test();
        }
         * */
    }
}
