using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int cash = 100;
    public Text cashText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getMoney(int money){
        cash += money;
        cashText.GetComponent<Text>().text = "Cash: " + cash;
    }
}
