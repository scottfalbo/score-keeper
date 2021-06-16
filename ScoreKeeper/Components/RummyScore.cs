using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Components
{
    [ViewComponent]
    public class RummyScore : ViewComponent
    {
        public RummyScore()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            ViewModel vm = new ViewModel()
            {

            };

            return View(vm);
        }


        public class ViewModel
        {
            public List<int> PlayerOne { get; set; }
            public List<int> PlayerTwo { get; set; }
        }

    }
}
