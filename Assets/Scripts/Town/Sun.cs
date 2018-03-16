using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    private float dayTimeTween = 0;

    private const int SUN_MIN_HEIGHT = 650;
    private const int SUN_RISE_HEIGHT = 340;

    public float DayTimeTween
    {
        get
        {
            return dayTimeTween;
        }

        set
        {
            dayTimeTween = value;

            Vector2 pos = new Vector2();
            pos.x = 100 + (1 - dayTimeTween) * 1720;

            dayTimeTween = 1 - dayTimeTween * 2;
 
            pos.y = Mathf.Sqrt(1 - dayTimeTween * dayTimeTween);
            pos.y *= SUN_RISE_HEIGHT;
            pos.y += SUN_MIN_HEIGHT;
            transform.position = pos;
 
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
