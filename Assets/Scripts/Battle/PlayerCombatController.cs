using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : CombatController {

    // Simple 1v1 Combat Controller Driven by event input

    [SerializeField] EnemyCombatController enemyController;
    [SerializeField] CombatAnimator combatAnimator;

    [SerializeField] BuffItem quickBuff;
    [SerializeField] BuffItem preciseBuff;
    [SerializeField] BuffItem heavyBuff;

    public void QuickAttack()
    {
        float damage = myStats.Power + quickBuff.powerUp;
        float toHit = myStats.HitChance + quickBuff.hitUp;
        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("single_attack", () =>
        {
            enemyController.Attack(damage, toHit);
            combatAnimator.DoCombatAnimation("single_attack", () =>
            {
                enemyController.Attack(damage, toHit);
                combatAnimator.DoCombatAnimation("return");
            });
        });
    }

    public void PreciseAttack()
    {
        float damage = myStats.Power + preciseBuff.powerUp;
        float toHit = myStats.HitChance + preciseBuff.hitUp;

        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("single_attack", () =>
        {
            enemyController.Attack(damage, toHit);
            combatAnimator.DoCombatAnimation("return");
        });
    }

    public void HeavyAttack()
    {
        float damage = myStats.Power + heavyBuff.powerUp;
        float toHit = myStats.HitChance + heavyBuff.hitUp;

        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("slow_attack", () =>
        {
            enemyController.Attack(damage, toHit);
            combatAnimator.DoCombatAnimation("return");
        });
    }

}
