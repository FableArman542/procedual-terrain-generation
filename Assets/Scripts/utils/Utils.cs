using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    
    public static float smooth = .002f;
    public static float smooth3D = 10 * smooth;
    public static int maxHeight = 150;
    public static int octaves = 6;
    public static float persistence = .7f;
    static float offset = 32000;

    public static int GeneratesStoneHeight(float x, float z) {
        return (int) Map(0, maxHeight - 5, 0, 1, fBM(x * 2 * smooth, z * 2 * smooth, octaves - 1, 1.2f*persistence));
    }
    public static int GenerateHeight(float x, float z) {
        return (int) Map(0, maxHeight, 0, 1, fBM(x*smooth, z*smooth, octaves, persistence));
    }

    static float Map(float newMin, float newMax, float oriMin, float oriMax, float val) {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(oriMin, oriMax, val));
    }

    public static float fBM3D (float x, float y, float z, int octaves, float persistence) {
        float xy = fBM(x * smooth3D, y * smooth3D, octaves, persistence);
        float yx = fBM(y * smooth3D, x * smooth3D, octaves, persistence);
        float xz = fBM(x * smooth3D, z * smooth3D, octaves, persistence);
        float zx = fBM(z * smooth3D, x * smooth3D, octaves, persistence);
        float yz = fBM(y * smooth3D, z * smooth3D, octaves, persistence);
        float zy = fBM(z * smooth3D, y * smooth3D, octaves, persistence);

        return ( xy + yx + xz + zx + yz + zy ) / 6;
    }

    public static float fBM (float x, float z, int octaves, float persistence) {
        
        float total = 0;
        float amplitude = 1;
        float frequency = 1;
        float maxValue = 0;
        for (int i = 0; i < octaves; ++i) {
            total += Mathf.PerlinNoise((x+offset)*frequency, (z+offset)*frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }

        return total / maxValue;
    }

}
