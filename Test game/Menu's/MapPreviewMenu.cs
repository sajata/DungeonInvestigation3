using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Test_game.Menu_s
{
    public class MapPreviewMenu : BaseMenu
    {
        private ScrollingConsole MapPreviewConsole;
        private Window MapPreviewWindow;
        public bool isLoading = false;

        public MapPreviewMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight, background, title)
        {
            Controls.Position = new Point((width - controlsConsoleWidth) - 10, (height - controlsConsoleHeight) - 1);  
            
        }
               
        public void CreateMap()
        {            
            MapPreviewConsole = null;
            MapPreviewWindow = null;
           
            MapPreviewConsole = new ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height / 2), GameLoop.World.CurrentMap.Tiles);
            MapPreviewConsole.IsVisible = true;
            MapPreviewConsole.Position = new Point(25, 2);
            Children.Add(MapPreviewConsole);

            CreateMapPreviewWindow(GameLoop.World.CurrentMap.Width + 2, 52, "Preview of the Map");
        }

        private void CreateMapPreviewWindow(int width, int height, string title)
        {
            MapPreviewWindow = new Window(width, height);
            MapPreviewWindow.Position = new Point(24, 1);
            MapPreviewWindow.Title = title.Align(HorizontalAlignment.Center, MapPreviewConsole.Width);


            MapPreviewConsole.ViewPort = new Rectangle(0,0, width - 2, height - 2);

            MapPreviewWindow.CanDrag = false;

            MapPreviewWindow.Children.Add(MapPreviewConsole);

            MapPreviewConsole.Position = new Point(1, 1);

            Children.Add(MapPreviewWindow);

            MapPreviewWindow.Show();
        }

        public override void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;

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

            //event handlers
            BackButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[0].IsVisible = true;
            };

            PlayButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                GameLoop.World.GenerateEntities();
                Parent.Parent = null;
                GameLoop.GameUIManager.Init();
            };

            Controls.Add(BackButton);
            Controls.Add(PlayButton);
        }
    }
}
