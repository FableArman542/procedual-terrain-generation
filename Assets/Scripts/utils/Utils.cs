using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    
    static float smooth = .01f;
    static int maxHeight = 40;
    static int octaves = 6;
    static float persistence = .7f;

    public static int GenerateHeight(float x, float z) {
        return (int) Map(0, maxHeight, 0, 1, fBM(x*smooth, z*smooth, octaves, persistence));
    }

    static float Map(float newMin, float newMax, float oriMin, float oriMax, float val) {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(oriMin, oriMax, val));
    }

    static float fBM (float x, float z, int octaves, float persistence) {
        
        float total = 0;
        float amplitude = 1;
        float frequency = 1;
        float maxValue = 0;
        for (int i = 0; i < octaves; ++i) {
            total += Mathf.PerlinNoise(x*frequency, z*frequency) * amplitude;
            amplitude *= persistence;
            frequency *= 2;
            maxValue += amplitude;
        }

        return total / maxValue;
    }

}
