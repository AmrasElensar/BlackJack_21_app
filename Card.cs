using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public enum SuitType { Clubs, Spades, Diamonds, Hearts }
    public enum CardValue { Ace = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13 }

    public class Card
    {   
        SuitType suit;
        CardValue value;  
        bool isBlack;

        public Card(SuitType suit, CardValue value)
        {
            this.suit = suit;
            this.value = value;
        }

        public bool Compare(Card drawnCard)
        {
            bool higher;
            if (drawnCard.value > this.value)
            { higher = true; }
            else
            { higher = false; }

            return higher;      
        } // not used at this time

        public SuitType Suit { get { return suit; } }
        public CardValue Value { get { return value; } }

        public bool IsBlack
        {
            get
            {
                if (suit == SuitType.Clubs || suit == SuitType.Spades)
                {
                    isBlack = true;
                }
                else
                    isBlack = false;
                return isBlack;
            }
            set
            {
                isBlack = value;
            }
        } // not used at this time
    }
}
