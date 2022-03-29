using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

    BlockType bType;
    Material material;
    Chunk owner;
    Vector3 pos;
    bool isSolid;

    public Block(BlockType bType, Vector3 pos, Chunk owner, Material material) {

        this.bType = bType;
        this.pos = pos;
        this.owner = owner;
        this.material = material;

        if (bType == BlockType.AIR) { this.isSolid = false; }
        else { this.isSolid = true; }

    }

    void CreateQuad(Cubeside side) {

        Mesh mesh = new Mesh();

        Vector3[] vertices = VoxelData.getBySide(side)[0];
        Vector3[] normals = VoxelData.getBySide(side)[1];

        if (bType == BlockType.GRASS && side == Cubeside.TOP) {
            VoxelData.uv[2] = VoxelTextures.blockUVs[0, 0];
            VoxelData.uv[3] = VoxelTextures.blockUVs[0, 1];
            VoxelData.uv[1] = VoxelTextures.blockUVs[0, 2];
            VoxelData.uv[0] = VoxelTextures.blockUVs[0, 3];
        } else if (bType == BlockType.GRASS && side == Cubeside.BOTTOM) {
            VoxelData.uv[2] = VoxelTextures.blockUVs[2, 0];
            VoxelData.uv[3] = VoxelTextures.blockUVs[2, 1];
            VoxelData.uv[1] = VoxelTextures.blockUVs[2, 2];
            VoxelData.uv[0] = VoxelTextures.blockUVs[2, 3];
        } else {
            VoxelData.uv[2] = VoxelTextures.blockUVs[(int)(bType + 1), 0];
            VoxelData.uv[3] = VoxelTextures.blockUVs[(int)(bType + 1), 1];
            VoxelData.uv[1] = VoxelTextures.blockUVs[(int)(bType + 1), 2];
            VoxelData.uv[0] = VoxelTextures.blockUVs[(int)(bType + 1), 3];
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = VoxelData.triangles;
        mesh.uv = VoxelData.uv;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.position = this.pos;
        quad.transform.parent = this.owner.chunk.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    bool HasSolidNeighbour(Vector3Int coordinates) {
        Block[,,] chunkData = owner.chunkData;
        try {
            return chunkData[coordinates.x, coordinates.y, coordinates.z].isSolid;
        } catch(System.IndexOutOfRangeException ex) { }

        return false;
        
    }

    public void Draw() {

        if (bType == BlockType.AIR) return;

        if (!HasSolidNeighbour(new Vector3Int((int)pos.x-1, (int)pos.y, (int)pos.z)))
            CreateQuad(Cubeside.LEFT);
        if (!HasSolidNeighbour(new Vector3Int((int)pos.x+1, (int)pos.y, (int)pos.z)))
            CreateQuad(Cubeside.RIGHT);
        if (!HasSolidNeighbour(new Vector3Int((int)pos.x, (int)pos.y-1, (int)pos.z)))
            CreateQuad(Cubeside.BOTTOM);
        if (!HasSolidNeighbour(new Vector3Int((int)pos.x, (int)pos.y+1, (int)pos.z)))
            CreateQuad(Cubeside.TOP);
        if (!HasSolidNeighbour(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z-1)))
            CreateQuad(Cubeside.BACK);
        if (!HasSolidNeighbour(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z+1)))
            CreateQuad(Cubeside.FRONT);
    }
}


