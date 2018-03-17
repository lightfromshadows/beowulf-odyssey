using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

    [SerializeField] protected CharacterStatsObj myStats;
    [SerializeField] GameObject messagePrefab;

    [SerializeField] protected AudioSource audioSource;

    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip missSound;

    public void FlashMessage(string message, Color color)
    {
        var go = Instantiate(messagePrefab) as GameObject;
        (go.transform as RectTransform).position = transform.position + (Vector3)(Random.insideUnitCircle * 60f);
        go.GetComponent<FloatingMessageController>().Init(message, color);
    }


    public virtual void Attack(float damage, float toHit)
    {
        if (toHit > Random.Range(0f, 100f))
        {
            ChangeHealth(-damage);
            audioSource.PlayOneShot(hitSound);
        }
        else {
            FlashMessage("Miss!!!", Color.grey);
            audioSource.PlayOneShot(missSound);
        }
    }

    public virtual void ChangeHealth(float health)
    {
        if (health > 0f) {
            FlashMessage(string.Format("{0}!!", health), Color.green);
        }
        else {
            FlashMessage(string.Format("-{0}!!", health), Color.red);
        }

        myStats.Health += health;
    }

    public void ChangeHitChance(float hitUp)
    {
        if (hitUp > 0f)
        {
            FlashMessage(string.Format("Hit +{0}%", hitUp), Color.cyan);
        }
        else
        {
            FlashMessage(string.Format("Hit -{0}%", hitUp), Color.grey);
        }
    }
}
