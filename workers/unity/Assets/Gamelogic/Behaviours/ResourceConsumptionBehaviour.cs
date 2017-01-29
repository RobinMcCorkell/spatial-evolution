using UnityEngine;
using Improbable.Math;
using Improbable.Worker;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Improbable.Entity.Component;
using Evolution.Environment;
using Resources = Evolution.Environment.Resources;

namespace Assets.Gamelogic.Behaviours
{
    [EngineType(EnginePlatform.FSim)]
    public class ResourceConsumptionBehaviour : MonoBehaviour
    {
        [Require]
        private Resources.Writer ResourcesWriter;

        public void OnEnable()
        {
            ResourcesWriter.CommandReceiver.OnConsumeResources += ConsumeResources;
        }

        public void OnDisable()
        {
            ResourcesWriter.CommandReceiver.OnConsumeResources -= ConsumeResources;
        }

        private void ConsumeResources(ResponseHandle<Resources.Commands.ConsumeResources, ConsumptionRequest, ConsumptionResponse> responseHandle)
        {
            Evolution.Material consumed = responseHandle.Request.consumed;
            Evolution.Material produced = responseHandle.Request.produced;

            var resources = ResourcesWriter.Data.resources;
            if (resources[consumed] > 0)
            {
                resources[consumed] -= 1;
                resources[produced] += 1;
                ResourcesWriter.Send(new Resources.Update().SetResources(resources));

                responseHandle.Respond(new ConsumptionResponse(true));
            }
            else
            {
                responseHandle.Respond(new ConsumptionResponse(false));
            }
        }
    }
}
