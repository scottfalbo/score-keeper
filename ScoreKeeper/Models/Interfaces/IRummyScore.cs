using ScoreKeeper.Models.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces
{
    public interface IRummyScore
    {
        public Task<int> StartGame(string playerOne, string playerTwo, int limit, Rummy game);
        public Task<Winner> AddScores(int scoreOne, int scoreTwo, int gameId);
        public Task DeleteGame(Rummy game);
        public Task<Rummy> GetGame(int id);
        public Task<List<Rummy>> GetGames();
        public Task ClearScoreSheet(Rummy game);
        public Task ResetCurrent(Rummy game);
    }
}
