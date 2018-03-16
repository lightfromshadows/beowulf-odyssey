using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public TownManager town;

    //[SerializeField] public BuffItem buff;
    [SerializeField] public TownsPerson person;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        town.HouseClicked(this);
    }
}
