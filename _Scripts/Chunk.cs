using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Mesh mesh;
    MeshCollider meshcollider;

    public Dictionary<Vector3Int, BlockDatabase.BlockType> blockPositions = new Dictionary<Vector3Int, BlockDatabase.BlockType>();

    public int chunkSize = 16; //Chunk will be made up of chunkSize*chunkSize*chunkSize blocks.

    Chunk rightChunk;
    Chunk leftChunk;
    Chunk topChunk;
    Chunk belowChunk;
    Chunk frontChunk;
    Chunk behindChunk;

    // Start is called before the first frame update
    public void Init()
    {
        if (!meshRenderer)
            meshRenderer = GetComponent<MeshRenderer>();

        if (!mesh)
            mesh = GetComponent<MeshFilter>().mesh;

        if (!meshcollider)
            meshcollider = GetComponent<MeshCollider>();

        meshRenderer.materials = World.instance.materials;
        chunkSize = World.instance.ChunkSize;

        GeneratePositions();
    }


    List<int> CheckAdjacentPositions(Vector3Int pos)
    {
        List<int> verts = new List<int>();

        Vector3Int originalPos = pos;

        BlockDatabase.BlockType type = BlockDatabase.BlockType.AIR;
        blockPositions.TryGetValue(pos,out type);

        if (type == BlockDatabase.BlockType.WATER)
        {
            if (pos.y == 5)
            {
                foreach (int i in Block.top)
                    verts.Add(i);
            }

            return verts;
        }

        type = BlockDatabase.BlockType.AIR;

        pos.x -= 1;
        blockPositions.TryGetValue(pos, out type);
        if ((type == BlockDatabase.BlockType.AIR || type == BlockDatabase.BlockType.WATER) && !(pos.x < 0 && CheckPositionInAdjacentChunk(pos, leftChunk)))
            foreach (int i in Block.left)
                verts.Add(i);

        type = BlockDatabase.BlockType.AIR;
        pos.x += 2;
        blockPositions.TryGetValue(pos, out type);
        if ((type == BlockDatabase.BlockType.AIR || type == BlockDatabase.BlockType.WATER) && !(pos.x == chunkSize && CheckPositionInAdjacentChunk(pos, rightChunk)))
            foreach (int i in Block.right)
                verts.Add(i);

        pos = originalPos;

        type = BlockDatabase.BlockType.AIR;
        pos.y -= 1;
        blockPositions.TryGetValue(pos, out type);
        if ((type == BlockDatabase.BlockType.AIR || type == BlockDatabase.BlockType.WATER) && !(pos.y < 0 && CheckPositionInAdjacentChunk(pos, belowChunk)))
            foreach (int i in Block.bottom)
                verts.Add(i);

        type = BlockDatabase.BlockType.AIR;
        pos.y += 2;
        blockPositions.TryGetValue(pos, out type);
        if ((type == BlockDatabase.BlockType.AIR || type == BlockDatabase.BlockType.WATER) && !(pos.y == chunkSize && CheckPositionInAdjacentChunk(pos, topChunk)))
            foreach (int i in Block.top)
                verts.Add(i);

        pos = originalPos;

        type = BlockDatabase.BlockType.AIR;
        pos.z -= 1;
        blockPositions.TryGetValue(pos, out type);
        if ((type == BlockDatabase.BlockType.AIR || type == BlockDatabase.BlockType.WATER) && !(pos.z < 0 && CheckPositionInAdjacentChunk(pos, behindChunk)))
            foreach (int i in Block.front)
                verts.Add(i);

        type = BlockDatabase.BlockType.AIR;
        pos.z += 2;
        blockPositions.TryGetValue(pos, out type);
        if ((type == BlockDatabase.BlockType.AIR || type == BlockDatabase.BlockType.WATER) && !(pos.z == chunkSize && CheckPositionInAdjacentChunk(pos, frontChunk)))
            foreach (int i in Block.back)
                verts.Add(i);

        return verts;

    }

    bool CheckPositionInAdjacentChunk(Vector3Int position, Chunk chunk)
    {
        BlockDatabase.BlockType type = BlockDatabase.BlockType.AIR;

        if (chunk)
        {
            Vector3 worldPos = transform.TransformPoint(position);
            worldPos = chunk.transform.InverseTransformPoint(worldPos);

            position.x = (int)worldPos.x;
            position.y = (int)worldPos.y;
            position.z = (int)worldPos.z;
            chunk.blockPositions.TryGetValue(position, out type);
        }

        if (type != BlockDatabase.BlockType.AIR &&  type != BlockDatabase.BlockType.WATER)
            return true;

        return false;
    }

    void GeneratePositions()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                if (transform.localPosition.y >= 0)
                {
                    int height = (int)(Noise.EvaluateNoise2D(new Vector2((x + transform.localPosition.x + World.instance.seed) * World.instance.noiseFrequency, (z + transform.localPosition.z + World.instance.seed) * World.instance.noiseFrequency)) * World.instance.noiseAmplitude);

                    for (int h = height; h >= 0; h--)
                    {
                        float n3d = Noise.EvaluateNoise3D((new Vector3(((x + transform.localPosition.x + World.instance.seed) * 0.04f), ((h + transform.localPosition.y + World.instance.seed) * 0.04f), ((z + transform.localPosition.z + World.instance.seed) * 0.04f))));

                        if (n3d > -0.5f)
                        {

                            if (h == height)
                            {

                                if (h > 7)
                                    AddPos(new Vector3Int(x, h, z), BlockDatabase.BlockType.GRASS);

                                else
                                    AddPos(new Vector3Int(x, h, z), BlockDatabase.BlockType.SAND);

                                int spawnTree = Random.Range(0, 200);



                                if (h > 8 && spawnTree > 196 && World.instance.GenerateTrees)
                                {
                                    GenerateTree(new Vector3Int(x, h, z));
                                }


                            }
                            else if (h > height - 6)
                                AddPos(new Vector3Int(x, h, z), BlockDatabase.BlockType.DIRT);
                            else
                            {

                                float oreNoise = Noise.EvaluateNoise3D((new Vector3(((x + transform.localPosition.x + World.instance.seed) * 0.9f), ((h + transform.localPosition.y + World.instance.seed) * 0.9f), ((z + transform.localPosition.z + World.instance.seed) * 0.9f))));


                                if(oreNoise < 0.7f)
                                    AddPos(new Vector3Int(x, h, z), BlockDatabase.BlockType.STONE);
                                else if(oreNoise < 0.85f)
                                    AddPos(new Vector3Int(x, h, z), BlockDatabase.BlockType.COAL);
                                else
                                    AddPos(new Vector3Int(x, h, z), BlockDatabase.BlockType.IRON);
                            }
                        }

                        if (h <= 5)
                        {
                            for (int w = h + 1; w <= 5; w++)
                            {
                                AddPos(new Vector3Int(x, w, z), BlockDatabase.BlockType.WATER);
                            }
                        }
                    }


                }
                else if (transform.localPosition.y < 0)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        float n3d = Noise.EvaluateNoise3D((new Vector3(((x + transform.localPosition.x + World.instance.seed) * 0.04f), ((y + transform.localPosition.y + World.instance.seed) * 0.04f), ((z + transform.localPosition.z + World.instance.seed) * 0.04f))));

                        if (n3d > -0.4f)
                        {
                            float oreNoise = Noise.EvaluateNoise3D((new Vector3(((x + transform.localPosition.x + World.instance.seed) * 0.9f), ((y + transform.localPosition.y + World.instance.seed) * 0.9f), ((z + transform.localPosition.z + World.instance.seed) * 0.9f))));

                            if (oreNoise < 0.6f)
                                AddPos(new Vector3Int(x, y, z), BlockDatabase.BlockType.STONE);
                            else if (oreNoise < 0.8f)
                                AddPos(new Vector3Int(x, y, z), BlockDatabase.BlockType.COAL);
                            else
                                AddPos(new Vector3Int(x, y, z), BlockDatabase.BlockType.IRON);
                            
                        }
                    }
                }  
            }
            
        }
    }

    void AddPos(Vector3Int pos, BlockDatabase.BlockType type)
    {
        if (!blockPositions.ContainsKey(pos))
            blockPositions.Add(pos, type);
    }

    void GenerateTree(Vector3Int pos)
    {
        int trunkHeight = Random.Range(4, 6);

        for (int trunkPos = 0; trunkPos < trunkHeight; trunkPos++)
        {
            pos += new Vector3Int(0, 1, 0);
            AddPos(pos, BlockDatabase.BlockType.BARK);
        }

        for (int t = 1; t < 2; t++)
        {
            for (int p = 0; p < 3; p++)
            {
                AddPos(pos + new Vector3Int(t, p, 0), BlockDatabase.BlockType.LEAF);
                AddPos(pos + new Vector3Int(0, p, t), BlockDatabase.BlockType.LEAF);
                AddPos(pos + new Vector3Int(0, p, -t), BlockDatabase.BlockType.LEAF);
                AddPos(pos + new Vector3Int(-t, p, 0), BlockDatabase.BlockType.LEAF);
                AddPos(pos + new Vector3Int(0, p + 1, 0), BlockDatabase.BlockType.LEAF);

                if (p == 0)
                {
                    AddPos(pos + new Vector3Int(-t, p, t), BlockDatabase.BlockType.LEAF);
                    AddPos(pos + new Vector3Int(t, p, -t), BlockDatabase.BlockType.LEAF);
                    AddPos(pos + new Vector3Int(t, p, t), BlockDatabase.BlockType.LEAF);
                    AddPos(pos + new Vector3Int(-t, p, -t), BlockDatabase.BlockType.LEAF);
                }
            }
        }
    }



    public void GenerateChunk()
    {
        if (!belowChunk)
            World.instance.chunks.TryGetValue(transform.localPosition + new Vector3(0, -chunkSize, 0), out belowChunk);
        if (!topChunk)
            World.instance.chunks.TryGetValue(transform.localPosition + new Vector3(0, chunkSize, 0), out topChunk);
        if (!frontChunk)
            World.instance.chunks.TryGetValue(transform.localPosition + new Vector3(0, 0, chunkSize), out frontChunk);
        if (!behindChunk)
            World.instance.chunks.TryGetValue(transform.localPosition + new Vector3(0, 0, -chunkSize), out behindChunk);
        if (!leftChunk)
            World.instance.chunks.TryGetValue(transform.localPosition + new Vector3(-chunkSize, 0, 0), out leftChunk);
        if (!rightChunk)
            World.instance.chunks.TryGetValue(transform.localPosition + new Vector3(chunkSize, 0, 0), out rightChunk);



        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();
        List<int> mainMeshTriangles = new List<int>();
        List<int> waterMeshTriangles = new List<int>();

        foreach (KeyValuePair<Vector3Int, BlockDatabase.BlockType> pair in blockPositions)
        {
            List<int> neededVerts = CheckAdjacentPositions(pair.Key);
            BlockDatabase.BlockData blockData = null;

            foreach (BlockDatabase.BlockData data in BlockDatabase.instance.blockData)
            {
                if (data.type == pair.Value)
                    blockData = data;
            }

            for (int i = 0; i < neededVerts.Count; i += 2)
            {
                for (int t = neededVerts[i]; t <= neededVerts[i + 1]; t++)
                {
                    uvs.Add(Block.GetUV(t, blockData));
                    vertices.Add(Block.vertices[Block.triangles[t]] + pair.Key);

                    if (pair.Value != BlockDatabase.BlockType.WATER)
                        mainMeshTriangles.Add(vertices.Count - 1);
                    else
                        waterMeshTriangles.Add(vertices.Count - 1);
                }
            }
        }

        //Build the mesh
        mesh.Clear();
        mesh.subMeshCount = 2;
        mesh.vertices = vertices.ToArray();
        mesh.SetTriangles(mainMeshTriangles.ToArray(), 0);
        mesh.SetTriangles(waterMeshTriangles.ToArray(), 1);
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        //collider.sharedMesh = mesh;
    }

}
