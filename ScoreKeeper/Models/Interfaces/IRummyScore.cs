using ScoreKeeper.Models.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces
{
    public interface IRummyScore
    {
        public Task<int> StartGame(string playerOne, string playerTwo, int limit, int currentId);
        public Task<Winner> AddScores(int scoreOne, int scoreTwo, int gameId);
        public void Undo();
        public void DeleteGame(int id);
        public Task<Rummy> GetGame(int id);
        public Task ClearScoreSheet(Rummy game);
    }
}
