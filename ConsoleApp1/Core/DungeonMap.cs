using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using RogueSharpV3Tutorial;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Core
{
    public class DungeonMap : Map
    {
        private readonly List<Monster> _monsters;
        public List<Rectangle> Rooms;
        Random random = new Random();
        public List<Door> Doors;
        
        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
            int i = 0;
            foreach(Monster monster in _monsters)
            {
                monster.Draw(mapConsole, this);
                if(IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(statConsole, i);
                    i++;
                }
            }
            foreach(Door door in Doors)
            {
                door.Draw(mapConsole, this);
            }
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            // When we haven't explored a cell yet, we don't want to draw anything
            if (!cell.IsExplored)
            {
                return;
            }

            // When a cell is currently in the field-of-view it should be drawn with ligher colors
            if (IsInFov(cell.X, cell.Y))
            {
                // Choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            ComputeFov(player.X, player.Y, player.Awareness, true);
            foreach (Cell cell in GetAllCells())
            {
                if(IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public bool SetActorPosition(Actor actor, int x, int y)
        {
            if(GetCell (x , y).IsWalkable)
            {
                SetIsWalkable(actor.X, actor.Y, true);
                
                actor.X = x;
                actor.Y = y;

                SetIsWalkable(actor.X, actor.Y, false);
                OpenDoor(actor, x, y);
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable) 
        {
            ICell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

       

        public DungeonMap()
        {
            Rooms = new List<Rectangle> ();
            _monsters = new List<Monster>();
            Doors = new List<Door>();
        }

        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X , player.Y, false);
            UpdatePlayerFieldOfView();
            Game.SchedulingSystem.Add(player);
        }
        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            SetIsWalkable(monster.X, monster.Y, false);
            Game.SchedulingSystem.Add(monster);
        }

        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(1, room.Width - 2) + room.X;
                    int y = random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return (Point)default;
        }

        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
            SetIsWalkable(monster.X, monster.Y, true);
            Game.SchedulingSystem.Remove(monster);
        }

        public Monster GetMonsterAt(int x, int y)
        {
            return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
        }

   


        public Door GetDoor(int x, int y)
        {
            return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
        }

        private void OpenDoor(Actor actor, int x, int y)
        {
            Door door = GetDoor(x, y);
            if(door != null && !door.IsOpen)
            {
                door.IsOpen = true;
                var cell = GetCell(x, y);
                SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);
                Game.MessageLog.Add($"{actor.Name} opened a door");
            }
        }
        
    }
}
