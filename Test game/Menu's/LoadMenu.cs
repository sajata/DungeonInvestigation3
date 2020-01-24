using System;
using SadConsole;
using SadConsole.Controls;
using System.IO;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;


namespace Test_game.Menu_s
{
    public class LoadMenu : BaseMenu
    {
         

        public LoadMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight, background, title)
        {
            this.Controls.IsFocused = true;
        }

        public override void CreateControls()
        {
            int ButtonWidth = 12;
            int ButtonHeight = 3;
            int Seed = 0;
            bool isValidInput = false;

            TextBox TextInput = new TextBox(20)
            {
                Position = new Point(10, 10)
            };

            TextInput.IsNumeric = true;

            Button BackButton = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(14, 17),
                Text = "Back",
                TextAlignment = HorizontalAlignment.Center,
                Name = "BackButton",               
            };

            Button LoadMap = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(14, 14),
                Text = "Load",
                TextAlignment = HorizontalAlignment.Center,
                Name = "LoadButton"
            };

            //event handlers

            BackButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[0].IsVisible = true;
                
            };

            LoadMap.Click += (s, e) =>
            {       
                int index = -1;
                isValidInput = ValidateTextInput(ref Seed, ref TextInput, ref index);

                if(isValidInput)
                {
                    ChangeMenu(Seed, index);
                }                
                else
                {
                    TextInput.Text = "Invalid Seed";
                }
            };

            Controls.Add(TextInput);
            Controls.Add(BackButton);
            Controls.Add(LoadMap);
        }

        private void ChangeMenu(int Seed, int index)
        {
            GameLoop.World.LoadMap(Seed, index);
            GameLoop.MenuManager.PreviewMenu.CreateMap();
            this.IsVisible = false;
            this.IsFocused = false;
            Parent.Children[3].IsVisible = true;
        }

        private bool ValidateTextInput(ref int Seed, ref TextBox TextInput, ref int index)
        {
            bool isValid = false;
            Seed = int.Parse(TextInput.EditingText);
            int CurrentLine = 0;
            GameLoop.sr = new StreamReader("MapSeeds.txt");

            while (GameLoop.sr.EndOfStream == false && isValid == false)
            {
                CurrentLine = int.Parse(GameLoop.sr.ReadLine());
                if (Seed == CurrentLine)
                {
                    isValid = true;
                }                
                index++;
            }
            GameLoop.sr.Close();
            return isValid;
        }
    }
}
