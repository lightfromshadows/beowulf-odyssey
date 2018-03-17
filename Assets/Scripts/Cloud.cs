using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    private float speed;
    private float destroyAt = 3000;

	// Use this for initialization
	void Start () {
        speed = Random.Range(22, 60);
        float y = Random.Range(930, 1150);
        transform.position = new Vector2(transform.position.x, y);
	}
	
    public void SetXPos(float x)
    {
        Vector2 vec = new Vector2(x, transform.position.y);
        transform.position = vec;
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
