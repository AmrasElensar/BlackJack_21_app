using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlackJack
{
    public class Deck
    {
        protected List<Card> cardsInDeck = new List<Card>();
        protected int numberOfCardsInDeck;
        protected Card drawnCard;
        private Random cardShuffler = new Random();

        public Deck()
        {

            Array suitTypeValues = Enum.GetValues(typeof(SuitType));
            Array cardValues = Enum.GetValues(typeof(CardValue));

            foreach (SuitType suit in suitTypeValues)
            {
                foreach (CardValue value in cardValues)
                {
                    Card card = new Card(suit, value);
                    cardsInDeck.Add(card);
                }
            }
            numberOfCardsInDeck = cardsInDeck.Count;

            ShuffleDeck();
        }

        public void ShuffleDeck() // interpretatie van Fisher Yates shuffle
        {
            int n = cardsInDeck.Count;
            while (n > 1)
            {
                n--;
                int k = cardShuffler.Next(n + 1);
                Card value = cardsInDeck[k];
                cardsInDeck[k] = cardsInDeck[n];
                cardsInDeck[n] = value;
            }
        }
        
        // deze radomnizer werkte niet correct en zette soms kaarten dubbel.

        //public void ShuffleDeck() 
        //{
        //    Random shuffler = new Random();
        //    int swapIndex = 0;
        //    Card tempCard;

        //    for (int i = 0; i < cardsInDeck.Count; i++ )
        //    {
        //        swapIndex = shuffler.Next(0, cardsInDeck.Count);
        //        tempCard = cardsInDeck[i];
        //        cardsInDeck[swapIndex] = tempCard;
        //    }
        //}

        public Card DrawCard()
        {
            Random cardPicker = new Random();
            int cardNumber;

            if (numberOfCardsInDeck == 0)
            {
                drawnCard = null;
            }
            else
            {
                cardNumber = cardPicker.Next(0, numberOfCardsInDeck);
                drawnCard = cardsInDeck[cardNumber];
                cardsInDeck.RemoveAt(cardNumber);
                numberOfCardsInDeck = cardsInDeck.Count;
            }
            return drawnCard;
        }

        public int NumberOfCardsInDeck { get { return numberOfCardsInDeck; } }

        public List<Card> ReturnListofCards()
        {
            return cardsInDeck;
        }
    }
}
    