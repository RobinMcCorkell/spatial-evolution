using Improbable.Unity.Core.Acls;
using Evolution.Environment;
using Improbable.Collections;
using Improbable.Worker;
using Improbable.Math;
using UnityEngine;

namespace Assets.EntityTemplates
{
    public class EnvironmentNodeEntityTemplate : MonoBehaviour
    {
        public static Entity GenerateEnvironmentNodeEntityTemplate(Coordinates position, Map<MaterialType, uint> initialResources)
        {
            var entity = new Entity();

            entity.Add(new Position.Data(new PositionData(position)));
            entity.Add(new Environment.Data(new EnvironmentData(initialResources)));

            var acl = Acl.Build()
                .SetReadAccess(CommonPredicates.PhysicsOrVisual)
                .SetWriteAccess<Position>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Environment>(CommonPredicates.PhysicsOrVisual);

            entity.SetAcl(acl);

            return entity;
        }
    }
}
