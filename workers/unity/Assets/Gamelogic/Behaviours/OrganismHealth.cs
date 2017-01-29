using UnityEngine;
using Improbable.Math;
using Improbable.Worker;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Evolution.Organism;

namespace Assets.Gamelogic.Behaviours
{
    [EngineType(EnginePlatform.FSim)]
    public class OrganismHealth : MonoBehaviour
    {
        [Require]
        private Health.Writer HealthWriter;
        [Require]
        private Affectable.Reader AffectReader;
        [Require]
        private Consumer.Reader ConsumerReader;

        public void OnEnable() {
            AffectReader.ComponentUpdated += OnAffectUpdated;
            ConsumerReader.ComponentUpdated += OnConsumerUpdated;
        }

        public void OnDisable() {
            AffectReader.ComponentUpdated -= OnAffectUpdated;
            ConsumerReader.ComponentUpdated -= OnConsumerUpdated;
        }

        public void OnAffectUpdated(Affectable.Update update) {
            int damage = 0;
            for (var i = 0; i < update.takenDamage.Count; ++i) {
                damage += 1;
            }
            if (damage > 0) {
                HealthWriter.Send(new Health.Update().SetValue(HealthWriter.Data.value - damage));
            }
        }

        public void OnConsumerUpdated(Consumer.Update update) {
            int damage = 0;
            for (var i = 0; i < update.starving.Count; ++i) {
                damage += 2;
            }
            if (damage > 0) {
                HealthWriter.Send(new Health.Update().SetValue(HealthWriter.Data.value - damage));
            }
        }
    }
}
