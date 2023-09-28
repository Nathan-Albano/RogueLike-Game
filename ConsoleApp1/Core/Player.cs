﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace ConsoleApp1.Core
{
    public class Player : Actor
    {
        public Player() 
        {
            Awareness = 15;
            Name = "Rogue";
            Color = Colors.Player;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
        
    }
}
