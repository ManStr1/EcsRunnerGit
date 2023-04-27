using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsRunner.Systems {
    public class DeleteModelSystem : IEcsRunSystem {
        private readonly EcsWorldInject _world = default;

        private readonly EcsCustomInject<Configuration> _configuration = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;

        private readonly EcsFilterInject<Inc<View, Position, DeleteOrder>> _filterView = default;
        private readonly EcsFilterInject<Inc<Player, Loser>> _filterPlayerLoser = default;

        public void Run(IEcsSystems systems) {
            if (_filterPlayerLoser.Value.GetEntitiesCount() > 0) return;
            foreach (var entity in _filterView.Value) {
                ref var position = ref _filterView.Pools.Inc2.Get(entity);
                ref var view = ref _filterView.Pools.Inc1.Get(entity);

                if (position.value.x >= _configuration.Value.ModelDeletePointX) {

                    ref var dO = ref _filterView.Pools.Inc3.Get(entity);
                    
                    _sceneData.Value.StartDeleteOrderInt = dO.value;

                    Object.Destroy(view.value.gameObject);
                    _world.Value.DelEntity(entity);
                }
            }
        }
    }
}