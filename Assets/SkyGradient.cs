using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGradient : MonoBehaviour {

    public Gradient gradient;

    public void SetSkyColor(float percent)
    {
        GetComponent<SpriteRenderer>().color = gradient.Evaluate(percent);
    }
    
}
