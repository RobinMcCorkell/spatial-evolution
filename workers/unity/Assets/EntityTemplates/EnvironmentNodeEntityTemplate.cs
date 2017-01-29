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
        public static SnapshotEntity GenerateEnvironmentNodeEntityTemplate(Coordinates position, Map<Evolution.Material, uint> initialResources)
        {
            var entity = new SnapshotEntity { Prefab = "EnvironmentNode" };

            entity.Add(new Position.Data(new PositionData(position)));
            entity.Add(new Evolution.Environment.Resources.Data(new ResourcesData(initialResources)));

            var acl = Acl.Build()
                .SetReadAccess(CommonPredicates.PhysicsOrVisual)
                .SetWriteAccess<Position>(CommonPredicates.PhysicsOnly)
                .SetWriteAccess<Evolution.Environment.Resources>(CommonPredicates.PhysicsOrVisual);

            entity.SetAcl(acl);

            return entity;
        }
    }
}
