using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Test_game.Tiles;
using Test_game.Entities;

namespace Test_game
{
    public class Map
    {
        private int _width; // width of map
        private int _height; // 
       
        BaseTile[] _tiles; // stores all of the tiles on the map

        public BaseTile[] Tiles { get { return _tiles; } set { _tiles = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        
        //MultiSpatial map in this case stores all the entities on the map
        //And allows for entities to be on the same postion on the map
        public GoRogue.MultiSpatialMap<Entity> Entities = new GoRogue.MultiSpatialMap<Entity>(); // Keeps track of all entities on the map
        public static GoRogue.IDGenerator IDGenerator = new GoRogue.IDGenerator(); // static IDGenerator which all entities can access

        //Builds map with specified width and length
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new BaseTile[Width * Height]; // creates a 2D array the size of the map to contain all tiles 
        }

        //IsTileWalkable checks
        //if the actor has tried to move off the map limits
        //or into a non-walkable tile
        //returns true if you can walk
        //false otherwise
        public bool IsTileWalkable(Point location)
        {
            //Checks if the actor isn't trying to move of the limits of the map
            if (location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
            {
                return false;
            }
            //then returns wether the tile is walkables
            //by returning the opposite of what IsBlockingMove returns
            return !_tiles[location.Y * Width + location.X].IsBlockingMove;
        }

        //Resturns the location of a specific tile
        //int the map's array of tiles
        public Point GetTilePosition(BaseTile Tile)
        {
            Point Position = new Point();
            int TileIndex = Array.IndexOf(_tiles, Tile);

            Position.Y = (TileIndex / _width);

            if (TileIndex % _width > 0)
            {
                Position.Y += 1;
            }

            Position.X = (TileIndex % _width);

            return Position;
        }


        // Checking whether a certain type of
        // entity is at a specified location the manager's list of entities
        // and if it exists, return that Entity
        // T stands for the type of object were interested in finding
        // T can be any object that is a child of Entity
        // essentially this this is the same as : "public Player GetPlayerAt (Point location)"
        public T GetEntityAt<T>(Point location) where T : Entity
        {
            //searches through the multiSpatialMap for object of type T
            //at Point location and returns the first object it finds at that location 
            //of type T
            return Entities.GetItems(location).OfType<T>().FirstOrDefault();
        }

        // Removes an Entity from the MultiSpatialMap
        public void Remove(Entity entity)
        {
            // remove from SpatialMap
            Entities.Remove(entity);

            // unsubscribes the OnEntityMoved handler form entitys moved event
            // i.e stops this entity triggering OnEntity Moved event
            entity.Moved -= OnEntityMoved;
        }

        // Adds an Entity to the MultiSpatialMap
        public void Add(Entity entity)
        {
            // add entity to the SpatialMap
            Entities.Add(entity, entity.Position);

            // Link up the entity's Moved event to a new handler
            // i.e everytime the entity moves it triggers the OnEntityMoved
            entity.Moved += OnEntityMoved;
        }

        // When the Entity's .Moved value changes, it triggers this event handler
        // which updates the Entity's current position in the SpatialMap
        private void OnEntityMoved(object sender, Entity.EntityMovedEventArgs args)
        {
            Entities.Move(args.Entity as Entity, args.Entity.Position);
        }
    }
}
