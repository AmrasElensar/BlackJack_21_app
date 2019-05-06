using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private int totalSeconds;
        private string niceTimer = "";
        private Card drawnCard;
        private Deck casinoDeck;
        private PlayerHand hand1 = new PlayerHand();
        private PlayerHand hand2 = new PlayerHand();
        private int cardsLeft;
        private List<Card> currentDeckContents;
        private string player1Name;
        private string player2Name;
        private bool player1hold = false;
        private bool player2hold = false;

        
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            startGameBtn.Click += ButtonPress;
            shuffleDeckBtn.Click += ButtonPress;
            drawCardBtn1.Click += ButtonPress;
            drawCardBtn2.Click += ButtonPress;
            openDeckBtn.Click += ButtonPress;
            resetBtn.Click += ButtonPress;
            hold1.Click += ButtonPress;
            hold2.Click += ButtonPress;
            dealPlayers.Click += ButtonPress;
            coinTossBtn.Click += ButtonPress;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            totalSeconds++;
            MakeTimer();
            timerField.Text = niceTimer;
        }

        private void MakeTimer()
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds - minutes * 60;
            niceTimer = string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        private void ButtonPress(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            switch (buttonName)
            {
                case "openDeckBtn":
                    OpenDeckOfCards();
                    break;

                case "shuffleDeckBtn":
                    casinoDeck.ShuffleDeck();
                    AddCardsToList();
                    break;

                case "drawCardBtn1":
                    DrawCard(hand1, player1CardList, player1ListValue, count1);
                    drawCardBtn2.IsEnabled = true;
                    hold2.IsEnabled = true;
                    drawCardBtn1.IsEnabled = false;
                    hold1.IsEnabled = false;
                    break;

                case "drawCardBtn2":
                    DrawCard(hand2, player2CardList, player2ListValue, count2);
                    drawCardBtn2.IsEnabled = false;
                    hold2.IsEnabled = false;
                    drawCardBtn1.IsEnabled = true;
                    hold1.IsEnabled = true;
                    break;

                case "startGameBtn":
                    InputBox.Visibility = System.Windows.Visibility.Visible;
                    break;

                case "dealPlayers":
                    DealCards(hand1, player1CardList, player1ListValue, count1, hand2, player2CardList, player2ListValue, count2);
                    
                    break;

                case "resetBtn":
                    ResetGame();
                    break;

                case "hold1":
                    drawCardBtn1.IsEnabled = false;
                    player1hold = true;
                    CheckIfBothPlayersPressedHold();
                    drawCardBtn2.IsEnabled = true;
                    hold2.IsEnabled = true;
                    break;

                case "hold2":
                    drawCardBtn2.IsEnabled = false;
                    player2hold = true;
                    CheckIfBothPlayersPressedHold();
                    drawCardBtn1.IsEnabled = true;
                    hold1.IsEnabled = true;
                    break;

                case "coinTossBtn":
                    FlipCoin();
                    break;

            }
        }

        private void AddCardsToList()
        {
            deckList.Items.Clear();
            currentDeckContents = casinoDeck.ReturnListofCards();
            Card cardForList;

            for (int i = 0; i < currentDeckContents.Count; i++)
            {
                cardForList = currentDeckContents[i];
                deckList.Items.Add($"The {cardForList.Value} of {cardForList.Suit}");
            }
        }

        private void AddHandToList(PlayerHand hand, ListBox handList, TextBlock value )
        {
            handList.Items.Clear();
            List<Card> listOfHand = hand.ShowHand();
            Card cardForList;

            for (int i = 0; i < listOfHand.Count; i++)
            {
                cardForList = listOfHand[i];
                handList.Items.Add($"The {cardForList.Value} of {cardForList.Suit}");
            }

            value.Text = Convert.ToString(hand.ValueOfHand);
        }

        private Deck CreateDeckAndShuffle()
        {
            Deck tempDeck = new Deck();
            return tempDeck;
        } // volgens mij niet echt nodig om dit in methode te steken. Dit gebeurt al bij instantieren van een Deck object.

        private void DrawCard(PlayerHand hand, ListBox handlist, TextBlock value, TextBlock count)
        {
            drawnCard = casinoDeck.DrawCard();

            if (drawnCard == null)
            {
                MessageBox.Show("You're all out of cards");
            }
            else
            {
                hand.AddCardToHand(drawnCard);
                AddHandToList(hand, handlist, value);
                cardsLeft = casinoDeck.NumberOfCardsInDeck;
                nrOfCardsInDeck.Text = Convert.ToString(cardsLeft);
                count.Text = Convert.ToString(hand.NumberOfCardsInHand);
                AddCardsToList();
                MessageBox.Show(Convert.ToString($"The {drawnCard.Value} of {drawnCard.Suit} has been added to your hand."));
            }

            CheckFor21(hand);
        }

        private void OpenDeckOfCards()
        {
            casinoDeck = CreateDeckAndShuffle();
            shuffleDeckBtn.IsEnabled = true;
            resetBtn.IsEnabled = true;
            startGameBtn.IsEnabled = true;
            openDeckBtn.IsEnabled = false;
            AddCardsToList();
            cardsLeft = casinoDeck.NumberOfCardsInDeck;
            nrOfCardsInDeck.Text = Convert.ToString($"There are {cardsLeft} cards left.");
        }

        private void DealCards(PlayerHand hand1, ListBox handlist1, TextBlock value1,TextBlock count1, PlayerHand hand2, ListBox handlist2, TextBlock value2, TextBlock count2)
        {
            Card card1 = casinoDeck.DrawCard();
            Card card2 = casinoDeck.DrawCard();
            Card card3 = casinoDeck.DrawCard();
            Card card4 = casinoDeck.DrawCard();
            hand1.AddCardToHand(card1);
            hand1.AddCardToHand(card2);
            AddHandToList(hand1, handlist1, value1);
            cardsLeft = casinoDeck.NumberOfCardsInDeck;
            nrOfCardsInDeck.Text = Convert.ToString(cardsLeft);
            count1.Text = Convert.ToString(hand1.NumberOfCardsInHand);
            AddCardsToList();

            hand2.AddCardToHand(card3);
            hand2.AddCardToHand(card4);
            AddHandToList(hand2, handlist2, value2);
            cardsLeft = casinoDeck.NumberOfCardsInDeck;
            nrOfCardsInDeck.Text = Convert.ToString(cardsLeft);
            count2.Text = Convert.ToString(hand2.NumberOfCardsInHand);
            AddCardsToList();

            if (!CheckFor21(hand1, hand2))
            { MessageBox.Show("Flip a coin to see who may start..."); }
            
            dealPlayers.IsEnabled = false;
            coinTossBtn.IsEnabled = true;
        }

        private void ResetGame()
        {
            shuffleDeckBtn.IsEnabled = false;
            drawCardBtn1.IsEnabled = false;
            drawCardBtn2.IsEnabled = false;
            startGameBtn.IsEnabled = false;
            resetBtn.IsEnabled = false;
            hold1.IsEnabled = false;
            hold2.IsEnabled = false;
            dealPlayers.IsEnabled = false;
            coinTossBtn.IsEnabled = false;
            openDeckBtn.IsEnabled = true;
            deckList.Items.Clear();
            player1CardList.Items.Clear();
            player2CardList.Items.Clear();
            player1ListValue.Text = "";
            player2ListValue.Text = "";
            nrOfCardsInDeck.Text = "";
            count1.Text = "";
            count2.Text = "";
            player1NameLabel.Text = "";
            player2NameLabel.Text = "";
            drawnCard = null;
            casinoDeck = null;
            cardsLeft = 0;
            currentDeckContents = null;
            hand1 = new PlayerHand();
            hand2 = new PlayerHand();
            timer.Stop();
            totalSeconds = 0;
            timerField.Text = "";
           

        }

        private void CheckFor21(PlayerHand hand)
        {
            int valueOfHand = hand1.CalculateValueOfHand();

            if (valueOfHand == 21)
            {
                MessageBox.Show($"21! {hand.PlayerName} has won!");
                ResetGame();
            }
            else if (valueOfHand > 21)
            {
                MessageBox.Show($"Bust! {hand.PlayerName} has lost...");
                ResetGame();
            }
            else
                MessageBox.Show(Application.Current.MainWindow, $"Will {hand.PlayerName} draw another card or hold?");
        }

        private bool CheckFor21(PlayerHand hand1, PlayerHand hand2)
        {
            int valueOfHand1 = hand1.CalculateValueOfHand();
            int valueOfHand2 = hand2.CalculateValueOfHand();
            if (valueOfHand1 == 21)
            {
                MessageBox.Show($"21! {hand1.PlayerName} has won!");
                ResetGame();
                return true;
            }
            else if (valueOfHand1 > 21)
            {
                MessageBox.Show($"Bust! {hand1.PlayerName} has lost...");
                ResetGame();
                return true;
            }
            else if (valueOfHand2 == 21)
            {
                MessageBox.Show($"21! {hand2.PlayerName} has won!");
                ResetGame();
                return true;
            }
            else if (valueOfHand2 > 21)
            {
                MessageBox.Show($"Bust! {hand2.PlayerName} has lost...");
                ResetGame();
                return true;
            }
            else if (valueOfHand1 == valueOfHand2)
            {
                MessageBox.Show("");
                return true;
            }
            else
                return false;       
        }

        private void SetNames()
        {
            player1Name = InputName1Box.Text;
            player2Name = InputName2Box.Text;
            hand1.PlayerName = player1Name;
            hand2.PlayerName = player2Name;
            player1NameLabel.Text = player1Name;
            player2NameLabel.Text = player2Name;
            startGameBtn.IsEnabled = false;
            dealPlayers.IsEnabled = true;
            timer.Start();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputName1Box.Text == "" || InputName2Box.Text == "")
            { MessageBox.Show("Please fill in both names."); }
            else
            {
                InputBox.Visibility = System.Windows.Visibility.Collapsed;
                SetNames();
                InputName1Box.Text = String.Empty;
                InputName2Box.Text = String.Empty;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            InputName1Box.Text = String.Empty;
            InputName2Box.Text = String.Empty;
        }

        private void CheckIfBothPlayersPressedHold()
        {
            if (player1hold && player2hold)
            {
                if (hand1.ValueOfHand > hand2.ValueOfHand)
                { MessageBox.Show($"{hand1.PlayerName} has won!"); }
                else if (hand1.ValueOfHand == hand2.ValueOfHand)
                { MessageBox.Show($"Draw! Both {hand1.PlayerName} and {hand2.PlayerName} are losers... euhm I mean winners!"); }
                else
                { MessageBox.Show($"{hand2.PlayerName} has won!"); }

                ResetGame();
            }
        }

        private void FlipCoin()
        {
            Random coinToss = new Random();
            int coin = coinToss.Next(0, 2);

            if (coin == 0)
            {
                drawCardBtn1.IsEnabled = true;
                hold1.IsEnabled = true;
            }
            else
            {
                drawCardBtn2.IsEnabled = true;
                hold2.IsEnabled = true;
            }
            coinTossBtn.IsEnabled = false;
        }
    }
}

