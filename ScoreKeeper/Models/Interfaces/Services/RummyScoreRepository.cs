using Microsoft.EntityFrameworkCore;
using ScoreKeeper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces.Services
{
    public class RummyScoreRepository : IRummyScore
    {
        public ScoreKeeperDbContext _db;

        public RummyScoreRepository(ScoreKeeperDbContext context)
        {
            _db = context;
        }
        public void AddScores(int scoreOne, int scoreTwo)
        {
            throw new NotImplementedException();
        }

        public void ContinueGame(string SaveAs)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame()
        {
            throw new NotImplementedException();
        }

        public void StartGame(string playerOne, string playerTwo, string save)
        {

        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool SaveExists(string save)
        {
            return false;
        }

        private void SaveGame(Rummy game)
        {

        }

        public async Task<Rummy> GetGame(int Id)
        {
            return await _db.Rummy
                .Where(x => x.Id == 1)
                .Include(y => y.RummyPlayers)
                .ThenInclude(a => a.Player)
                .ThenInclude(b => b.PlayerScores)
                .ThenInclude(c => c.Score)
                .Select(z => new Rummy
                {
                    Id = z.Id,
                    SaveAs = z.SaveAs,
                    RummyPlayers = z.RummyPlayers
                }).FirstOrDefaultAsync();
        }
    }
}
