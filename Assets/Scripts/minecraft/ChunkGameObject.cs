using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGameObject : MonoBehaviour {

    public Chunk chunk { get; set; }
    private World world;
    public Block[,,] chunkData { get; set; }

    public List<GameObject> addedBlocks = new List<GameObject>();

    private void Start() {
        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
    }

    public void RemoveBlock(Vector3 pos) {
        Vector3Int blockPos = Vector3Int.FloorToInt(pos);

        chunk.RemoveBlock(blockPos);
        world.Drawing();
        
    }

    public string BlockTypeToString(BlockType blockType) {
        switch (blockType) {
            case BlockType.DIRT: return "Dirt";
            case BlockType.GRASS: return "Grass";
            case BlockType.STONE: return "Stone";
            case BlockType.SAND: return "Sand";
            case BlockType.WOODEN_PLANK: return "Wooden Plank";
            case BlockType.TNT: return "TNT";
        }
        return null;
    } 

    public string GetBlockType(Vector3 pos) {
        Vector3Int blockPos = Vector3Int.FloorToInt(pos);
        BlockType selectedBlockType = chunk.GetBlockType(blockPos);

        return BlockTypeToString(selectedBlockType);

    }

    public void PlaceBlock(Vector3 pos, BlockType blockType) {
        Vector3Int blockPos = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)+1, Mathf.FloorToInt(pos.z));

        chunk.PlaceBlock(blockPos, blockType);
        world.Drawing();

    }

    public void HighLightBlock(Vector3 pos, GameObject highlight) {
        Vector3Int blockPos = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y + 1), Mathf.FloorToInt(pos.z));

        highlight.SetActive(true);
        highlight.transform.position = blockPos;
        highlight.transform.rotation = Quaternion.identity;
    }



}
