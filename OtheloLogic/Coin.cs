using System;

namespace OthelloLogic
{
    public class Coin
    {
        private eCoinColor m_CoinColor;
        private Point m_LocationOfCoin;

        public Coin(eCoinColor i_Color, int i_Row, int i_Column)
        {
            m_CoinColor = i_Color;
            m_LocationOfCoin = new Point(i_Row, i_Column);
        }

        public Coin CloneCoin()
        {
            Coin clonedCoin;

            return clonedCoin = new Coin(this.Color, this.Point.Row, this.Point.Column);
        }

        public Point Point
        {
            get
            {
                return m_LocationOfCoin;
            }
        }

        public eCoinColor Color
        {
            get
            {
                return m_CoinColor;
            }

            set
            {
                m_CoinColor = value;
            }
        }

        public enum eCoinColor
        {
            Red,
            Yellow
        }
    }
}
