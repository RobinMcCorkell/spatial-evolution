using System.Collections.Generic;
using System.Text;
using System.IO;
using Assets.EntityTemplates;
using Improbable;
using Improbable.Worker;
using Improbable.Math;
using UnityEngine;
using JetBrains.Annotations;
using UnityEditor;

public class SnapshotMenu : MonoBehaviour
{
    private static readonly string InitialWorldSnapshotPath = Application.dataPath +
                                                              "/../../../snapshots/initial.snapshot";

    [MenuItem("Improbable/Snapshots/Generate Snapshot Programmatically")]
    [UsedImplicitly]
    private static void GenerateSnapshotProgrammatically()
    {
        var snapshotEntities = new Dictionary<EntityId, SnapshotEntity>();
        var currentEntityId = 0;

        while (currentEntityId <= 20)
        {
            Coordinates pos = new Coordinates(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            string gen1 = GetRandomGenome();
            string gen2 = GetRandomGenome();
            snapshotEntities.Add(new EntityId(currentEntityId++), OrganismEntityTemplate.GenerateOrganismEntityTemplate(pos, new Evolution.Organism.Genome(gen1), new Evolution.Organism.Genome(gen2)));
        }

        SaveSnapshot(snapshotEntities);
    }

    private static string GetRandomGenome()
    {
        string genome = "";
        for (int i = 0; i < 6; i++)
        {
            byte[] b = new byte[6];
            for (int j = 0; i < 4; i++)
            {
                b[i] = (byte) Random.Range(0, 1);
            }
            genome += System.Convert.ToBase64String(b);
        }
        return genome;
    }

    private static void SaveSnapshot(IDictionary<EntityId, SnapshotEntity> snapshotEntities)
    {
        File.Delete(InitialWorldSnapshotPath);
        var maybeError = Snapshot.Save(InitialWorldSnapshotPath, snapshotEntities);

        if (maybeError.HasValue)
        {
            Debug.LogErrorFormat("Failed to generate initial world snapshot: {0}", maybeError.Value);
        }
        else
        {
            Debug.LogFormat("Successfully generated initial world snapshot at {0}", InitialWorldSnapshotPath);
        }
    }
}
