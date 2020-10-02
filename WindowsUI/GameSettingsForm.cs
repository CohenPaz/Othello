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
    public partial class GameSettingsForm : Form
    {
        private const int m_IncreaseSizeBy = 2;
        private const int m_MinimumBoardSize = 6;
        private const int m_MaximumBoardSize = 12;
        private GameManager.eGameModes m_GameMode;
        private int m_ChosenBoardSize = m_MinimumBoardSize;

        public GameSettingsForm()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
        }

        public GameManager.eGameModes GameMode
        {
            get
            {
                return m_GameMode;
            }
        }

        public int BoardSize
        {
            get
            {
                return m_ChosenBoardSize;
            }
        }

        private void AgainstComputerButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            m_GameMode = GameManager.eGameModes.HumanVsComputer;
            this.Close();
        }

        private void AgainstPlayerButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            m_GameMode = GameManager.eGameModes.HumanVsHuman;
            this.Close();
        }

        private void BoardSizeButton_Click(object sender, EventArgs e)
        {
            int oldBoardSize;

            oldBoardSize = m_ChosenBoardSize;
            m_ChosenBoardSize = oldBoardSize == m_MaximumBoardSize ? m_MinimumBoardSize : oldBoardSize + m_IncreaseSizeBy;
            BoardSizeButton.Text = BoardSizeButton.Text.Replace(oldBoardSize.ToString(), m_ChosenBoardSize.ToString());
        }
    }
}
