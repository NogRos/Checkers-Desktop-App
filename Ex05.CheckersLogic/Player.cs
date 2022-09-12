using System.Collections.Generic;

namespace Ex05.CheckersLogic
{
    public class Player
    {
        public string Name { get; set; }

        public bool IsHuman { get; set; }

        public HashSet<Checker> CheckerList { get; set; }

        public int GameScore { get; set; } = 0;

        public Checker.eTeams Team { get; set; }

        /*
         * Creates and retruns a *deep copy* clone of this Player instance.
         */
        internal Player GetClone()
        {
            Player clonedPlayer = new Player();
            clonedPlayer.CheckerList = new HashSet<Checker>();
            clonedPlayer.Team = Team;
            clonedPlayer.Name = Name;
            return clonedPlayer;
        }
    }

}
