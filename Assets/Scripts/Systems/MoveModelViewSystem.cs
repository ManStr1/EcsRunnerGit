using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsRunner.Systems {
    public class MoveModelViewSystem : IEcsRunSystem {
        private readonly EcsFilterInject<Inc<Position, View, Movable>> _filterMovableViews = default;

        public void Run(IEcsSystems systems) {
            foreach (int entity in _filterMovableViews.Value) {
                ref var view = ref _filterMovableViews.Pools.Inc2.Get(entity).value;
                ref var position = ref _filterMovableViews.Pools.Inc1.Get(entity).value;

                view.transform.position = position;
            }
        }
    }
}