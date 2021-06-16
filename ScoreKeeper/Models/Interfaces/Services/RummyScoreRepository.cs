using Microsoft.EntityFrameworkCore;
using ScoreKeeper.Data;
using ScoreKeeper.Pages.Games;
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
        public async Task AddScores(int scoreOne, int scoreTwo)
        {
            Rummy game = await GetGame(1);
            if (game.RummyPlayers[0].Player.PlayerScores.Count() == 0)
            {
                await AddScore(scoreOne);
                Score scoreId = await GetLastScore();
                await AssignScore(scoreId.Id, game.RummyPlayers[0].Player.Id);
                await AddScore(scoreTwo);
                scoreId = await GetLastScore();
                await AssignScore(scoreId.Id, game.RummyPlayers[1].Player.Id);
            }
            else
            {

            }

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

        /// <summary>
        /// Get a game score sheet by id
        /// </summary>
        /// <param name="Id"> Rummy object id </param>
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

        /// <summary>
        /// Helper method to add a new score to the database
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        private async Task AddScore(int score)
        {
            Score newScore = new Score()
            {
                Points = score
            };
            _db.Entry(newScore).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }
        private async Task AssignScore(int scoreId, int playerId)
        {
            PlayerScore score = new PlayerScore()
            {
                PlayerId = playerId,
                ScoreId = scoreId
            };
            _db.Entry(score).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Helper method to get the players most recent total score
        /// </summary>
        /// <returns></returns>
        private async Task<Score> GetLastScore()
        {
            var scores = await _db.Scores
                .Select(x => new Score
                {
                    Id = x.Id,
                    Points = x.Points
                }).ToListAsync();
            return scores.Last();
        }
    }

}
