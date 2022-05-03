using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public enum ChunkStatus { DRAW, DONE };
    public enum Biome { NORMAL, DESERT, ROCKY };
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

        b = GetBiome((int)pos.x, (int)pos.z, 5);

        BuildChunk();
    }

    Vector3Int TransformToLocalPosition(Vector3Int old_pos) {
        return Vector3Int.FloorToInt(chunk.transform.InverseTransformPoint(old_pos));
    }

    private bool PointInChunk (Vector3Int localPosition) {
        return (localPosition.x >= 0 && localPosition.x < chunkSize) &&
            (localPosition.y >= 0 && localPosition.y < chunkSize) &&
            (localPosition.z >= 0 && localPosition.z < chunkSize) &&
            (chunkData[localPosition.x, localPosition.y, localPosition.z] != null);
    }

    public BlockType GetBlockType(Vector3Int position) {
        Vector3Int localPosition = TransformToLocalPosition(position);
        if (PointInChunk(localPosition))
            return chunkData[localPosition.x, localPosition.y, localPosition.z].bType;
            
        return BlockType.AIR;
    }

    public void RemoveBlock(Vector3Int position) {
        Vector3Int localPosition = TransformToLocalPosition(position);
        if (PointInChunk(localPosition))
            chunkData[localPosition.x, localPosition.y, localPosition.z].bType = BlockType.AIR;
        status = ChunkStatus.DRAW;
    }

    public void PlaceBlock(Vector3Int position, BlockType blockType) {

        Vector3Int localPosition = TransformToLocalPosition(position);
        if (PointInChunk(localPosition)) {
            chunkData[localPosition.x, localPosition.y, localPosition.z].bType = blockType;
            status = ChunkStatus.DRAW;
        }
        
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
                    return (Random.Range(0f, 1f) < .7f) ? Biome.DESERT : Biome.ROCKY;
        } else {

            for (int i=0; i < size; ++i)
                if (z-negative[i]%newMax == 0)
                    return (Random.Range(0f, 1f) < .7f) ? Biome.DESERT : Biome.ROCKY;
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

                    int h = Utils.GenerateHeight(worldX, worldZ);
                    int hs = Utils.GeneratesStoneHeight(worldX, worldZ);
                    
                    if (b == Biome.NORMAL) {

                        // Grapher.Log(Utils.fBM3D(worldX, worldY, worldZ, 1, .5f), "noise3D", Color.yellow);
                        // Grapher.Log( Utils.fBM(worldX * 2 * Utils.smooth, worldZ * 2 * Utils.smooth, Utils.octaves - 1, 1.2f*Utils.persistence), "Stone height", Color.green);
                        // Grapher.Log( Utils.fBM(worldX * Utils.smooth, worldZ * Utils.smooth, Utils.octaves, Utils.persistence), "Normal height", Color.red);

                        // if (worldY > h + 30) {
                        //     if (Utils.fBM3D(worldX, worldY, worldZ, 1, .006f) > .60f)
                        //         chunkData[x, y, z] = new Block(BlockType.WOODEN_PLANK, pos, this, this.material);
                        //     else
                        //         chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                        // } else 
                        if (worldY <= hs) { // Criar grutas
                            if (Utils.fBM3D(worldX, worldY, worldZ, 1, .6f) < .7f)
                                chunkData[x, y, z] = new Block(BlockType.STONE, pos, this, this.material);
                            else
                                chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                        } else if (worldY == h)
                            chunkData[x, y, z] = new Block(BlockType.GRASS, pos, this, this.material);
                        else if (worldY < h)
                            chunkData[x, y, z] = new Block(BlockType.DIRT, pos, this, this.material);
                        else
                            chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);

                    } else if (b == Biome.DESERT) {
                        int newHeight = Utils.GenerateHeight(worldX, worldZ, 0.0001f, Utils.octaves, Utils.persistence);
                        int calculatedHeight = (int)((newHeight + h)/2);
                        
                        if (worldY <= hs) { // Criar grutas
                            if (Utils.fBM3D(worldX, worldY, worldZ, 1, .6f) < .7f)
                                chunkData[x, y, z] = new Block(BlockType.STONE, pos, this, this.material);
                            else
                                chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                        } else if (worldY < calculatedHeight || worldY == calculatedHeight)
                            chunkData[x, y, z] = new Block(BlockType.SAND, pos, this, this.material);
                        else
                            chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                    } else if (b == Biome.ROCKY) {
                        int newHeight = Utils.GenerateHeight(worldX, worldZ, 0.004f, Utils.octaves, Utils.persistence);
                        h = (int)((newHeight + h)/2);
                        
                        if (worldY <= hs) { // Criar grutas
                            if (Utils.fBM3D(worldX, worldY, worldZ, 1, .6f) < .7f)
                                chunkData[x, y, z] = new Block(BlockType.STONE, pos, this, this.material);
                            else
                                chunkData[x, y, z] = new Block(BlockType.AIR, pos, this, this.material);
                        } else if (worldY == h || worldY < h)
                            chunkData[x, y, z] = new Block((Random.Range(0f, 1f) > .3f ? BlockType.STONE : BlockType.DIRT), pos, this, this.material);
                        else if (worldY < h)
                            chunkData[x, y, z] = new Block(BlockType.DIRT, pos, this, this.material);
                        else
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
            if (!meshFilters[i].CompareTag("RealChunk")) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            i ++;
        }
        
        //2. Create a new mesh on the parent object
        MeshFilter mf = null;
        if (this.chunk.GetComponent<MeshFilter>() != null) {
            mf = this.chunk.GetComponent<MeshFilter>();
        } else {
            mf = (MeshFilter)this.chunk.AddComponent<MeshFilter>();
        }
        
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);
        mf.tag = "RealChunk";

        //4. Create a rendeder for the parent
        MeshRenderer renderer;
        if (this.chunk.GetComponent<MeshRenderer>() != null) {
            renderer = this.chunk.GetComponent<MeshRenderer>();
        } else {
            renderer = this.chunk.AddComponent<MeshRenderer>();
        }

        renderer.material = material;

        ChunkGameObject chunkGameObject = this.chunk.AddComponent<ChunkGameObject>();
        chunkGameObject.chunkData = chunkData;
        chunkGameObject.chunk = this;

        
        //5 Delete all uncombined children
        foreach(Transform quad in this.chunk.transform)
            GameObject.Destroy(quad.gameObject);
    }

}
