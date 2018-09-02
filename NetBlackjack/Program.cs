using System;
using Blackjack;

namespace NetBlackjack
{
    class Program
    {
        static string[] menuItems = new string[5] { "Initialize (New Game)", "Ready", "Hit", "Stand", "Quit" };
        static int selectedItem = 0;

        static void Redraw(BlackjackGame blackjackGame, GameEventType gameEventType, String eventMessage)
        {
            Console.Clear();
            // Print Menu
            for (int i = 0; i < menuItems.Length - 1; i++)
                Console.WriteLine(string.Format(" {0}. {1} \t {2}", (i + 1), menuItems[i], selectedItem == i ? "<<<" : ""));
            Console.WriteLine(string.Format(" 0. {0} \t {1}", menuItems[menuItems.Length - 1], selectedItem == menuItems.Length - 1 ? "<<<" : ""));
            Console.WriteLine();

            // Game Stuff
            Console.WriteLine(blackjackGame?.ToString());
            Console.WriteLine("Event> " + eventMessage + "\n");
        }

        static void Main(string[] args)
        {
            Redraw(null, GameEventType.GameUpdated, "App Started");

            BlackjackGame blackjackGame = null;
            PlayerController playerController = null;

            while (true)
            {
                var cmd = Console.ReadKey();
                switch (cmd.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedItem = selectedItem > 0 ? selectedItem - 1 : selectedItem;
                        Redraw(blackjackGame, GameEventType.GameUpdated, "Menu updated");
                        break;

                    case ConsoleKey.DownArrow:
                        selectedItem = selectedItem < menuItems.Length - 1 ? selectedItem + 1 : selectedItem;
                        Redraw(blackjackGame, GameEventType.GameUpdated, "Menu updated");
                        break;

                    case ConsoleKey.Enter:
                        switch (selectedItem)
                        {
                            case 0: // Initialize (New Game)
                                if (blackjackGame != null) blackjackGame.GameEvent -= Redraw;
                                blackjackGame = new BlackjackGame("Console Dealer");
                                blackjackGame.GameEvent += Redraw;
                                playerController = blackjackGame.InitializePlayer("Console Player #1");
                                break;
                            case 1: // Ready
                                try { playerController?.SetReady(); }
                                catch (OperationCanceledException e) { Console.WriteLine("Exception: " + e.Message); }
                                break;
                            case 2: // Hit
                                try { playerController?.Hit(); }
                                catch (OperationCanceledException e) { Console.WriteLine("Exception: " + e.Message); }
                                break;
                            case 3: // Stand
                                try { playerController?.Stand(); }
                                catch (OperationCanceledException e) { Console.WriteLine("Exception: " + e.Message); }
                                break;
                            case 4:
                                return;
                        }
                        break;
                }

            }
        }
    }
}
