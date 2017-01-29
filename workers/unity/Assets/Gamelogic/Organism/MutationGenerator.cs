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
        [Require]
        private Reproducer.Writer OrganismGenomeWriter;

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
            byte[] b = new byte[3];

            for (int i = 0; i < 3; i++)
            {
                b[i] = (byte)Random.Range(0, 255);
            }
            string stringGenome = Convert.ToBase64String(b);

            Genome newGenome = new Genome(stringGenome);

            OrganismGenomeWriter.Send(new Reproducer.Update().SetGenome1(newGenome));
        }
    }

}