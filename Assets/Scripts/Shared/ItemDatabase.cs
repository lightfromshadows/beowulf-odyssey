using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject {
    public List<BuffItem> database = new List<BuffItem>();
}

[System.Serializable]
public struct BuffItem
{
    public string name;
    [TextArea]
    public string description;
    public Sprite sprite;
    public bool passive;
    public bool consumable;

    public float powerUp;
    public float hitUp;
    public float healthUp;
}