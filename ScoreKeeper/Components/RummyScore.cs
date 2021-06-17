using Microsoft.AspNetCore.Mvc;
using ScoreKeeper.Data;
using ScoreKeeper.Models;
using ScoreKeeper.Models.Interfaces;
using ScoreKeeper.Models.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Components
{
    [ViewComponent]
    public class RummyScore : ViewComponent
    {
        public ScoreKeeperDbContext _db;
        public IRummyScore _rummy;

        public RummyScore(ScoreKeeperDbContext context, IRummyScore rummy)
        {
            _db = context;
            _rummy = rummy;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int gameId = Int32.Parse(HttpContext.Request.Cookies["game id"]);
            Rummy game = await _rummy.GetGame(gameId);
            ViewModel vm = new ViewModel()
            {
                PlayerOnePoints = game.RummyPlayers[0].Player.PlayerScores,
                PlayerTwoPoints = game.RummyPlayers[1].Player.PlayerScores
            };

            return View(vm);
        }


        public class ViewModel
        {
            public List<PlayerScore> PlayerOnePoints { get; set; }
            public List<PlayerScore> PlayerTwoPoints { get; set; }
        }

    }
}
