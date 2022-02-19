using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish", order = 1)]
public class FishObj : ScriptableObject
{
    public string fishName;
    public string description;
    public string hint;
    public string condition;

    public Sprite icon;

    public int basePay;
}
