using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Material material;
    public static int columnHeight = 4;
    public static int chunkSize = 16;
    public static int worldSize = 10;
    public static Dictionary<string, Chunk> chunkDict;

    public static string CreateChunkName (Vector3 v) {
        return (int)v.x + " " + (int)v.y + " " + (int)v.z;
    }

    IEnumerator BuildWorld() {
        for (int x = 0; x < worldSize; ++x) {
            for (int z = 0; z < worldSize; ++z) {
                for (int y = 0; y < columnHeight; ++y) {
                    Vector3 chunkPos = new Vector3(x*chunkSize, y*chunkSize, z*chunkSize);
                    Chunk c = new Chunk(chunkPos, material, chunkSize);
                    c.chunk.transform.parent = this.transform;
                    chunkDict.Add(c.chunk.name, c);
                }
                yield return null;
            }
        }

        foreach(KeyValuePair<string, Chunk> c in chunkDict) {
            c.Value.DrawChunk();
            yield return null;
        }
    }

    IEnumerator BuildChunkColumn() {
        for(int i = 0; i < columnHeight; ++i) {
            Vector3 chunkPos = new Vector3(this.transform.position.x, i*chunkSize, this.transform.position.z);
            Chunk c = new Chunk(chunkPos, material, chunkSize);
            c.chunk.transform.parent = this.transform;
            chunkDict.Add(c.chunk.name, c);
        }

        foreach(KeyValuePair<string, Chunk> c in chunkDict) {
            c.Value.DrawChunk();
            yield return null;
        }
    }

    private void Start() {
        chunkDict = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        
        StartCoroutine(BuildWorld());
    }
    
}
