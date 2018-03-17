using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CombatAnimator : MonoBehaviour {

    [System.Serializable]
    public struct CombatAnimation {
        public string name;
        public string animTrigger;
        public Transform jumpPoint;
        public float jumpTime;
        public float jumpPower;
        public AudioClip audioClip;
    }

    [SerializeField] CombatAnimation[] animations;
    [SerializeField] public Animator animator;
    [SerializeField] AudioSource audioSource;

    public void DoCombatAnimation(string name)
    {
        DoCombatAnimation(name, null);
    }

    public void DoCombatAnimation(string name, TweenCallback callback)
    {
        var anim = from a in animations where a.name == name select a;
        if (anim.Count() > 0)
            DoCombatAnimation(anim.First(), callback);
    }

    public void DoCombatAnimation(CombatAnimation anim, TweenCallback callback = null)
    {
        if (anim.jumpPoint) {
            var t = transform.DOJump(anim.jumpPoint.position, anim.jumpPower, 1, anim.jumpTime);

            if (audioSource && anim.audioClip)
                audioSource.PlayOneShot(anim.audioClip);
            if (animator)
                animator.SetTrigger(anim.animTrigger);
            if (callback != null)
                 t.onComplete += callback;
            
        }
    }
}
