using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public static Vector2 GetUV(int index, BlockDatabase.BlockData data)
    {
        Vector2 uv = UVs[index];

        if (index <= 5)
            uv += data.frontOffset;
        else if (index <= 11)
            uv += data.topoffset;
        else if (index <= 17)
            uv += data.rightOffset;
        else if (index <= 23)
            uv += data.leftOffset;
        else if (index <= 29)
            uv += data.backOffset;
        else if (index <= 35)
            uv += data.bottomOffset;

        return uv;
    }


    //Triangle/UV indexes that make up each face.
    public static int[] front = { 0, 5 };
    public static int[] back = { 24 , 29 };
    public static int[] left = { 18, 23 };
    public static int[] right = { 12, 17 };
    public static int[] top = { 6, 11 };
    public static int[] bottom = { 30, 35 };

    public static Vector3[] vertices =
    {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,0,1),
            new Vector3(0,0,1)
    };

    public static int[] triangles =
    {
        0, 2, 1, //face front
		0, 3, 2,
        2, 3, 4, //face top
		2, 4, 5,
        1, 2, 5, //face right
		1, 5, 6,
        0, 7, 4, //face left
		0, 4, 3,
        5, 4, 7, //face back
		5, 7, 6,
        0, 6, 7, //face bottom
		0, 1, 6
    };

    public static Vector2[] UVs =
    {
        new Vector2(0.0f, 0.9f), //Front
        new Vector2(0.1f, 1.0f),
        new Vector2(0.1f, 0.9f),
        new Vector2(0.0f, 0.9f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.1f, 1.0f),
        new Vector2(0.1f, 0.9f), //Top
        new Vector2(0.0f, 0.9f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.1f, 0.9f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.1f, 1.0f),
        new Vector2(0.0f, 0.9f), //Right
        new Vector2(0.0f, 1.0f),
        new Vector2(0.1f, 1.0f),
        new Vector2(0.0f, 0.9f),
        new Vector2(0.1f, 1.0f),
        new Vector2(0.1f, 0.9f),
        new Vector2(0.1f, 0.9f), //Left
        new Vector2(0.0f, 0.9f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.1f, 0.9f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.1f, 1.0f),
        new Vector2(0.1f, 1.0f), //Back
        new Vector2(0.0f, 1.0f),
        new Vector2(0.0f, 0.9f),
        new Vector2(0.1f, 1.0f),
        new Vector2(0.0f, 0.9f),
        new Vector2(0.1f, 0.9f),
        new Vector2(0.0f, 0.9f),//Bottom
        new Vector2(0.1f, 1.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.0f, 0.9f),
        new Vector2(0.1f, 0.9f),
        new Vector2(0.1f, 1.0f)
    };
}
