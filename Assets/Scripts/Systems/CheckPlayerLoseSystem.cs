using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsRunner.Systems {
    public class CheckPlayerLoseSystem : IEcsRunSystem {

        private readonly EcsFilterInject<Inc<Movable>> _filterMovable = default;
        private readonly EcsFilterInject<Inc<RotationX>> _filterRotation = default;
        private readonly EcsFilterInject<Inc<Player, Loser>> _filterPlayerLoser = default;

        public void Run(IEcsSystems systems) {
            if (_filterPlayerLoser.Value.GetEntitiesCount() > 0) {
                foreach (int movableEntity in _filterMovable.Value) {
                    _filterMovable.Pools.Inc1.Del(movableEntity);
                }


                foreach (int rotationEntity in _filterRotation.Value) {
                    _filterRotation.Pools.Inc1.Del(rotationEntity);
                }
            }
        }
    }
}