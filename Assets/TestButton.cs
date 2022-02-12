using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public Text toChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeText(){
        toChange.GetComponent<Text>().text = "Button Pressed";
    }
}
