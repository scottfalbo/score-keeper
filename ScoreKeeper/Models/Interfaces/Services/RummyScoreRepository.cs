using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ScoreKeeper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

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
        /// ///<returns> true if winner </returns>
        public async Task<Winner> AddScores(int scoreOne, int scoreTwo, int gameId)
        {
            Rummy game = await GetGame(gameId);
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
                scoreOne += scores[^2].Points;
                scoreTwo += scores[^1].Points;
                await ScoreController(scoreOne, game, 0);
                await ScoreController(scoreTwo, game, 1);

            }
            return await CheckWinner(scoreOne, scoreTwo, game);
        }



        public void ContinueGame(string SaveAs)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame()
        {
            throw new NotImplementedException();
        }

        public async Task<int> StartGame(string playerOne, string playerTwo, int limit)
        {
            await MakeNewGame(limit);
            int gameId = GetGameId().Result;
            await CreatePlayer(playerOne);
            int playerOneId = await GetPlayerId();
            await CreatePlayer(playerTwo);
            int playerTwoId = await GetPlayerId();
            await AssignPlayer(gameId, playerOneId);
            await AssignPlayer(gameId, playerTwoId);

            return gameId;
        }

        private async Task AssignPlayer(int gameId, int playerId)
        {
            RummyPlayer player = new RummyPlayer()
            {
                PlayerId = playerId,
                RummyId = gameId
            };
            _db.Entry(player).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        private async Task CreatePlayer(string name)
        {
            Player newPlayer = new Player()
            {
                Name = name,
                Wins = 0
            };
            _db.Entry(newPlayer).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        private async Task<int> GetPlayerId()
        {
            List<Player> games = await _db.Players
                .Select(x => new Player
                {
                    Id = x.Id,
                    Name = x.Name,
                    Wins = x.Wins
                }).ToListAsync();
            return (games.Last()).Id;
        }

        private async Task MakeNewGame(int limit)
        {
            Rummy game = new Rummy()
            {
                Limit = limit
            };
            _db.Entry(game).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        private async Task<int> GetGameId()
        {
            List<Rummy> games = await _db.Rummy
                .Select(x => new Rummy
                {
                    Id = x.Id,
                    Limit = x.Limit
                }).ToListAsync();
            return (games.Last()).Id;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get a game score sheet by id
        /// </summary>
        /// <param name="Id"> Rummy object id </param>
        public async Task<Rummy> GetGame(int id)
        {
            return await _db.Rummy
                .Where(x => x.Id == id)
                .Include(y => y.RummyPlayers)
                .ThenInclude(a => a.Player)
                .ThenInclude(b => b.PlayerScores)
                .ThenInclude(c => c.Score)
                .Select(z => new Rummy
                {
                    Id = z.Id,
                    RummyPlayers = z.RummyPlayers
                }).FirstOrDefaultAsync();
        }

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
            else if (game.RummyPlayers[1].Player.PlayerScores[^1].Score.Points >
                game.RummyPlayers[0].Player.PlayerScores[^1].Score.Points)
            {
                game.RummyPlayers[1].Player.Wins++;
            }
            await ClearScoreSheet(game);
        }

        /// <summary>
        /// Checks for a winner and returns and object with winner info
        /// </summary>
        /// <param name="playerOne"> player name </param>
        /// <param name="playerTwo"> player name </param>
        /// <param name="game"> Rummy object</param>
        /// <returns> Winner object </returns>
        private async Task<Winner> CheckWinner(int playerOne, int playerTwo, Rummy game)
        {
            Winner gameOver = new Winner()
            {
                GameOver = false
            };
            if (playerOne >= 1000 || playerTwo >= 1000)
            {
                gameOver.AWinnerIsYou = playerOne > playerTwo ?
                    game.RummyPlayers[0].Player.Name : game.RummyPlayers[1].Player.Name;
                if (playerOne == playerTwo) gameOver.AWinnerIsYou = "It's a tie!";
                gameOver.GameOver = true;
                gameOver.PlayerOneScore = playerOne;
                gameOver.PlayerTwoScore = playerTwo;
                await Winner(game.Id);
            }
            return gameOver;
        }

        /// <summary>
        /// Driver method that clears the score sheet and removes data from database after each game
        /// </summary>
        /// <param name="game"> Rummy object </param>
        public async Task ClearScoreSheet(Rummy game)
        {
            await RemovePlayerScores(game.RummyPlayers[0].Player);
            await RemovePlayerScores(game.RummyPlayers[1].Player);
        }
        /// <summary>
        /// Go through the players PlayerScore List and remove each Score and Join Table
        /// </summary>
        /// <param name="player"> Player object </param>
        private async Task RemovePlayerScores(Player player)
        {
            List<PlayerScore> playerScores = new List<PlayerScore>();
            foreach (PlayerScore score in player.PlayerScores)
            {
                PlayerScore playerScore = await _db.PlayerScores
                    .FirstOrDefaultAsync(x => x.ScoreId == score.ScoreId
                                        && x.PlayerId == score.PlayerId);
                playerScores.Add(playerScore);
            }
            foreach (PlayerScore playerScore in playerScores)
            {
                _db.Entry(playerScore).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                await RemoveScore(playerScore.ScoreId);
            }
        }

        /// <summary>
        /// Remove a Score from the database by id
        /// </summary>
        /// <param name="id"> score id </param>
        private async Task RemoveScore(int id)
        {
            Score score = await _db.Scores.FindAsync(id);
            _db.Entry(score).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Winner object to return when game is over
    /// </summary>
    public class Winner
    {
        public bool GameOver { get; set; }
        public string AWinnerIsYou { get; set; }
        public int PlayerOneScore { get; set; }
        public int PlayerTwoScore { get; set; }
    }
}
