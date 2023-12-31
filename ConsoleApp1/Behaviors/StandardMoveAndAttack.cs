﻿using ConsoleApp1.Core;
using ConsoleApp1.System;
using RogueSharp;
using RogueSharpV3Tutorial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Behaviors
{
    public class StandardMoveAndAttack : IBehavior
    {
        public bool Act(Monster monster, CommandSystem commandSystem)
        {
            DungeonMap dungeonMap = Game.DungeonMap;
            Player player = Game.Player;
            FieldOfView monsterFov = new FieldOfView(dungeonMap);

            if (!monster.TurnsAlerted.HasValue)
            {
                monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);
                if(monsterFov.IsInFov(player.X, player.Y))
                {
                    Game.MessageLog.Add($"{monster.Name} is eager to fight {player.Name}");
                    monster.TurnsAlerted = 1;
                }
            }

            if (monster.TurnsAlerted.HasValue)
            {
                dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
                dungeonMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder pathFinder = new PathFinder(dungeonMap);
                RogueSharp.Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(dungeonMap.GetCell(monster.X, monster.Y), dungeonMap.GetCell(player.X, player.Y));
                }
                catch (PathNotFoundException)
                {
                    Game.MessageLog.Add($"{monster.Name} waits for a turn");
                }

                dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
                dungeonMap.SetIsWalkable(player.X, player.Y, false);

                if(path != null)
                {
                    try
                    {
                        commandSystem.MoveMonster(monster, (Cell) path.StepForward());
                    }
                    catch (NoMoreStepsException)
                    {
                        Game.MessageLog.Add($"{monster.Name} growls in frustration");
                    }
                }

                monster.TurnsAlerted++;
                if(monster.TurnsAlerted > 15)
                {
                    monster.TurnsAlerted = null;
                }
            }
            return true;
        }
    }
}
