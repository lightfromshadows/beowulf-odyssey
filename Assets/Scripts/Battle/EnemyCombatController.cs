using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : CombatController
{

    [SerializeField] PlayerCombatController playerController;
    [SerializeField] CombatAnimator combatAnimator;

    public void TakeTurn()
    {
        float power = myStats.Power;
        float toHit = myStats.HitChance;

        combatAnimator.DoCombatAnimation("attack", () =>
        {
            playerController.Attack(power, toHit);
            combatAnimator.DoCombatAnimation("return", EndTurn);
        });
    }

    void EndTurn()
    {
        playerController.TakeTurn();
    }
}
