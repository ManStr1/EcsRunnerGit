using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsRunner.Systems {
    public class EndGamePresetDeleteSystem : IEcsRunSystem {
        private readonly EcsCustomInject<Configuration> _configuration = default;

        private readonly EcsPoolInject<DeleteTime> _deleteTimePool = default;

        private readonly EcsFilterInject<Inc<Player, Loser>, Exc<DeleteTime>> _filterPlayerLoser = default;
        private readonly EcsFilterInject<Inc<Position, View, DeleteOrder>> _filterObjectsWithView = default;

        public void Run(IEcsSystems systems) {
            if (_filterPlayerLoser.Value.GetEntitiesCount() > 0) {

                foreach (int playerEntity in _filterPlayerLoser.Value) {
                    ref DeleteTime d = ref _deleteTimePool.Value.Add(playerEntity);
                    d.deleteTime = _configuration.Value.deleteTimeRemaining;
                    d.timeRemaining = _configuration.Value.deleteTimeRemaining;
                    d.offset = _configuration.Value.deleteTimeRemaining / _filterObjectsWithView.Value.GetEntitiesCount();
                }
            }
        }
    }
}