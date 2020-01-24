using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Test_game
{
    public abstract class BaseMenu : Console
    {
        protected ControlsConsole Controls;

        public BaseMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height)
        {
            this.Width = width;
            this.Height = height;
            this.DefaultBackground = background;
            this.IsVisible = false;
            this.IsFocused = false;

            this.Print(6, 6, title);

            Controls = new ControlsConsole(controlsConsoleWidth, controlsConsoleHeight)
            {
                Parent = this,
                Position = new Point((width - controlsConsoleWidth)/2,(height - controlsConsoleHeight)/2)
            };

            var consoletheme = SadConsole.Themes.Library.Default.Clone();
            consoletheme.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme();
            consoletheme.TextBoxTheme = new SadConsole.Themes.TextBoxTheme();
            Controls.Theme = consoletheme;

            CreateControls();

        }

        public virtual void CreateControls()
        {

        }
    }
}
