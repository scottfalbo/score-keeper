using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreKeeper.Data;
using ScoreKeeper.Models;
using ScoreKeeper.Models.Interfaces;

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
        }

        [BindProperty]
        public GameData GameData { get; set; }
        [BindProperty]
        public Rummy Rummy { get; set; }
        [BindProperty]
        public ScoreInput ScoreInput { get; set; }
        [BindProperty]
        public bool GameOver { get; set; }
        public bool SaveExists { get; set; }

        public async Task OnGet()
        {
            Rummy = await _rummy.GetGame(1);
        }

        public IActionResult OnPostNewGame()
        {
            Console.WriteLine("");
            //if (_rummy.SaveExists(GameData.SaveAs))
            //{
            //    SaveExists = true;
            //    return Redirect("/Games/Rummy");
            //}
            //_rummy.StartGame(GameData.PlayerOne, GameData.PlayerTwo, GameData.SaveAs);
            return Redirect("/Games/Rummy");
        }

        public async Task<IActionResult> OnPostAddScore()
        {
            GameOver = await _rummy.AddScores(ScoreInput.PlayerOne, ScoreInput.PlayerTwo);
            return Redirect("/Games/Rummy");
        }
    }

    public class GameData
    {
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public string SaveAs { get; set; }
        public int Limit { get; set; }
    }

    public class ScoreInput
    {
        public int PlayerOne { get; set; }
        public int PlayerTwo { get; set; }
    }
}
