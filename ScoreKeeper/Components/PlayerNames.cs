using Microsoft.AspNetCore.Mvc;
using ScoreKeeper.Models;
using ScoreKeeper.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Components
{
    [ViewComponent]
    public class PlayerNames : ViewComponent
    {
        public IRummyScore _rummy;

        public PlayerNames(IRummyScore rummy)
        {
            _rummy = rummy;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string gameId = HttpContext.Request.Cookies["game id"];
            int id = gameId != null ? Int32.Parse(gameId) : -1;
            Rummy game = await _rummy.GetGame(id);
            ViewModel vm = new ViewModel()
            {
                PlayerOne = game.RummyPlayers[0].Player.Name,
                PlayerOneWins = game.RummyPlayers[0].Player.Wins,
                PlayerTwo = game.RummyPlayers[1].Player.Name,
                PlayerTwoWins = game.RummyPlayers[1].Player.Wins,
                Limit = game.Limit
            };

            return View(vm);
        }

        public class ViewModel
        {
            public string PlayerOne { get; set; }
            public int PlayerOneWins { get; set; }
            public string PlayerTwo { get; set; }
            public int PlayerTwoWins { get; set; }
            public int Limit { get; set; }
        }
    }
}
