﻿using System;
using System.Collections.Generic;
using SadConsole;
using SadConsole.Controls;
using System.IO;
using System.Linq;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Test_game.Tiles;
using Test_game.Entities;

namespace Test_game
{
    public class GameUIManager : ContainerConsole
    {
        public ScrollingConsole MapConsole;// contains the console to store the Map data
        public Window MapWindow; // window to contain the MapConsole which contains the game map
        public ListTextBox MessageLogWindow;
        public ControlsConsole Controls;
        private MapNamer MapNamer;
        public string MapName;
        private bool DisableMovement = false;
        public void Init()
        {
            IsVisible = true;
            IsFocused = true;

            Parent = SadConsole.Global.CurrentScreen;

            CreateMessageLogWindow(50, GameLoop.GameHeight / 2, "Message Log");

            //Loading the World's map into the MapConsole
            LoadMap(GameLoop.World.CurrentMap);

            //Crates the map window
            CreateMapWindow(60, 30, "Map");
            UseMouse = true;                        
            
            CreateControls();

            MapNamer = new MapNamer(40, 20);
            MapNamer.Position = new Point(70, 1);
            MapNamer.Parent = this;           
            //Sets the camera on the player when the game starts
            CenterOnActor(GameLoop.World.Player);
        }

        

        private void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;            

            Controls = new ControlsConsole(40, 5)
            {
                Parent = this,
                Position = new Point(2, (GameLoop.GameHeight+2) / 2)
            };

            var consoletheme = SadConsole.Themes.Library.Default.Clone();
            consoletheme.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme();
            consoletheme.TextBoxTheme = new SadConsole.Themes.TextBoxTheme();
            Controls.Theme = consoletheme;

            Button QuitButton = new Button(ButtonWidth, ButtonHeight)
            {
                //width 40, height 5
                Position = new Point(4, 1),
                Text = "Quit",
                TextAlignment = HorizontalAlignment.Center,
                Name = "QuitButton",
            };

            Button SaveMapButton = new Button(ButtonWidth, ButtonHeight)
            {
                //width 40, height 5
                Position = new Point(26, 1),
                Text = "Save Map",
                TextAlignment = HorizontalAlignment.Center,
                Name = "SaveMapButton",
            };

            Controls.Add(QuitButton);
            Controls.Add(SaveMapButton);

            QuitButton.Click += (s, e) =>
            {
                SadConsole.Game.Instance.Exit();
            };

            SaveMapButton.Click += (s, e) =>
            {
                DisableMovement = true;
                MapWindow.IsFocused = false;
                MapConsole.IsFocused = false;
                MapConsole.UseKeyboard = false;
                MapWindow.UseKeyboard = false;

                
                MapNamer.IsVisible = true;
                MapNamer.IsFocused = true;
                MapNamer.Controls.IsFocused = true;
                MapNamer.Controls.IsExclusiveMouse = true;
                
            };

           

            Children.Add(Controls);
        }

        public void SaveMap()
        {
            GameLoop.sw = File.AppendText("MapSeeds.txt");
            GameLoop.swMapType = File.AppendText("MapGenTypes.txt");
            GameLoop.swMapName = File.AppendText("MapNames.txt");
            
            GameLoop.sw.WriteLine(GameLoop.World.Seed);
            GameLoop.swMapType.WriteLine(GameLoop.World.MapGenType);
            GameLoop.swMapName.WriteLine(MapName);
            
            GameLoop.sw.Close();
            GameLoop.swMapType.Close();
            GameLoop.swMapName.Close();
        }

        //Loads the map into the console
        public void LoadMap(Map map)
        {
            //Loads all of the map tiles into the console
            MapConsole = new ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, 50, GameLoop.GameHeight/2), map.Tiles);
            //Syncs the map entities
            SyncMapEntities(map);
        }


        //Adds the entire of list of entities found in the 
        //Wolrd's.CurrentMap's Entities SpatialMap to the MapConsole
        //so they can be displayed on screen
        private void SyncMapEntities(Map map)
        {
            //Remove all entities from the console 
            MapConsole.Children.Clear();

            //Adding all the entities into the MapConsole in bulk
            foreach (Entity entity in map.Entities.Items) // everything stored in SpatialMaps are reffered to as Items which all have their own ID's
            {
                MapConsole.Children.Add(entity);
            }

            //Subscribe to the Entities ItemAdded listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemAdded += OnMapEntityAdded;

            // Subscribe to the Entities ItemRemoved listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemRemoved += OnMapEntityRemoved;

        }

        // Creates a window which encloses a map console
        // of specified height and width
        // and displays as a centered window title
        // added as a child of UIManger
        // so its updated and drawn
        public void CreateMapWindow(int width, int height, string title)
        {

            MapWindow = new Window(width, height);
            MapWindow.CanDrag = false;

            // makes console short enough to show the window title 
            // and borders and position it away from the borders
            int mapConsoleWidth = width - 2;
            int mapConsoleHeight = height - 2;
          

            //Resize the Map console's to fit inside the windows borders
            MapConsole.ViewPort = new Rectangle(0, 0, mapConsoleWidth, mapConsoleHeight);

            //repositioning the MapConsole so it doesnt overlap with left/top window edges
            MapConsole.Position = new Point(1, 1);

            // Centre the title text at the top of the window
            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);

            // add the map viewer to the window
            //MapWindow.Children.Add(MapConsole);

            // The MapWindow becomes a child console of the UIManager
            MapWindow.Children.Add(MapConsole);

            Children.Add(MapWindow);

            // Displays the map window on screen
            MapWindow.Show();
        } 

        public void CreateMapConsole(int width, int height)
        {

            // sets the size of the MapConsole

            int mapConsoleWidth = width;
            int mapConsoleHeight = height;

            //Resizes the Map viewport to fit inside the window borders
            MapConsole.ViewPort = new Rectangle(0, 0, mapConsoleWidth, mapConsoleHeight);

            //Reposition the map console
            //so its centered inside the game window
            MapConsole.Position = new Point(0, 0);


            //MapWindow becomes a child of UIManager
            Children.Add(MapConsole);
        }


        //Overrides ConsoleContainer's Update method
        //which is triggered before evry game frame update
        //base.Update updates all of its children
        public override void Update(TimeSpan timeElapsed)
        {
            //Checks for keyboard input before every gamer fram update
            CheckKeyboard();
            //After that it updates 
            base.Update(timeElapsed);
        }

        //Checks if the actor is moving by
        // Scaning the SadConsole's Global KeyboardState and triggering behaviour
        // based on the button pressed.
        private void CheckKeyboard()
        {
            //movement only is disable whil the user is entering the map name in order to save it
            if(!DisableMovement)
            {
                //When W is pressed moves actor by +1 along Y-axis
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, -1));
                    CenterOnActor(GameLoop.World.Player);
                }
                //When S is pressed moves actor by -1 along Y-axis
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, 1));
                    CenterOnActor(GameLoop.World.Player);

                }
                //When A is pressed moves actor by -1 along X-axis
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 0));
                    CenterOnActor(GameLoop.World.Player);

                }
                //When D is pressed moves actor by +1 along X-axis
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 0));
                    CenterOnActor(GameLoop.World.Player);

                }
            }           
        }

        // centers the viewport camera on an Actor
        public void CenterOnActor(Actor actor)
        {
            MapConsole.CenterViewPortOnPoint(actor.Position);
        }

        // Creates all child consoles to be managed
        public void CreateConsoles()
        {
            // Temporarily create a console with *no* tile data that will later be replaced with map data
            MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight);
        }

        public void CreateMessageLogWindow(int width, int height, string title)
        {
            //Initialising MessageLog
            MessageLogWindow = new ListTextBox(width, height, title);
            Children.Add(MessageLogWindow);
            MessageLogWindow.Show();
            MessageLogWindow.Position = new Point(GameLoop.GameWidth - width, GameLoop.GameHeight / 2);
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("Hello world");
            MessageLogWindow.Add("TESTING");
            MessageLogWindow.Add("TESTING");
        }

        //THESE ARE THE LISTENERS
        // Add an Entity to the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Add(args.Item);
        }

        // Remove an Entity from the MapConsole every time the Map's entity collection changes
        public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Remove(args.Item);
        }
        //All this is done in order to sync the map entities to the map console 
        //So if an enemy is added, add to both the MultiSpatialMap and the MapConsole

    }
}
