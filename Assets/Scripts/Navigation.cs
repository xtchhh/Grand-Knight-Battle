using UnityEngine;
using Random = UnityEngine.Random;

public class Navigation : MonoBehaviour
{
    private Terrain terrain;

    void Awake()
    {
        terrain = FindAnyObjectByType<Terrain>();
    }

    public Vector3 GetDestination()
    {
        float zPosition = Random.Range(0, terrain.terrainData.size.z - 10f);
        float xPosition = Random.Range(0, terrain.terrainData.size.x - 10f);

        Vector3 position = new(xPosition, 0, zPosition);

        return position;
    }
}
