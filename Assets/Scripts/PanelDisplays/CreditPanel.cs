using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditPanel : MonoBehaviour
{
    public Image profile;
    public Text name;
    public Text blurb;
    public Text roles;
    public Text socialA;
    public Text socialB;

    public void displayCredit(Credit c){
        profile.sprite = c.pic;
        name.text = c.fullName;
        blurb.text = c.blurb;
        roles.text = c.roles;
        socialA.text = c.socialA;
        socialB.text = c.socialB;
    }
}
