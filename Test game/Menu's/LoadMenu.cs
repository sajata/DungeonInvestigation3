using System;
using SadConsole;
using SadConsole.Controls;
using System.IO;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;


namespace Test_game.Menu_s
{ 
    
    //Responisble for letting the user enter a Map Seed of a 
    //map they have saved
    public class LoadMenu : BaseMenu
    {
        //Will display all of the saved map seeds on the local machine
        private ListTextBox SavedMaps;
        
        public LoadMenu(int width, int height, int controlsConsoleWidth, int controlsConsoleHeight, Color background, string title) : base(width, height, controlsConsoleWidth, controlsConsoleHeight, background, title)
        {
            //allows text input which is unique to this menu
            this.Controls.IsFocused = true;
            CreateSavedMapsConsole(controlsConsoleWidth, controlsConsoleHeight);
        }

        public override void CreateControls()
        {
            //The button width and height is the same for each button
            int ButtonWidth = 12;
            int ButtonHeight = 3;
            //Seed is stores the number read from the textbox 
            int Seed = 0;
            //determines whether the inut from the user corresponds to an existing map seed 
            //saved by the user
            bool isValidInput = false;

            //Instantiates the text box with its width and position within the controls console
            TextBox TextInput = new TextBox(20)
            {
                Position = new Point(10, 10)
            };           

            //Instantiates back button which brings the user back to the main menu
            Button BackButton = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(14, 17),
                Text = "Back",
                TextAlignment = HorizontalAlignment.Center,
                Name = "BackButton",               
            };

            //Instantiates LoadMap button 
            Button LoadMap = new Button(ButtonWidth, ButtonHeight)
            {
                Position = new Point(14, 14),
                Text = "Load",
                TextAlignment = HorizontalAlignment.Center,
                Name = "LoadButton"
            };

            //event handlers

            //When pressed makes this menu invisible and MainMenu visible
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

            //Adds all of the controls to the controls console so they can be rendered 
            Controls.Add(TextInput);
            Controls.Add(BackButton);
            Controls.Add(LoadMap);
        }

        private void CreateSavedMapsConsole(int width, int height)
        {
            SavedMaps = new ListTextBox(width + 25, height, "Saved Maps");
            this.Children.Add(SavedMaps);
            SavedMaps.Show();
            SavedMaps.Position = new Point(5, this.Controls.Position.Y);
            string message = "";
            GameLoop.sr = new StreamReader("MapSeeds.txt");
            GameLoop.srMapType = new StreamReader("MapGenTypes.txt");
            GameLoop.srMapName = new StreamReader("MapNames.txt");
            while (GameLoop.sr.EndOfStream == false && GameLoop.srMapType.EndOfStream == false && GameLoop.srMapName.EndOfStream == false)
            {
                message = "Map Name: " + GameLoop.srMapName.ReadLine(); 
                message += " | Map Seed: " + GameLoop.sr.ReadLine();
                message += " | Map Type: " + GameLoop.srMapType.ReadLine();
                SavedMaps.Add(message);
            }
            GameLoop.sr.Close();
            GameLoop.srMapType.Close();
            GameLoop.srMapName.Close();
            //for(int i =0; i < 40; i++)
            //{
            //    SavedMaps.Add("testing " + i);
            //}
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
            int CurrentLine = 0;
            GameLoop.sr = new StreamReader("MapSeeds.txt");

            //Checks if the input is null or non numeric
            if (!(int.TryParse(TextInput.EditingText, out CurrentLine)))
            {
                return false;
            }

            Seed = int.Parse(TextInput.EditingText);

            while (GameLoop.sr.EndOfStream == false && isValid == false && TextInput.EditingText != null)
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
