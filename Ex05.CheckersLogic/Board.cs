using System;
using System.Collections.Generic;

namespace Ex05.CheckersLogic
{
    internal class Board
    {
        private readonly Checker[][] r_Board;
        private readonly Player r_WhitePlayer;
        private readonly Player r_BlackPlayer;

        //Test
        private Checker followedChecker;
        //

        internal Board(Player i_WhitePlayer, Player i_BlackPlayer, int i_BoardSize)
        {
            r_WhitePlayer = i_WhitePlayer;
            r_BlackPlayer = i_BlackPlayer;
            Size = i_BoardSize;
            r_Board = getBoardArray(i_BoardSize);

            spawnCheckers();
        }

        /*
         * A private constructor that receives the checkers array field.
         */
        private Board(Player i_WhitePlayer, Player i_BlackPlayer, Checker[][] i_Board)
        {
            r_WhitePlayer = i_WhitePlayer;
            r_BlackPlayer = i_BlackPlayer;
            r_Board = i_Board;
            Size = r_Board.Length;
        }

        internal int Size { get; }

        internal Checker[][] Snapshot { get => r_Board; }

        internal Move LastMove { get; private set; }

        /*
         * Creates and returns a new instance of Board, where the m_Board field and the Player fields are cloned with a *deep copy*.
         */
        internal Board GetClone(Player i_WhitePlayer, Player i_BlackPlayer)
        {
            Checker[][] clonedBoard = getBoardArray(Size);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (r_Board[i][j] != null)
                    {
                        clonedBoard[i][j] = r_Board[i][j].GetClone();
                        if (clonedBoard[i][j].Team == Checker.eTeams.White)
                        {
                            i_WhitePlayer.CheckerList.Add(clonedBoard[i][j]);
                        }
                        else
                        {
                            i_BlackPlayer.CheckerList.Add(clonedBoard[i][j]);
                        }
                    }
                }
            }

            return new Board(i_WhitePlayer, i_BlackPlayer, clonedBoard);
        }

        internal void PerformMove(Move i_Move)
        {
            LastMove = i_Move;
            r_Board[i_Move.TargetPosition[0]][i_Move.TargetPosition[1]] = i_Move.Checker;
            i_Move.Checker.Position = i_Move.TargetPosition;
            r_Board[i_Move.StartPosition[0]][i_Move.StartPosition[1]] = null;
            if (i_Move.IsEating)
            {
                Checker eatenChecker = r_Board[i_Move.EatenCheckerPosition[0]][i_Move.EatenCheckerPosition[1]];
                r_Board[i_Move.EatenCheckerPosition[0]][i_Move.EatenCheckerPosition[1]] = null;
                if (eatenChecker.Team == Checker.eTeams.White)
                {
                    r_WhitePlayer.CheckerList.Remove(eatenChecker);
                }
                else
                {
                    r_BlackPlayer.CheckerList.Remove(eatenChecker);
                }
            }

            if (isBecomingKing(i_Move))
            {
                i_Move.Checker.Type = Checker.eTypes.King;
            }
        }

        internal List<Move> GetPossibleCheckerEatingMoves(Checker i_Checker)
        {
            List<Move> possibleCheckerEatingMoves;
            Player matchingPlayer;

            if (i_Checker.Team == Checker.eTeams.White)
            {
                matchingPlayer = r_WhitePlayer;
            }
            else
            {
                matchingPlayer = r_BlackPlayer;
            }

            possibleCheckerEatingMoves = getPossibleCheckerEatingMoves(matchingPlayer, i_Checker);

            return possibleCheckerEatingMoves;
        }

        internal List<Move> GetPossibleCheckerMoves(Checker i_Checker)
        {
            List<Move> possibleCheckerMoves;
            Player matchingPlayer;

            if (i_Checker.Team == Checker.eTeams.White)
            {
                matchingPlayer = r_WhitePlayer;
            }
            else
            {
                matchingPlayer = r_BlackPlayer;
            }

            possibleCheckerMoves = getPossibleCheckerEatingMoves(matchingPlayer, i_Checker);
            if (possibleCheckerMoves.Count == 0)
            {
                possibleCheckerMoves.AddRange(getPossibleCheckerRegularMoves(matchingPlayer, i_Checker));
            }

            return possibleCheckerMoves;
        }

        internal List<Move> GetPossibleMoves(Player i_Player)
        {
            List<Move> possibleMoves = GetPossibleEatingMoves(i_Player);
            if (possibleMoves.Count == 0)
            {
                possibleMoves.AddRange(GetPossibleRegularMoves(i_Player));
            }

            return possibleMoves;
        }

        internal List<Move> GetPossibleRegularMoves(Player i_Player)
        {
            List<Move> playerPossibleRegularMoves = new List<Move>();
            foreach (Checker checker in i_Player.CheckerList)
            {
                List<Move> checkerMoves = getPossibleCheckerRegularMoves(i_Player, checker);
                playerPossibleRegularMoves.AddRange(checkerMoves);
            }

            return playerPossibleRegularMoves;
        }

        internal List<Move> GetPossibleEatingMoves(Player i_Player)
        {
            List<Move> playerPossibleEatingMoves = new List<Move>();
            foreach (Checker checker in i_Player.CheckerList)
            {
                List<Move> checkerMoves = getPossibleCheckerEatingMoves(i_Player, checker);
                playerPossibleEatingMoves.AddRange(checkerMoves);
            }

            return playerPossibleEatingMoves;
        }

        internal Move GetRandomPossibleMove(Player i_Player)
        {
            Random randomGenerator = new Random();
            int randomIndex;
            Move randomMove;

            List<Move> possibleEatingMoves = GetPossibleEatingMoves(i_Player);
            if (possibleEatingMoves.Count > 0)
            {
                randomIndex = randomGenerator.Next(0, possibleEatingMoves.Count);
                randomMove = possibleEatingMoves[randomIndex];
            }
            else
            {
                List<Move> possibleRegularMoves = GetPossibleRegularMoves(i_Player);
                randomIndex = randomGenerator.Next(0, possibleRegularMoves.Count);
                randomMove = possibleRegularMoves[randomIndex];
            }

            return randomMove;
        }

        internal Move GetRandomEatingPossibleMove(Player i_Player)
        {
            List<Move> possibleMoves = GetPossibleEatingMoves(i_Player);
            Random randomGenerator = new Random();
            int randomIndex = randomGenerator.Next(0, possibleMoves.Count);
            return possibleMoves[randomIndex];
        }

        internal bool AnyPossibleEatingMoves(Player i_Player)
        {
            List<Move> possibleEatingMoves = GetPossibleEatingMoves(i_Player);
            return possibleEatingMoves.Count > 0;
        }

        internal bool AnyPossibleMoves(Player i_Player)
        {
            List<Move> possibleMoves = GetPossibleMoves(i_Player);
            return possibleMoves.Count > 0;
        }

        private bool isPositionInBounds(int[] i_Position)
        {
            return i_Position[0] < Size && i_Position[0] >= 0 && i_Position[1] < Size && i_Position[1] >= 0;
        }

        private List<Move> getPossibleCheckerRegularMoves(Player i_Player, Checker checker)
        {
            List<Move> checkerPossibleRegularMoves = new List<Move>();

            int direction = 1 - ((int)checker.Team * 2);
            bool isEatingMoves = false;
            checkerPossibleRegularMoves = getPossibleMoves(i_Player, checker, isEatingMoves, direction);
            if (checker.Type == Checker.eTypes.King)
            {
                direction *= -1;
                List<Move> checkerPossibleRegularMovesOppositeDirection = getPossibleMoves(i_Player, checker, isEatingMoves, direction);
                checkerPossibleRegularMoves.AddRange(checkerPossibleRegularMovesOppositeDirection);
            }

            return checkerPossibleRegularMoves;
        }

        private List<Move> getPossibleCheckerEatingMoves(Player i_Player, Checker checker)
        {
            List<Move> checkerPossibleEatingMoves = new List<Move>();

            int direction = 1 - ((int)checker.Team * 2);
            bool isEatingMoves = true;
            checkerPossibleEatingMoves = getPossibleMoves(i_Player, checker, isEatingMoves, direction);
            if (checker.Type == Checker.eTypes.King)
            {
                direction *= -1;
                List<Move> checkerPossibleRegularMovesOppositeDirection = getPossibleMoves(i_Player, checker, isEatingMoves, direction);
                checkerPossibleEatingMoves.AddRange(checkerPossibleRegularMovesOppositeDirection);
            }

            return checkerPossibleEatingMoves;
        }

        private void spawnCheckers()
        {
            r_WhitePlayer.CheckerList = new HashSet<Checker>();
            r_WhitePlayer.Team = Checker.eTeams.White;
            r_BlackPlayer.CheckerList = new HashSet<Checker>();
            r_BlackPlayer.Team = Checker.eTeams.Black;

            for (int i = 0; i < 2 + ((Size - 6) / 2); i++)
            {
                for (int j = (i + 1) % 2; j < r_Board[0].Length; j += 2)
                {
                    Checker newChecker = new Checker(new int[] { i, j }, Checker.eTeams.Black);
                    r_Board[i][j] = newChecker;
                    r_BlackPlayer.CheckerList.Add(newChecker);
                }
            }

            for (int i = r_Board.Length - 1; i >= r_Board.Length - 2 - ((Size - 6) / 2); i--)
            {
                for (int j = (i + 1) % 2; j < r_Board[0].Length; j += 2)
                {
                    Checker newChecker = new Checker(new int[] { i, j }, Checker.eTeams.White);
                    r_Board[i][j] = newChecker;
                    r_WhitePlayer.CheckerList.Add(newChecker);
                    if (i == 4 && j == 5)
                    {
                        followedChecker = newChecker;
                    }
                }
            }
        }

        private bool isBecomingKing(Move i_Move)
        {
            return (i_Move.TargetPosition[0] == 0 && i_Move.Checker.Team == Checker.eTeams.White) || (i_Move.TargetPosition[0] == Size - 1 && i_Move.Checker.Team == Checker.eTeams.Black);
        }

        private List<Move> getPossibleMoves(Player i_Player, Checker i_Checker, bool i_EatingMoves, int i_Direction)
        {
            List<Move> possibleMoves = new List<Move>();
            int jumpDistance = 1;

            if (i_EatingMoves)
            {
                jumpDistance = 2;
            }

            for (int j = -1; j <= 1; j += 2)
            {
                int[] targetPosition = new int[2] { i_Checker.Position[0] + (jumpDistance * i_Direction), i_Checker.Position[1] + (jumpDistance * j * i_Direction) };
                if (isPositionInBounds(targetPosition))
                {
                    int[][] moveCoordinates = new int[2][];
                    moveCoordinates[0] = i_Checker.Position;
                    moveCoordinates[1] = targetPosition;
                    Move possibleMove = new Move(moveCoordinates, Snapshot, i_Player, LastMove);
                    if (possibleMove.IsValid)
                    {
                        possibleMoves.Add(possibleMove);
                    }
                }
            }

            return possibleMoves;
        }

        private Checker[][] getBoardArray(int i_BoardSize)
        {
            Checker[][] boardArray = new Checker[i_BoardSize][];
            for (int i = 0; i < i_BoardSize; i++)
            {
                boardArray[i] = new Checker[i_BoardSize];
            }

            return boardArray;
        }
    }
}