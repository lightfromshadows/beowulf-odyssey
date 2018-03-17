using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : CombatController
{

    [SerializeField] PlayerCombatController playerController;
    [SerializeField] CombatAnimator combatAnimator;

    [SerializeField] GameObject gameOverPanel;

    public void TakeTurn()
    {
        float power = myStats.Power;
        float toHit = myStats.HitChance;

        if (myStats.Health <= 0f)
        {
            gameOverPanel.SetActive(true);
            return;
        }

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
