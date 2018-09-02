# NetBlackjack

Blackjack .NET Library and console demo application.

## Usage Example
```
var blackjackGame = new BlackjackGame("Dealer Name"); // init
blackjackGame.GameEvent += (gameInstance, eventType, message) =>
    { 
        Console.WriteLine(gameInstance.ToString());
        Console.WriteLine("Event> " + message + "\n");
    }; // Game state updated event

var playerController = blackjackGame.InitializePlayer("Console Player #1");
playerController.SetReady();
playerController.Hit();
playerController.Stand();
```
