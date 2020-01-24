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
        public static World World;
        public static GameUIManager GameUIManager;
        public static CommandManager CommandManager;
        public static MenuManager MenuManager;
        public static StreamReader sr;
        public static StreamReader srMapType;
        public static StreamWriter sw;
        public static StreamWriter swMapType;

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
            MenuManager = new MenuManager();
            World = new World();
            GameUIManager = new GameUIManager();
            CommandManager = new CommandManager();


            MenuManager.Init();            
        }

       
    }
}