using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public Material material;
    public Block[,,] chunkData;
    public GameObject chunk;

    private int chunkSize;

    public Chunk(Vector3 pos, Material material, int chunkSize) {
        this.chunk = new GameObject(World.CreateChunkName(pos));
        this.chunk.transform.position = pos;
        this.material = material;
        this.chunkSize = chunkSize;

        BuildChunk();
    }

    void BuildChunk() {
        chunkData = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; ++z) {
            for (int y = 0; y < chunkSize; ++y) {
                for (int x = 0; x < chunkSize; ++x) {
                    Vector3 pos = new Vector3(x, y, z);
                    if(Random.Range(0f, 1f) < .5f)
                        chunkData[x, y, z] = new Block(BlockType.STONE, pos, this, this.material);
                    else
                        chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                }
            }
        }
    }

    public void DrawChunk() {
        for (int z = 0; z < chunkSize; ++z) {
            for (int y = 0; y < chunkSize; ++y) {
                for (int x = 0; x < chunkSize; ++x) {
                    chunkData[x, y, z].Draw();
                }
            }
        }

        CombineQuads();
    }
    
    void CombineQuads() {

        //1. Combine all children meshes
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i< meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)this.chunk.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a rendeder for the parent
        MeshRenderer renderer = this.chunk.AddComponent<MeshRenderer>();
        renderer.material = material;
        
        //5 Delete all uncombined children
        foreach(Transform quad in this.chunk.transform)
            GameObject.Destroy(quad.gameObject);
    }

}
