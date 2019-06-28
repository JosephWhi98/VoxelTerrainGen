using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int ChunkSize;
    public int worldSize = 10;
    public int worldDepth = 2;
    public float seed;

    public Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();

    public float noiseAmplitude;
    public float noiseFrequency;

    public static World instance;

    public Material[] materials;

    public bool GenerateTrees;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(0, 2000000);

        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y > -worldDepth; y--)
            {
                for (int z = 0; z < worldSize; z++)
                {
                    Vector3 position = new Vector3(x * ChunkSize, y * ChunkSize, z * ChunkSize);
                    GameObject chunk = new GameObject("Chunk");
                    chunk.transform.SetParent(transform);
                    chunk.transform.localPosition = position;
                    Chunk chunkComponent = chunk.AddComponent<Chunk>();
                    chunkComponent.Init();
                    chunks.Add(position, chunkComponent);
                }
            }
        }


        foreach (Chunk chunk in chunks.Values)
            chunk.GenerateChunk();

    }
}
