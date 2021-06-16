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
        public string PlayerOne { get; set; }
        [BindProperty]
        public string PlayerTwo { get; set; }
        [BindProperty]
        public string SaveAs { get; set; }
        [BindProperty]
        public Rummy Rummy { get; set; }

        public bool SaveExists { get; set; }

        public async Task OnGet()
        {
            Rummy = await _rummy.GetGame(1);
        }

        public IActionResult NewGame()
        {
            if (_rummy.SaveExists(SaveAs))
            {
                SaveExists = true;
                return Redirect("/Games/Rummy");
            }
            _rummy.StartGame(PlayerOne, PlayerTwo, SaveAs);
            return Redirect("/Games/Rummy");
        }
    }
}
