using System;
using System.Collections.Generic;

namespace OthelloLogic
{
    public class Player
    {
        private List<Point> m_CellsPossibilitiesForNextTurn;
        private Coin.eCoinColor m_PlayerColorOfCoin;
        private ePlayerTypes m_PlayerType;
        private int m_NumberOfWinnings;

        public enum ePlayerTypes
        {
            HumanPlayer,
            Computer
        }

        public Player(ePlayerTypes i_PlayerType, Coin.eCoinColor i_PlayerColorOfCoin)
        {
            m_PlayerType = i_PlayerType;
            m_PlayerColorOfCoin = i_PlayerColorOfCoin;
        }

        public ePlayerTypes PlayerType
        {
            get
            {
                return m_PlayerType;
            }
        }

        public Coin.eCoinColor PlayerColorOfCoin
        {
            get
            {
                return m_PlayerColorOfCoin;
            }
        }

        public List<Point> PossibilitiesCellsForNextTurn
        {
            get
            {
                return m_CellsPossibilitiesForNextTurn;
            }

            set
            {
                m_CellsPossibilitiesForNextTurn = value;
            }
        }

        public int NumberOfWinnings
        {
            get
            {
                return m_NumberOfWinnings;
            }

            set
            {
                m_NumberOfWinnings = value;
            }
        }
    }
}
