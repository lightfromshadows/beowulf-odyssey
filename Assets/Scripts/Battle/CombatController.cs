using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

    [SerializeField] CharacterStatsObj myStats;
    [SerializeField] GameObject messagePrefab;

    public void FlashMessage(string message, Color color)
    {
        StartCoroutine(FloatingMessageRoutine(message, color);
    }

    IEnumerator FloatingMessageRoutine(string message, Color color)
    {
        // TODO
        yield return null;
    }

    public void TakeDamage(float damage)
    {
        FlashMessage(string.Format("{0}!!", damage), Color.red);
        myStats.Health -= damage;
    }
}
