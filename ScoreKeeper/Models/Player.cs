using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public List<PlayerScore> PlayerScores { get; set; }
        public Rummy Rummy { get; set; }
    }
}
