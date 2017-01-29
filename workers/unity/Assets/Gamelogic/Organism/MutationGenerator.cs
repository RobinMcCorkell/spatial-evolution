using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Improbable.Math;
using System.Collections;
using Evolution.Organism;
using Improbable.Unity;
using Improbable.Unity.Visualizer;

namespace Assets.Gamelogic.Organisms
{
    public class MutationGenerator : MonoBehaviour
    {

        [Require]
        private Affectable.Writer OrganismMutationWriter;

        double mutationProbability;

        public void OnEnable() {
        }

        public void Update() {
            maybeMutate();
        }

        public void maybeMutate() {
            mutationProbability = OrganismMutationWriter.Data.mutationProbability;
            if (mutationProbability > Random.Range(0,1)) {
                mutate();
            }
        }

        public void mutate()
        {
            
        }

    }

}