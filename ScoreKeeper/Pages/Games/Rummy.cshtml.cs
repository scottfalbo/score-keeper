using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreKeeper.Data;
using ScoreKeeper.Models;
using ScoreKeeper.Models.Interfaces;
using ScoreKeeper.Models.Interfaces.Services;

namespace ScoreKeeper.Pages.Games
{
    public class RummyModel : PageModel
    {
        public IRummyScore _rummy;
        public ScoreKeeperDbContext _db;

        public RummyModel(IRummyScore rummy, ScoreKeeperDbContext context)
        {
            _rummy = rummy;
            _db = context;
            GameOver = new Winner();
        }

        [BindProperty]
        public GameData GameData { get; set; }
        [BindProperty]
        public Rummy Rummy { get; set; }
        [BindProperty]
        public ScoreInput ScoreInput { get; set; }
        [BindProperty]
        public Winner GameOver { get; set; }
        [BindProperty]

        // Menu bools
        public bool NextGame { get; set; }
        [BindProperty]
        public bool HideGameMenu { get; set; }

        public async Task OnGet()
        {
            HideGameMenu = true;
            string gameId = HttpContext.Request.Cookies["game id"];
            int id = gameId != null ? Int32.Parse(gameId) : -1;
            Rummy = await _rummy.GetGame(id);
        }

        /// <summary>
        /// Start a new game, method called from Next Game option
        /// </summary>
        public async Task OnPostGameOver()
        {
            HideGameMenu = true;
            string gameId = HttpContext.Request.Cookies["game id"];
            int id = gameId != null ? Int32.Parse(gameId) : -1;
            Rummy = await _rummy.GetGame(id);
        }

        /// <summary>
        /// Start a fresh score sheet with new players and limits
        /// </summary>
        public async Task<IActionResult> OnPostNewGame()
        {
            HideGameMenu = true;
            string gameId = HttpContext.Request.Cookies["game id"];
            int id = gameId != null ? Int32.Parse(gameId) : -1;
            Rummy = await _rummy.GetGame(id);

            id = await _rummy.StartGame(GameData.PlayerOne, GameData.PlayerTwo, GameData.Limit, Rummy);
            MakeCookie(id);

            return Redirect("/Games/Rummy");
        }

        /// <summary>
        /// Brings up the new game menu
        /// </summary>
        public void OnPostNewGameMenu()
        {
            HideGameMenu = false;
        }

        /// <summary>
        /// Takes the players scores for the round and adds them to the total
        /// </summary>
        public async Task OnPostAddScore()
        {
            GameOver = await _rummy.AddScores(ScoreInput.PlayerOne, ScoreInput.PlayerTwo, Rummy.Id);
            NextGame = GameOver.GameOver;
            string gameId = HttpContext.Request.Cookies["game id"];
            int id = gameId != null ? Int32.Parse(gameId) : -1;
            Rummy = await _rummy.GetGame(id);
            ScoreInput.PlayerOne = 0;
            ScoreInput.PlayerTwo = 0;
            HideGameMenu = true;
            Redirect("/");
        }

        public void OnPostNew()
        {
            HideGameMenu = false;
        }

        // reset current game

        // quit

        /// <summary>
        /// Save the gameId as a cookie for later access
        /// </summary>
        /// <param name="id"></param>
        private void MakeCookie(int id)
        {
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = new DateTimeOffset(DateTime.Now.AddMonths(12)),
                Secure = true,
                HttpOnly = true
            };
            HttpContext.Response.Cookies.Append("game id", id.ToString(), cookieOptions);
        }
    }

    /// <summary>
    /// Data objects for properties
    /// </summary>
    public class GameData
    {
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public int Limit { get; set; }
    }

    public class ScoreInput
    {
        public int PlayerOne { get; set; }
        public int PlayerTwo { get; set; }
    }
}
