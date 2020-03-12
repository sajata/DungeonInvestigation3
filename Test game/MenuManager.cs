using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using Test_game.Menu_s;
namespace Test_game
{
    /// <summary>
    /// Contains all the menus before the GameScreen is used
    /// </summary>
    public class MenuManager : ContainerConsole
    {
        
        private MainMenu MainMenu;
        private MapSelectMenu MapSelectMenu;
        private LoadMenu LoadMenu;

        //MapPreviewMenu is et to public because its CreateMap() function needs to be accessed directly 
        //by MapSelectMenu and LoadMenu
        public MapPreviewMenu PreviewMenu;

        public MenuManager()
        {
            IsVisible = true; // tells SadConsole screen to process this object
            IsFocused = true; // tells SadConsole to listen for keyboard and mouse input

            //Tells SadConsole that MenuManages
            //is the only screen that it processes
            //and to actively process it
            Parent = SadConsole.Global.CurrentScreen;
        }
        /// <summary>
        /// Creates all the menus
        /// </summary>
        public void Init()
        {
            CreateMenus();                        
        }
               
        /// <summary>
        /// Creates the menus with and adds them to the container console MenuManager
        /// Except the GameUIManager
        /// </summary>
        private void CreateMenus()
        {
            //INDEX OF MENUS
            //TO BE USED WHEN CHANGING BETWEEN MENUS
            //[0] = > Main Menu
            //[1] = > Map Generator Selection Menu
            //[2] = > Load Map Menu
            //[3] = > Map Preview 

            
            MainMenu = new MainMenu(GameLoop.GameWidth, GameLoop.GameHeight, 20, 15, Color.Navy, "Main Menu");
            Children.Add(MainMenu);

            MapSelectMenu = new MapSelectMenu(GameLoop.GameWidth, GameLoop.GameHeight,40,20, Color.MediumPurple, "Select Map Generation Algorithm");
            Children.Add(MapSelectMenu);

            LoadMenu= new LoadMenu(GameLoop.GameWidth, GameLoop.GameHeight, 40, 20, Color.DeepSkyBlue, "Enter the map seed into text box");
            Children.Add(LoadMenu);

            PreviewMenu = new MapPreviewMenu(GameLoop.GameWidth, GameLoop.GameHeight, 40, 5, Color.DeepSkyBlue, "");
            Children.Add(PreviewMenu);
            
        }
    }
}
