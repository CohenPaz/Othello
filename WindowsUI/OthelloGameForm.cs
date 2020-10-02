using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OthelloLogic;

namespace WindowsUI
{
    public partial class OthelloGameForm : Form
    {
        private const int k_ButtonSize = 50;
        private const int k_StartLocationOfButton = 10;
        private const int k_SpacingBetweenButtonAndClientWindow = 20;
        private GameManager m_OthelloGame;
        private Control[,] m_BoardControls;

        public OthelloGameForm(GameManager.eGameModes i_Gamemode, int i_BoardSize)
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            m_OthelloGame = new GameManager(i_BoardSize, i_Gamemode);
            m_OthelloGame.GameBoard.CoinAdded += coinAdded;
            m_OthelloGame.GameBoard.CoinFlipped += coinFlipped;
            m_OthelloGame.SkipTurn += skipTurn;
            m_OthelloGame.PossibilitiesCellAdded += showPossibilites;
            initializeUIBoard(m_OthelloGame.GameBoard.Size);
            m_OthelloGame.InitializeBoard();
            this.Text = string.Format("Otehllo - {0}'s Turn", m_OthelloGame.CurrentPlayerTurn.PlayerColorOfCoin);
            handleNewTurn(m_OthelloGame.CurrentPlayerTurn);
        }

        private void skipTurn(Player i_CurrentPlayer)
        {
            string messageBoxTitle = "Othello - Skip Turn";
            string otherPlayer = "Red";

            if (i_CurrentPlayer.Equals(m_OthelloGame.Player1))
            {
                otherPlayer = "Yellow";
            }

            string skipTurnMessage = string.Format(
 @"{0} Has No Vaild Cells, Skipping To {1} Turn ", i_CurrentPlayer.PlayerColorOfCoin, otherPlayer);
            MessageBoxButtons endOfGameBox = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(skipTurnMessage, messageBoxTitle, endOfGameBox, MessageBoxIcon.Information);
        }

        private void boardButton_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int row, column;

            row = (button.Top - k_StartLocationOfButton) / k_ButtonSize;
            column = (button.Left - k_StartLocationOfButton) / k_ButtonSize;
            m_OthelloGame.AddCoinToBoard(new OthelloLogic.Point(row, column));
            Refresh();
            removePossibilitiesButtons(m_OthelloGame.CurrentPlayerTurn);
            Refresh();
            m_OthelloGame.ChangeCurrentPlayerTurn();
            handleNewTurn(m_OthelloGame.CurrentPlayerTurn);
        }

        private void handleNewTurn(Player i_CurrentPlayer)
        {
            if (m_OthelloGame.HandlePlayerTurn(i_CurrentPlayer))
            {
                handleEndGame();
            }
            else
            {
                ChangeFormText(m_OthelloGame.CurrentPlayerTurn.PlayerColorOfCoin);
                Refresh();
                handleComputerTurn();
            }
        }

        private void handleComputerTurn()
        {
            if (m_OthelloGame.CurrentPlayerTurn.PlayerType.Equals(Player.ePlayerTypes.Computer))
            {
                OthelloLogic.Point computerBestMove;
                computerBestMove = ComputerAI.FindBestMoveForComputer(m_OthelloGame, m_OthelloGame.CurrentPlayerTurn, m_OthelloGame.CurrentPlayerTurn.PossibilitiesCellsForNextTurn);
                Button buttonClickedByComputer = (Button)m_BoardControls[computerBestMove.Row, computerBestMove.Column];
                System.Threading.Thread.Sleep(650);
                boardButton_Clicked(buttonClickedByComputer, EventArgs.Empty);
            }
        }

        private void handleEndGame()
        {
            GameManager.eGameStatus whoWon;
            whoWon = m_OthelloGame.CheckingScoreBoard();
            m_OthelloGame.HandleWhoWon(whoWon);
            string messageBoxTitle = "Othello";
            string endGameMessage = createEndGameMessage(whoWon);
            MessageBoxButtons endOfGameBox = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(endGameMessage, messageBoxTitle, endOfGameBox, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                startNewGame();
            }
            else
            {
                this.Close();
            }
        }

        private void startNewGame()
        {
            this.Controls.Clear();
            createBoardMatrix(m_OthelloGame.GameBoard.Size);
            m_OthelloGame.InitializeBoard();
            m_OthelloGame.CurrentPlayerTurn = m_OthelloGame.Player1;
            this.Text = string.Format("Otehllo - {0}'s Turn", m_OthelloGame.CurrentPlayerTurn.PlayerColorOfCoin);
            handleNewTurn(m_OthelloGame.CurrentPlayerTurn);
        }

        private string createEndGameMessage(GameManager.eGameStatus i_WhoWon)
        {
            StringBuilder endGameMesaage = new StringBuilder();
            int winnerScore, loserScore, winnerNumberOfWinnings, loserNumberOfWinnings;

            if (i_WhoWon.Equals(GameManager.eGameStatus.Player2Won))
            {
                endGameMesaage.AppendFormat("{0} Won!! ", m_OthelloGame.Player2.PlayerColorOfCoin.ToString());
                winnerScore = m_OthelloGame.GameBoard.YellowLinkedList.Count;
                loserScore = m_OthelloGame.GameBoard.RedLinkedList.Count;
                winnerNumberOfWinnings = m_OthelloGame.Player2.NumberOfWinnings;
                loserNumberOfWinnings = m_OthelloGame.Player1.NumberOfWinnings;
            }
            else
            {
                if (i_WhoWon.Equals(GameManager.eGameStatus.Player1Won))
                {
                    endGameMesaage.AppendFormat("{0} Won!! ", m_OthelloGame.Player1.PlayerColorOfCoin.ToString());
                }
                else
                {
                    endGameMesaage.AppendFormat("There Is A Tie!! ");
                }

                winnerScore = m_OthelloGame.GameBoard.RedLinkedList.Count;
                loserScore = m_OthelloGame.GameBoard.YellowLinkedList.Count;
                winnerNumberOfWinnings = m_OthelloGame.Player1.NumberOfWinnings;
                loserNumberOfWinnings = m_OthelloGame.Player2.NumberOfWinnings;
            }

            endGameMesaage.AppendFormat(
@"({0}/{1}) ({2}/{3})
Would You Like Another Round?",
winnerScore,
loserScore,
winnerNumberOfWinnings,
loserNumberOfWinnings);
            return endGameMesaage.ToString();
        }

        private void showPossibilites(Player i_Player)
        {
            foreach (OthelloLogic.Point point in i_Player.PossibilitiesCellsForNextTurn)
            {
                m_BoardControls[point.Row, point.Column].BackColor = Color.LimeGreen;
                if (i_Player.PlayerType != Player.ePlayerTypes.Computer)
                {
                    m_BoardControls[point.Row, point.Column].Enabled = true;
                }
            }
        }

        private void coinAdded(Coin i_Coin)
        {
            Control newCoin = new PictureBox();

            ((PictureBox)newCoin).Location = m_BoardControls[i_Coin.Point.Row, i_Coin.Point.Column].Location;
            ((PictureBox)newCoin).Enabled = false;
            ((PictureBox)newCoin).SizeMode = PictureBoxSizeMode.StretchImage;
            ((PictureBox)newCoin).Size = new Size(k_ButtonSize, k_ButtonSize);
            ((PictureBox)newCoin).BorderStyle = BorderStyle.Fixed3D;
            ((PictureBox)newCoin).BackColor = Color.FromKnownColor(KnownColor.LightGray);
            m_BoardControls[i_Coin.Point.Row, i_Coin.Point.Column].Visible = false;
            m_BoardControls[i_Coin.Point.Row, i_Coin.Point.Column] = newCoin;
            addPicture(i_Coin.Color, (PictureBox)newCoin);
            if (m_OthelloGame.CurrentPlayerTurn.PlayerType.Equals(Player.ePlayerTypes.Computer))
            {
                System.Threading.Thread.Sleep(350);
            }

            this.Controls.Add(newCoin);
        }

        private void addPicture(Coin.eCoinColor i_Color, PictureBox i_PictureBox)
        {
            if (i_Color == Coin.eCoinColor.Red)
            {
                i_PictureBox.Image = WindowsUI.Properties.Resources.CoinRed;
            }
            else
            {
                i_PictureBox.Image = WindowsUI.Properties.Resources.CoinYellow;
            }
        }

        private void coinFlipped(Coin i_Coin)
        {
            addPicture(i_Coin.Color, (PictureBox)m_BoardControls[i_Coin.Point.Row, i_Coin.Point.Column]);
        }

        private void ChangeFormText(Coin.eCoinColor i_Color)
        {
            string formText;
            if (i_Color.Equals(Coin.eCoinColor.Red))
            {
                formText = string.Format(@"Otehllo - {0}'s Turn", Coin.eCoinColor.Red.ToString());
            }
            else
            {
                formText = string.Format(@"Otehllo - {0}'s Turn", Coin.eCoinColor.Yellow.ToString());
            }

            this.Text = formText;
        }

        private void initializeUIBoard(int i_BoardSize)
        {
            int clientSize = (i_BoardSize * k_ButtonSize) + k_SpacingBetweenButtonAndClientWindow;

            this.ClientSize = new System.Drawing.Size(clientSize, clientSize);
            m_BoardControls = new Control[i_BoardSize, i_BoardSize];
            createBoardMatrix(i_BoardSize);
        }

        private void createBoardMatrix(int i_BoardSize)
        {
            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int column = 0; column < i_BoardSize; column++)
                {
                    addButtonToBoard(row, column);
                }
            }
        }

        private void addButtonToBoard(int i_RowPosition, int i_ColumnPosition)
        {
            Button newButton = new Button();
            int buttonLeft = k_StartLocationOfButton + (i_ColumnPosition * k_ButtonSize);
            int buttonTop = k_StartLocationOfButton + (i_RowPosition * k_ButtonSize);

            newButton.Location = new System.Drawing.Point(buttonLeft, buttonTop);
            newButton.Size = new System.Drawing.Size(k_ButtonSize, k_ButtonSize);
            newButton.Enabled = false;
            newButton.TabStop = true;
            newButton.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            newButton.Click += new System.EventHandler(boardButton_Clicked);
            this.Controls.Add(newButton);
            m_BoardControls[i_RowPosition, i_ColumnPosition] = newButton;
        }

        private void removePossibilitiesButtons(Player i_Player)
        {
            foreach (OthelloLogic.Point point in i_Player.PossibilitiesCellsForNextTurn)
            {
                m_BoardControls[point.Row, point.Column].BackColor = Color.FromKnownColor(KnownColor.LightGray);
                m_BoardControls[point.Row, point.Column].Enabled = false;
            }
        }
    }
}
