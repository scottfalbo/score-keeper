using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Models.Interfaces.Services
{
    public class RummyScoreRepository : IRummyScore
    {
        public void AddScores(int scoreOne, int scoreTwo)
        {
            throw new NotImplementedException();
        }

        public void ContinueGame(string SaveAs)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame()
        {
            throw new NotImplementedException();
        }

        public void StartGame(string playerOne, string playerTwo, string save)
        {
            PlayerOne p1 = new PlayerOne()
            {
                Name = playerOne,
                Score = new List<int>(),
                Wins = 0
            };

            PlayerTwo p2 = new PlayerTwo()
            {
                Name = playerTwo,
                Score = new List<int>(),
                Wins = 0
            };

            Rummy newGame = new Rummy() 
            {
                PlayerOne = p1,
                PlayerTwo = p2,
                SaveAs = save
            };

        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool SaveExists(string save)
        {
            return false;
        }

        private void SaveGame(Rummy game)
        {

        }
    }
}
