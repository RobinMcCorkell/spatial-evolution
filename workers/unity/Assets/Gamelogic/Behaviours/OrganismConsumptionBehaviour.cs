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

        private void ConsumeFromNode(EntityId nodeId)
        {
            SpatialOS.Commands.SendCommand(
                ConsumerWriter,
                Resources.Commands.ConsumeResources.Descriptor,
                new ConsumptionRequest(ConsumerWriter.Data.consumes, ConsumerWriter.Data.produces),
                nodeId,
                result =>
                {
                    if (result.StatusCode != StatusCode.Success)
                    {
                        Debug.LogError("failed to send consumption command with error " + result.ErrorMessage);
                        return;
                    }
                    if (!result.Response.Value.success)
                    {
                        ConsumerWriter.Send(new Consumer.Update().AddStarving(new Starving()));
                    }
                }
            );
        }

        private void ConsumeFromNearby()
        {
            var query = Query.And(
                Query.HasComponent<Resources>(),
                Query.InSphere(transform.position.x, transform.position.y, transform.position.z, ConsumerWriter.Data.radius)
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
