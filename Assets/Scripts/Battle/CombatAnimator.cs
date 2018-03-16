using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CombatAnimator : MonoBehaviour {

    [System.Serializable]
    public struct CombatAnimation {
        public string name;
        public AnimationClip clip;
        public Transform jumpPoint;
        public float jumpTime;
        public float jumpPower;
    }

    [SerializeField] CombatAnimation[] animations;

    public void DoCombatAnimation(string name, TweenCallback callback = null)
    {
        var anim = from a in animations where a.name == name select a;
        if (anim.Count() > 0)
            DoCombatAnimation(anim.First(), callback);
    }

    public void DoCombatAnimation(CombatAnimation anim, TweenCallback callback = null)
    {
        if (anim.jumpPoint)
            transform.DOJump(anim.jumpPoint.position, anim.jumpPower, 1, anim.jumpTime).onComplete += callback;
    }
}
