using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Test_game
{
    /// <summary>
    /// User selects what dungeon generation algorithm they want to use
    /// </summary>
    public class MapSelectMenu : BaseMenu
    {
               
        public MapSelectMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight, background, title)
        {
            
        }
        public override void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;

            //instatiates the buttons
            Button BackButton = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(14, 17),
                Text = "Back",
                TextAlignment = HorizontalAlignment.Center,
                Name = "BackButton"
            };

            Button BSP = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(4, 2),
                Text = "BSP",
                TextAlignment = HorizontalAlignment.Center,
                Name = "BSP"
            };
            Button CellAuto = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(4, 6),
                Text = "Cell. Auto.",
                TextAlignment = HorizontalAlignment.Center,
                Name = "CellAuto"
            };            
            Button Angband = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(24, 2),
                Text = "Angband",
                TextAlignment = HorizontalAlignment.Center,
                Name = "Angband"
            };            
            Button DrunkenWalk = new Button(ButtonWidth+3, ButtonHeight)
            {
                Position = new Point(24, 6),
                Text = "DrunkenWalk",
                TextAlignment = HorizontalAlignment.Center,
                Name = "Drunken Walk"
            };

            this.Controls.Add(BackButton);
            this.Controls.Add(DrunkenWalk);
            this.Controls.Add(Angband);
            this.Controls.Add(BSP);
            this.Controls.Add(CellAuto);

            //event handlers

            //returns back to the main menu
            BackButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[0].IsVisible = true;                
            };

            //tells the World class to generate a dungeon using BSP
            //and swtiches menu to the map preveiw menu
            BSP.Click += (s, e) =>
            {
                //creates a BSP dungeon
                GameLoop.World.MapGenType = "bsp";
                GameLoop.World.CreateMap();
                //add that dungeon to the preview menus map console
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;                
            };
            //tells the World class to generate a dungeon using the ANGBAND algorithm
            //and swtiches menu to the map preveiw menu
            Angband.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "angband";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };
            //tells the World class to generate a dungeon using cellular automata
            //and swtiches menu to the map preveiw menu
            CellAuto.Click += (s, e) =>
            {
                //creates a cellular automata dungeon
                GameLoop.World.MapGenType = "cellauto";
                GameLoop.World.CreateMap();
                //adds the dungeon to the preview menus map console
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };
            //tells the World class to generate a dungeon using drunken walk algorithm
            //and swtiches menu to the map preveiw menu
            DrunkenWalk.Click += (s, e) =>
            {
                //creates a drunken walk dungeon
                GameLoop.World.MapGenType = "drunkwalk";
                GameLoop.World.CreateMap();
                //adds the dungeon to the preview menus map console
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };
        }
    }
}
