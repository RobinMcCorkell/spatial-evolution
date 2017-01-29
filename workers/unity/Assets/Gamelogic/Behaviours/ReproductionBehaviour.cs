using UnityEngine;
using Improbable.Math;
using Improbable.Worker;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Evolution.Organism;
using Assets.EntityTemplates;

namespace Assets.Gamelogic.Behaviours
{
    [EngineType(EnginePlatform.FSim)]
    public class ReproductionBehaviour : MonoBehaviour
    {
        [Require]
        private Reproducer.Reader ReproducerReader;
        [Require]
        private Mover.Reader MoverReader;

        private Genome generateGamete()
        {
            Genome genome1 = ReproducerReader.Data.genome1;
            Genome genome2 = ReproducerReader.Data.genome2;

            string result = "";
            for (int i = 0; i < genome1.genes.Length; ++i)
            {
                if (Random.value < 0.5)
                {
                    result += genome1.genes[i];
                }
                else
                {
                    result += genome2.genes[i];
                }
            }
            return new Genome(result);
        }

        private void onTriggerEnter(Collider other)
        {
            if (ReproducerReader == null)
                return;

            if (other != null && other.gameObject.tag == "Organism")
            {
                // contact between two organisms, trigger reproduction
                Genome childGenome1 = generateGamete();
                Genome childGenome2 = other.GetComponent<ReproductionBehaviour>()
                    .generateGamete();

                Coordinates childCoords = MoverReader.Data.position;

                var template = OrganismEntityTemplate.GenerateOrganismEntityTemplate(
                    childCoords, childGenome1, childGenome2
                );
                SpatialOS.WorkerCommands.CreateEntity("Organism", template, result =>
                {
                    if (result.StatusCode != StatusCode.Success)
                    {
                        Debug.Log("failed to create organism with error: " + result.ErrorMessage);
                        return;
                    }
                    Debug.Log("spawned child organism with ID: " + result.Response.Value);
                });
            }
        }
    }
}
