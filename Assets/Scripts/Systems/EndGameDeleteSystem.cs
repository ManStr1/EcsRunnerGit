using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class EndGameDeleteSystem : IEcsRunSystem {
        private readonly EcsWorldInject _world = default;

        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsPoolInject<EndGame> _endGamePool = default;

        private readonly EcsFilterInject<Inc<Player, Loser, DeleteTime>, Exc<EndGame>> _filterPlayerLoser = default;
        private readonly EcsFilterInject<Inc<Position, View, DeleteOrder>> _filterObjectsWithView = default;

        public void Run(IEcsSystems systems) {
            if (_filterPlayerLoser.Value.GetEntitiesCount() > 0 && _filterObjectsWithView.Value.GetEntitiesCount() > 0) {
                int startCounter = _sceneData.Value.StartDeleteOrderInt;

                foreach (int playerEntity in _filterPlayerLoser.Value) {

                    ref var time = ref _filterPlayerLoser.Pools.Inc3.Get(playerEntity);

                    if (time.timeRemaining > 0) {
                        time.timeRemaining -= Time.deltaTime;

                        if (Mathf.Abs(time.timeRemaining - time.deleteTime) < 0.01f) {

                            foreach (int entity in _filterObjectsWithView.Value) {

                                ref var objView = ref _filterObjectsWithView.Pools.Inc2.Get(entity);
                                ref var objOrder = ref _filterObjectsWithView.Pools.Inc3.Get(entity);

                                if (objOrder.value == startCounter) {
                                    Object.Destroy(objView.value.gameObject);
                                    _world.Value.DelEntity(entity);
                                }
                            }
                            _sceneData.Value.StartDeleteOrderInt++;

                            time.deleteTime -= time.offset;
                        }
                    }
                }
            } else 
            if (_filterPlayerLoser.Value.GetEntitiesCount() > 0 && _filterObjectsWithView.Value.GetEntitiesCount() == 0) {
                foreach (int entity in _filterPlayerLoser.Value) {
                    _endGamePool.Value.Add(entity);
                }
            }
        }
    }
}