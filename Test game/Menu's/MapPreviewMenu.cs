﻿using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Test_game.Menu_s
{
    /// <summary>
    /// Shows the top down perspective on the entire dungeon
    /// </summary>
    public class MapPreviewMenu : BaseMenu
    {
        //stores the map generated by the world
        private ScrollingConsole MapPreviewConsole;
        //contains the map console
        private Window MapPreviewWindow;
        

        public MapPreviewMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight, background, title)
        {
            Controls.Position = new Point((width - controlsConsoleWidth) - 10, (height - controlsConsoleHeight) - 1);  
            
        }
        
        /// <summary>
        /// Adds the world's dungeon to the map console of the preview menu
        /// </summary>
        public void CreateMap()
        {            
            //clears the console and the window from the previous map generated
            MapPreviewConsole = null;
            MapPreviewWindow = null;
           
            MapPreviewConsole = new ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height / 2), GameLoop.World.CurrentMap.Tiles);
            MapPreviewConsole.IsVisible = true;
            MapPreviewConsole.Position = new Point(25, 2);
            Children.Add(MapPreviewConsole);

            CreateMapPreviewWindow(GameLoop.World.CurrentMap.Width + 2, 52, "Preview of the Map");
        }
        /// <summary>
        /// Adds the mapconsole to the window
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="title"></param>
        private void CreateMapPreviewWindow(int width, int height, string title)
        {
            MapPreviewWindow = new Window(width, height);
            MapPreviewWindow.Position = new Point(24, 1);
            MapPreviewWindow.Title = title.Align(HorizontalAlignment.Center, MapPreviewConsole.Width);

            //sets the map inside the window to allow for borders
            MapPreviewConsole.ViewPort = new Rectangle(0,0, width - 2, height - 2);

            MapPreviewWindow.CanDrag = false;

            //adds the mapconsole to the map window
            MapPreviewWindow.Children.Add(MapPreviewConsole);

            MapPreviewConsole.Position = new Point(1, 1);

            //Adds the window and thus the mapconsole to this menu
            Children.Add(MapPreviewWindow);
            //Tells this console to draw this window and its contents
            MapPreviewWindow.Show();
        }

        public override void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;

            //instatiating the buttons
            Button BackButton = new Button(ButtonWidth, ButtonHeight)
            {
                //width 40, height 5
                Position = new Point(4, 1),
                Text = "Back",
                TextAlignment = HorizontalAlignment.Center,
                Name = "BackButton",
            };

            Button PlayButton = new Button(ButtonWidth, ButtonHeight)
            {
                //width 40, height 5
                Position = new Point(26, 1),
                Text = "Play",
                TextAlignment = HorizontalAlignment.Center,
                Name = "PlayButton",
            };

            //event handlers for when the buttons are pressed

            //returns the user back to the main menu
            BackButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[0].IsVisible = true;
            };

            //tells the world to genrate the entities 
            //user enter the game screen via the GameUIManagers.Init function
            PlayButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                GameLoop.World.GenerateEntities();
                Parent.Parent = null;
                GameLoop.GameUIManager.Init();
            };

            //adds controls to the controls console
            Controls.Add(BackButton);
            Controls.Add(PlayButton);
        }
    }
}
