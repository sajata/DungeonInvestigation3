using System;
using Microsoft.Xna.Framework;

//Parent class of every entity in the game 
//Including Player, Monsters and Items
//Also Extends the SadConsole.Entities.Entity class
//by adding an ID to it using GoRogues ID system
//i.e Entity inherits from the Sadconsole entity class and GoRogue's IHasID interface
//interface requieres a class use its data structures and methods but have no implementation code of their own
//i.e the intrerface IHasID tells Entity class to use the its methods and data structures
//in this case a unsigned int with the name ID

namespace Test_game.Entities
{
    public abstract class Entity : SadConsole.Entities.Entity, GoRogue.IHasID
    {
        public uint ID { get; private set; }

        protected Entity(Color foreground, Color background, int glyph, int width = 1, int height = 1) : base(width, height)
        {
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph; // glyph sets the character

            //Create a new unique identifier for this entity
            ID = Map.IDGenerator.UseID();
        }
    }
}