using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    private float speed;
    private float destroyAt = 2200;

	// Use this for initialization
	void Start () {
        speed = Random.Range(22, 60);
	}
	
	// Update is called once per frame
	void Update () {
        float x = transform.position.x;
        x += Time.deltaTime * speed;
        transform.position = new Vector3(x, transform.position.y);

        if (transform.position.x > destroyAt)
        {
            Destroy(gameObject);
        }

    }
}
