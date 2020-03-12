using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using System.IO;
using Console = SadConsole.Console;

namespace Test_game
{
     public class GameLoop
    {
        public const int GameWidth = 200;
        public const int GameHeight = 60;
        public static World World; // contains Player, monsters, map
        public static GameUIManager GameUIManager; // manages Game UI when user is playing the map
        public static CommandManager CommandManager; // manages commands during the game state
        public static MenuManager MenuManager; // this class contains all of the menus 
        public static StreamReader sr; // responsible for reading in mapseeds from MapSeeds.txt
        public static StreamReader srMapType; // responsible for reading in map types from MapTypes.txt
        public static StreamReader srMapName; // responsible for reading in map names from MapNames.txt
        public static StreamWriter sw;// responsible for writing to MapSeeds.txt
        public static StreamWriter swMapType; // responsible for writing to MapTypes.txt
        public static StreamWriter swMapName; // responsible for writing to MapNames.txt
        static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(GameWidth, GameHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
            
            
        }


        static void Init()
        {                     
            //excutes constructors
            MenuManager = new MenuManager();
            World = new World();
            GameUIManager = new GameUIManager();
            CommandManager = new CommandManager();

            //Tells the menumanager to execute its Init function
            MenuManager.Init();            
        }

       
    }
}