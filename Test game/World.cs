using System;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using Console = SadConsole.Console;
using System.IO;
using Test_game.Tiles;
using Test_game.Entities;
namespace Test_game
{
    public class World
    {
        //Map generation data
        private int _mapWidth = 150;
        private int _mapHeight = 50;
        // will be used later
        private BaseTile[] _mapTiles;
        private const int _maxRooms = 25; // only really used in Tunneling allgorithm        
        private int _maxRoomSize = 20;
        private int _minRoomSize = 8;
        private string _mapGenType;
        private int _seed;
        private int _numMonsters = 10;

        //random number generator for the player and monsters postions        
        private Random rndNum = new Random();
        //Acts as validation if an invalid map generation type 
        private TestMapGenerator DefaultMap = new TestMapGenerator();
        //Map data
        public Map CurrentMap { get; set; }

        //Player data
        public Player Player { get; set; }

       

        //Interface for the type of map generated
        public string MapGenType { get => _mapGenType; set => _mapGenType = value; }

        //Interface for the map seed
        public int Seed { get => _seed; set => _seed = value; }

             
        
        

        public World()
        {
            _mapTiles = new BaseTile[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            CurrentMap = DefaultMap.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }
        
        /// <summary>
        /// Creates the map according tmap seed
        /// passed in by the user
        /// also gets the mapgeneration algorithm used 
        /// as its the same index as the map seed
        /// </summary>
        /// <param name="MapSeed"></param>
        /// <param name="index"></param>
        public void LoadMap(int MapSeed,int index)
        {            
            //Sets the map seed to the one read from MapSeeds.txt 
            Seed = MapSeed;
            MapGenType = ReadMapType(index);

            //Generates the map according to what map generation type has been read from the file
            switch (MapGenType)
            {
                case "angband":
                    AngbandMapGenerator MapGen1 = new AngbandMapGenerator();
                    MapGen1.Seed = MapSeed;
                    CurrentMap = MapGen1.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
                case "cellauto":
                    CellAutoGenerator MapGen2 = new CellAutoGenerator();
                    MapGen2.Seed = MapSeed;
                    CurrentMap = MapGen2.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);                   
                    break;
                case "bsp":
                    BSPGenerator MapGen3 = new BSPGenerator();
                    MapGen3.Seed = MapSeed;
                    CurrentMap = MapGen3.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
                case "drunkwalk":
                    DrunkenWalkGenerator MapGen4 = new DrunkenWalkGenerator();
                    MapGen4.Seed = MapSeed;
                    CurrentMap = MapGen4.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    
                    break;
                //if an invalid mpa generation algorithm has been passed in it generates the default map
                default:
                    TestMapGenerator MapGen0 = new TestMapGenerator();
                    CurrentMap = MapGen0.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
            }            
        }

        /// <summary>
        /// Creates the map using the map generation algorthm selected by the user
        /// </summary>
        public void CreateMap()
        {          
            switch(MapGenType)
            {
                case "angband":
                    AngbandMapGenerator MapGen1 = new AngbandMapGenerator();
                    CurrentMap = MapGen1.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    Seed = MapGen1.Seed;
                    break;
                case "cellauto":
                    CellAutoGenerator MapGen2 = new CellAutoGenerator();
                    CurrentMap = MapGen2.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    Seed = MapGen2.Seed;
                    break;
                case "bsp":
                    BSPGenerator MapGen3 = new BSPGenerator();
                    CurrentMap = MapGen3.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    Seed = MapGen3.Seed;
                    break;
                case "drunkwalk":
                    DrunkenWalkGenerator MapGen4 = new DrunkenWalkGenerator();
                    CurrentMap = MapGen4.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    Seed = MapGen4.Seed;
                    break;
                default:
                    TestMapGenerator MapGen0 = new TestMapGenerator();
                    CurrentMap = MapGen0.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
            }
        }
        /// <summary>
        /// Creates the player and monsters and adds them to the map
        /// </summary>
        public void GenerateEntities()
        {
            CreatePlayer();
            CreateMonsters();
        }

        private string ReadMapType(int index)
        {
            string MapGenType = "";
            GameLoop.srMapType = new StreamReader("MapGenTypes.txt");
            for (int i = 0; i < index + 1; i++)
            {
                if (i == index)
                {
                    MapGenType = GameLoop.srMapType.ReadLine();
                }
                GameLoop.srMapType.ReadLine();
            }

            GameLoop.srMapType.Close();
            return MapGenType;
        }


        //Creates the player 
        private void CreatePlayer()
        {
            Player = new Player(Color.Yellow, Color.Transparent);
            //This adds the viewport to the player
            //Which acts as a sort of top down camera on the player
            //Allowing to only see a portion of the map
            Player.Components.Add(new EntityViewSyncComponent()); // allows the player to appear on the map
            int PlayerPosition = 0;            

            while (CurrentMap.Tiles[PlayerPosition].IsBlockingMove)
            {
                // pick a random spot on the map
                PlayerPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap.Height);
            }

            Player.Position = new Point(PlayerPosition % CurrentMap.Width, PlayerPosition / CurrentMap.Width);

            //Adds the player to the maps collection of entities
            CurrentMap.Add(Player);
        }

        /// <summary>
        /// Creates the monsters and adds them to the map
        /// </summary>
        private void CreateMonsters()
        {         
            //stores 
            int monsterPosition = 0;
           

            //Create multiple monsters
            //and pick a random postion on the map to place them
            //check if the placement spot it floor tile
            //if not try again
            for (int i = 0; i < _numMonsters; i++)
            {
                //instantiates a monster
                Monster newMonster = new Monster(Color.Orange, Color.Transparent);
                newMonster.Components.Add(new EntityViewSyncComponent());//Allows the monster to appear on the scrolling console
                newMonster.Position = new Point(0, 0);
                monsterPosition = 0;

                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random position on the map
                    monsterPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
              
                // Set the monster's new position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }
        }
    }
}
