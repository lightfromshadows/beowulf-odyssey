using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuffItem : ScriptableObject
{
    public enum Target {
        Self, Other
    }

    public Target target = Target.Self;

    [TextArea]
    public string description;
    public Sprite sprite;
    public bool passive;
    public bool consumable;

    public float powerUp;
    public float hitUp;
    public float healthUp;

    public float passiveHealth;

}
