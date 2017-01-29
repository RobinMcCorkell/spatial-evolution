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
        public static Entity GenerateOrganismEntityTemplate(Coordinates position, Genome genome)
        {
            var entity = new Entity();

            entity.Add(new Consumer.Data(
                        new ConsumerData(MaterialType.A, MaterialType.B, 1.0)
                        ));
            entity.Add(new Reproducer.Data(new ReproducerData(genome, genome)));
            entity.Add(new Mover.Data(new MoverData(position, 1.0, 1)));
            entity.Add(new Affectable.Data(
                        new AffectableData(new Map<MaterialType, uint>())
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
