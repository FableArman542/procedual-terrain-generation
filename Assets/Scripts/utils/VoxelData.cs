using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData {

    public static readonly Vector3[] vertices = new Vector3[8] {
        new Vector3(-0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, -0.5f),
    };

    public static readonly int[] triangles = new int[6] { 3, 1, 0, 3, 2, 1};

    public static Vector2[] uv = new Vector2[4] {
        new Vector2(1, 1),
        new Vector2(0, 1),
        new Vector2(0, 0),
        new Vector2(1, 0)
    };

    public static List<Vector3[]> getBySide(Cubeside side) {

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];

        switch (side) {
            case Cubeside.FRONT:
                vertices = new Vector3[] { VoxelData.vertices[4], VoxelData.vertices[5], VoxelData.vertices[1], VoxelData.vertices[0] };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { VoxelData.vertices[0], VoxelData.vertices[1], VoxelData.vertices[2], VoxelData.vertices[3] };
                normals = new Vector3[] {Vector3.down, Vector3.down , Vector3.down, Vector3.down};
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { VoxelData.vertices[7], VoxelData.vertices[6], VoxelData.vertices[5], VoxelData.vertices[4] };
                normals = new Vector3[] {Vector3.up, Vector3.up , Vector3.up, Vector3.up};
                break;
           case Cubeside.LEFT:
                vertices = new Vector3[] { VoxelData.vertices[7], VoxelData.vertices[4], VoxelData.vertices[0], VoxelData.vertices[3] };
                normals = new Vector3[] {Vector3.left, Vector3.left , Vector3.left, Vector3.left};
                break;

           case Cubeside.RIGHT:
                vertices = new Vector3[] { VoxelData.vertices[5], VoxelData.vertices[6], VoxelData.vertices[2], VoxelData.vertices[1] };
                normals = new Vector3[] {Vector3.right, Vector3.right , Vector3.right, Vector3.right};
                break;
          
            case Cubeside.BACK:
                vertices = new Vector3[] { VoxelData.vertices[6], VoxelData.vertices[7], VoxelData.vertices[3], VoxelData.vertices[2] };
                normals = new Vector3[] {Vector3.back, Vector3.back , Vector3.back, Vector3.back};
                break;
        }
        List<Vector3[]> a = new List<Vector3[]>();
        a.Add(vertices);
        a.Add(normals);
        return a;
    }

}