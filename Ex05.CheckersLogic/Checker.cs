namespace Ex05.CheckersLogic
{
    public class Checker
    {
        internal Checker(int[] i_Position, eTeams i_Team)
        {
            Position = i_Position;
            Team = i_Team;
            Type = eTypes.Regular;
        }

        internal Checker(int[] i_Position, eTeams i_Team, eTypes i_Type)
            : this(i_Position, i_Team)
        {
            Type = i_Type;
        }

        public enum eTypes
        {
            Regular = 1,
            King = 4,
        }

        public enum eTeams
        {
            Black = 0,
            White = 1,
        }

        public int[] Position { get; set; }

        public eTypes Type { get; set; }

        public eTeams Team { get; }

        /*
         * Creates and returns a *deep copy* of this checker.
         */
        internal Checker GetClone()
        {
            Checker clonedChecker = new Checker((int[])Position.Clone(), Team, Type);
            return clonedChecker;
        }
    }
}
