using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Credit", menuName = "ScriptableObjects/Credit", order = 1)]
public class Credit : ScriptableObject
{
    public string fullName;
    public string blurb;
    [TextArea]
    public string roles;
    [TextArea]
    public string socialA;
    [TextArea]
    public string socialB;

    public Sprite pic;
}
