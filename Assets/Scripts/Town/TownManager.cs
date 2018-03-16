using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TownManager : MonoBehaviour {

    public Sun sun;
    private const float MAX_DAY_TIME = 10;
    private float dayTime = 0;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        dayTime += Time.deltaTime;

        if(dayTime >= MAX_DAY_TIME)
        {
            //Run end of day logic.
            dayTime = 0;
        }
        sun.DayTimeTween = dayTime / MAX_DAY_TIME;
    }
}
