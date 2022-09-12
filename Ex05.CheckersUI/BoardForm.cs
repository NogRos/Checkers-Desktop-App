using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.CheckersUI
{
    internal partial class BoardForm : Form
    {
        private readonly BoardButton[][] r_BoardButtons;
        private readonly GameManager r_GameManager;
        private readonly HashSet<BoardButton> r_NextPossibleDestinations = new HashSet<BoardButton>();
        private readonly int[][] r_NextMoveCoordinates = new int[2][];
        private readonly bool r_ShowPossibleMovesMode;
        private bool m_DuringMove = false;
        private BoardButton m_HighlightedButton;

        internal BoardForm(GameSettingsForm.InitPackage i_InitPackage)
        {
            InitializeComponent();
            r_GameManager = new GameManager(i_InitPackage.FirstUserName, i_InitPackage.GameMode, i_InitPackage.BoardSize, i_InitPackage.SecondUserName);
            r_BoardButtons = new BoardButton[i_InitPackage.BoardSize][];
            r_ShowPossibleMovesMode = i_InitPackage.ShowPossibleMoves;
            r_NextMoveCoordinates[0] = new int[2];
            r_NextMoveCoordinates[1] = new int[2];
            prepareBoard(i_InitPackage);
        }

        private BoardButton HighlightedButton
        {
            get
            {
                return m_HighlightedButton;
            }

            set
            {
                if (value != null)
                {
                    value.Highlight();
                }
                else
                {
                    if (m_HighlightedButton != null)
                    {
                        m_HighlightedButton.Dehighlight();
                    }
                }

                m_HighlightedButton = value;
            }
        }

        private void prepareBoard(GameSettingsForm.InitPackage i_InitPackage)
        {
            int sizeMultiplier = 95;

            ClientSize = new Size(i_InitPackage.BoardSize * sizeMultiplier, i_InitPackage.BoardSize * sizeMultiplier);

            setPlayerLabels();

            setBoardButtons(i_InitPackage.BoardSize);
        }

        private void setBoardButtons(int i_BoardSize)
        {
            Checker[][] boardSnapshot = r_GameManager.BoardSnapshot;
            float boardYMultiplier = 0.15f;
            int buttonSize = ClientSize.Width / (i_BoardSize + 2);
            for (int i = 0; i < i_BoardSize; i++)
            {
                r_BoardButtons[i] = new BoardButton[i_BoardSize];
                for (int j = 0; j < i_BoardSize; j++)
                {
                    BoardButton currentButton;

                    if ((i + j) % 2 == 0)
                    {
                        currentButton = new BoardButton(BoardButton.eBoardButtonType.Black, i, j);
                        currentButton.Enabled = false;
                    }
                    else
                    {
                        currentButton = new BoardButton(BoardButton.eBoardButtonType.White, i, j);
                        currentButton.Click += boardButton_Click;
                    }

                    currentButton.Size = new Size(buttonSize, buttonSize);
                    currentButton.Location = new Point((j + 1) * buttonSize, (int)(ClientSize.Height * boardYMultiplier) + (i * buttonSize));

                    r_BoardButtons[i][j] = currentButton;
                    Controls.Add(currentButton);
                }
            }

            updateFormWithCheckers();
        }

        private void updateFormWithCheckers()
        {
            Checker[][] boardSnapshot = r_GameManager.BoardSnapshot;
            for (int i = 0; i < r_GameManager.BoardSize; i++)
            {
                for (int j = 0; j < r_GameManager.BoardSize; j++)
                {
                    Checker currentChecker = boardSnapshot[i][j];
                    BoardButton currentButton = r_BoardButtons[i][j];
                    if (currentChecker != null)
                    {
                        if (currentChecker.Team == Checker.eTeams.White)
                        {
                            if (currentChecker.Type == Checker.eTypes.King)
                            {
                                currentButton.BackgroundImage = Properties.Resources.WhiteKingIcon;
                            }
                            else
                            {
                                currentButton.BackgroundImage = Properties.Resources.WhiteCheckerIcon;
                            }
                        }
                        else
                        {
                            if (currentChecker.Type == Checker.eTypes.King)
                            {
                                currentButton.BackgroundImage = Properties.Resources.BlackKingIcon;

                            }
                            else
                            {
                                currentButton.BackgroundImage = Properties.Resources.BlackCheckerIcon;
                            }
                        }

                        currentButton.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        currentButton.BackgroundImage = null;
                    }
                }
            }
        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            BoardButton clickedButton = sender as BoardButton;
            Checker[][] gameBoard = r_GameManager.BoardSnapshot;
            Checker clickedChecker = gameBoard[clickedButton.X][clickedButton.Y];

            if (clickedChecker != null && clickedChecker.Team == r_GameManager.CurrentTurnPlayer.Team && clickedButton != HighlightedButton)
            {
                if (m_DuringMove)
                {
                    r_BoardButtons[r_NextMoveCoordinates[0][0]][r_NextMoveCoordinates[0][1]].Dehighlight();
                }

                r_NextMoveCoordinates[0][0] = clickedButton.X;
                r_NextMoveCoordinates[0][1] = clickedButton.Y;
                m_DuringMove = true;
                HighlightedButton = clickedButton;
                highlightPossibleDestinations(clickedChecker);
            }
            else if (m_DuringMove && clickedChecker == null)
            {
                r_NextMoveCoordinates[1][0] = clickedButton.X;
                r_NextMoveCoordinates[1][1] = clickedButton.Y;
                Move inputMove = r_GameManager.BuildMove(cloneCoordinates(r_NextMoveCoordinates));
                if (r_GameManager.IsMoveValid(inputMove))
                {
                    makeMove(inputMove);
                    m_DuringMove = false;
                    if (r_GameManager.IsRoundOver())
                    {
                        announceWinner();
                    }
                    else if (!r_GameManager.IsCurrentTurnHuman)
                    {
                        Task.Delay(100).ContinueWith(t => makeComputerMove());
                    }
                }
                else
                {
                    MessageBox.Show("Invalid move.\nPlease select a valid square.", "Invalid move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (clickedButton == HighlightedButton)
            {
                m_DuringMove = false;
                HighlightedButton = null;
                dehighlightPossibleDestinations();
            }
        }

        private void highlightPossibleDestinations(Checker i_Checker)
        {
            if (r_ShowPossibleMovesMode)
            {
                dehighlightPossibleDestinations();
                List<Move> possibleCheckerMoves = r_GameManager.GetPossibleCheckerMoves(i_Checker);
                foreach (Move move in possibleCheckerMoves)
                {
                    BoardButton buttonToHighlight = r_BoardButtons[move.TargetPosition[0]][move.TargetPosition[1]];
                    buttonToHighlight.HighlightAsDestination();
                    r_NextPossibleDestinations.Add(buttonToHighlight);
                }
            }
        }

        private void dehighlightPossibleDestinations()
        {
            if (r_ShowPossibleMovesMode)
            {
                foreach (BoardButton button in r_NextPossibleDestinations)
                {
                    button.Dehighlight();
                }

                r_NextPossibleDestinations.Clear();
            }
        }

        private void makeMove(Move i_Move)
        {
            r_GameManager.PerformMove(i_Move);
            HighlightedButton = null;
            updateFormWithCheckers();
            updateTurnLabel();
            dehighlightPossibleDestinations();
        }

        private void makeComputerMove()
        {
            r_GameManager.PerformComputerMove();
            updateFormWithCheckers();
            updateTurnLabel();
            if (!r_GameManager.IsCurrentTurnHuman)
            {
                Task.Delay(100).ContinueWith(t => makeComputerMove());
            }
            else if (r_GameManager.IsRoundOver())
            {
                announceWinner();
            }
        }

        private void setPlayerLabels()
        {
            float player1NameXMultiplier = 0.3f;
            float player2NameXMultiplier = 0.6f;
            float playerNamesYMultiplier = 0.08f;

            updateScore(r_GameManager.WhitePlayer);
            updateScore(r_GameManager.BlackPlayer);

            player1Label.Location = new Point((int)(ClientSize.Width * player1NameXMultiplier), (int)(ClientSize.Height * playerNamesYMultiplier));
            player2Label.Location = new Point((int)(ClientSize.Width * player2NameXMultiplier), (int)(ClientSize.Height * playerNamesYMultiplier));

            setLabelSizes(new Label[] { player1Label, player2Label, turnLabel });

            updateTurnLabel();
        }

        private void setLabelSizes(Label[] i_Labels)
        {
            foreach (Label label in i_Labels)
            {
                label.Font = new Font(label.Font.FontFamily, label.Font.Size * r_GameManager.BoardSize * 0.15f);
            }
        }

        private int[][] cloneCoordinates(int[][] i_Coordinates)
        {
            int[][] clonedCoordinates = new int[i_Coordinates.Length][];
            clonedCoordinates[0] = new int[i_Coordinates[0].Length];
            clonedCoordinates[1] = new int[i_Coordinates[1].Length];
            clonedCoordinates[0][0] = i_Coordinates[0][0];
            clonedCoordinates[0][1] = i_Coordinates[0][1];
            clonedCoordinates[1][0] = i_Coordinates[1][0];
            clonedCoordinates[1][1] = i_Coordinates[1][1];

            return clonedCoordinates;
        }

        private void updateTurnLabel()
        {
            turnLabel.Text = string.Format("It's {0}'s turn", r_GameManager.CurrentTurnPlayer.Name);
        }

        private void updateScore(Player i_WinningPlayer)
        {
            if (i_WinningPlayer.Team == Checker.eTeams.White)
            {
                player1Label.Text = string.Format("{0}: {1}", i_WinningPlayer.Name, i_WinningPlayer.GameScore);
            }
            else
            {
                player2Label.Text = string.Format("{0}: {1}", i_WinningPlayer.Name, i_WinningPlayer.GameScore);
            }
        }

        private void announceWinner()
        {
            Player winningPlayer = r_GameManager.GetWinner();
            DialogResult dialogResult;
            if (winningPlayer != null)
            {
                r_GameManager.UpdateWinningPlayerScore(winningPlayer);
                updateScore(winningPlayer);
                playWinSound();
                dialogResult = MessageBox.Show(string.Format("{0} Won!\nAnother Round?", r_GameManager.GetWinner().Name), "Checkers", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                dialogResult = MessageBox.Show("Tie!\nAnother Round?", "Checkers", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            haveAnotherRound(dialogResult);
        }

        private void haveAnotherRound(DialogResult i_DialogResult)
        {
            if (i_DialogResult == DialogResult.Yes)
            {
                r_GameManager.RestartRound();
                updateFormWithCheckers();
                updateTurnLabel();
            }
            else
            {
                Close();
            }
        }

        private void playWinSound()
        {
            Stream stream = Properties.Resources.tada;
            SoundPlayer winningSound = new SoundPlayer(stream);
            winningSound.Play();
        }
    }
}
