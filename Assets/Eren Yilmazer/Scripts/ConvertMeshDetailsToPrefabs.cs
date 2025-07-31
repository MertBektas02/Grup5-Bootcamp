using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ConvertMeshDetailsToPrefabs : MonoBehaviour
{
    [MenuItem("Tools/Convert Mesh Details To Prefabs")]
    [Obsolete("Obsolete")]
    public static void ConvertDetails()
    {
        var terrain = FindObjectOfType<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("No terrain found!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        DetailPrototype[] detailPrototypes = terrainData.detailPrototypes;

        for (int layer = 0; layer < detailPrototypes.Length; layer++)
        {
            var proto = detailPrototypes[layer];
            GameObject protoPrefab = proto.prototype;

            if (proto.usePrototypeMesh == false || protoPrefab == null)
            {
                Debug.LogWarning($"Layer {layer} is not using mesh prototype or prefab is missing.");
                continue;
            }

            int[,] detailLayer = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, layer);

            for (int x = 0; x < terrainData.detailWidth; x++)
            {
                for (int y = 0; y < terrainData.detailHeight; y++)
                {
                    int count = detailLayer[x, y];

                    for (int i = 0; i < count; i++)
                    {
                        float normX = (float)x / terrainData.detailWidth;
                        float normZ = (float)y / terrainData.detailHeight;

                        float posX = normX * terrainData.size.x + terrainPos.x;
                        float posZ = normZ * terrainData.size.z + terrainPos.z;
                        float posY = terrain.SampleHeight(new Vector3(posX, 0, posZ)) + terrainPos.y;

                        Vector3 worldPos = new Vector3(posX, posY, posZ);

                        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(protoPrefab);
                        newObj.transform.position = worldPos;
                        newObj.transform.rotation = Quaternion.identity;
                        newObj.transform.SetParent(terrain.transform); // istersen sahnede ayrı da tutabilirsin
                    }
                }
            }

            Debug.Log($"Converted detail layer {layer} to prefabs.");
        }
    }
}
