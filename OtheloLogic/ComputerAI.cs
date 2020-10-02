using System.Collections.Generic;

namespace OthelloLogic
{
    public class ComputerAI
    {
        public static Point FindBestMoveForComputer(GameManager i_RealGame, Player i_Player, List<Point> i_PossibilitiesCellsOfComputer)
        {
            int currentMoveFlippedCoinsCount, miniMaxFlippedCoinsCount;
            GameManager simulationGame;
            Point bestMoveForComputer = new Point();

            miniMaxFlippedCoinsCount = int.MinValue;
            foreach (Point currentPossibilityMove in i_PossibilitiesCellsOfComputer)
            {
                simulationGame = i_RealGame.GameClone();
                currentMoveFlippedCoinsCount = CalculateBestMoveForComputer(simulationGame, i_Player, i_PossibilitiesCellsOfComputer, currentPossibilityMove)
                    + BoardRegionsScores(simulationGame.GameBoard, currentPossibilityMove);
                if (currentMoveFlippedCoinsCount > miniMaxFlippedCoinsCount)
                {
                    miniMaxFlippedCoinsCount = currentMoveFlippedCoinsCount;
                    bestMoveForComputer = currentPossibilityMove;
                }
            }

            return bestMoveForComputer;
        }

        public static int BoardRegionsScores(Board i_GameBoard, Point i_CurrentPossibilityMove)
        {
            int scoreBonus = 0;

            if (i_GameBoard.IsInCorner(i_CurrentPossibilityMove.Row, i_CurrentPossibilityMove.Column))
            {
                scoreBonus += 10;
            }
            else if (i_GameBoard.IsNearCorners(i_CurrentPossibilityMove.Row, i_CurrentPossibilityMove.Column))
            {
                scoreBonus -= 5;
            }

            return scoreBonus;
        }

        public static int CalculateBestMoveForComputer(GameManager i_SimulationGame, Player i_Player, List<Point> i_PossibilitiesCells, Point i_CurrentPossibilityMove)
        {
            int currentMoveFlippedCoinsCount, maxFlippedCoinsCount;

            maxFlippedCoinsCount = int.MinValue;
            currentMoveFlippedCoinsCount = NumberOfFlippedCoinsForComputer(i_SimulationGame, i_CurrentPossibilityMove);
            i_Player = i_SimulationGame.Player1;
            i_PossibilitiesCells = i_SimulationGame.CalculatePossibilitiesCells(i_Player);
            if (i_PossibilitiesCells.Count == 0)
            {
                maxFlippedCoinsCount = currentMoveFlippedCoinsCount;
            }
            else
            {
                maxFlippedCoinsCount = FindingBestMoveForHuman(i_SimulationGame, i_PossibilitiesCells);
                maxFlippedCoinsCount *= -1;
            }

            return maxFlippedCoinsCount;
        }

        public static int NumberOfFlippedCoinsForComputer(GameManager i_SimulationGame, Point i_CurrentPossibilityMove)
        {
            int currentMoveFlippedCoinsCount, oldYellowCoindCount, newYellowCoindCount;

            oldYellowCoindCount = i_SimulationGame.GameBoard.YellowLinkedList.Count;
            i_SimulationGame.AddCoinToBoard(i_CurrentPossibilityMove);
            newYellowCoindCount = i_SimulationGame.GameBoard.YellowLinkedList.Count;
            currentMoveFlippedCoinsCount = newYellowCoindCount - oldYellowCoindCount - 1;
            return currentMoveFlippedCoinsCount;
        }

        public static int FindingBestMoveForHuman(GameManager i_SimulationGame, List<Point> i_PossibilitiesCellsOfHuman)
        {
            int currentMoveFlippedCoinsCount, maxFlippedCoinsCount;

            maxFlippedCoinsCount = 0;
            foreach (Point currentPossibilityMove in i_PossibilitiesCellsOfHuman)
            {
                currentMoveFlippedCoinsCount = NumberOfFlippedCoinsOfCertainMoveForHuman(i_SimulationGame, currentPossibilityMove);
                if (currentMoveFlippedCoinsCount > maxFlippedCoinsCount)
                {
                    maxFlippedCoinsCount = currentMoveFlippedCoinsCount;
                }
            }

            return maxFlippedCoinsCount;
        }

        public static int NumberOfFlippedCoinsOfCertainMoveForHuman(GameManager i_SimulationGame, Point CurrentPossibilityMove)
        {
            int numberOfCoinsFlipped, oldRedCoindCount, newRedCoindCount;
            GameManager clonedGame;

            oldRedCoindCount = i_SimulationGame.GameBoard.RedLinkedList.Count;
            clonedGame = i_SimulationGame.GameClone();
            clonedGame.AddCoinToBoard(CurrentPossibilityMove);
            newRedCoindCount = clonedGame.GameBoard.RedLinkedList.Count;
            numberOfCoinsFlipped = newRedCoindCount - oldRedCoindCount - 1;
            return numberOfCoinsFlipped;
        }
    }
}
