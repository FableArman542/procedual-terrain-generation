using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelTextures {
    private static readonly Vector2 grassSide_LBC = new Vector2(3f, 15f)/16;
    private static readonly Vector2 grassTop_LBC = new Vector2(2f, 6f)/16;
    private static readonly Vector2 dirt_LBC = new Vector2(2f, 15f)/16;
    private static readonly Vector2 stone_LBC = new Vector2(0f, 14f)/16;
    private static readonly Vector2 sand_LBC = new Vector2(0f, 4f)/16;
    private static readonly Vector2 woodenPlank_LBC = new Vector2(4f, 15f)/16;

    private static readonly Vector2[] vectors = {
        new Vector2(1f, 0f)/16,
        new Vector2(0f, 1f)/16,
        new Vector2(1f, 1f)/16
    };

    public static readonly Vector2[,] blockUVs = {
        /* Grass Top */ {   grassTop_LBC, 
                            grassTop_LBC + vectors[0], 
                            grassTop_LBC + vectors[1], 
                            grassTop_LBC + vectors[2]},
        /* Grass Side */ {  grassSide_LBC, 
                            grassSide_LBC + vectors[0], 
                            grassSide_LBC + vectors[1], 
                            grassSide_LBC + vectors[2]},
        /* Dirt */ {        dirt_LBC, 
                            dirt_LBC + vectors[0],
                            dirt_LBC + vectors[1],
                            dirt_LBC + vectors[2]},
        /* Stone */ {       stone_LBC, 
                            stone_LBC + vectors[0],
                            stone_LBC + vectors[1],
                            stone_LBC + vectors[2]},
        /* Sand */ {        sand_LBC,
                            sand_LBC + vectors[0], 
                            sand_LBC + vectors[1], 
                            sand_LBC + vectors[2]},
        /* Wood Planks */ { woodenPlank_LBC, 
                            woodenPlank_LBC + vectors[0], 
                            woodenPlank_LBC + vectors[1], 
                            woodenPlank_LBC + vectors[2]}
    };

}