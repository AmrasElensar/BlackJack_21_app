using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class PlayerHand
    {
        private List<Card> cardsInHand = new List<Card>();
        private int numberOfCardsInHand;
        private int valueOfHand;
        private string playerName;

        public PlayerHand()
        {
            numberOfCardsInHand = cardsInHand.Count;
            valueOfHand = CalculateValueOfHand();
        }

        public void AddCardToHand(Card drawnCard)
        {
            cardsInHand.Add(drawnCard);
            numberOfCardsInHand = cardsInHand.Count;
            valueOfHand = CalculateValueOfHand();
        }

        public int CalculateValueOfHand()
        {
            int sum = 0;
            foreach (Card card in cardsInHand)
            {
                sum = sum + (int)card.Value; 
            }
            return sum;
        }

        public List<Card> ShowHand()
        {
            return cardsInHand;
        }

        public int ValueOfHand { get { return valueOfHand; } }
        public int NumberOfCardsInHand { get { return numberOfCardsInHand; } }
        public string PlayerName { get { return playerName; } set { playerName = value; } }

    }
}
