using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models
{
    public class Rummy
    {
        public PlayerOne PlayerOne { get; set; }
        public PlayerTwo PlayerTwo { get; set; }
        public string  SaveAs { get; set; }
    }

    public class PlayerOne
    {
        public string Name { get; set; }
        public List<int> Score { get; set; }
        public int Wins { get; set; }
    }
    public class PlayerTwo
    {
        public string Name { get; set; }
        public List<int> Score { get; set; }
        public int Wins { get; set; }
    }
}
