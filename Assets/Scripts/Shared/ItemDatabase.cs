using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject {
    public List<BuffItem> database = new List<BuffItem>();
}