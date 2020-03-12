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
            int InputSeed = 0;
            //determines whether the inut from the user corresponds to an existing map InputSeed 
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
            //Takes user back to the main menu
            BackButton.Click += (s, e) =>
            {
                this.IsVisible = false;
                this.IsFocused = false;
                Parent.Children[0].IsVisible = true;
                
            };

            //validates the user input
            // if its valid change the menu to the Map Preview menu
            LoadMap.Click += (s, e) =>
            {       
                int index = -1;
                isValidInput = ValidateTextInput(ref InputSeed, ref TextInput, ref index);

                if(isValidInput)
                {
                    ChangeMenu(InputSeed, index);
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

        /// <summary>
        /// Loads the map's name, seed and generation type into the console
        /// By reading it fro their respective text files
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void CreateSavedMapsConsole(int width, int height)
        {
            SavedMaps = new ListTextBox(width + 25, height, "Saved Maps");
            this.Children.Add(SavedMaps);
            SavedMaps.Show();
            SavedMaps.Position = new Point(5, this.Controls.Position.Y);
            string message = "";
            //Each map's properties are indexed on the same line in the text files
            //eg if the map seed is on the 2nd line
            //that maps name and type are on the 2nd line of their respective text files
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
        }

        /// <summary>
        /// Takes the user to the map preview menu
        /// tells the world to generate the given map
        /// with the given map seed and map generation tyoe read from the text file
        /// </summary>
        /// <param name="Seed"></param>
        /// <param name="index"></param>
        private void ChangeMenu(int InputSeed, int index)
        {
            //Tells the world to create the map with the given seed and index for the other map properties
            GameLoop.World.LoadMap(InputSeed, index);
            //adds the World's map to the PreviewMenu's mapconsole/window
            GameLoop.MenuManager.PreviewMenu.CreateMap();
            //take the user to the map preview menu
            this.IsVisible = false;
            this.IsFocused = false;
            Parent.Children[3].IsVisible = true;
        }
        /// <summary>
        /// Takes the text inputed into the text box
        /// And checks if it matches with and existing map seed
        /// </summary>
        /// <param name="Seed"></param>
        /// <param name="TextInput"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ValidateTextInput(ref int InputSeed, ref TextBox TextInput, ref int index)
        {
            bool isValid = false;                        
            int CurrentLine = 0;
            GameLoop.sr = new StreamReader("MapSeeds.txt");

            //VALIDATION to check if the input is null or non numeric
            //returns false if its null or non-numeric
            if (!(int.TryParse(TextInput.EditingText, out CurrentLine)))
            {
                return false;
            }

            //sets the input seed to the numeric seed entered by the user
            InputSeed = int.Parse(TextInput.EditingText);

            //VALIDATION to check if the seed exists in the MapSeeds.txt file
            //Keep looping until the end of the text file or when the seed is found
            while (GameLoop.sr.EndOfStream == false && isValid == false && TextInput.EditingText != null)
            {                
                CurrentLine = int.Parse(GameLoop.sr.ReadLine());
                //if the inputseed matches the one on the current line
                //set isValid to true
                if (InputSeed == CurrentLine)
                {
                    isValid = true;
                }
                //increment the index with each iteration
                //this will be used to get the maps name and type in other text files 
                //since they are all indexed on the same line
                index++;
            }
            GameLoop.sr.Close();
            return isValid;
        }
    }
}
