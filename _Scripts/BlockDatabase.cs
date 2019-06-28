using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDatabase : MonoBehaviour
{
    public enum BlockType { AIR, GRASS, DIRT, STONE, BARK, LEAF, SAND, WATER, COAL, IRON};
    public static BlockDatabase instance;

    [System.Serializable]
    public class BlockData
    { 
        public BlockType type;
        public Vector2 frontOffset;
        public Vector2 backOffset;
        public Vector2 leftOffset;
        public Vector2 rightOffset;
        public Vector2 topoffset;
        public Vector2 bottomOffset;
    }

    public List<BlockData> blockData = new List<BlockData>();

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
}
