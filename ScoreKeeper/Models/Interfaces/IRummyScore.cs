using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces
{
    public interface IRummyScore
    {
        public void StartGame(string playerOne, string playerTwo, string SaveAs);
        public void ContinueGame(string SaveAs);
        public Task<bool> AddScores(int scoreOne, int scoreTwo);
        public void Undo();
        public void DeleteGame();
        public bool SaveExists(string save);
        public Task<Rummy> GetGame(int Id);
        public Task ClearScoreSheet(Rummy game);
    }
}
