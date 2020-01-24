using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;


namespace Test_game
{
    public class MainMenu : BaseMenu
    {
      

        public MainMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight , Color background , string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight , background, title)
        {
            IsVisible = true;
        }

        public override void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;

            Button SelectMap;
            Button LoadMap;
            Button Quit;

            SelectMap = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(4, 2),
                Text = "Select Map",
                Name = "SelectMap"
            };

            LoadMap = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(4, 6),
                Text = "Load Map",
                TextAlignment = HorizontalAlignment.Center,
                Name = "LoadMap"
            };

            Quit = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(4, 10),
                Text = "Quit",
                TextAlignment = HorizontalAlignment.Center,
                Name = "QuitGame"
            };

            //event handlers

            SelectMap.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[1].IsVisible = true;
                
            };

            Quit.Click += (s, e) =>
            {
                SadConsole.Game.Instance.Exit();
            };

            LoadMap.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[2].IsVisible = true;
                
            };

            this.Controls.Add(Quit);
            this.Controls.Add(SelectMap);
            this.Controls.Add(LoadMap);


        }
    }
}
