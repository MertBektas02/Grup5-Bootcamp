using System;
using UnityEngine;
using UnityEditor;

public class TerrainTreeConverter : MonoBehaviour
{
    [MenuItem("Tools/Convert Terrain Trees To Prefabs")]
    [Obsolete("Obsolete")]
    static void ConvertTrees()
    {
        var terrain = FindObjectOfType<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("No terrain found in scene!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        foreach (TreeInstance treeInstance in terrainData.treeInstances)
        {
            TreePrototype prototype = terrainData.treePrototypes[treeInstance.prototypeIndex];
            GameObject prefab = prototype.prefab;

            Vector3 worldPos = Vector3.Scale(treeInstance.position, terrainData.size) + terrainPos;
            GameObject tree = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            tree.transform.position = worldPos;
            tree.transform.localScale = treeInstance.widthScale * Vector3.one; // Ölçek de aktarılır
            tree.transform.rotation = Quaternion.Euler(0, treeInstance.rotation * Mathf.Rad2Deg, 0);
        }

        Debug.Log("Terrain trees converted to prefab instances.");
    }
}