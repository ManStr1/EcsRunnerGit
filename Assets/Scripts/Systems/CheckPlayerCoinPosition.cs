using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class CheckPlayerCoinPosition : IEcsRunSystem {
        private readonly EcsWorldInject _world = default;

        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsFilterInject<Inc<Coin, RotationX, View, Position>> _filterCoinWithView = default;
        private readonly EcsFilterInject<Inc<TilesAndColsContainerIdentificator, RotationX>> _filterIdentificator = default;

        private readonly EcsFilterInject<Inc<Player, Position>, Exc<Loser>> _filterPlayer = default;

        public void Run(IEcsSystems systems) {
            foreach (int idenEntity in _filterIdentificator.Value) {

                ref var idenRotationX = ref _filterIdentificator.Pools.Inc2.Get(idenEntity);

                foreach (int coinEntity in _filterCoinWithView.Value) {
                    ref var coinRotationX = ref _filterCoinWithView.Pools.Inc2.Get(coinEntity);

                    if (idenRotationX.value == coinRotationX.value) {
                        ref var coinPosition = ref _filterCoinWithView.Pools.Inc4.Get(coinEntity);

                        foreach (int playerEntity in _filterPlayer.Value) {
                            ref var player = ref _filterPlayer.Pools.Inc1.Get(playerEntity);
                            var playerPosition = player.value.gameObject.transform.position;


                            if (playerPosition.x < coinPosition.value.x + 0.5f &&
                                playerPosition.x > coinPosition.value.x - 0.5f) {
                                ref var coinView = ref _filterCoinWithView.Pools.Inc3.Get(coinEntity);

                                _sceneData.Value.SceneCoins++;

                                Object.Destroy(coinView.value.gameObject);
                                _world.Value.DelEntity(coinEntity);
                            }
                        }
                    }
                }
            }
        }
    }
}