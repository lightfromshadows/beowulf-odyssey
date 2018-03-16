﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TownsPerson : ScriptableObject
{
    [TextArea]
    public string description;
    [SerializeField] public BuffItem buff;
    public string[] choice;
    public string[] responses;
    public int chooseWisely;
    public bool dead;
    public bool visited;
    
    public Sprite sprite;

}
