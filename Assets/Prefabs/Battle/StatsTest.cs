using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTest : MonoBehaviour {

    [SerializeField]CharacterStatsObj playerStats;
    [SerializeField]CharacterStatsObj wolfStats;

    [SerializeField] BuffItem[] buffs;

	// Use this for initialization
	void Awake () {
        playerStats.Init();
        //foreach (var b in buffs)
        //{
        //    playerStats.AddBuff(b);
        //}
        wolfStats.Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            // Player Attack
            wolfStats.Health -= playerStats.Power;
            Debug.Log("Wolf HP " + wolfStats.Health);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            foreach(var b in buffs)
            {
                playerStats.AddBuff(b);
            }
        }
	}
}
