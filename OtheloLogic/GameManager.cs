using System.Collections.Generic;

namespace OthelloLogic
{
    public delegate void SkipTurnNotifierDelgate(Player i_Player);

    public delegate void PossibilitiesCellsAddedNotifierDelgate(Player i_Player);

    public class GameManager
    {
        private Board m_GameBoard;
        private Player m_Player1;
        private Player m_Player2;
        private eGameModes m_GameMode;
        private Player m_CurrentPlayerTurn;

        public event SkipTurnNotifierDelgate SkipTurn;

        public event PossibilitiesCellsAddedNotifierDelgate PossibilitiesCellAdded;

        public enum eGameStatus
        {
            Player1Won,
            Player2Won,
            Tie,
        }

        public enum eGameModes
        {
            HumanVsHuman,
            HumanVsComputer
        }

        protected virtual void OnSkipTurn(Player i_Player)
        {
            if (SkipTurn != null)
            {
                SkipTurn.Invoke(i_Player);
            }
        }

        protected virtual void OnPossibilitiesCellAdded(Player i_Player)
        {
            if (PossibilitiesCellAdded != null)
            {
                PossibilitiesCellAdded.Invoke(i_Player);
            }
        }

        public GameManager GameClone()
        {
            GameManager clonedGame = new GameManager(m_GameBoard.Size, m_GameMode);
            clonedGame.m_GameBoard = this.m_GameBoard.BoardClone();

            return clonedGame;
        }

        public GameManager(int i_BoardSize, eGameModes i_GameMode)
        {
            m_GameBoard = new Board(i_BoardSize);
            m_Player1 = new Player(Player.ePlayerTypes.HumanPlayer, Coin.eCoinColor.Red);
            if (i_GameMode.Equals(eGameModes.HumanVsHuman))
            {
                m_Player2 = new Player(Player.ePlayerTypes.HumanPlayer, Coin.eCoinColor.Yellow);
            }
            else
            {
                m_Player2 = new Player(Player.ePlayerTypes.Computer, Coin.eCoinColor.Yellow);
            }

            m_GameMode = i_GameMode;
            m_CurrentPlayerTurn = m_Player1;
        }

        public Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public Player CurrentPlayerTurn
        {
            get
            {
                return m_CurrentPlayerTurn;
            }

            set
            {
                m_CurrentPlayerTurn = value;
            }
        }

        public Player Player1
        {
            get
            {
                return m_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return m_Player2;
            }
        }

        public void InitializeBoard()
        {
            m_GameBoard.ResetBoard();
            m_GameBoard.firstRoundOfBoard();
        }

        public void ChangeCurrentPlayerTurn()
        {
            if (CurrentPlayerTurn.Equals(m_Player1))
            {
                CurrentPlayerTurn = m_Player2;
            }
            else
            {
                CurrentPlayerTurn = m_Player1;
            }
        }

        public List<Point> CalculatePossibilitiesCells(Player i_CurrentPlayer)
        {
            List<Point> possibilitiesCells = new List<Point>();

            if (i_CurrentPlayer.Equals(m_Player1))
            {
                foreach (Coin CurrentCoin in m_GameBoard.YellowLinkedList)
                {
                    addPossibilitiesCellToInput(CurrentCoin, possibilitiesCells);
                }

                m_Player1.PossibilitiesCellsForNextTurn = possibilitiesCells;
            }
            else
            {
                foreach (Coin CurrentCoin in m_GameBoard.RedLinkedList)
                {
                    addPossibilitiesCellToInput(CurrentCoin, possibilitiesCells);
                }

                m_Player2.PossibilitiesCellsForNextTurn = possibilitiesCells;
            }

            OnPossibilitiesCellAdded(i_CurrentPlayer);
            return possibilitiesCells;
        }

        public bool HandlePlayerTurn(Player i_CurrentPlayer)
        {
            bool isEndGame = false;
            List<Point> possibilitiesCellsOfCurrentPlayer, possibilitiesCellsOfOtherPlayer;

            possibilitiesCellsOfCurrentPlayer = CalculatePossibilitiesCells(i_CurrentPlayer);
            if (possibilitiesCellsOfCurrentPlayer.Count == 0)
            {
                ChangeCurrentPlayerTurn();
                possibilitiesCellsOfOtherPlayer = CalculatePossibilitiesCells(m_CurrentPlayerTurn);
                if (possibilitiesCellsOfOtherPlayer.Count == 0)
                {
                    isEndGame = true;
                }
                else
                {
                    OnSkipTurn(i_CurrentPlayer);
                }
            }

            return isEndGame;
        }

        public void AddCoinToBoard(Point i_UserCellInput)
        {
            Coin.eCoinColor playerColor;
            if (m_CurrentPlayerTurn.Equals(m_Player1))
            {
                playerColor = Coin.eCoinColor.Red;
            }
            else
            {
                playerColor = Coin.eCoinColor.Yellow;
            }

            Coin cellToAdd = new Coin(playerColor, i_UserCellInput.Row, i_UserCellInput.Column);
            m_GameBoard.addCoin(cellToAdd);
            coinsToFlip(cellToAdd);
        }

        private void coinsToFlip(Coin i_AddedCoin)
        {
            List<Coin> sequenceOfCoinsToFlip = new List<Coin>();

            flipUpperCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipLowerCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipRightCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipLeftCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipUpperRightCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipUpperLeftCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipLowerRightCoins(sequenceOfCoinsToFlip, i_AddedCoin);
            flipLowerLeftCoins(sequenceOfCoinsToFlip, i_AddedCoin);
        }

        public eGameStatus CheckingScoreBoard()
        {
            eGameStatus whoWon = eGameStatus.Tie;

            if (m_GameBoard.RedLinkedList.Count != m_GameBoard.YellowLinkedList.Count)
            {
                whoWon = m_GameBoard.RedLinkedList.Count > m_GameBoard.YellowLinkedList.Count ? eGameStatus.Player1Won : eGameStatus.Player2Won;
            }

            return whoWon;
        }

        public void HandleWhoWon(eGameStatus io_GameStatus)
        {
            switch (io_GameStatus)
            {
                case eGameStatus.Player1Won:
                    m_Player1.NumberOfWinnings++;
                    break;
                case eGameStatus.Player2Won:
                    m_Player2.NumberOfWinnings++;
                    break;
                case eGameStatus.Tie:
                    m_Player1.NumberOfWinnings++;
                    m_Player2.NumberOfWinnings++;
                    break;
            }
        }

        private void flipUpperCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row - 1, i_AddedCoin.Point.Column))
            {
                if (!isCellEmpty(m_GameBoard.GetUpperCell(i_AddedCoin)) && !m_GameBoard.GetUpperCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetUpperCell(i_AddedCoin), i_AddedCoin, -1, 0);
                }
            }
        }

        private void flipLowerCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row + 1, i_AddedCoin.Point.Column))
            {
                if (!isCellEmpty(m_GameBoard.GetLowerCell(i_AddedCoin)) && !m_GameBoard.GetLowerCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetLowerCell(i_AddedCoin), i_AddedCoin, 1, 0);
                }
            }
        }

        private void flipRightCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row, i_AddedCoin.Point.Column + 1))
            {
                if (!isCellEmpty(m_GameBoard.GetRightCell(i_AddedCoin)) && !m_GameBoard.GetRightCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetRightCell(i_AddedCoin), i_AddedCoin, 0, 1);
                }
            }
        }

        private void flipLeftCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row, i_AddedCoin.Point.Column - 1))
            {
                if (!isCellEmpty(m_GameBoard.GetLeftCell(i_AddedCoin)) && !m_GameBoard.GetLeftCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetLeftCell(i_AddedCoin), i_AddedCoin, 0, -1);
                }
            }
        }

        private void flipUpperRightCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row - 1, i_AddedCoin.Point.Column + 1))
            {
                if (!isCellEmpty(m_GameBoard.GetUpperRightCell(i_AddedCoin)) && !m_GameBoard.GetUpperRightCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetUpperRightCell(i_AddedCoin), i_AddedCoin, -1, 1);
                }
            }
        }

        private void flipUpperLeftCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row - 1, i_AddedCoin.Point.Column - 1))
            {
                if (!isCellEmpty(m_GameBoard.GetUpperLeftCell(i_AddedCoin)) && !m_GameBoard.GetUpperLeftCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetUpperLeftCell(i_AddedCoin), i_AddedCoin, -1, -1);
                }
            }
        }

        private void flipLowerRightCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row + 1, i_AddedCoin.Point.Column + 1))
            {
                if (!isCellEmpty(m_GameBoard.GetLowerRightCell(i_AddedCoin)) && !m_GameBoard.GetLowerRightCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetLowerRightCell(i_AddedCoin), i_AddedCoin, 1, 1);
                }
            }
        }

        private void flipLowerLeftCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin i_AddedCoin)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_AddedCoin.Point.Row + 1, i_AddedCoin.Point.Column - 1))
            {
                if (!isCellEmpty(m_GameBoard.GetLowerLeftCell(i_AddedCoin)) && !m_GameBoard.GetLowerLeftCell(i_AddedCoin).Color.Equals(i_AddedCoin.Color))
                {
                    checkIfCoinNeedToAddToFlipCoinsList(i_SequenceOfCoinsToFlip, m_GameBoard.GetLowerLeftCell(i_AddedCoin), i_AddedCoin, 1, -1);
                }
            }
        }

        private void checkIfCoinNeedToAddToFlipCoinsList(List<Coin> i_SequenceOfCoinsToFlip, Coin i_CoinToCheck, Coin i_CurrentCoin, int i_UpdateRow, int i_UpdateColumn)
        {
            bool emptyCell = false;
            int column, row;
            Coin.eCoinColor colorOfCoinToCheck;

            column = i_CoinToCheck.Point.Column;
            row = i_CoinToCheck.Point.Row;
            colorOfCoinToCheck = i_CoinToCheck.Color;
            while (!emptyCell && !colorOfCoinToCheck.Equals(i_CurrentCoin.Color) && m_GameBoard.CheckIfInBoardLimit(row, column))
            {
                i_SequenceOfCoinsToFlip.Add(m_GameBoard.GetThisCell(row, column));
                row += i_UpdateRow;
                column += i_UpdateColumn;
                if (m_GameBoard.CheckIfInBoardLimit(row, column))
                {
                    emptyCell = isCellEmpty(m_GameBoard.GetThisCell(row, column));
                    if (!emptyCell)
                    {
                        colorOfCoinToCheck = m_GameBoard.GetThisCell(row, column).Color;
                    }
                }
            }

            if (!emptyCell && colorOfCoinToCheck.Equals(i_CurrentCoin.Color))
            {
                flipCoins(i_SequenceOfCoinsToFlip, i_CurrentCoin.Color);
            }

            i_SequenceOfCoinsToFlip.Clear();
        }

        private void flipCoins(List<Coin> i_SequenceOfCoinsToFlip, Coin.eCoinColor i_NewColor)
        {
            foreach (Coin itrCoin in i_SequenceOfCoinsToFlip)
            {
                itrCoin.Color = i_NewColor;
                if (i_NewColor.Equals(Coin.eCoinColor.Red))
                {
                    m_GameBoard.RedLinkedList.AddLast(itrCoin);
                    m_GameBoard.YellowLinkedList.Remove(itrCoin);
                }
                else
                {
                    m_GameBoard.YellowLinkedList.AddLast(itrCoin);
                    m_GameBoard.RedLinkedList.Remove(itrCoin);
                }

                m_GameBoard.FlipCoinInNotifiers(itrCoin);
            }
        }

        private bool checkIfCellNeedToAddToPossibilitiesList(Coin i_CoinToCheck, Coin i_CurrentCoin, int i_UpdateRow, int i_UpdateColumn)
        {
            bool emptyCell = isCellEmpty(i_CoinToCheck);
            bool addCellToList = false;
            int column, row;
            Coin.eCoinColor colorOfCoinToCheck;

            if (!emptyCell)
            {
                column = i_CoinToCheck.Point.Column;
                row = i_CoinToCheck.Point.Row;
                colorOfCoinToCheck = i_CoinToCheck.Color;

                while (!emptyCell && colorOfCoinToCheck.Equals(i_CurrentCoin.Color) && m_GameBoard.CheckIfInBoardLimit(row, column))
                {
                    column += i_UpdateColumn;
                    row += i_UpdateRow;
                    if (m_GameBoard.CheckIfInBoardLimit(row, column))
                    {
                        emptyCell = isCellEmpty(m_GameBoard.GetThisCell(row, column));
                        if (!emptyCell)
                        {
                            colorOfCoinToCheck = m_GameBoard.GetThisCell(row, column).Color;
                        }
                    }
                }

                if (!emptyCell && !colorOfCoinToCheck.Equals(i_CurrentCoin.Color))
                {
                    addCellToList = true;
                }
            }

            return addCellToList;
        }

        private bool isCellEmpty(Coin i_Coin)
        {
            return i_Coin == null;
        }

        private void addPossibilitiesCellToInput(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            addPossibilitiesUpper(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesLower(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesRight(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesLeft(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesUpperLeft(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesUpperRight(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesLowerRight(i_CurrentCoin, i_PossibilitiesCells);
            addPossibilitiesLowerLeft(i_CurrentCoin, i_PossibilitiesCells);
        }

        private void addPossibilitiesUpper(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column)
                && isCellEmpty(m_GameBoard.GetUpperCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetLowerCell(i_CurrentCoin), i_CurrentCoin, 1, 0))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column));
                    }
                }
            }
        }

        private void addPossibilitiesLower(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column)
                && isCellEmpty(m_GameBoard.GetLowerCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetUpperCell(i_CurrentCoin), i_CurrentCoin, -1, 0))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column));
                    }
                }
            }
        }

        private void addPossibilitiesRight(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column + 1)
                && isCellEmpty(m_GameBoard.GetRightCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column - 1)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetLeftCell(i_CurrentCoin), i_CurrentCoin, 0, -1))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column + 1)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column + 1));
                    }
                }
            }
        }

        private void addPossibilitiesLeft(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column - 1)
                && isCellEmpty(m_GameBoard.GetLeftCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column + 1)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetRightCell(i_CurrentCoin), i_CurrentCoin, 0, 1))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column - 1)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row, i_CurrentCoin.Point.Column - 1));
                    }
                }
            }
        }

        private void addPossibilitiesUpperLeft(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column - 1)
                && isCellEmpty(m_GameBoard.GetUpperLeftCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column + 1)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetLowerRightCell(i_CurrentCoin), i_CurrentCoin, 1, 1))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column - 1)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column - 1));
                    }
                }
            }
        }

        private void addPossibilitiesUpperRight(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column + 1)
                && isCellEmpty(m_GameBoard.GetUpperRightCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column - 1)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetLowerLeftCell(i_CurrentCoin), i_CurrentCoin, 1, -1))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column + 1)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column + 1));
                    }
                }
            }
        }

        private void addPossibilitiesLowerRight(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column + 1)
                && isCellEmpty(m_GameBoard.GetLowerRightCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column - 1)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetUpperLeftCell(i_CurrentCoin), i_CurrentCoin, -1, -1))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column + 1)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column + 1));
                    }
                }
            }
        }

        private void addPossibilitiesLowerLeft(Coin i_CurrentCoin, List<Point> i_PossibilitiesCells)
        {
            if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column - 1)
                && isCellEmpty(m_GameBoard.GetLowerLeftCell(i_CurrentCoin)))
            {
                if (m_GameBoard.CheckIfInBoardLimit(i_CurrentCoin.Point.Row - 1, i_CurrentCoin.Point.Column + 1)
                    && checkIfCellNeedToAddToPossibilitiesList(m_GameBoard.GetUpperRightCell(i_CurrentCoin), i_CurrentCoin, -1, 1))
                {
                    if (!i_PossibilitiesCells.Contains(new Point(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column - 1)))
                    {
                        i_PossibilitiesCells.Add(new Point(i_CurrentCoin.Point.Row + 1, i_CurrentCoin.Point.Column - 1));
                    }
                }
            }
        }
    }
}