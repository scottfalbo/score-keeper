using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models
{
    public class PlayerScore
    {
        public int PlayerId { get; set; }
        public int ScoreId { get; set; }

        public Player Player { get; set; }
        public Score Score { get; set; }
    }
}
