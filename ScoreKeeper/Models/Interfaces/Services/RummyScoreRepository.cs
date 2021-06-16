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

        /// <summary>
        /// Add current rounds score to each players running total
        /// Adds the new scores and new running total to database and view
        /// </summary>
        /// <param name="scoreOne"> player one score </param>
        /// <param name="scoreTwo"> player two score </param>
        public async Task AddScores(int scoreOne, int scoreTwo)
        {
            Rummy game = await GetGame(1);
            if (game.RummyPlayers[0].Player.PlayerScores.Count() == 0)
            { 
                await ScoreController(scoreOne, game, 0);
                await ScoreController(scoreTwo, game, 1);
            }
            else
            {
                List<Score> scores = await GetScores();
                await ScoreController(scoreOne, game, 0);
                await ScoreController(scoreTwo, game, 1);
                int playerOneTotal = scoreOne + scores[^2].Points;
                int playerTwoTotal = scoreTwo + scores[^1].Points;
                await ScoreController(playerOneTotal, game, 0);
                await ScoreController(playerTwoTotal, game, 1);

                if (playerOneTotal >= 1000 || playerTwoTotal >= 1000)
                    await Winner(game.Id);
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

        ///---------------------- Helper methods for adding score -------------------------
        /// <summary>
        /// Controller method to make a series of other method calls
        /// </summary>
        /// <param name="score"> score to add </param>
        /// <param name="game"> Rummy object </param>
        /// <param name="playerId"> player id to add score to </param>
        /// <returns></returns>
        private async Task ScoreController(int score, Rummy game, int playerIndex)
        {
            await AddScore(score);
            Score playerOneScoreId = (await GetScores()).Last();
            await AssignScore(playerOneScoreId.Id, game.RummyPlayers[playerIndex].Player.Id);
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

        /// <summary>
        /// Assign the new score to the player PlayerScore List
        /// </summary>
        /// <param name="scoreId"> score id </param>
        /// <param name="playerId"> player id </param>
        /// <returns></returns>
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
        /// Helper method to get the players list of scores
        /// </summary>
        /// <returns></returns>
        private async Task<List<Score>> GetScores()
        {
            return await _db.Scores
                .Select(x => new Score
                {
                    Id = x.Id,
                    Points = x.Points
                }).ToListAsync();
        }

        /// <summary>
        /// If a player wins it will update there Wins total and reset the scores to 0
        /// </summary>
        private async Task Winner(int id)
        {
            Rummy game = await GetGame(id);
            Console.WriteLine("");

            if (game.RummyPlayers[0].Player.PlayerScores[^1].Score.Points >
                game.RummyPlayers[1].Player.PlayerScores[^1].Score.Points)
            {
                game.RummyPlayers[0].Player.Wins++;
            }
            else
                game.RummyPlayers[1].Player.Wins++;

            await ScoreController(0, game, 0);
            await ScoreController(0, game, 1);
        }
        /// ------------------------ End score adding methods-----------------------
    }
}
