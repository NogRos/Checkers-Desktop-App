using System;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.CheckersUI
{
    public partial class GameSettingsForm : Form
    {
        public GameSettingsForm()
        {
            InitializeComponent();
        }

        internal InitPackage GetInitPackage()
        {
            InitPackage initPackage = null;
            string firstUsername;
            string secondUsername;
            int boardSize;
            GameManager.eGameMode gameMode;
            bool showPossibleMoves;

            DialogResult dialogResult = ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                firstUsername = player1TextBox.Text;
                secondUsername = player2TextBox.Text;

                boardSize = getBoardSizeByRadioButtons();

                gameMode = getGameModeByCheckBox();

                showPossibleMoves = showMovesCB.Checked;

                initPackage = new InitPackage(firstUsername, gameMode, boardSize, secondUsername, showPossibleMoves);
            }

            return initPackage;
        }

        private int getBoardSizeByRadioButtons()
        {
            int boardSize;

            if (radioButton6.Checked)
            {
                boardSize = 6;
            }
            else if (radioButton8.Checked)
            {
                boardSize = 8;
            }
            else
            {
                boardSize = 10;
            }

            return boardSize;
        }

        private GameManager.eGameMode getGameModeByCheckBox()
        {
            GameManager.eGameMode gameMode;

            if (player2CheckBox.Checked)
            {
                gameMode = GameManager.eGameMode.TwoPlayers;
            }
            else
            {
                gameMode = GameManager.eGameMode.OnePlayer;
            }

            return gameMode;
        }

        private void player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (player2CheckBox.Checked)
            {
                player2TextBox.Enabled = true;
                player2TextBox.Text = string.Empty;
            }
            else
            {
                player2TextBox.Enabled = false;
                player2TextBox.Text = "[Computer]";
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            if (!arePlayerNamesGiven())
            {
                MessageBox.Show("Please provide both player names.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private bool arePlayerNamesGiven()
        {
            return player1TextBox.Text != string.Empty && player2TextBox.Text != string.Empty;
        }

        internal class InitPackage
        {
            internal InitPackage(string i_FirstUserName, GameManager.eGameMode i_GameMode, int i_BoardSize, string i_SecondUserName, bool i_ShowPossibleMoves)
            {
                FirstUserName = i_FirstUserName;
                GameMode = i_GameMode;
                BoardSize = i_BoardSize;
                SecondUserName = i_SecondUserName;
                ShowPossibleMoves = i_ShowPossibleMoves;
            }

            internal string FirstUserName { get; }

            internal GameManager.eGameMode GameMode { get; }

            internal int BoardSize { get; }

            internal string SecondUserName { get; }

            internal bool ShowPossibleMoves { get; }
        }
    }
}
