using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : CombatController {

    // Simple 1v1 Combat Controller Driven by event input

    [SerializeField] CharacterStatsObj playerStats;
    [SerializeField] EnemyCombatController enemyController;
    [SerializeField] CombatAnimator combatAnimator;

    public void QuickAttack()
    {
        float damage = playerStats.Power;
        // Quick and dirty lazy way
        combatAnimator.DoCombatAnimation("single_attack", () =>
        {
            enemyController.TakeDamage(damage);
            combatAnimator.DoCombatAnimation("single_attack", () =>
            {
                enemyController.TakeDamage(damage);
                combatAnimator.DoCombatAnimation("return");
            });
        });
    }

}
