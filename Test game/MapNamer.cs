using System;
using SadConsole;
using SadConsole.Controls;
using Console = SadConsole.Console;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Test_game
{
    /// <summary>
    /// A console that takes user input and validates it 
    /// Names the map 
    /// </summary>
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


            //Sets the buttons theme/appearance to the 3D box theme
            //in my opinion more intuitive than the default
            //This is for purely aesthetic purposes
            var consoletheme = SadConsole.Themes.Library.Default.Clone();
            consoletheme.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme();
            consoletheme.TextBoxTheme = new SadConsole.Themes.TextBoxTheme();
            Controls.Theme = consoletheme;

            this.Children.Add(this.Controls);
        }
        private void CreateControls()
        {           
            //instantiates the controls
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

            //event handlers for when the button is pressed

            //executes GameUIManagers.SaveMap function if the 
            //input in the input box
            //is valid
            SaveButton.Click += (s, e) =>
            {
                isValidInput = ValidateInput(ref InputBox);

                if(isValidInput)
                {
                    //sets the string in the input box as the map name
                    GameLoop.GameUIManager.MapName = InputBox.EditingText;
                    //tells GameUiManager to svae the map
                    GameLoop.GameUIManager.SaveMap();
                    //Quits the game
                    SadConsole.Game.Instance.Exit();
                }
                else
                {
                    InputBox.Text = "Invalid Name";
                }
            };
            //adds the controls to the controls console
            Controls.Add(SaveButton);
            Controls.Add(InputBox);
        }
        
        /// <summary>
        /// Validates the text input from the text box        
        /// </summary>
        /// <param name="InputBox"></param>
        /// <returns></returns>
        private bool ValidateInput(ref TextBox InputBox)
        {
            bool isValid = false;
            string input = InputBox.EditingText;
            
            //VALIDATION : if the input is null or just spaces and its length is too long
            //its invalid
            if(string.IsNullOrWhiteSpace(input) || input.Length > 8)
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
