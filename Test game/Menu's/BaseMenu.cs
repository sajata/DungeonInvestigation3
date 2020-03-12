using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace Test_game
{
    /// <summary>
    // Parent class acts as template for each menu with the exception of the GameUIManager
    // It's also a child of the SadConsole.Console class since each menu is effectively a console
    /// </summary>
    public abstract class BaseMenu : Console
    {
        //this console is designed for storing the controls
        //stores most of the controls in each menu
        protected ControlsConsole Controls;

        public BaseMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height) // base parameters means the parameters that have to be passed into the parent class Console
        {
            this.Width = width;
            this.Height = height;
            this.DefaultBackground = background;
            //sets the console to be invisible by default
            this.IsVisible = false;
            this.IsFocused = false;

            //Prints the title which tells the user what this menu is and what to do
            this.Print(6, 6, title);

            //Creates the controls console which acts like a contianer console
            //but instead of containing consoles it contains UI elements such as buttons
            Controls = new ControlsConsole(controlsConsoleWidth, controlsConsoleHeight)
            {
                Parent = this,
                Position = new Point((width - controlsConsoleWidth)/2,(height - controlsConsoleHeight)/2)
            };

            //Sets the buttons theme/appearance to the 3D box theme
            //in my opinion more intuitive than the default
            //This is for purely aesthetic purposes
            var consoletheme = SadConsole.Themes.Library.Default.Clone();
            consoletheme.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme();
            consoletheme.TextBoxTheme = new SadConsole.Themes.TextBoxTheme();
            Controls.Theme = consoletheme;

            //creates the controls
            CreateControls();

        }


        /// <summary>
        /// This is mainly used to instantiate all of the ui elements and add them to the controls console
        /// as well as their event handlers
        /// each menu has it's own unique set of controls
        /// </summary>
        public virtual void CreateControls()
        {

        }
    }
}
