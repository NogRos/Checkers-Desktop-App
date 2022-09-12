using System;

namespace Ex05.CheckersLogic
{
    public class Move
    {
        private Checker[][] m_BoardSnapshot;
        private Move m_LastMove;

        public Move(int[][] i_Coordinates, Checker[][] i_BoardSnapshot, Player i_Player, Move i_LastMove)
        {
            m_BoardSnapshot = i_BoardSnapshot;
            StartPosition = i_Coordinates[0];
            TargetPosition = i_Coordinates[1];
            MovingPlayer = i_Player;
            m_LastMove = i_LastMove;
            Checker = m_BoardSnapshot[StartPosition[0]][StartPosition[1]];
            EatenCheckerPosition = getMiddlePosition(StartPosition, TargetPosition);
            evaluateMove();
        }

        private Move(Move i_Move, Checker[][] i_BoardSnapshot, Player i_Player, Move i_LastMove)
        {
            m_BoardSnapshot = i_BoardSnapshot;
            StartPosition = i_Move.StartPosition;
            TargetPosition = i_Move.TargetPosition;
            MovingPlayer = i_Player;
            m_LastMove = i_LastMove;
            Checker = m_BoardSnapshot[StartPosition[0]][StartPosition[1]];

            EatenCheckerPosition = i_Move.EatenCheckerPosition;
            IsEating = i_Move.IsEating;
            IsContinuous = i_Move.IsContinuous;
            IsValid = i_Move.IsValid;
        }

        public int[] StartPosition { get; private set; }

        public int[] TargetPosition { get; private set; }

        public bool IsEating { get; private set; }

        public int[] EatenCheckerPosition { get; }

        public bool IsContinuous { get; private set; }

        public Checker Checker { get; }

        public Player MovingPlayer { get; }

        public bool IsValid { get; private set; }

        /*
         * Creates and returns a new distinct instance similar to this Move instance, but with other given Board, Player and LastMove references.
         */
        public Move GetClone(Checker[][] i_BoardSnapshot, Player i_Player, Move i_LastMove)
        {
            return new Move(this, i_BoardSnapshot, i_Player, i_LastMove);
        }

        private void evaluateMove()
        {
            if (Checker != null && Checker.Team == MovingPlayer.Team)
            {
                int direction = getDirection(Checker);
                if (Checker.Type == Checker.eTypes.Regular)
                {
                    if (isValidRegularMove(direction))
                    {
                        IsValid = true;
                    }

                    if (isValidEatingMove(StartPosition, TargetPosition, direction))
                    {
                        IsEating = true;
                        IsValid = true;
                        if (isContinuousEatingMove(TargetPosition, direction))
                        {
                            IsContinuous = true;
                        }
                    }
                }
                else
                {
                    for (int i = -1; i < 2; i += 2)
                    {
                        if (isValidRegularMove(i * direction))
                        {
                            IsValid = true;
                        }

                        if (isValidEatingMove(StartPosition, TargetPosition, i * direction))
                        {
                            IsEating = true;
                            IsValid = true;
                            if (isContinuousEatingMove(TargetPosition, i * direction))
                            {
                                IsContinuous = true;
                            }
                        }
                    }
                }

                if (m_LastMove != null && m_LastMove.IsContinuous)
                {
                    enforceContinuousMoveRules();
                }
            }
        }

        private bool isValidEatingMove(int[] i_StartPosition, int[] i_TargetPosition, int i_Direction)
        {
            int rowDifference = calculateRowDifference(i_StartPosition, i_TargetPosition);
            int columnDifference = calculateColumnDifference(i_StartPosition, i_TargetPosition);
            Checker eatenChecker = getCheckerBetweenPositions(i_StartPosition, i_TargetPosition);
            return rowDifference == 2 * i_Direction && Math.Abs(columnDifference) == 2 && isPositionEmpty(i_TargetPosition) && isValidEatenChecker(eatenChecker);
        }

        private bool isValidRegularMove(int i_Direction)
        {
            int rowDifference = calculateRowDifference(StartPosition, TargetPosition);
            int columnDifference = calculateColumnDifference(StartPosition, TargetPosition);
            return (rowDifference == i_Direction) && Math.Abs(columnDifference) == 1 && isPositionEmpty(TargetPosition);
        }

        private bool isContinuousEatingMove(int[] i_TargetPosition, int i_Direction)
        {
            int[] firstNextPossibleTarget = { i_TargetPosition[0] + (2 * i_Direction), i_TargetPosition[1] + (2 * i_Direction) };
            int[] secondNextPossibleTarget = { (i_TargetPosition[0] + (2 * i_Direction)), i_TargetPosition[1] - (2 * i_Direction) };
            return (isPositionInBounds(firstNextPossibleTarget) && isValidEatingMove(i_TargetPosition, firstNextPossibleTarget, i_Direction)) || (isPositionInBounds(secondNextPossibleTarget) && isValidEatingMove(i_TargetPosition, secondNextPossibleTarget, i_Direction));
        }

        private void enforceContinuousMoveRules()
        {
            if (!(m_LastMove.Checker == Checker && IsEating))
            {
                IsValid = false;
            }
        }

        private Checker getCheckerBetweenPositions(int[] i_StartPosition, int[] i_TargetPosition)
        {
            int rowDifference = calculateRowDifference(i_StartPosition, i_TargetPosition);
            int columnDifference = calculateColumnDifference(i_StartPosition, i_TargetPosition);
            return m_BoardSnapshot[i_StartPosition[0] + (rowDifference / 2)][i_StartPosition[1] + (columnDifference / 2)];
        }

        private int[] getMiddlePosition(int[] i_Pos1, int[] i_Pos2)
        {
            return new int[] { (i_Pos1[0] + i_Pos2[0]) / 2, (i_Pos1[1] + i_Pos2[1]) / 2 };
        }

        private bool isPositionEmpty(int[] i_Position)
        {
            return m_BoardSnapshot[i_Position[0]][i_Position[1]] == null;
        }

        private bool isValidEatenChecker(Checker i_EatenChecker)
        {
            return i_EatenChecker != null && i_EatenChecker.Team != Checker.Team;
        }

        private int calculateRowDifference(int[] i_StartPosition, int[] i_TargetPosition)
        {
            return i_TargetPosition[0] - i_StartPosition[0];
        }

        private int calculateColumnDifference(int[] i_StartPosition, int[] i_TargetPosition)
        {
            return i_TargetPosition[1] - i_StartPosition[1];
        }

        private int getDirection(Checker i_Checker)
        {
            int direction;

            if (i_Checker.Team == Checker.eTeams.Black)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            return direction;
        }

        private bool isPositionInBounds(int[] i_Position)
        {
            return i_Position[0] < m_BoardSnapshot.Length && i_Position[0] >= 0 && i_Position[1] < m_BoardSnapshot.Length && i_Position[1] >= 0;
        }
    }
}
