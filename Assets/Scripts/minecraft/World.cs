using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public GameObject player;
    public Material material;
    public static int chunkSize = 16;
    public static int radius = 4;
    public static Dictionary<string, Chunk> chunkDict;

    Vector3 lastBuildPosition;

    public static string CreateChunkName (Vector3 v) {
        return (int)v.x + " " + (int)v.y + " " + (int)v.z;
    }

    void BuildRecursiveWorld(Vector3 chunkPos, int rad) {

        int x = (int)chunkPos.x;
        int y = (int)chunkPos.y;
        int z = (int)chunkPos.z;

        BuildChunkAt(chunkPos);

        if (--rad < 0) return;

        BuildRecursiveWorld(new Vector3(x, y, z+chunkSize), rad-1);
        BuildRecursiveWorld(new Vector3(x, y, z-chunkSize), rad-1);
        BuildRecursiveWorld(new Vector3(x, y+chunkSize, z), rad-1);
        BuildRecursiveWorld(new Vector3(x, y-chunkSize, z+chunkSize), rad-1);
        BuildRecursiveWorld(new Vector3(x+chunkSize, y, z), rad-1);
        BuildRecursiveWorld(new Vector3(x-chunkSize, y, z), rad-1);
    }

    void BuildChunkAt(Vector3 chunkPos) {
        string name = CreateChunkName(chunkPos);
        Chunk c;
        if (!chunkDict.TryGetValue(name, out c)) {
            c = new Chunk(chunkPos, material, chunkSize);
            c.chunk.transform.parent = this.transform;
            chunkDict.Add(c.chunk.name, c);
        }
    }

    void DrawChunks () {
        foreach(KeyValuePair<string, Chunk> c in chunkDict) {
            if (c.Value.status == Chunk.ChunkStatus.DRAW)
                c.Value.DrawChunk();
        }
    }

    Vector3 WhichChunk(Vector3 position) {
        Vector3 chunkPos = new Vector3();
        chunkPos.x = (int)(position.x / chunkSize) * chunkSize;
        chunkPos.y = (int)(position.y / chunkSize) * chunkSize;
        chunkPos.z = (int)(position.z / chunkSize) * chunkSize;

        return chunkPos;

    }

    private void Start() {
        chunkDict = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        
        // StartCoroutine(BuildWorld());

        Vector3 ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, Utils.GenerateHeight(ppos.x, ppos.z), ppos.z);

        lastBuildPosition = player.transform.position;

        BuildRecursiveWorld(WhichChunk(player.transform.position), radius);
        DrawChunks();
    }

    private void Update() {
        Vector3 movement = player.transform.position - lastBuildPosition;
        if (movement.magnitude > chunkSize) {
            lastBuildPosition = player.transform.position;
            BuildRecursiveWorld(WhichChunk(player.transform.position), radius);
            DrawChunks();
        }
    }
    
}
