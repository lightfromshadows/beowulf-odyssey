using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

    [SerializeField] protected CharacterStatsObj myStats;
    [SerializeField] GameObject messagePrefab;

    public void FlashMessage(string message, Color color)
    {
        var go = Instantiate(messagePrefab, transform.position + (Vector3)(Random.insideUnitCircle * 100f), Quaternion.identity) as GameObject;
        go.GetComponent<FloatingMessageController>().Init(message, color);
    }


    public void Attack(float damage, float toHit)
    {
        FlashMessage(string.Format("{0}!!", damage), Color.red);
        myStats.Health -= damage;
    }
}
