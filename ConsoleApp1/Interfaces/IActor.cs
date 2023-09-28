using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace ConsoleApp1.Interfaces
{
    public interface IActor
    {
        string Name { get; set; }
        int Awareness { get; set; }
    }
}
