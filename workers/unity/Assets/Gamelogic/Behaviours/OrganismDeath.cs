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
    public class OrganismDeath : MonoBehaviour
    {
        [Require]
        private Health.Writer HealthWriter;

        public void OnEnable() {
            HealthWriter.ComponentUpdated += OnHealthUpdated;
        }

        public void OnDisable() {
            HealthWriter.ComponentUpdated -= OnHealthUpdated;
        }

        public void OnHealthUpdated(Health.Update update) {
            if (update.value.HasValue && update.value.Value <= 0) {
                SpatialOS.Commands.DeleteEntity(HealthWriter, gameObject.EntityId(), result => {
                    if (result.StatusCode != StatusCode.Success) {
                        Debug.Log("failed to delete entity with error: " + result.ErrorMessage);
                        return;
                    }
                    Debug.Log("deleted entity: " + result.Response.Value);
                });
            }
        }
    }
}
