using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models
{
    public class Rummy
    {
        public int Id { get; set; }
        public int Limit { get; set; }
        public List<RummyPlayer> RummyPlayers { get; set; }
    }
}
