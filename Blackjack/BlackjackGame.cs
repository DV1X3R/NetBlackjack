using System;
using System.Text;
using System.Collections.Generic;

namespace Blackjack
{
    public class BlackjackGame
    {
        public bool InProgress { get; protected set; } = false;
        public bool IsFinished { get; protected set; } = false;

        private readonly Deck deck = new Deck();
        public Hand Dealer { get; }

        private readonly List<Hand> players = new List<Hand>();
        public IReadOnlyList<Hand> Players { get; }

        public event Action<BlackjackGame, GameEventType, String> GameEvent;

        public BlackjackGame(String dealerName)
        {
            Dealer = new Hand(dealerName);
            Dealer.Ready = true;
            Players = players.AsReadOnly();
        }

        private void PlayerReadyEvent(Hand currentPlayer)
        {
            if (!players.Exists((x) => x.Ready == false))
            {
                InProgress = true;

                foreach (var player in players)
                {
                    player.cards.Add(GetCardFromDeck());
                    player.cards.Add(GetCardFromDeck());
                }

                Dealer.cards.Add(GetCardFromDeck());
                Dealer.hiddenCards.Add(GetCardFromDeck());

                GameEvent(this, GameEventType.GameStarted, "Game started!");
            }
            else
                GameEvent(this, GameEventType.GameUpdated, currentPlayer.Name + " ready");
        }

        private void PlayerStandEvent(Hand currentPlayer)
        {
            if (!players.Exists((x) => x.Standing == false))
            {
                Dealer.cards.AddRange(Dealer.hiddenCards);
                Dealer.hiddenCards.Clear();

                while (Dealer.Value < 17)
                    Dealer.cards.Add(GetCardFromDeck());

                InProgress = false; IsFinished = true;
                GameEvent(this, GameEventType.GameFinished, "Game finished!");
            }
            else
                GameEvent(this, GameEventType.GameUpdated, currentPlayer.Name + " stands with " + currentPlayer.Value);
        }

        private void PlayerHitEvent(Hand currentPlayer)
        {
            GameEvent(this, GameEventType.GameUpdated, currentPlayer.Name + " hits " + currentPlayer.Value);
        }

        public PlayerController InitializePlayer(string playerName)
        {
            if (InProgress)
                throw new OperationCanceledException(
                    string.Format("Unable to initialize player {0}: Game is already started", playerName));
            else if (IsFinished)
                throw new OperationCanceledException(
                    string.Format("Unable to initialize player {0}: Game is finished", playerName));
            else if (players.Exists((x) => x.Name.Equals(playerName)))
                throw new OperationCanceledException(
                    string.Format("Unable to initialize player {0}: Player already exists", playerName));

            var playerController = new PlayerController(new Hand(playerName), this);
            playerController.PlayerReady += PlayerReadyEvent;
            playerController.PlayerStand += PlayerStandEvent;
            playerController.PlayerHit += PlayerHitEvent;
            players.Add(playerController.Hand);

            GameEvent(this, GameEventType.GameUpdated, playerName + " joins game");
            return playerController;
        }

        internal Card GetCardFromDeck()
        {
            if (!InProgress)
                throw new OperationCanceledException("Unable to get card from deck: Game is not startedƒ");

            return deck.GetCard();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Dealer.ToString() + "\n");
            foreach (Hand player in Players)
                sb.Append(player.ToString() + "\n");

            return sb.ToString();
        }
    }

    public enum GameEventType
    {
        GameStarted, GameFinished, GameUpdated
    }

}
