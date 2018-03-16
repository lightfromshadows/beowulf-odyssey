using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

    [SerializeField] protected CharacterStatsObj myStats;
    [SerializeField] GameObject messagePrefab;

    public void FlashMessage(string message, Color color)
    {
        var go = Instantiate(messagePrefab) as GameObject;
        (go.transform as RectTransform).position = transform.position + (Vector3)(Random.insideUnitCircle * 60f);
        go.GetComponent<FloatingMessageController>().Init(message, color);
    }


    public void Attack(float damage, float toHit)
    {
        if (toHit > Random.Range(0f, 100f))
        {
			FlashMessage(string.Format("{0}!!", damage), Color.red);
			myStats.Health -= damage;         
        }
        else {
            FlashMessage("Miss!!!", Color.grey);
        }

    }
}
