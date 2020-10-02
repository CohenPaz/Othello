using System.Collections.Generic;

namespace OthelloLogic
{
    public delegate void AddCoinNotifierDelgate(Coin i_Coin);

    public delegate void FlipCoinNotifierDelgate(Coin i_Coin);

    public class Board
    {
        private Coin[,] m_GameBoard;
        private LinkedList<Coin> m_RedCoins;
        private LinkedList<Coin> m_YellowCoins;

        public event AddCoinNotifierDelgate CoinAdded;

        public event FlipCoinNotifierDelgate CoinFlipped;

        protected virtual void OnCoinAdded(Coin i_Coin)
        {
            if (CoinAdded != null)
            {
                CoinAdded.Invoke(i_Coin);
            }
        }

        protected virtual void OnCoinFliped(Coin i_Coin)
        {
            if (CoinFlipped != null)
            {
                CoinFlipped.Invoke(i_Coin);
            }
        }

        public void FlipCoinInNotifiers(Coin i_Coin)
        {
            OnCoinFliped(i_Coin);
        }

        public Board(int i_Size)
        {
            m_RedCoins = new LinkedList<Coin>();
            m_YellowCoins = new LinkedList<Coin>();
            m_GameBoard = new Coin[i_Size, i_Size];
        }

        public Board BoardClone()
        {
            Board clonedBoard = new Board(this.Size);
            for (int row = 0; row < this.Size; row++)
            {
                for (int column = 0; column < this.Size; column++)
                {
                    if (this.GetThisCell(row, column) != null)
                    {
                        clonedBoard.m_GameBoard[row, column] = this.GetThisCell(row, column).CloneCoin();
                        if (clonedBoard.m_GameBoard[row, column].Color.Equals(Coin.eCoinColor.Red))
                        {
                            clonedBoard.m_RedCoins.AddLast(m_GameBoard[row, column]);
                        }
                        else
                        {
                            clonedBoard.m_YellowCoins.AddLast(m_GameBoard[row, column]);
                        }
                    }
                }
            }

            return clonedBoard;
        }

        public void firstRoundOfBoard()
        {
            m_GameBoard[Size / 2, Size / 2] = new Coin(Coin.eCoinColor.Yellow, Size / 2, Size / 2);
            OnCoinAdded(m_GameBoard[Size / 2, Size / 2]);
            m_GameBoard[(Size / 2) - 1, (Size / 2) - 1] = new Coin(Coin.eCoinColor.Yellow, (Size / 2) - 1, (Size / 2) - 1);
            OnCoinAdded(m_GameBoard[(Size / 2) - 1, (Size / 2) - 1]);
            m_GameBoard[(Size / 2) - 1, Size / 2] = new Coin(Coin.eCoinColor.Red, (Size / 2) - 1, Size / 2);
            OnCoinAdded(m_GameBoard[(Size / 2) - 1, Size / 2]);
            m_GameBoard[Size / 2, (Size / 2) - 1] = new Coin(Coin.eCoinColor.Red, Size / 2, (Size / 2) - 1);
            OnCoinAdded(m_GameBoard[Size / 2, (Size / 2) - 1]);
            m_YellowCoins.AddFirst(m_GameBoard[Size / 2, Size / 2]);
            m_YellowCoins.AddLast(m_GameBoard[(Size / 2) - 1, (Size / 2) - 1]);
            m_RedCoins.AddFirst(m_GameBoard[(Size / 2) - 1, Size / 2]);
            m_RedCoins.AddLast(m_GameBoard[Size / 2, (Size / 2) - 1]);
        }

        public void ResetBoard()
        {
            for (int row = 0; row < this.Size; row++)
            {
                for (int column = 0; column < this.Size; column++)
                {
                    m_GameBoard[row, column] = null;
                }
            }

            m_RedCoins.Clear();
            m_YellowCoins.Clear();
        }

        public LinkedList<Coin> YellowLinkedList
        {
            get
            {
                return m_YellowCoins;
            }
        }

        public LinkedList<Coin> RedLinkedList
        {
            get
            {
                return m_RedCoins;
            }
        }

        public int Size
        {
            get
            {
                return m_GameBoard.GetLength(0);
            }
        }

        public void addCoin(Coin i_Coin)
        {
            m_GameBoard[i_Coin.Point.Row, i_Coin.Point.Column] = i_Coin;
            if (i_Coin.Color.Equals(Coin.eCoinColor.Red))
            {
                m_RedCoins.AddLast(m_GameBoard[i_Coin.Point.Row, i_Coin.Point.Column]);
            }
            else
            {
                m_YellowCoins.AddLast(m_GameBoard[i_Coin.Point.Row, i_Coin.Point.Column]);
            }

            OnCoinAdded(i_Coin);
        }

        public Coin GetThisCell(int i_Row, int i_Column)
        {
            return m_GameBoard[i_Row, i_Column];
        }

        public Coin GetUpperCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row - 1, i_Coin.Point.Column];
        }

        public Coin GetLowerCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row + 1, i_Coin.Point.Column];
        }

        public Coin GetLeftCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row, i_Coin.Point.Column - 1];
        }

        public Coin GetRightCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row, i_Coin.Point.Column + 1];
        }

        public Coin GetUpperRightCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row - 1, i_Coin.Point.Column + 1];
        }

        public Coin GetUpperLeftCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row - 1, i_Coin.Point.Column - 1];
        }

        public Coin GetLowerRightCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row + 1, i_Coin.Point.Column + 1];
        }

        public Coin GetLowerLeftCell(Coin i_Coin)
        {
            return m_GameBoard[i_Coin.Point.Row + 1, i_Coin.Point.Column - 1];
        }

        public bool CheckIfInBoardLimit(int i_Row, int i_Column)
        {
            return (i_Column >= 0 && i_Column < this.Size) &&
                (i_Row >= 0 && i_Row < this.Size);
        }

        public bool IsInCorner(int i_Row, int i_Column)
        {
            return (i_Row == 0 && i_Column == 0) || (i_Row == 0 && i_Column == this.Size - 1)
                || (i_Row == this.Size - 1 && i_Column == this.Size - 1) || (i_Row == this.Size - 1 && i_Column == 0);
        }

        public bool IsNearCorners(int i_Row, int i_Column)
        {
            return (i_Row == 0 && i_Column == 1) || (i_Row == 1 && i_Column == 0)
                || (i_Row == 1 && i_Column == 1) || (i_Row == 0 && i_Column == this.Size - 2)
                || (i_Row == 1 && i_Column == this.Size - 2) || (i_Row == 1 && i_Column == this.Size - 1)
                || (i_Row == this.Size - 2 && i_Column == 0) || (i_Row == this.Size - 2 && i_Column == 1)
                || (i_Row == this.Size - 1 && i_Column == 1) || (i_Row == this.Size - 1 && i_Column == this.Size - 2)
                || (i_Row == this.Size - 2 && i_Column == this.Size - 2) || (i_Row == this.Size - 2 && i_Column == this.Size - 1);
        }
    }
}
