﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces
{
    public interface IRummyScore
    {
        public void StartGame(string playerOne, string playerTwo, string SaveAs);
        public void ContinueGame(string SaveAs);
        public void AddScores(int scoreOne, int scoreTwo);
        public void Undo();
        public void DeleteGame();
    }
}
