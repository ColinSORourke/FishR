using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchButton : MonoBehaviour
{
    bool success = true;
    public PlayerData player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fail(){
        success = false;
        this.transform.GetChild(0).GetComponent<Text>().text = "Return";
    }

    public void onClick(){
        if (success){
            player.getMoney(10);
        } else {
            success = true;
            this.transform.GetChild(0).GetComponent<Text>().text = "Catch!";
        }
    }
}
