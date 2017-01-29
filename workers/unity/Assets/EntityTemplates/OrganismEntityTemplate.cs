using Improbable.Unity.Core.Acls;
using Evolution.Organism;
using Evolution.Environment;
using Improbable.Collections;
using Improbable.Worker;
using Improbable.Math;
using UnityEngine;

namespace Assets.EntityTemplates
{
    public class OrganismEntityTemplate : MonoBehaviour
    {
        public static SnapshotEntity GenerateOrganismEntityTemplate(Coordinates position, Genome genome1, Genome genome2)
        {
            var entity = new SnapshotEntity { Prefab = "OrganismPrefab" };

            //Debug.Log(position);

            entity.Add(new Consumer.Data(
                        new ConsumerData(Evolution.Material.A, Evolution.Material.B, 1.0)
                        ));
            entity.Add(new Reproducer.Data(new ReproducerData(genome1, genome2)));
            entity.Add(new Mover.Data(new MoverData(position, 10f, 0.0f, 10)));
            entity.Add(new Affectable.Data(
                        new AffectableData(new Map<Evolution.Material, uint>())
                        ));

            var acl = Acl.Build()
                .SetReadAccess(CommonPredicates.PhysicsOrVisual)
                .SetWriteAccess<Consumer>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Reproducer>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Mover>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Affectable>(CommonPredicates.PhysicsOnly);

            entity.SetAcl(acl);

            return entity;
        }
    }
}
