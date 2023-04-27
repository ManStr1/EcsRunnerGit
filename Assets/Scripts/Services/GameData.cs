using System.Collections.Generic;

namespace EcsRunner {
    [System.Serializable]
    public class GameData {
        public int bestScore;
        public int coins;
        public int diamonds;
        public int skinNumber;
        public int badgeNumber;
        public bool[] availableSkins;

        public GameData(int bestScoreInt, int coinsInt, int diamondsInt, int skinNumberInt, int badgeNumberInt, bool[] availableSkinsBool) {
            bestScore = bestScoreInt;
            coins = coinsInt;
            diamonds = diamondsInt;
            skinNumber = skinNumberInt;
            badgeNumber = badgeNumberInt;
            availableSkins = availableSkinsBool;
        }
    }
}