using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePanel : MonoBehaviour
{
    public void enableChildPanel(){
        this.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
    }
}
