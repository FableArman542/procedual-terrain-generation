using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelTextures {
    private static readonly Vector2 grassSide_LBC = new Vector2(3f, 15f)/16;
    private static readonly Vector2 grassTop_LBC = new Vector2(2f, 6f)/16;
    private static readonly Vector2 dirt_LBC = new Vector2(2f, 15f)/16;
    private static readonly Vector2 stone_LBC = new Vector2(0f, 14f)/16;

    public static readonly Vector2[,] blockUVs = {
        /* Grass Top */ {grassTop_LBC, grassTop_LBC + new Vector2(1f, 0f)/16, grassTop_LBC + new Vector2(0f, 1f)/16, grassTop_LBC + new Vector2(1f, 1f)/16},
        /* Grass Side */ {grassSide_LBC, grassSide_LBC + new Vector2(1f, 0f)/16, grassSide_LBC + new Vector2(0f, 1f)/16, grassSide_LBC + new Vector2(1f, 1f)/16},
        /* Dirt */ {dirt_LBC, dirt_LBC + new Vector2(1f, 0f)/16, dirt_LBC + new Vector2(0f, 1f)/16, dirt_LBC + new Vector2(1f, 1f)/16},
        /* Stone */ {stone_LBC, stone_LBC + new Vector2(1f, 0f)/16, stone_LBC + new Vector2(0f, 1f)/16, stone_LBC + new Vector2(1f, 1f)/16}
    };

}