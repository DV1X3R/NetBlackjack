using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Deck
    {
        private static readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        private readonly List<Card> cards = new List<Card>(); 
        private readonly int count;

        public Deck(int count = 1)
            => this.count = count;

        public Card GetCard()
        {
            if (cards.Count == 0)
                ReShuffle();

            Card card = cards[random.Next(cards.Count)];
            cards.Remove(card);

            return card;
        }

        public void ReShuffle()
        {
            cards.Clear();

            for (int i = 0; i < count; i++)
                for (int s = 0; s < 4; s++)
                    for (int f = 0; f < 13; f++)
                        cards.Add(new Card((Suit)s, (Face)f));
        }
    }
}
