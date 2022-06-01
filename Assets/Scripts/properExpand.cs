using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class properExpand : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float defaultAnchor;

    public void Start(){
        Invoke("correctSize", 0.05f);
    }

    public void correctSize(){
        if (!scrollbar.IsActive()){
            this.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        } else {
            this.GetComponent<RectTransform>().offsetMin = new Vector2(defaultAnchor, 0.0f);
        }
    }
}
