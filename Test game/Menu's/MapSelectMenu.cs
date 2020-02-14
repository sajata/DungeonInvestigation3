using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Test_game
{
    public class MapSelectMenu : BaseMenu
    {
               
        public MapSelectMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight, background, title)
        {
            
        }
        public override void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;
            

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
            Button CityBlocks = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(4, 10),
                Text = "City Blocks",
                TextAlignment = HorizontalAlignment.Center,
                Name = "CityBlocks"
            };
            Button Tunneling = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(24, 2),
                Text = "Tunneling",
                TextAlignment = HorizontalAlignment.Center,
                Name = "TUnneling"
            };
            Button MazeWithRooms = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(24, 6),
                Text = "Maze+Rooms",
                TextAlignment = HorizontalAlignment.Center,
                Name = "MazeWithRooms"
            };
            Button MessyBSP = new Button(ButtonWidth+3, ButtonHeight)
            {
                Position = new Point(23, 10),
                Text = "Messy BSP",
                TextAlignment = HorizontalAlignment.Center,
                Name = "MessyBSP"
            };

            this.Controls.Add(BackButton);
            this.Controls.Add(MessyBSP);
            this.Controls.Add(CityBlocks);
            this.Controls.Add(Tunneling);
            this.Controls.Add(BSP);
            this.Controls.Add(CellAuto);
            this.Controls.Add(MazeWithRooms);

            //event handlers

            BackButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[0].IsVisible = true;                
            };

            BSP.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "bsp";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;                
            };

            Tunneling.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "tunneling";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };

            CellAuto.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "cellauto";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };

            MazeWithRooms.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };

            CityBlocks.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };

            MessyBSP.Click += (s, e) =>
            {
                GameLoop.World.MapGenType = "messbsp";
                GameLoop.World.CreateMap();
                GameLoop.MenuManager.PreviewMenu.CreateMap();
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[3].IsVisible = true;
            };
        }
    }
}
