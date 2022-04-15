using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public enum ChunkStatus { DRAW, DONE };
    public enum Biome { NORMAL, DESERT };
    public ChunkStatus status;

    public Biome b;

    public Material material;
    public Block[,,] chunkData;
    public GameObject chunk;

    private int chunkSize;

    public Chunk(Vector3 pos, Material material, int chunkSize) {
        this.chunk = new GameObject(World.CreateChunkName(pos));
        this.chunk.transform.position = pos;
        this.material = material;
        this.chunkSize = chunkSize;
        
        // if ((pos.x + pos.z) % 100 < 3) {
        //     b = Biome.DESERT;
        // } else {
        //     b = Biome.NORMAL;
        // }
        
        b = GetBiome((int)pos.x, (int)pos.z, 2);

        BuildChunk();
    }

    Biome GetBiome (int x, int z, int size) {
        
        bool pertence_x = false;
        List<int> xs = new List<int>();
        List<int> negative = new List<int>();
        int maxValue = (size-1) * 16;
        int newMax = maxValue + ((size+1) * 16);
        int counter = 2;

        for (int i=0; i < size; ++i) {
            int value = i*16;
            if ((x-value)%newMax == 0) {
                pertence_x = true;
            }
            if (i == 0) negative.Add(value - 16);
            else {
                negative.Add(value - (16 * counter));
                ++ counter;
            }
            
            xs.Add(i*16);
        }

        if (pertence_x) {
            for (int i=0; i < size; ++i)
                if ((z-xs[i])%newMax == 0)
                    return Biome.DESERT;
        } else {

            for (int i=0; i < size; ++i)
                if (z-negative[i]%newMax == 0)
                    return Biome.DESERT;
        }

        return Biome.NORMAL;
    }

    void BuildChunk() {
        chunkData = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; ++z) {
            for (int y = 0; y < chunkSize; ++y) {
                for (int x = 0; x < chunkSize; ++x) {
                    Vector3 pos = new Vector3(x, y, z);

                    int worldX = (int) chunk.transform.position.x + x;
                    int worldY = (int) chunk.transform.position.y + y;
                    int worldZ = (int) chunk.transform.position.z + z;

                    
                    if (b == Biome.NORMAL) {
                        int h = Utils.GenerateHeight(worldX, worldZ);
                        int hs = Utils.GeneratesStoneHeight(worldX, worldZ);

                        Grapher.Log(Utils.fBM3D(worldX, worldY, worldZ, 1, .5f), "noise3D", Color.yellow);
                        Grapher.Log( Utils.fBM(worldX * 2 * Utils.smooth, worldZ * 2 * Utils.smooth, Utils.octaves - 1, 1.2f*Utils.persistence), "Stone height", Color.green);
                        Grapher.Log( Utils.fBM(worldX * Utils.smooth, worldZ * Utils.smooth, Utils.octaves, Utils.persistence), "Normal height", Color.red);


                        if (worldY <= hs) { // Criar grutas
                        if (Utils.fBM3D(worldX, worldY, worldZ, 1, .6f) < .7f)
                            chunkData[x, y, z] = new Block(BlockType.STONE, pos, this, this.material);
                        else
                            chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                        } else if (worldY == h)
                            chunkData[x, y, z] = new Block(BlockType.GRASS, pos, this, this.material);
                        else if (worldY < h)
                            chunkData[x, y, z] = new Block(BlockType.DIRT, pos, this, this.material);
                        else // Tudo o resto e ar
                            chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                    } else if (b == Biome.DESERT) {
                        int h = Utils.GenerateHeight(worldX, worldZ, 0.001f, 3, 0.6f);

                        // Grapher.Log(Utils.fBM3D(worldX, worldY, worldZ, 1, .5f), "noise3D", Color.yellow);
                        // Grapher.Log( Utils.fBM(worldX * 2 * Utils.smooth, worldZ * 2 * Utils.smooth, Utils.octaves - 1, 1.2f*Utils.persistence), "Stone height", Color.green);
                        // Grapher.Log( Utils.fBM(worldX * Utils.smooth, worldZ * Utils.smooth, Utils.octaves, Utils.persistence), "Normal height", Color.red);
                        
                        if (worldY == h)
                            chunkData[x, y, z] = new Block(BlockType.SAND, pos, this, this.material);
                        else if (worldY < h)
                            chunkData[x, y, z] = new Block(BlockType.SAND, pos, this, this.material);
                        else // Tudo o resto e ar
                            chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                    }

                    
                    
                }
            }
        }
        status = ChunkStatus.DRAW;
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
        MeshCollider collider = chunk.AddComponent<MeshCollider>();
        collider.sharedMesh = chunk.GetComponent<MeshFilter>().mesh;
        status = ChunkStatus.DONE;
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
