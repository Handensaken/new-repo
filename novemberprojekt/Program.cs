using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Projekt
{
    class Program
    {
        static Random rand = new Random();
        static double coins = 100;
        static double tokens = 5;
        static string currentRoom = Rooms.Start.ToString();
        static bool[] usedCard = { false, false, false, false, false, };
        static string[,] map = new string[10, 9] {
                                                    { " ___", "___", "____", "___", "___", "___" , "___", "___", "_______" },
                                                    { "|   ", "   ", "   |", "   ", "   ", "   |", "   ", "   ", "      |" },
                                                    { "|   ", "   ", "   |", "   ", "   ", "   |", "   ", "   ", "      |" },
                                                    { "|___", "___", "___|", "___", "___", "___|", "___", "___", "______|" },
                                                    { "|   ", "   ", "   |", "   ", "   ", "   |", "   ", "   ", "      |" },
                                                    { "|   ", "   ", "   |", "   ", "   ", "   |", "   ", "   ", "      |" },
                                                    { "|___", "___", "___|", "___", "___", "___|", "___", "___", "______|" },
                                                    { "|   ", "   ", "   |", "   ", "   ", "   |", "   ", "   ", "      |" },
                                                    { "|   ", "   ", "   |", "   ", "   ", "   |", "   ", "   ", "      |" },
                                                    { "|___", "___", "___|", "___", "___", "___|", "___", "___", "______|" },

            };//en karta av rummen man kan vara i
        public enum Rooms   //rummen man kan vara i
        {
            SlotMachines, CoinToss, Dice,
            Pazaak, Start, PlaceholderMiddleRight,
            Shop, Exit, PlaceholderBotRight
        }                     //nyttjar enum som en failsafe så att man undviker att skriva fel lättare
        public enum Directions
        {
            North,
            West,
            South,
            East
        }   //de olika riktningarna                //nyttjar enum som en failsafe så att man undviker att skriva fel lättare
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Casino (registered trademark) press 'H' for help inside the game. The start room contains information on " +
                "the various games.");
            Press();
            string Direction;
            while (true)
            {
                Console.Clear();
                RoomMarker();
                RoomDirections(currentRoom);
                Map();
                Direction = Input();
                Movement(Direction);
            }//Main kallar endast på metoder som får spelet att göra någonting
        }
        static string Input()       //låter användaren gå runt genom att returnera en sträng till Direction. Låter även användaren välja rum med E och kollar om användaren har råd att spela i rummet
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.W) { return Directions.North.ToString(); }      //returnerar riktingen man vill gå i
                if (key.Key == ConsoleKey.A) { return Directions.West.ToString(); }       //returnerar riktingen man vill gå i
                if (key.Key == ConsoleKey.S) { return Directions.South.ToString(); }      //returnerar riktingen man vill gå i
                if (key.Key == ConsoleKey.D) { return Directions.East.ToString(); }       //returnerar riktingen man vill gå i
                if (key.Key == ConsoleKey.H) { Console.WriteLine("This is the main menu or the map, whatever you wish to call it. Move around the map with the WASD keys. Enter a room with E"); }
                if (currentRoom == Rooms.CoinToss.ToString() && key.Key == ConsoleKey.E)
                {
                    if (coins >= 1)
                    {
                        CoinToss();
                        Console.Clear();
                        Map();
                    }
                    else
                    {
                        Console.WriteLine("Out of money.");
                    }
                }        //gör att användaren kan välja ett rum och kallar på rummets metod
                if (currentRoom == Rooms.SlotMachines.ToString() && key.Key == ConsoleKey.E)
                {
                    if (tokens >= 1)
                    {
                        SlotMachine();
                        Console.Clear();
                        Map();
                    }
                    else { Console.WriteLine("Out of tokens"); }
                }    //gör att användaren kan välja ett rum och kallar på rummets metod
                if (currentRoom == Rooms.Pazaak.ToString() && key.Key == ConsoleKey.E)
                {
                    if (coins >= 1)
                    {
                        Pazaak();
                        Console.Clear();
                        Map();
                    }
                    else { Console.WriteLine("Out of money"); }
                }          //gör att användaren kan välja ett rum och kallar på rummets metod
                if (currentRoom == Rooms.Shop.ToString() && key.Key == ConsoleKey.E)
                {
                    Shop();
                    Console.Clear();
                    Map();
                }            //gör att användaren kan välja ett rum och kallar på rummets metod
                if (currentRoom == Rooms.Exit.ToString() && key.Key == ConsoleKey.E)
                {
                    string question = "Are you sure you want to exit?";
                    if (YesOrNo(question))
                    {
                        Console.Clear();
                        Console.Write("quitting");
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(".");
                            Thread.Sleep(500);
                        }
                        Environment.Exit(1);
                    }
                    else
                    {
                        Console.Clear();
                        Map();
                    }
                }            //gör att användaren kan välja ett rum och kallar på rummets metod
                if (currentRoom == Rooms.Dice.ToString() && key.Key == ConsoleKey.E)
                {
                    if (coins >= 1)
                    {
                        Dice();
                        Console.Clear();
                        Map();
                    }
                    else { Console.WriteLine("Out of coins"); }
                }            //gör att användaren kan välja ett rum och kallar på rummets metod
                if (currentRoom == Rooms.Start.ToString() && key.Key == ConsoleKey.E)
                {
                    string[] games = { "CoinToss", "SlotMachines", "Pazaak", "Dice" };
                    int currentlySelected = 0;
                    string Selected = "";
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("What game do you need information on?");
                        for (int i = 0; i < games.Length; i++)
                        {
                            if (currentlySelected == i) { Console.WriteLine($">  {games[i]}"); }
                            else { Console.WriteLine($"  {games[i]}"); }
                        }
                        key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { currentlySelected--; }
                        if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { currentlySelected++; }
                        if (currentlySelected < 0) { currentlySelected = games.Length - 1; }
                        if (currentlySelected > games.Length - 1) { currentlySelected = 0; }
                        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E)
                        {
                            Selected = games[currentlySelected];
                            break;
                        }
                    }
                    switch (Selected)
                    {
                        case "CoinToss":
                            {
                                Console.WriteLine("This game is very straight forward. You select the amount you wish to bet then if you want to bet on heads or tails." +
                                    "A coin is then flipped and rewards are given if you won. Else you lose your bet.");
                                break;
                            }
                        case "SlotMachines":
                            {
                                Console.WriteLine("First off In this game you use Tokens rather than coins. They can be bought in the shop." +
                                    "This game consists of two stages. The betting and the spinning. First you place your bet with a maximum of 3 tokens" +
                                    " Secondly three wheels will be spinned if they all end up showing the same number you win. Due to the low chance the rewards are very high");
                                break;
                            }
                        case "Pazaak":
                            {
                                Console.WriteLine("This is a card game similar to blackjack. Each turn you and your opponent will be dealt a card. If you exceed 20 you lose." +
                                    "The game continues untill either both participants have withdrawn or one exceeds a value of 20." +
                                    "At the end of each turn the player has the choice to first add a card from their hand, that card can not be used more than once in a match." +
                                    "Then they will have the choice to fold or keep going. When both participants have folded and no one exceeding 20 the scores will be compared," +
                                    "the one with the highest score wins.");
                                break;
                            }
                        case "Dice":
                            {
                                Console.WriteLine("This is a simple game. A dealer puts two dice in a cup and shakes them. The player will then call their bet. Will the added number of oth die's value be equal or odd?");
                                break;
                            }
                    }
                    Press(); Console.Clear();
                    Map();
                }
            }
        }
        static void RoomMarker()   //Tar emot alla "mittpunkter" i mappen och sätter ett X där användaren befinner sig för tillfället
        {
            if (currentRoom == Rooms.Start.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = " X "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.CoinToss.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = " X "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.SlotMachines.ToString())
            {
                map[2, 1] = " X "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.Pazaak.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = " X "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.Shop.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = " X "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.Exit.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = " X "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.PlaceholderBotRight.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = " X    |";
            }
            if (currentRoom == Rooms.PlaceholderMiddleRight.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = "      |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = " X    |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
            if (currentRoom == Rooms.Dice.ToString())
            {
                map[2, 1] = "   "; map[2, 4] = "   "; map[2, 8] = " X    |";
                map[5, 1] = "   "; map[5, 4] = "   "; map[5, 8] = "      |";
                map[8, 1] = "   "; map[8, 4] = "   "; map[8, 8] = "      |";
            }
        }
        static void Map()         //printar mappen, vad rummet man är i heter och mängden tokens & coins
        {
            for (int i = 0; i < 10; i++)                                      //printar mappen
            {                                                                 //printar mappen
                for (int j = 0; j < 7; j++)                                   //printar mappen
                {                                                             //printar mappen
                    Console.Write(map[i, j]);                                 //printar mappen
                }                                                             //printar mappen
                Console.WriteLine(map[i, 8]);                                 //printar mappen
            }                                                                 //printar mappen
            Console.WriteLine(currentRoom);                                  //printar vad rummet heter
            Console.WriteLine($"Coins: {coins}      Tokens: {tokens}");      //printar tokens och coins
        }
        static void RoomDirections(string cR)      //hanterar vilka riktingar man kan gå mot i de olika rummen.
        {
            directions.Clear();   //rensar listan med riktingar 
            switch (cR)           //switch som tar in vilket rum man är i
            {
                case "Start":
                    {
                        directions.Add(Directions.North.ToString());
                        directions.Add(Directions.West.ToString());
                        directions.Add(Directions.South.ToString());
                        directions.Add(Directions.East.ToString());
                        break;
                    }
                case "CoinToss":
                    {
                        directions.Add(Directions.West.ToString());
                        directions.Add(Directions.East.ToString());
                        directions.Add(Directions.South.ToString());
                        break;
                    }
                case "SlotMachines":
                    {
                        directions.Add(Directions.East.ToString());
                        directions.Add(Directions.South.ToString());
                        break;
                    }
                case "Pazaak":
                    {
                        directions.Add(Directions.North.ToString());
                        directions.Add(Directions.South.ToString());
                        directions.Add(Directions.East.ToString());
                        break;
                    }
                case "Shop":
                    {
                        directions.Add(Directions.North.ToString());
                        directions.Add(Directions.East.ToString());
                        break;
                    }
                case "Exit":
                    {
                        directions.Add(Directions.North.ToString());
                        directions.Add(Directions.West.ToString());
                        directions.Add(Directions.East.ToString());
                        break;
                    }
                case "PlaceholderBotRight":
                    {
                        directions.Add(Directions.North.ToString());
                        directions.Add(Directions.West.ToString());
                        break;
                    }
                case "PlaceholderMiddleRight":
                    {
                        directions.Add(Directions.North.ToString());
                        directions.Add(Directions.West.ToString());
                        directions.Add(Directions.South.ToString());
                        break;
                    }
                case "Dice":
                    {
                        directions.Add(Directions.West.ToString());
                        directions.Add(Directions.South.ToString());
                        break;
                    }
            }
        }
        static void Movement(string d)    //flyttar användaren till rummet de väljer att gå till
        {
            if (directions.Contains(d))     //ser till att användaren bara kan flytta sig om riktingen är giltig
            {
                switch (currentRoom)       //switch som faktiskt ser till att användaren flyttas
                {
                    case "Start":
                        {
                            switch (d)
                            {
                                case "North":
                                    {
                                        currentRoom = Rooms.CoinToss.ToString();
                                        break;
                                    }
                                case "West":
                                    {
                                        currentRoom = Rooms.Pazaak.ToString();
                                        break;
                                    }
                                case "South":
                                    {
                                        currentRoom = Rooms.Exit.ToString();
                                        break;
                                    }
                                case "East":
                                    {
                                        currentRoom = Rooms.PlaceholderMiddleRight.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "CoinToss":
                        {
                            switch (d)
                            {
                                case "South":
                                    {
                                        currentRoom = Rooms.Start.ToString();
                                        break;
                                    }
                                case "West":
                                    {
                                        currentRoom = Rooms.SlotMachines.ToString();
                                        break;
                                    }
                                case "East":
                                    {
                                        currentRoom = Rooms.Dice.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "SlotMachines":
                        {
                            switch (d)
                            {
                                case "East":
                                    {
                                        currentRoom = Rooms.CoinToss.ToString();
                                        break;
                                    }
                                case "South":
                                    {
                                        currentRoom = Rooms.Pazaak.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Pazaak":
                        {
                            switch (d)
                            {
                                case "North":
                                    {
                                        currentRoom = Rooms.SlotMachines.ToString();
                                        break;
                                    }
                                case "South":
                                    {
                                        currentRoom = Rooms.Shop.ToString();
                                        break;
                                    }
                                case "East":
                                    {
                                        currentRoom = Rooms.Start.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Shop":
                        {
                            switch (d)
                            {
                                case "North":
                                    {
                                        currentRoom = Rooms.Pazaak.ToString();
                                        break;
                                    }
                                case "East":
                                    {
                                        currentRoom = Rooms.Exit.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Exit":
                        {
                            switch (d)
                            {
                                case "North":
                                    {
                                        currentRoom = Rooms.Start.ToString();
                                        break;
                                    }
                                case "West":
                                    {
                                        currentRoom = Rooms.Shop.ToString();
                                        break;
                                    }
                                case "East":
                                    {
                                        currentRoom = Rooms.PlaceholderBotRight.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PlaceholderBotRight":
                        {
                            switch (d)
                            {
                                case "North":
                                    {
                                        currentRoom = Rooms.PlaceholderMiddleRight.ToString();
                                        break;
                                    }
                                case "West":
                                    {
                                        currentRoom = Rooms.Exit.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PlaceholderMiddleRight":
                        {
                            switch (d)
                            {
                                case "North":
                                    {
                                        currentRoom = Rooms.Dice.ToString();
                                        break;
                                    }
                                case "West":
                                    {
                                        currentRoom = Rooms.Start.ToString();
                                        break;
                                    }
                                case "South":
                                    {
                                        currentRoom = Rooms.PlaceholderBotRight.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Dice":
                        {
                            switch (d)
                            {
                                case "West":
                                    {
                                        currentRoom = Rooms.CoinToss.ToString();
                                        break;
                                    }
                                case "South":
                                    {
                                        currentRoom = Rooms.PlaceholderMiddleRight.ToString();
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }
        static void CoinToss()
        {
            string help = "Navigate and increase/decrease amount ith the arrowkeys. Confirm selection with Enter";
            string headOrTail;
            Console.WriteLine("Welcome to CoinToss! Begin by placing your bet");
            double bet = 1;
            Press();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Coins: " + coins);
                Console.WriteLine("Bet: " + bet);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { bet++; }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { bet--; }
                if (bet < 1) { bet = 1; }
                if (bet > coins) { bet = coins; }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E) { break; }
                if (key.Key == ConsoleKey.H) { Console.WriteLine(help); Press(); }
            }//låter användaren satsa hur mycket pengar den vill och kan på spelet
            int selection = 0;
            int currentlySelected = 0;
            string[] HoT = { "Heads", "Tails" };
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Heads or Tails?");
                for (int i = 0; i < HoT.Length; i++)
                {
                    if (i == currentlySelected)
                    {
                        Console.WriteLine(">  " + HoT[i]);
                    }
                    else
                    {
                        Console.WriteLine("  " + HoT[i]);
                    }
                    selection = i;
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { currentlySelected--; }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { currentlySelected++; }
                if (currentlySelected < 0) { currentlySelected = HoT.Length - 1; }
                if (currentlySelected > HoT.Length - 1) { currentlySelected = 0; }
                if (key.Key == ConsoleKey.H) { Console.WriteLine(help); Press(); }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E) { headOrTail = HoT[selection]; break; }
            }//låter användaren välja krona eller klave
            int toss = rand.Next(2);
            if (headOrTail == "Heads" && toss == 1)
            {
                Console.WriteLine("you won");
                coins = coins + bet * 2;
            }
            else if (headOrTail == "Tails" && toss == 0)
            {
                Console.WriteLine("you won");
                coins = coins + bet * 2;
            }
            else
            {
                Console.WriteLine("You lose");
                coins = coins - bet;
            }
            Press();
            //slumpar ut ett värde för att kolla om användaren vunnit eller inte
        }      //minigamet singla slant
        static void SlotMachine()
        {
            string help = "Increase bet with UpArrow and DownArrow. Confirm selection with enter";
            Console.WriteLine("Welcome to the slot machines. Here you use tokens rather than money. If you have none they can be bought in the shop.");
            int bet = 1;
            Press();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Tokenks: " + tokens);
                Console.Write("Bet: " + bet);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { bet++; }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { bet--; }
                if (bet < 1) { bet = 1; }
                if (bet > 3) { bet = 3; }
                if (key.Key == ConsoleKey.H) { Console.WriteLine(help); Press(); }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E) { tokens = tokens - bet; break; }
            }//låter användaren satsa tokens med en maximum av 3 tokens
            int[] slots = { 0, 0, 0 };
            for (int j = 0; j < slots.Length; j++)
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.Clear();
                    BalanceSlots(j, slots);
                    if (j == 0) { Console.WriteLine(slots[j]); }
                    if (j == 1) { Console.WriteLine($"{slots[j - 1]}  {slots[j]}"); }
                    if (j == 2) { Console.WriteLine($"{slots[j - 2]}  {slots[j - 1]}  {slots[j]}"); }
                    if (i < 30)
                    {
                        Thread.Sleep(50);
                    }
                    else if (i >= 30 && i < 40)
                    {
                        Thread.Sleep(250);
                    }
                    else if (i >= 40)
                    {
                        Thread.Sleep(750);
                    }
                }//slumpar ut 3 värden strax efter varandra för att simulera snurrande hjul
            }
            if (slots[0] == slots[1] && slots[0] == slots[2])
            {
                Console.WriteLine("Win");
                coins = coins * (slots[0] * 10000) * bet;
            }
            else
            {
                Console.WriteLine("lose");
            }
            Press();
            //kollar ifall siffrorna matchar och belönar användaren för ifall den vunnit. En absurd summa eftersom chansen är låg
        }      //en slot machine
        static void BalanceSlots(int j, int[] slots)     //chanserna att få de olika värdena i slot machine
        {
            slots[j] = rand.Next(1, 91);
            if (slots[j] <= 12) { slots[j] = 1; }
            if (slots[j] <= 22 && slots[j] > 12) { slots[j] = 2; }
            if (slots[j] <= 32 && slots[j] > 22) { slots[j] = 3; }
            if (slots[j] <= 42 && slots[j] > 32) { slots[j] = 4; }
            if (slots[j] <= 51 && slots[j] > 42) { slots[j] = 5; }
            if (slots[j] <= 62 && slots[j] > 51) { slots[j] = 6; }
            if (slots[j] <= 79 && slots[j] > 62) { slots[j] = 7; }
            if (slots[j] <= 85 && slots[j] > 79) { slots[j] = 8; }
            if (slots[j] <= 90 && slots[j] > 85) { slots[j] = 9; }
        }
        static void Pazaak()      //minigamet pazaak från star wars knights of the old republic
        {
            for (int i = 0; i < 5; i++)
            {
                usedCard[i] = false;
            }//ser till att inga kort anses vara använda
            string[] personalities =
            {
                "Aggressive","Careful","Stupid"
            };//skapar en array bet de olika personligheterna
            string currentPersonality = "";
            int personality;
            int playerValue = 0;
            int AIValue = 0;
            bool stop = false;
            bool playerWithdrawn = false;
            bool AIWithdrawn = false;
            string help = "Increase bet with UpArrow and DownArrow. Confirm selection with enter";
            Console.WriteLine("Welcome to Pazaak. This game is a card game similar to BlackJack. You and the opponent will both be dealt one card. After each turn chose to draw or stop. Player closest to 20 wins.");
            string question = "";
            double bet = 1;
            bool win = false;
            Press();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Coins: " + coins);
                Console.WriteLine("Bet: " + bet);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { bet++; }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { bet--; }
                if (bet < 1) { bet = 1; }
                if (bet > coins) { bet = coins; }
                if (key.Key == ConsoleKey.H) { Console.WriteLine(help); Press(); }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E) { Console.Clear(); break; }
            }//låter användaren satsa pengar. Maximum är användarens pengar
            personality = rand.Next(101);
            if (personality <= 45)
            {
                currentPersonality = personalities[0];
                Console.WriteLine($"Enemy is {personalities[0]}");
            }
            else if (personality <= 90)
            {
                currentPersonality = personalities[1];
                Console.WriteLine($"Enemy is {personalities[1]}");
            }
            else if (personality > 90)
            {
                currentPersonality = personalities[2];
                Console.WriteLine($"Enemy is {personalities[2]}");
            }//slumpar ut motståndarens personlighet
            while (true)
            {
                if (!playerWithdrawn)
                {
                    playerValue += rand.Next(1, 11);
                    Console.WriteLine($"Player: {playerValue}");
                    Press();
                }
                if (!AIWithdrawn)
                {
                    AIValue = AIValue + rand.Next(1, 11);
                    Console.WriteLine($"Opponent: {AIValue}");
                    Press();
                }
                if (!playerWithdrawn)
                {
                    question = "Add a card from your hand?";
                    if (YesOrNo(question))
                    {
                        playerValue += PlayerCards();
                        Console.WriteLine($"Player: {playerValue}");
                    }
                    else
                    {
                        Console.WriteLine("No card then");
                    }
                    Press();
                    Console.Clear();
                }
                //ger användaren och motståndaren ett slumpat värde mellan 1 och 10. Användaren får sedan välja om hen vill lägga till ett av sina fem kort
                if (currentPersonality == "Aggressive")
                {
                    if (AIValue >= 18) { AIWithdrawn = true; }
                    else if (AIValue >= 15 && AIValue < 18)
                    {
                        if (rand.Next(10) >= 6) { AIWithdrawn = true; }
                    }
                }
                if (currentPersonality == "Careful")
                {
                    if (AIValue >= 15) { AIWithdrawn = true; }
                    else if (AIValue >= 13 && AIValue < 15)
                    {
                        if (rand.Next(2) == 1) { AIWithdrawn = true; }
                    }
                }
                if (currentPersonality == "Stupid")
                {
                    if (AIValue >= 19) { AIWithdrawn = true; }
                    else if (AIValue >= 17 && AIValue < 19)
                    {
                        if (rand.Next(11) == 10) { AIWithdrawn = true; }
                    }
                }
                //bestämmer när motståndaren vill sluta dra kort baserat på deras personlighet
                if (playerValue > 20 && AIValue <= 20) { win = false; break; }       //gör att användaren förlorar om användarpoängen är över 20 medan motståndarens inte är det
                if (AIValue > 20 && playerValue <= 20) { win = true; break; }        //gör att motståndaren förlorar om dess poäng är över 20 medan användarens inte är det
                question = "Fold?";
                if (!playerWithdrawn)
                {
                    if (YesOrNo(question)) { playerWithdrawn = true; Console.Clear(); }
                }//låter användaren välja om hen vill sluta dra kort
                if (AIWithdrawn && playerWithdrawn)
                {
                    if (playerValue > AIValue) { win = true; break; }            //kollar om användaren eller motståndaren vinner
                    else if (AIValue > playerValue) { win = false; break; }      //kollar om användaren eller motståndaren vinner
                    else
                    {
                        Console.WriteLine("Draw."); question = "play again?"; Press();     //säger att det blev lika
                        if (YesOrNo(question))
                        {
                            AIValue = 0; playerValue = 0; playerWithdrawn = false; AIWithdrawn = false;
                            for (int i = 0; i < usedCard.Length; i++)
                            {
                                usedCard[i] = false;
                            }
                        }//kollar om användaren vill spela igen, om den vill resettas alla värden som påverkar spelet
                        else { stop = true; break; }    //om inte avslutas spelet
                    }
                }
            }
            if (!stop)
            {

                if (win)
                {
                    Console.WriteLine("You won!");                   //gör att användaren tjänar eller förlorar pengar
                    coins = coins + bet * 4;                         //gör att användaren tjänar eller förlorar pengar
                }                                                    //gör att användaren tjänar eller förlorar pengar
                else                                                 //gör att användaren tjänar eller förlorar pengar
                {                                                    //gör att användaren tjänar eller förlorar pengar
                    Console.WriteLine("You lost.");                  //gör att användaren tjänar eller förlorar pengar
                    coins = coins - bet;                             //gör att användaren tjänar eller förlorar pengar
                }
            }
            Press();
        }
        static int PlayerCards()
        {
            bool[] activeCard = { true, false, false, false, false };
            while (true)
            {

                Console.Clear();
                if (activeCard[0])
                {
                    Console.WriteLine(@"                                    _____
                                   |     |      _____        _____       _____       _____
                                   | -2  |     |     |      |     |     |     |     |     |
                                   |_____|     | -1  |      |  1  |     |  2  |     |  3  |
                                               |_____|      |_____|     |_____|     |_____|
                                                                                            ");
                }
                else if (activeCard[1])
                {
                    Console.WriteLine(@"                                                _____                        
                                    _____      |     |       _____       _____       _____
                                   |     |     | -1  |      |     |     |     |     |     |
                                   | -2  |     |_____|      |  1  |     |  2  |     |  3  |
                                   |_____|                  |_____|     |_____|     |_____|
                                                                                            ");
                }
                else if (activeCard[2])
                {
                    Console.WriteLine(@"                                                             _____
                                    _____       _____       |     |      _____       _____
                                   |     |     |     |      |  1  |     |     |     |     |
                                   | -2  |     | -1  |      |_____|     |  2  |     |  3  |
                                   |_____|     |_____|                  |_____|     |_____|
                                                                                            ");
                }
                else if (activeCard[3])
                {
                    Console.WriteLine(@"                                                                         _____
                                    _____       _____        _____      |     |      _____
                                   |     |     |     |      |     |     |  2  |     |     |
                                   | -2  |     | -1  |      |  1  |     |_____|     |  3  |
                                   |_____|     |_____|      |_____|                 |_____|
                                                                                            ");
                }
                else if (activeCard[4])
                {
                    Console.WriteLine(@"                                                                                     _____
                                    _____       _____        _____       _____      |     |
                                   |     |     |     |      |     |     |     |     |  3  |
                                   | -2  |     | -1  |      |  1  |     |  2  |     |_____|
                                   |_____|     |_____|      |_____|     |_____|     
                                                                                            ");
                }
                //printar korten samt det aktiva kortet
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                {
                    if (activeCard[0]) { activeCard[0] = false; activeCard[4] = true; }
                    else if (activeCard[1]) { activeCard[1] = false; activeCard[0] = true; }
                    else if (activeCard[2]) { activeCard[2] = false; activeCard[1] = true; }
                    else if (activeCard[3]) { activeCard[3] = false; activeCard[2] = true; }
                    else if (activeCard[4]) { activeCard[4] = false; activeCard[3] = true; }
                }
                if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                {
                    if (activeCard[0]) { activeCard[0] = false; activeCard[1] = true; }
                    else if (activeCard[1]) { activeCard[1] = false; activeCard[2] = true; }
                    else if (activeCard[2]) { activeCard[2] = false; activeCard[3] = true; }
                    else if (activeCard[3]) { activeCard[3] = false; activeCard[4] = true; }
                    else if (activeCard[4]) { activeCard[4] = false; activeCard[0] = true; }
                }
                //markerar rätt kort baserat på vilken knapp användaren trycker ned
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E)
                {
                    if (activeCard[0]) { if (!usedCard[0]) { usedCard[0] = true; return -2; } }    //returnerar värdet av kortet användaren väljer
                    if (activeCard[1]) { if (!usedCard[1]) { usedCard[1] = true; return -1; } }    //returnerar värdet av kortet användaren väljer
                    if (activeCard[2]) { if (!usedCard[2]) { usedCard[2] = true; return 1; } }     //returnerar värdet av kortet användaren väljer
                    if (activeCard[3]) { if (!usedCard[3]) { usedCard[3] = true; return 2; } }     //returnerar värdet av kortet användaren väljer
                    if (activeCard[4]) { if (!usedCard[4]) { usedCard[4] = true; return 3; } }     //returnerar värdet av kortet användaren väljer
                    for (int i = 0; i < usedCard.Length; i++)
                    {
                        if (usedCard[i]) { Console.WriteLine("Card already used this game"); Press(); Console.Clear(); break; }
                    }//säger ifrån om användaren försöker använda ett redan använt kort
                }
                if (key.Key == ConsoleKey.Escape) { return 0; }       //låter användaren avbryta om hen ångrar sig och inte vill lägga ett kort
                if (key.Key == ConsoleKey.H) { Console.WriteLine("Use the arrow keys to move left and right. Select a card with Enter, choose no card with Escape."); Press(); }    //visar hjälpmeddelandet
            }
        }
        static bool YesOrNo(string q)     //låter användaren svara ja eller nej på en importerad fråga
        {
            int currentlySelected = 0;
            string[] alternatives = { "Yes", "No" };
            while (true)
            {
                Console.Clear();
                Console.WriteLine(q);
                for (int i = 0; i < alternatives.Length; i++)
                {
                    if (currentlySelected == i)
                    {
                        Console.WriteLine($">  {alternatives[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"  {alternatives[i]}");
                    }
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { currentlySelected++; }
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { currentlySelected--; }
                if (currentlySelected < 0) { currentlySelected = alternatives.Length - 1; }
                if (currentlySelected > alternatives.Length - 1) { currentlySelected = 0; }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E)
                {
                    if (currentlySelected == 0)
                    {
                        return true;
                    }
                    else if (currentlySelected == 1)
                    {
                        return false;
                    }
                }
                if (key.Key == ConsoleKey.H) { Console.WriteLine("navigate with the arrow keys and select with enter. cancel with escape"); Press(); }
            }
        }
        static void Shop()
        {
            string question = "Buy tokens?";
            if (YesOrNo(question))
            {
                int currentAmount = 1;
                int cost = 25;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Cost: {cost}");
                    Console.WriteLine($"<-- {currentAmount} -->");
                    ConsoleKeyInfo key = Console.ReadKey(true);                                                                                 //låter användaren välja mängd tokens att köpa
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D) { currentAmount++; }                                                                  //låter användaren välja mängd tokens att köpa
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A) { currentAmount--; }                                                                   //låter användaren välja mängd tokens att köpa
                    if (currentAmount > coins / cost) { currentAmount = (int)coins / cost; if (currentAmount < 1) { currentAmount = 0; } }      //låter användaren välja mängd tokens att köpa
                    if (currentAmount < 0) { currentAmount = 0; }                                                                               //låter användaren välja mängd tokens att köpa
                    if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E)
                    {
                        tokens = tokens + currentAmount;      //ger användaren fler tokens för priset av användarens coins
                        break;
                    }
                    if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }//avbryter köpet
                    if (key.Key == ConsoleKey.H) { Console.WriteLine("adjust the amount you want to buy with left and right arrow keys, select with enter, cancel with escape."); Press(); }
                }
            }
        }
        static void Dice()
        {
            string help = "navigate with the arrow keys and select with enter.";
            Console.WriteLine("Welcome to the Dice room. Here the dealer will put two dide in a cup, shake and place the cup it on the table. You will then call odd or even. If the added amount matches your call you win");
            double bet = 1;
            Press();
            int die1 = rand.Next(1, 7);            //slumpar värdet på den första tärningen
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Coins: " + coins);
                Console.WriteLine("Bet: " + bet);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { bet++; }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { bet--; }
                if (bet < 1) { bet = 1; }
                if (bet > coins) { bet = coins; }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E) { break; }
                if (key.Key == ConsoleKey.H) { Console.WriteLine(help); Press(); }
            }//låter användaren satsa coins. maxvärde: användarens coins
            int die2 = rand.Next(1, 7);            //slumpar värdet på den andra tärningen
            string[] call = { "Odd", "Even" };
            string playerCall = "";
            int currentlySelected = 0;
            Console.WriteLine("Alright place your bets. Odd or even, what will it be?");
            Press();
            Console.Clear();
            while (true)
            {
                int selection = 0;
                Console.Clear();
                for (int i = 0; i < call.Length; i++)
                {
                    if (i == currentlySelected)
                    {
                        Console.WriteLine($">  {call[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"  {call[i]}");
                    }
                    selection = i;
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W) { currentlySelected--; }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S) { currentlySelected++; }
                if (currentlySelected < 0) { currentlySelected = call.Length - 1; }
                if (currentlySelected > call.Length - 1) { currentlySelected = 0; }
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.E) { playerCall = call[selection]; break; }
                if (key.Key == ConsoleKey.H) { Console.WriteLine("Change choice with Up- and DownArrow select with Enter"); Press(); }
            }//låter användaren välja udda eller jämn
            int value = die1 + die2;           //adderar tärningarnas värde
            if (playerCall == "Even" && value % 2 == 0)
            {
                Console.WriteLine($"{die1} and {die2} that's even. Nice");
                coins = coins + bet * 2;
            }
            else if (playerCall == "Odd" && value % 2 != 0)
            {
                Console.WriteLine($"{die1} and {die2} that's odd. Nice");
                coins = coins + bet * 2;
            }//ger användaren pengar om hen gissat rätt
            else
            {
                Console.WriteLine($"{die1} and {die2}. Damn");
                coins = coins - bet;
            }//gör att användaren förlorar pengar om hen gissat fel
            Press();
            
        }
        static void Press()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            Console.ResetColor();
        }
        static List<string> directions = new List<string>();
    }
}