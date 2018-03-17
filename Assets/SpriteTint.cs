using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTint : MonoBehaviour {

    public SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(float tint)
    {
        sr.color = Color.white * tint;
    }
}
