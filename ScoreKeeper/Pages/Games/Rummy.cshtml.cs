using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScoreKeeper.Pages.Games
{
    public class RummyModel : PageModel
    {
        [BindProperty]
        public string PlayerOne { get; set; }
        [BindProperty]
        public string PlayerTwo { get; set; }
        [BindProperty]
        public string SaveAs { get; set; }

        public void OnGet()
        {

        }

        public void NewGame()
        {

        }
    }
}
