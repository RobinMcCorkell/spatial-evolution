using UnityEngine;
using Improbable.Math;
using Improbable.Worker;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Improbable.Entity.Component;
using Improbable.Collections;
using Evolution.Environment;
using Resources = Evolution.Environment.Resources;
using System.Collections.Generic;

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
            Map<Evolution.Material, uint> toxicLimits = responseHandle.Request.toxicLimits;

            var resources = ResourcesWriter.Data.resources;

            uint toxicity = 0;
            foreach (KeyValuePair<Evolution.Material, uint> resource in resources) {
                if (resource.Value > toxicLimits[resource.Key]) {
                    toxicity += 1;
                }
            }

            if (resources[consumed] > 0)
            {
                resources[consumed] -= 1;
                resources[produced] += 1;
                ResourcesWriter.Send(new Resources.Update().SetResources(resources));

                responseHandle.Respond(new ConsumptionResponse(false, toxicity));
            }
            else
            {
                responseHandle.Respond(new ConsumptionResponse(true, toxicity));
            }
        }
    }
}
