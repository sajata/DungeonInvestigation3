using System;
using SadConsole;
using SadConsole.Controls;
using Console = SadConsole.Console;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Test_game
{
    public class MapNamer : Console
    {
        public ControlsConsole Controls;
        private bool isValidInput;
        public MapNamer(int width, int height) : base(width, height)
        {
            Controls = new ControlsConsole(width, height);
            IsVisible = false;
            IsFocused = false;
            CreateControls();

            

            var consoletheme = SadConsole.Themes.Library.Default.Clone();
            consoletheme.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme();
            consoletheme.TextBoxTheme = new SadConsole.Themes.TextBoxTheme();
            Controls.Theme = consoletheme;

            this.Children.Add(this.Controls);
        }
        private void CreateControls()
        {           
            
            TextBox InputBox = new TextBox(20)
            {
                Position = new Point(10, 10),
                MaxLength = 8, // so the text in the SavedMapsConsole in the LoadMenu doesn't overflow
            };            

            Button SaveButton = new Button(12, 3)
            {
                Position = new Point(14, 14),
                Text = "Save Map",
                TextAlignment = HorizontalAlignment.Center,
                Name = "SaveButton"
            };

            //event handlers

            SaveButton.Click += (s, e) =>
            {
                isValidInput = ValidateInput(ref InputBox);

                if(isValidInput)
                {
                    GameLoop.GameUIManager.MapName = InputBox.EditingText;
                    GameLoop.GameUIManager.SaveMap();
                    SadConsole.Game.Instance.Exit();
                }
                else
                {
                    InputBox.Text = "Invalid Name";
                }
            };

            Controls.Add(SaveButton);
            Controls.Add(InputBox);
        }
        

        private bool ValidateInput(ref TextBox InputBox)
        {
            bool isValid = false;
            string input = InputBox.EditingText;

            if(input.Length <= 0 || input == " " || input.Length > 20)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
