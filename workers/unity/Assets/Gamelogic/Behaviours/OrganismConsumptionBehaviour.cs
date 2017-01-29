using UnityEngine;
using Improbable;
using Improbable.Math;
using Improbable.Worker;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Core.EntityQueries;
using Improbable.Unity.Visualizer;
using Evolution.Environment;
using Evolution.Organism;
using Resources = Evolution.Environment.Resources;
using System.Collections.Generic;

namespace Assets.Gamelogic.Behaviours
{
    [EngineType(EnginePlatform.FSim)]
    public class OrganismConsumptionBehaviour : MonoBehaviour
    {
        [Require]
        private Consumer.Writer ConsumerWriter;
        [Require]
        private Affectable.Writer AffectWriter;

        public void OnEnable()
        {
            InvokeRepeating("ConsumeFromNearby", 0, 1.0f);
        }

        public void OnDisable()
        {
            CancelInvoke("ConsumeFromNearby");
        }

        private void ConsumeFromNode(EntityId nodeId)
        {
            SpatialOS.Commands.SendCommand(
                ConsumerWriter,
                Resources.Commands.ConsumeResources.Descriptor,
                new ConsumptionRequest(ConsumerWriter.Data.consumes, ConsumerWriter.Data.produces, AffectWriter.Data.materialLimits),
                nodeId,
                result =>
                {
                    if (result.StatusCode != StatusCode.Success)
                    {
                        Debug.LogError("failed to send consumption command with error " + result.ErrorMessage);
                        return;
                    }
                    if (result.Response.Value.starving)
                    {
                        ConsumerWriter.Send(new Consumer.Update().AddStarving(new Starving()));
                    }
                    if (result.Response.Value.toxicity > 0) {
                        AffectWriter.Send(new Affectable.Update().AddTakenDamage(new Toxicity(result.Response.Value.toxicity)));
                    }
                }
            );
        }

        public void ConsumeFromNearby()
        {
            var query = Query.And(
                Query.HasComponent<Resources>(),
                Query.InSphere(transform.position.x, transform.position.y, transform.position.z, 2.0)
            ).ReturnOnlyEntityIds();

            SpatialOS.Commands.SendQuery(ConsumerWriter, query, result => {
                if (result.StatusCode != StatusCode.Success) {
                    Debug.Log("query failed with error: " + result.ErrorMessage);
                    return;
                }
                ICollection<EntityId> nodes = result.Response.Value.Entities.Keys;
                foreach (var node in nodes) {
                    ConsumeFromNode(node);
                }
            });
        }
    }
}
