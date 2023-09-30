using RLNET;
using ConsoleApp1.Core;
using ConsoleApp1.System;
using RogueSharp.Random;
using System.Drawing.Text;

namespace RogueSharpV3Tutorial
{

    public class Game
    {
        //Height and Width of Screen 
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70 ;
        private static RLRootConsole _rootConsole;

        //Map
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        //Message
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        //Stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        //Inventory
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        //private static int _steps = 0;
        public static DungeonMap DungeonMap { get; private set; }

        public static bool _renderRequired = true;
        public static CommandSystem CommandSystem { get; private set; }

        public static IRandom Random { get; private set; }

        public static Player Player { get; set; }

        public static MessageLog MessageLog { get; private set; }
        public static void Main()
        {
            string fontFileName = "terminal8x8.png";
            string consoleTitle = "RogueSharp V3 Tutorial - Level 1";



            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7);
            DungeonMap = mapGenerator.CreateMap();

            DungeonMap.UpdatePlayerFieldOfView();

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            /*
            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            _messageConsole.Print(1, 1, "MESSAGES", RLColor.White);
            */
            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.Gold3);
            _statConsole.Print(1, 1, "STATS", RLColor.White);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.Blue4);
            _inventoryConsole.Print(1, 1, "INVENTORY", RLColor.White);

            CommandSystem = new CommandSystem();

            //RNG
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            string consoletitle = $"RogueSharp V3 Tutorial - Level 1- Seed {seed}";


            //Message Log
            MessageLog = new MessageLog();
            MessageLog.Add("The rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            _rootConsole.Run();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e) 
        {
            //Moved to main so it isn't continuously being called
            /*
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "MAP", RLColor.White);
            
            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            _messageConsole.Print(1, 1, "MESSAGES", RLColor.White);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.Gold3);
            _statConsole.Print(1, 1, "STATS", RLColor.White);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.Blue4);
            _inventoryConsole.Print(1, 1, "INVENTORY", RLColor.White);
            */

            //Event Handler
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Up || keyPress.Key == RLKey.W)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if (keyPress.Key == RLKey.Down || keyPress.Key == RLKey.S)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if (keyPress.Key == RLKey.Left || keyPress.Key == RLKey.A)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if (keyPress.Key == RLKey.Right || keyPress.Key == RLKey.D)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if (keyPress.Key == RLKey.Escape)
                {
                    _rootConsole.Close();
                }
            }
       
            if (didPlayerAct)
            {
                //MessageLog.Add($"Step # {++_steps}");
                _renderRequired = true;
            }
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e) 
        {            
            //Event Handler 
            if(_renderRequired)
            {
                DungeonMap.Draw(_mapConsole);
                MessageLog.Draw(_messageConsole);
                Player.Draw(_mapConsole, DungeonMap);
                //Blit
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
               
                
                
                _rootConsole.Draw();
                _renderRequired = false;
            }
            
            
        }
    }
}