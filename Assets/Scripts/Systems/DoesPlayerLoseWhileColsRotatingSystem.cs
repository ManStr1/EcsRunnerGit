using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class DoesPlayerLoseWhileColsRotatingSystem : IEcsRunSystem {
        private readonly EcsPoolInject<Loser> _loserPool = default;

        private readonly EcsFilterInject<Inc<Column, RotationX, View, Position>> _filterColumnWithView = default;
        private readonly EcsFilterInject<Inc<TilesAndColsContainerIdentificator, RotationX>> _filterIdentificator = default;
        private readonly EcsFilterInject<Inc<Player, Position>, Exc<Loser>> _filterPlayer = default;

        public void Run(IEcsSystems systems) {
            if (Input.GetMouseButtonDown(0)) {
                int direction = Input.mousePosition.x < Screen.width / 2 ? 1 : -1;

                foreach (int idenEntity in _filterIdentificator.Value) {
                    // Get container rotation entity
                    ref var idenRotationX = ref _filterIdentificator.Pools.Inc2.Get(idenEntity);

                    foreach (int colEntity in _filterColumnWithView.Value) { // Get columns entities

                        ref var colRotationX = ref _filterColumnWithView.Pools.Inc2.Get(colEntity);

                        
                        if (idenRotationX.value == colRotationX.value) { // if current container rotation equals col rotation

                            ref var columnPosition = ref _filterColumnWithView.Pools.Inc4.Get(colEntity);

                            foreach (int playerEntity in _filterPlayer.Value) {
                                ref var player = ref _filterPlayer.Pools.Inc1.Get(playerEntity);
                                var playerPosition = player.value.gameObject.transform.position;

                                
                                if (playerPosition.x < columnPosition.value.x + 0.5f &&
                                    playerPosition.x > columnPosition.value.x - 0.5f) { // check if player near this rotated column
                                    ref var gamePosition = ref _filterPlayer.Pools.Inc2.Get(playerEntity);

                                    if (direction > 0) {
                                        gamePosition.value.z += 0.5f;
                                        player.value.transform.position = gamePosition.value;
                                    } else {
                                        gamePosition.value.z -= 0.5f;
                                        player.value.transform.position = gamePosition.value;
                                    }
                                    player.value.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                                    player.value.GetComponent<BoxCollider>().enabled = false;

                                    Debug.Log("COLUMN!!");
                                    _loserPool.Value.Add(playerEntity);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}