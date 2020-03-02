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
            Button Tunneling = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(24, 2),
                Text = "Tunneling",
                TextAlignment = HorizontalAlignment.Center,
                Name = "TUnneling"
            };            
            Button MessyBSP = new Button(ButtonWidth+3, ButtonHeight)
            {
                Position = new Point(24, 6),
                Text = "Messy BSP",
                TextAlignment = HorizontalAlignment.Center,
                Name = "MessyBSP"
            };

            this.Controls.Add(BackButton);
            this.Controls.Add(MessyBSP);
            this.Controls.Add(Tunneling);
            this.Controls.Add(BSP);
            this.Controls.Add(CellAuto);

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
