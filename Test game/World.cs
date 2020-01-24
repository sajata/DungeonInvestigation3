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
        private const int _maxRooms = 17;
        private int _maxRoomSize = 20;
        private int _minRoomSize = 8;
        
        //Map data
        public Map CurrentMap { get; set; }

        //Player data
        public Player Player { get; set; }

        //Test monster data
        public Monster TestMonster { get; set; }

        private TestMapGenerator DefaultMap = new TestMapGenerator();        

        public string MapGenType;

        public int Seed;

        public World()
        {
            _mapTiles = new BaseTile[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            CurrentMap = DefaultMap.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }
        
        public void LoadMap(int MapSeed,int index)
        {            
            Seed = MapSeed;
            GameLoop.srMapType = new StreamReader("MapGenTypes.txt");
            for (int i = 0; i < index +1; i++)
            {
                if(i == index)
                {
                    MapGenType = GameLoop.srMapType.ReadLine();
                }
                GameLoop.srMapType.ReadLine();
            }

            GameLoop.srMapType.Close();

            switch (MapGenType)
            {
                case "tunneling":
                    TunnelingMapGenerator MapGen1 = new TunnelingMapGenerator();
                    MapGen1.Seed = MapSeed;
                    CurrentMap = MapGen1.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
                case "cellauto":
                    CellAutoGenerator MapGen2 = new CellAutoGenerator();
                    MapGen2.Seed = MapSeed;
                    CurrentMap = MapGen2.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                   
                    break;
                default:
                    TestMapGenerator MapGen0 = new TestMapGenerator();
                    CurrentMap = MapGen0.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
            }            
        }

        public void CreateMap()
        {
            _mapTiles = new BaseTile[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);

            switch(MapGenType)
            {
                case "tunneling":
                    TunnelingMapGenerator MapGen1 = new TunnelingMapGenerator();
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
                default:
                    TestMapGenerator MapGen0 = new TestMapGenerator();
                    CurrentMap = MapGen0.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
                    break;
            }
        }

        public void GenerateEntities()
        {
            CreatePlayer();
            CreateMonsters();
        }

        //Creates the player 
        private void CreatePlayer()
        {
            Player = new Player(Color.Yellow, Color.Transparent);
            //This adds the viewport to the player
            //Which acts as a sort of top down camera on the player
            //Allowing to only see a portion of the map
            Player.Components.Add(new EntityViewSyncComponent());
            int PlayerPosition = 0;
            Random rndNum = new Random();

            while (CurrentMap.Tiles[PlayerPosition].IsBlockingMove)
            {
                // pick a random spot on the map
                PlayerPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap.Height);
            }

            Player.Position = new Point(PlayerPosition % CurrentMap.Width, PlayerPosition / CurrentMap.Width);

            //Adds the player to the maps collection of entities
            CurrentMap.Add(Player);
        }

        //Creates a Monster 
        private void CreateMonsters()
        {
            int monsterPosition = 0;
            //Random position generator
            Random rndNum = new Random();
            //Sets the monster starting postion
            //to the center of the map
            TestMonster = new Monster(Color.Orange, Color.Transparent);
            TestMonster.Components.Add(new EntityViewSyncComponent());//Allows the monster to appear on the scrolling console without moving with the players viewport
            TestMonster.Position = new Point(40, 40);

            while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
            {
                // pick a random spot on the map
                monsterPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap.Height);
            }

            // Set the monster's new position
            TestMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);

            //Adds the monster to the maps collection of entities
            CurrentMap.Add(TestMonster);
        }
    }
}
