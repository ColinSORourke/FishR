using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChecker : MonoBehaviour
{
    public PlayerData playerData;

    void OnEnable()
    {
        this.gameObject.GetComponent<Image>().color = playerData.currZone.curtainColor;
    }
}
