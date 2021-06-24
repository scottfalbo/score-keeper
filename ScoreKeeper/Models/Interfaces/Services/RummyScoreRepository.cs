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

        /// <summary>
        /// Driver method to delete game, player and join table data for current game
        /// </summary>
        /// <param name="game"> Rummy object </param>
        public async Task DeleteGame(Rummy game)
        {
            if(game.Id >= 0)
            {
                await RemoveRummyPlayer(game.RummyPlayers[0].Player.Id, game.Id);
                await RemoveRummyPlayer(game.RummyPlayers[1].Player.Id, game.Id);
                _db.Entry(game).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Remove a RummyPlayer entry from database
        /// </summary>
        /// <param name="playerId"> player id </param>
        /// <param name="gameId"> game id </param>
        private async Task RemoveRummyPlayer(int playerId, int gameId)
        {
            RummyPlayer rummyPlayer = await _db.RummyPlayers
                .FirstOrDefaultAsync(x => x.RummyId == gameId
                                    && x.PlayerId == playerId);
            _db.Entry(rummyPlayer).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            await RemovePlayer(playerId);
        }

        /// <summary>
        /// Remove a player record from the database
        /// </summary>
        /// <param name="playerId"> player id </param>
        private async Task RemovePlayer(int playerId)
        {
            Player player = await _db.Players.FindAsync(playerId);
            _db.Entry(player).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task ResetCurrent(Rummy game)
        {
            await ClearScoreSheet(game);
            game.RummyPlayers[0].Player.Wins = 0;
            game.RummyPlayers[1].Player.Wins = 0;
            _db.Entry(game).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Starts a fresh score sheet for players
        /// </summary>
        /// <param name="playerOne"> string player one </param>
        /// <param name="playerTwo"> string player two </param>
        /// <param name="limit"> int game limit </param>
        /// <returns> int new game id </returns>
        public async Task<int> StartGame(string playerOne, string playerTwo, int limit, Rummy game)
        {
            await MakeNewGame(limit);
            int gameId = GetGameId().Result;
            await CreatePlayer(playerOne);
            int playerOneId = await GetPlayerId();
            await CreatePlayer(playerTwo);
            int playerTwoId = await GetPlayerId();
            await AssignPlayer(gameId, playerOneId);
            await AssignPlayer(gameId, playerTwoId);
            await DeleteGame(game);
            return gameId;
        }

        /// <summary>
        /// RummyPlayer join table to put new players into the game object
        /// </summary>
        /// <param name="gameId"> game id </param>
        /// <param name="playerId"> player id </param>
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

        /// <summary>
        /// Create a new player object from form input and put into database
        /// </summary>
        /// <param name="name"> string player name </param>
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

        /// <summary>
        /// Get the id of the last player added to the database
        /// </summary>
        /// <returns> int Player.Id </returns>
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

        /// <summary>
        /// Create a new game object and add it to database
        /// </summary>
        /// <param name="limit"> int limit </param>
        private async Task MakeNewGame(int limit)
        {
            Rummy game = new Rummy()
            {
                Limit = limit
            };
            _db.Entry(game).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Get id of last game in the database
        /// </summary>
        /// <returns> int Rummy.Id </returns>
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
                    Limit = z.Limit,
                    RummyPlayers = z.RummyPlayers
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a list of all of the games
        /// </summary>
        /// <returns> List<Rummy> /returns>
        public async Task<List<Rummy>> GetGames()
        {
            return await _db.Rummy
                .Include(y => y.RummyPlayers)
                .ThenInclude(a => a.Player)
                .ThenInclude(b => b.PlayerScores)
                .ThenInclude(c => c.Score)
                .Select(x => new Rummy
                {
                    Id = x.Id,
                    Limit = x.Limit,
                    RummyPlayers = x.RummyPlayers
                }).ToListAsync();
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
            if (playerOne >= game.Limit || playerTwo >= game.Limit)
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
