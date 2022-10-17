using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameChecker : MonoBehaviour
{
    public PlayerData playerData;

    void OnEnable()
    {
        this.transform.GetChild(0).GetComponent<Text>().text = playerData.currZone.zoneName;
    }
}
