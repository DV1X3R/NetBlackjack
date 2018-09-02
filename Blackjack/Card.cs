namespace Blackjack
{
    public class Card
    {
        public Suit Suit { get; }
        public Face Face { get; }

        public Card(Suit suit, Face face)
        {
            Suit = suit;
            Face = face;
        }

        public int Value(bool maxAceValue = true)
        {
            switch (Face)
            {
                case (Face.Ace):
                    return maxAceValue ? 11 : 1;

                case (Face.Jack):
                case (Face.Queen):
                case (Face.King):
                    return 10;

                default:
                    return (int)Face + 1;
            }
        }

        public override string ToString()
        {
            char cSuit = '?';

            switch (Suit)
            {
                case Suit.Spade: cSuit = '♠'; break;
                case Suit.Heart: cSuit = '♥'; break;
                case Suit.Diamond: cSuit = '♦'; break;
                case Suit.Club: cSuit = '♣'; break;
            }

            return string.Format("{0} {1} {0}", cSuit, Face);
        }
    }

    public enum Face
    {
        Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten
        , Jack, Queen, King
    }

    public enum Suit
    {
        Spade, Diamond, Heart, Club
    }

}
