using UnityEngine;

public class ConvertTerrainTrees : MonoBehaviour
{
    public Terrain terrain;
    public GameObject treePrefab;

    void Start()
    {
        foreach (TreeInstance tree in terrain.terrainData.treeInstances)
        {
            Vector3 worldPos = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
            Instantiate(treePrefab, worldPos, Quaternion.identity, transform);
        }
    }
}