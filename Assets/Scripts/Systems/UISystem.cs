using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.SceneManagement;

namespace EcsRunner {
    public class UISystem : IEcsRunSystem {
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<GameOver> _gameOverPool = default;

        private readonly EcsFilterInject<Inc<Player, Loser, DeleteTime, EndGame>, Exc<GameOver>> _filterEndGame = default;

        public void Run(IEcsSystems systems) {

            _sceneData.Value.gameUI.MainMenuUI.PlayerInfoPanelUI.CoinsContainerUI.CoinsText.text = _sceneData.Value.SceneCoins + "";
            _sceneData.Value.gameUI.MainMenuUI.BestScoreUI.ScoreText.text = _sceneData.Value.SceneScore + "";
            _sceneData.Value.gameUI.MainMenuUI.PlayerInfoPanelUI.MultiplierContainerUI.MultiplierText.text = "x" + _sceneData.Value.SceneMupltiplier;

            if (_sceneData.Value.SceneScore % 100 == 0 && _sceneData.Value.SceneScore != 0) _sceneData.Value.SceneMupltiplier = _sceneData.Value.SceneScore / 100;

            if (_filterEndGame.Value.GetEntitiesCount() > 0) {
                foreach (int entity in _filterEndGame.Value) {

                    SaveData.currentCoins = _sceneData.Value.SceneCoins;

                    if (_sceneData.Value.SceneScore > _sceneData.Value.SceneBestScore) {
                        SaveData.currentBestScore = _sceneData.Value.SceneScore;
                    }

                    SaveData.SaveFile();

                    //Bad idea
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                    _gameOverPool.Value.Add(entity);
                }
            }


        }
    }
}