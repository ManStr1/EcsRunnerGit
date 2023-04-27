using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsRunner.Systems {
    public class StartGameSystem : IEcsRunSystem {
        private readonly EcsPoolInject<RotationX> _rotationXPool = default;
        private readonly EcsPoolInject<Movable> _movablePool = default;

        private readonly EcsFilterInject<Inc<TilesAndColsContainerIdentificator, Starter>> _filterContainerWithStarter = default;
        private readonly EcsFilterInject<Inc<Tile>, Exc<Movable>> _filterTilesWithoutMove = default;

        public void Run(IEcsSystems systems) {
            if (_filterContainerWithStarter.Value.GetEntitiesCount() > 0) {
                foreach (int entity in _filterContainerWithStarter.Value) {

                    ref var val2 = ref _rotationXPool.Value.Add(entity);
                    val2.value = 0;

                    foreach (int tileEntity in _filterTilesWithoutMove.Value) {
                        _movablePool.Value.Add(tileEntity);
                    }

                    _filterContainerWithStarter.Pools.Inc2.Del(entity);
                }
            }
        }
    }
}