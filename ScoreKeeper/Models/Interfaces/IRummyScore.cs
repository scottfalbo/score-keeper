using ScoreKeeper.Models.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces
{
    public interface IRummyScore
    {
        public Task<int> StartGame(string playerOne, string playerTwo, int limit);
        public void ContinueGame(string SaveAs);
        public Task<Winner> AddScores(int scoreOne, int scoreTwo, int gameId);
        public void Undo();
        public void DeleteGame();
        public Task<Rummy> GetGame(int Id);
        public Task ClearScoreSheet(Rummy game);
    }
}
