using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreKeeper.Models.Interfaces;

namespace ScoreKeeper.Pages.Games
{
    public class RummyModel : PageModel
    {
        public IRummyScore _rummy;

        public RummyModel(IRummyScore rummy)
        {
            _rummy = rummy;
        }

        [BindProperty]
        public string PlayerOne { get; set; }
        [BindProperty]
        public string PlayerTwo { get; set; }
        [BindProperty]
        public string SaveAs { get; set; }
        [BindProperty]
        public bool SaveExists { get; set; }

        public void OnGet()
        {

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
