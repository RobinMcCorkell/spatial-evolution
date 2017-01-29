using System.Collections.Generic;
using System.IO;
using System;
using Assets.EntityTemplates;
using Improbable;
using Improbable.Worker;
using Improbable.Math;
using Improbable.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using JetBrains.Annotations;
using UnityEditor;

public class SnapshotMenu : MonoBehaviour
{
    private static readonly string InitialWorldSnapshotPath = Application.dataPath +
                                                              "/../../../snapshots/initial.snapshot";

    [MenuItem("Improbable/Snapshots/Generate Snapshot for spatial-evolution")]
    [UsedImplicitly]
    private static void GenerateSnapshotProgrammatically()
    {
        var snapshotEntities = new Dictionary<EntityId, SnapshotEntity>();
        var currentEntityId = 0;

        for (int i = 0; i < 40; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                Coordinates pos = new Coordinates(i, j, -0.01);
                Map<Evolution.Material, uint> initialRes = new Map<Evolution.Material, uint>(2);
                foreach (Evolution.Material mat in Enum.GetValues(typeof(Evolution.Material)))
                {
                    initialRes[mat] = (uint) Random.Range(0, 50);
                }
                snapshotEntities.Add(new EntityId(currentEntityId++), EnvironmentNodeEntityTemplate.GenerateEnvironmentNodeEntityTemplate(pos, initialRes));
            }
        }

        for (int id = currentEntityId; id < currentEntityId + 20; id++)
        {
            Coordinates pos = new Coordinates(Random.Range(0, 20), Random.Range(0, 20), 0);
            Evolution.Organism.Genome gen1 = new Evolution.Organism.Genome(GetRandomGenome());
            Evolution.Organism.Genome gen2 = new Evolution.Organism.Genome(GetRandomGenome());
            snapshotEntities.Add(new EntityId(id), OrganismEntityTemplate.GenerateOrganismEntityTemplate(pos, gen1, gen2));
        }

        SaveSnapshot(snapshotEntities);
    }

    private static string GetRandomGenome()
    {
        byte[] b = new byte[3];

        for (int i = 0; i < 3; i++)
        {
            b[i] = (byte) Random.Range(0, 255);
        }
        string genome = System.Convert.ToBase64String(b);
        
        //Debug.LogFormat(genome);
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
