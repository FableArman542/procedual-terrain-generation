using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuad : MonoBehaviour {

    public BlockType bType;
    public Material material;

    void Quad(Cubeside side) {

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

        GameObject quad = new GameObject("quad");
        quad.transform.parent = this.gameObject.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    void CombineQuads() {

        //1. Combine all children meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i< meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;

        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)this.gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a rendeder for the parent
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material = material;
        
        //5 Delete all uncombined children
        foreach(Transform quad in this.transform)
            Destroy(quad.gameObject);
    }


    void CreateCube() {
        Quad(Cubeside.LEFT);
        Quad(Cubeside.RIGHT);
        Quad(Cubeside.BOTTOM);
        Quad(Cubeside.TOP);
        Quad(Cubeside.BACK);
        Quad(Cubeside.FRONT);
        CombineQuads();
    }

    void Start() {
        CreateCube();

    }
}
