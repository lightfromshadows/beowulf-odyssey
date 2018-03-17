using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCombatController : CombatController {

    // Simple 1v1 Combat Controller Driven by event input

    [SerializeField] EnemyCombatController enemyController;
    [SerializeField] CombatAnimator combatAnimator;

    [SerializeField] BuffItem quickBuff;
    [SerializeField] BuffItem preciseBuff;
    [SerializeField] BuffItem heavyBuff;

    bool myTurn = true;

    public void QuickAttack()
    {
        if (!myTurn) return;
        myTurn = false;

        float damage = myStats.Power + quickBuff.powerUp;
        float toHit = myStats.HitChance + quickBuff.hitUp;
        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("single_attack", () =>
        {
            enemyController.Attack(damage, toHit);
            combatAnimator.DoCombatAnimation("single_attack", () =>
            {
                enemyController.Attack(damage, toHit);
                combatAnimator.DoCombatAnimation("return", EndTurn);
            });
        });
    }

    public void PreciseAttack()
    {
        if (!myTurn) return;
        myTurn = false;

        float damage = myStats.Power + preciseBuff.powerUp;
        float toHit = myStats.HitChance + preciseBuff.hitUp;

        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("single_attack", () =>
        {
            enemyController.Attack(damage, toHit);
            combatAnimator.DoCombatAnimation("return", EndTurn);
        });
    }

    public void HeavyAttack()
    {
        if (!myTurn) return;
        myTurn = false;

        float damage = myStats.Power + heavyBuff.powerUp;
        float toHit = myStats.HitChance + heavyBuff.hitUp;

        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("hang", () =>
        {
            combatAnimator.DoCombatAnimation("drop_attack", () => {
                enemyController.Attack(damage, toHit);
                combatAnimator.DoCombatAnimation("return", EndTurn);
            });
        });
    }

    void EndTurn()
    {
        // TODO is wolf dead?

        enemyController.TakeTurn();
        myTurn = false;
    }

    public void TakeTurn()
    {
		var passives = myStats.PassiveBuffs;

        if (myStats.Health < float.Epsilon)
        {
			var faith = from b in passives where b.name == "Faith" select b;
            if (faith.Count() > 0)
            {
                // Very special case
                myStats.ConsumeItem(faith.First());
                ChangeHealth(myStats.MaxHealth);
            }
            else {
                // TODO You dead!
                Debug.Log("Player died :/");
            }
        }

        foreach (var buff in passives)
        {
            if (Mathf.Abs(buff.passiveHealth) > float.Epsilon)
            {
                if (buff.target == BuffItem.Target.Self)
                {
                    this.ChangeHealth(buff.passiveHealth);
                }
                else {
                    enemyController.ChangeHealth(buff.passiveHealth);
                }
            }
        }
		myTurn = true;
    }

    public override void ChangeHealth(float health)
    {
        base.ChangeHealth(health);

        if (health < 0f)
        {
            Debug.Log("Hurt");
            combatAnimator.DoCombatAnimation("hurt", () => {
                combatAnimator.DoCombatAnimation("return");   
            });
        }
    }

    public void UseItem(BuffItem item)
    {
        if (item.target == BuffItem.Target.Self) {
            if (item.consumable && item.healthUp > 0f) {
                ChangeHealth(item.healthUp);
            }
        }
        else {
            if (item.consumable && item.hitUp < 0f)
            {
                enemyController.ChangeHitChance(item.hitUp);
            }
        }

        myStats.ConsumeItem(item);
    }
}
