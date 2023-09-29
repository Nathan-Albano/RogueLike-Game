using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using ConsoleApp1.Core;
using RogueSharpV3Tutorial;
using RogueSharp.Random;

namespace ConsoleApp1.System
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        //Rooms
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;

        private readonly DungeonMap _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        public MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
        {
            _width = width;
            _height = height;

            //Rooms
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize; 

            _map = new DungeonMap();
        }

        // Generate a new map that is a simple open floor with walls around the outside
        public DungeonMap CreateMap()
        {

            // Initialize every cell in the map by
            // setting walkable, transparency, and explored to true
            _map.Initialize(_width, _height);
            Random random = new Random();

            for(int r = _maxRooms; r > 0; r--)
            {
                int roomWidth = random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = random.Next(0, _width - roomWidth - 1);
                int roomYPosition = random.Next(0, _height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);
                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }
            foreach(Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }
            PlacePlayer();

            return _map;
        }

        private void CreateRoom(Rectangle room)
        {
            for(int x = room.Left + 1; x < room.Right; x++)
            {
                for(int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }

        private void PlacePlayer()
        {
            Player player = Game.Player;
            if(player == null)
            {
                player = new Player();
            }
            player.X = _map.Rooms[0].Center.X;
            player.Y = _map.Rooms[0].Center.Y;

            _map.AddPlayer(player);
        }
    }
}
