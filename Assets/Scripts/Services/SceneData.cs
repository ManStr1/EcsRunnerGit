using UnityEngine;

namespace EcsRunner {
    public class SceneData : MonoBehaviour {
        public PlayerView Player;
        public Transform TilesAndColsContainer;
        public GameUI gameUI;
        public int EndDeleteOrderInt = 0;
        public int StartDeleteOrderInt = 0;

        public int SceneScore = 0;
        public int SceneCoins = 0;
        public int SceneDiamonds = 0;
        public int SceneBestScore = 0;
        public int SceneSkinNumber = 0;
        public int SceneSkinStoreNumber = 0;
        public int SceneBadgeNumber = 0;
        public int SceneMupltiplier = 1;
    }
}