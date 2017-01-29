using Improbable.Unity.Core.Acls;
using Evolution.Organism;
using Evolution.Environment;
using Improbable.Collections;
using Improbable.Worker;
using Improbable.Math;
using UnityEngine;
using System;

namespace Assets.EntityTemplates
{
    public class OrganismEntityTemplate : MonoBehaviour
    {
        public static SnapshotEntity GenerateOrganismEntityTemplate(Coordinates position, Genome genome1, Genome genome2, int[][] genomeKey)
        {
            var entity = new SnapshotEntity { Prefab = "OrganismPrefab" };

            //Debug.Log(position);

            entity.Add(new Health.Data(new HealthData(50)));

            string dominantGenome = mixGenes(genome1, genome2);
            byte[] phenotype = Convert.FromBase64String(dominantGenome);

            var food = getFood(phenotype, genomeKey[0]);

            entity.Add(new Consumer.Data(
                        new ConsumerData(food, getWaste(food), getRadius(phenotype, genomeKey[2]))
                        ));
            entity.Add(new Reproducer.Data(new ReproducerData(genome1, genome2)));

            entity.Add(new Mover.Data(new MoverData(position, getSpeed(phenotype, genomeKey[3]), 0.0f, 10)));
            entity.Add(new Affectable.Data(
                        new AffectableData(getLimits(phenotype, genomeKey), getMutationP(phenotype, genomeKey[7]))
                        ));

            var acl = Acl.Build()
                .SetReadAccess(CommonPredicates.PhysicsOrVisual)
                .SetWriteAccess<Health>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Consumer>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Reproducer>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Mover>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Affectable>(CommonPredicates.PhysicsOnly);

            entity.SetAcl(acl);

            return entity;
        }

        public static string mixGenes(Genome g1, Genome g2)
        {
            string result = "";
            for (int i = 0; i < 4; i++)
            {
                result += (Char.GetNumericValue(g1.genes[i]) > Char.GetNumericValue(g2.genes[i])) ? g1.genes[i] : g2.genes[i];
            }
            return result;
        }

        public static Evolution.Material getFood(byte[] phenotype, int[] key)
        {
            var x = 0;
            foreach (int i in key)
            {
                x += phenotype[i];
            }
            switch(x)
            {
                case 0:
                    return Evolution.Material.A;
                case 1:
                    return Evolution.Material.B;
                case 2:
                    return Evolution.Material.C;
                default:
                    return Evolution.Material.A;
            }
        }

        public static Evolution.Material getWaste(Evolution.Material food)
        {
            if (food == Evolution.Material.A) { return Evolution.Material.B; }
            else if (food == Evolution.Material.B) { return Evolution.Material.C;  }
            else { return Evolution.Material.A;  }
        }

        public static double getRadius(byte[] phenotype, int[] key)
        {
            string s = "";
           
            foreach(int i in key)
            {
                s += phenotype[i].ToString();
            }
            return Convert.ToInt32(s, 2) / 2;
        }

        public static float getSpeed(byte[] phenotype, int[] key)
        {
            string s = "";
            foreach (int i in key)
            {
                s += phenotype[i].ToString();
            }
            return (Convert.ToInt32(s, 2) * 2) / 5;
        }

        public static Map<Evolution.Material, uint> getLimits(byte[] phenotype, int[][] key)
        {
            Map<Evolution.Material, uint> result = new Map<Evolution.Material, uint>(3);
            string limMat1 = "";
            foreach (int j in key[4])
            {
                limMat1 += phenotype[j].ToString();
            }
            result[Evolution.Material.A] = Convert.ToUInt32(limMat1);
            string limMat2 = "";
            foreach (int j in key[5])
            {
                limMat2 += phenotype[j].ToString();
            }
            result[Evolution.Material.B] = Convert.ToUInt32(limMat2);
            string limMat3 = "";
            foreach (int j in key[5])
            {
                limMat3 += phenotype[j].ToString();
            }
            result[Evolution.Material.C] = Convert.ToUInt32(limMat3);

            return result;
        }

        public static float getMutationP(byte[] phenotype, int[] key)
        {
            string s = "";
            foreach (int i in key)
            {
                s += phenotype[i].ToString();
            }
            if (Convert.ToInt32(s, 2) == 7) { return 0.005f; }
            else { return 0.001f; }
        }
    }
}
