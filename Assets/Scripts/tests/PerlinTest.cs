using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTest : MonoBehaviour {
    
    float tt1, tt2, tt3;

    public float inc1 = .01f;
    public float inc2 = .05f;
    public float inc3 = .01f;

    private void Update() {

        float hp1 = Mathf.PerlinNoise(tt1, 1);
        float hp2 = .25f * Mathf.PerlinNoise(tt2, 1);
        float hp3 = .125f * Mathf.PerlinNoise(tt3, 1);

        tt1 += inc1;
        tt2 += inc2;
        tt3 += inc3;
        
        Grapher.Log(hp1 + hp2 + hp3, "Perlin", Color.yellow);

    }

}
