using System;

namespace Blackjack
{
    public class PlayerController
    {
        public Hand Hand { get; }
        public BlackjackGame GameInstance { get; }

        internal event Action<Hand> PlayerReady;
        internal event Action<Hand> PlayerStand;
        internal event Action<Hand> PlayerHit;

        public PlayerController(Hand hand, BlackjackGame gameInstance)
        {
            Hand = hand;
            GameInstance = gameInstance;
        }

        public void SetReady()
        {
            if (GameInstance.InProgress)
                throw new OperationCanceledException("Game is already started");
            else if (Hand.Ready)
                throw new OperationCanceledException("User is already ready");

            Hand.Ready = true;
            PlayerReady(Hand);
        }

        public void Stand()
        {
            if (!GameInstance.InProgress)
                throw new OperationCanceledException("Game is not started yet");
            else if (Hand.Standing)
                throw new OperationCanceledException("User is already standing");

            Hand.Standing = true;
            PlayerStand(Hand);
        }

        public void Hit()
        {
            if (!GameInstance.InProgress || !Hand.Ready || Hand.Standing)
                throw new OperationCanceledException("Unable to hit right now");

            Hand.cards.Add(GameInstance.GetCardFromDeck());

            if (Hand.Value >= 21)
                Stand();
            else
                PlayerHit(Hand);
        }

    }
}