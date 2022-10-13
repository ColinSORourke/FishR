using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPanelManager : MonoBehaviour
{
    public void disableSelf(){
        this.gameObject.SetActive(false);
    }

    public void disableParent(){
        this.transform.parent.gameObject.SetActive(false);
    }
    
    public void enableChildPanel(){
        this.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
    }
}
