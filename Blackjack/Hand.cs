using System.Text;
using System.Collections.Generic;

namespace Blackjack
{
    public class Hand
    {
        public string Name { get; }
        public bool Ready { get; internal set; } = false;
        public bool Standing { get; internal set; } = false;

        public IReadOnlyList<Card> Cards { get; }
        public int HiddenCardsCount { get { return hiddenCards.Count; } }

        internal readonly List<Card> cards = new List<Card>();
        internal readonly List<Card> hiddenCards = new List<Card>();

        public Hand(string playerName)
        {
            Name = playerName;
            Cards = cards.AsReadOnly();
        }

        public int Value
        {
            get
            {
                int sum = 0;
                var aces = new List<Card>();

                foreach (var card in cards)
                {
                    if (card.Face != Face.Ace)
                        sum += card.Value();
                    else aces.Add(card);
                }

                var acesCount = aces.Count;

                foreach (var ace in aces)
                {
                    if (sum + (ace.Value(true) * acesCount) <= 21)
                        sum += ace.Value(true);
                    else
                        sum += ace.Value(false);

                    acesCount--;
                }

                return sum;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name +
                (Ready ? "" : " (Not Ready)") + (Standing ? " (Standing)" : "") + "\n");
            sb.Append("\t Cards:");
            foreach (var card in Cards)
                sb.Append(" | " + card.ToString() + " | ");
            for (int i = 0; i < HiddenCardsCount; i++)
                sb.Append(" | ? _ ? | ");
            sb.Append("\n\t Value: " + Value);

            return sb.ToString();
        }

    }
}
