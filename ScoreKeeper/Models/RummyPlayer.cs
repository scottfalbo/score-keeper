using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models
{
    public class RummyPlayer
    {
        public int RummyId { get; set; }
        public int PlayerId { get; set; }

        public Rummy Rummy { get; set; }
        public Player Player { get; set; }
    }
}
