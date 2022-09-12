using System;
using System.Collections.Generic;

namespace Ex05.CheckersLogic
{
    public class GameManager
    {
        private Board m_Board;
        private readonly eGameMode r_GameMode;

        public GameManager(string i_FirstUserName, eGameMode i_GameMode, int i_BoardSize, string i_SecondUserName)
        {
            r_GameMode = i_GameMode;
            initPlayers(i_FirstUserName, i_SecondUserName, i_GameMode);
            m_Board = new Board(WhitePlayer, BlackPlayer, i_BoardSize);
            CurrentTurnPlayer = WhitePlayer;
        }

        public enum eGameMode
        {
            OnePlayer,
            TwoPlayers,
        }

        public Player CurrentTurnPlayer { get; private set; }

        public Player WhitePlayer { get; private set; }

        public Player BlackPlayer { get; private set; }

        public Move LastMove { get { return m_Board.LastMove; } }

        public int BoardSize { get => m_Board.Size; }

        public Checker[][] BoardSnapshot { get => m_Board.Snapshot; }

        public bool IsCurrentTurnHuman { get { return CurrentTurnPlayer == WhitePlayer || r_GameMode == eGameMode.TwoPlayers; } }

        public void RestartRound()
        {
            m_Board = new Board(WhitePlayer, BlackPlayer, BoardSize);
            CurrentTurnPlayer = WhitePlayer;
        }

        public void PerformMove(Move i_Move)
        {
            m_Board.PerformMove(i_Move);
            if (!i_Move.IsContinuous)
            {
                switchTurns();
            }
        }

        public void PerformComputerMove()
        {
            performIntelligentComputerMove();
        }

        public Move BuildMove(int[][] i_Coordinates)
        {
            return new Move(i_Coordinates, BoardSnapshot, CurrentTurnPlayer, LastMove);
        }

        public bool IsMoveValid(Move i_Move)
        {
            return i_Move.IsValid && (i_Move.IsEating || !m_Board.AnyPossibleEatingMoves(i_Move.MovingPlayer));
        }

        public bool IsRoundOver()
        {
            return !m_Board.AnyPossibleMoves(CurrentTurnPlayer);
        }

        public Player GetWinner()
        {
            Player winner;

            bool blackPlayerHasPossibleMoves = m_Board.AnyPossibleMoves(BlackPlayer);
            bool whitePlayerHasPossibleMoves = m_Board.AnyPossibleMoves(WhitePlayer);

            if (!blackPlayerHasPossibleMoves && !whitePlayerHasPossibleMoves)
            {
                winner = null;
            }
            else if (blackPlayerHasPossibleMoves)
            {
                winner = BlackPlayer;
            }
            else
            {
                winner = WhitePlayer;
            }

            return winner;
        }

        public void UpdateWinningPlayerScore(Player i_Player)
        {
            int score = 0;
            foreach (Checker checker in i_Player.CheckerList)
            {
                score += (int)checker.Type;
            }

            foreach (Checker checker in GetOppositePlayer(i_Player).CheckerList)
            {
                score -= (int)checker.Type;
            }

            i_Player.GameScore += Math.Abs(score);
        }

        public Player GetOppositePlayer(Player i_Player)
        {
            if (i_Player == WhitePlayer)
            {
                return BlackPlayer;
            }
            else
            {
                return WhitePlayer;
            }
        }

        public List<Move> GetPossibleCheckerMoves(Checker i_Checker)
        {
            List<Move> possibleMoves;

            Player matchingPlayer;

            if (i_Checker.Team == Checker.eTeams.White)
            {
                matchingPlayer = WhitePlayer;
            }
            else
            {
                matchingPlayer = BlackPlayer;
            }

            if (m_Board.AnyPossibleEatingMoves(matchingPlayer))
            {
                possibleMoves = m_Board.GetPossibleCheckerEatingMoves(i_Checker);
            }
            else
            {
                possibleMoves = m_Board.GetPossibleCheckerMoves(i_Checker);
            }

            return possibleMoves;
        }

        private void initPlayers(string i_FirstUserName, string i_SecondUserName, eGameMode i_GameMode)
        {
            WhitePlayer = new Player() { Name = i_FirstUserName, Team = Checker.eTeams.White, IsHuman = true };
            if (i_GameMode == eGameMode.TwoPlayers)
            {
                BlackPlayer = new Player() { Name = i_SecondUserName, Team = Checker.eTeams.Black, IsHuman = true };
            }
            else
            {
                BlackPlayer = new Player() { Name = "Computer", Team = Checker.eTeams.Black, IsHuman = false };
            }
        }

        private void switchTurns()
        {
            CurrentTurnPlayer = GetOppositePlayer(CurrentTurnPlayer);
        }

        /*
         * Implementation of the Minimax algorithm.
         */
        private void performIntelligentComputerMove()
        {
            Player maximizingPlayer = CurrentTurnPlayer.GetClone();
            Player minimizingPlayer = GetOppositePlayer(CurrentTurnPlayer).GetClone();
            Board fictionalBoard = m_Board.GetClone(minimizingPlayer, maximizingPlayer);
            Move chosenMove = getMaxEvaluationMove(fictionalBoard, maximizingPlayer, minimizingPlayer);
            chosenMove = chosenMove.GetClone(BoardSnapshot, maximizingPlayer, LastMove);
            PerformMove(chosenMove);
        }

        private Move getMaxEvaluationMove(Board i_FictionalBoard, Player i_MaximizingPlayer, Player i_MinimizingPlayer)
        {
            Move maxEvaluatedMove = null;
            int maxEvaluation = -100;
            List<Move> possibleMoves = i_FictionalBoard.GetPossibleMoves(i_MaximizingPlayer);
            foreach (Move move in possibleMoves)
            {
                Player maximizingPlayer = i_MaximizingPlayer.GetClone();
                Player minimizingPlayer = i_MinimizingPlayer.GetClone();
                Board fictionalBoard = i_FictionalBoard.GetClone(minimizingPlayer, maximizingPlayer);
                int numberOfMovesToLookAhead;
                if (BoardSize < 10)
                {
                    numberOfMovesToLookAhead = 10;
                }
                else
                {
                    numberOfMovesToLookAhead = 8;
                }

                int moveEvaluation = evaluateSequence(fictionalBoard, move, maximizingPlayer, minimizingPlayer, numberOfMovesToLookAhead, false, -200, 200);
                if (moveEvaluation >= maxEvaluation)
                {
                    maxEvaluatedMove = move;
                    maxEvaluation = moveEvaluation;
                }
            }

            return maxEvaluatedMove;
        }

        private int evaluateSequence(Board i_FictionalBoard, Move i_Move, Player i_MaximizingPlayer, Player i_MinimizingPlayer, int i_Depth, bool i_IsMaximizingPlayer, int i_Alpha, int i_Beta)
        {
            int evaluation;

            if (i_Depth == 0)
            {
                evaluation = i_MaximizingPlayer.CheckerList.Count - i_MinimizingPlayer.CheckerList.Count;
            }
            else
            {

                i_FictionalBoard.PerformMove(i_Move);

                if (i_IsMaximizingPlayer)
                {
                    // Get all possible moves
                    // foreach (move in possibleMoves): Create a new copy of the board and players,
                    List<Move> possibleMoves = i_FictionalBoard.GetPossibleMoves(i_MaximizingPlayer);
                    int maxEvaluation = -100;
                    foreach (Move move in possibleMoves)
                    {
                        Player newMaximizingPlayer = i_MaximizingPlayer.GetClone();
                        Player newMinimizingPlayer = i_MinimizingPlayer.GetClone();
                        Board newFictionalBoard = i_FictionalBoard.GetClone(newMinimizingPlayer, newMaximizingPlayer);
                        int moveEvaluation = evaluateSequence(newFictionalBoard, move, newMaximizingPlayer, newMaximizingPlayer, i_Depth - 1, false, i_Alpha, i_Beta);
                        maxEvaluation = Math.Max(maxEvaluation, moveEvaluation);
                        i_Alpha = Math.Max(i_Alpha, moveEvaluation);
                        if (i_Beta <= i_Alpha)
                        {
                            break;
                        }
                    }

                    evaluation = maxEvaluation;
                }
                else
                {
                    List<Move> possibleMoves = i_FictionalBoard.GetPossibleMoves(i_MinimizingPlayer);
                    int minEvaluation = 100;
                    foreach (Move move in possibleMoves)
                    {
                        Player newMaximizingPlayer = i_MaximizingPlayer.GetClone();
                        Player newMinimizingPlayer = i_MinimizingPlayer.GetClone();
                        Board newFictionalBoard = i_FictionalBoard.GetClone(newMinimizingPlayer, newMaximizingPlayer);
                        int moveEvaluation = evaluateSequence(newFictionalBoard, move, newMaximizingPlayer, newMaximizingPlayer, i_Depth - 1, true, i_Alpha, i_Beta);
                        minEvaluation = Math.Min(minEvaluation, moveEvaluation);
                        i_Beta = Math.Min(i_Beta, moveEvaluation);
                        if (i_Beta <= i_Alpha)
                        {
                            break;
                        }
                    }

                    evaluation = minEvaluation;
                }
            }

            return evaluation;
        }
    }
}
