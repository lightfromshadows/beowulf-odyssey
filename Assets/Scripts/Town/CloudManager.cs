using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

    public Sprite[] cloudSprites;
    Cloud cloudPreFab;
    float nextCloudSpawn;

	// Use this for initialization
	void Start () {
        cloudPreFab = Resources.Load<Cloud>("cloud");

        CreateCloud(150);
        CreateCloud(450);
        CreateCloud(780);
        CreateCloud(1000);
        CreateCloud(1400);
        nextCloudSpawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
        nextCloudSpawn -= Time.deltaTime;
        if(nextCloudSpawn < 0)
        {
            nextCloudSpawn = Random.Range(5, 10);
            CreateCloud(-300);

        }

    }

    void CreateCloud(float xPos)
    {
        Cloud cloud = Instantiate(cloudPreFab);
        cloud.SetXPos(xPos);
        int cloudSpriteIndex = Random.Range(0, cloudSprites.Length);
        cloud.GetComponent<SpriteRenderer>().sprite = cloudSprites[cloudSpriteIndex];
    }
}
