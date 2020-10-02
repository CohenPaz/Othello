using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsUI
{
    public class OthelloWindowsUI
    {
        private OthelloGameForm m_OthelloFrom;
        private GameSettingsForm m_GameSettings;

        public void RunOthelloGame()
        {
            m_GameSettings = new GameSettingsForm();
            m_GameSettings.ShowDialog();
            if (m_GameSettings.DialogResult == DialogResult.OK)
            {
                m_OthelloFrom = new OthelloGameForm(m_GameSettings.GameMode, m_GameSettings.BoardSize);
                m_OthelloFrom.ShowDialog();
            }
        }
    }
}
