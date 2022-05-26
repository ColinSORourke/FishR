using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequencer : MonoBehaviour
{
    public GameObject ZoneButton;
    public GameObject ShopButton;
    public GameObject CollectButton;
    public GameObject TutorialScreen;
    public GameObject poleButtonsA;
    public GameObject poleButtonsB;

    public Text tutorialText;

    public TutorialStep[] steps;
    public tutorialProgress t;

    public void Awake(){
        if (System.IO.File.Exists(Application.persistentDataPath + "/TutorialStep.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/TutorialStep.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log("Reading player save JSON");
            reader.Close();
            t = JsonUtility.FromJson<tutorialProgress>(JSON);
        } else { 
            t = new tutorialProgress(steps.Length);
            save();
        }
    }

    public void eraseData(){
        t = new tutorialProgress(steps.Length);
        save();
    }

    public void Start(){
        if (!t.completedSteps[0]){
            TutorialScreen.SetActive(true);
        }
        if (!t.completedSteps[2]){
            ShopButton.SetActive(false);
        }
        if (!t.completedSteps[3]){
            poleButtonsA.SetActive(false);
            poleButtonsB.SetActive(false);
        }
        if (!t.completedSteps[4]){
            CollectButton.SetActive(false);
            ZoneButton.SetActive(false);
        }
    }

    public void updateProgress(){
        if (t.completedSteps[2]){
            ShopButton.SetActive(true);
        }
        if (t.completedSteps[3]){
            poleButtonsA.SetActive(true);
            poleButtonsB.SetActive(true);
        }
        if (t.completedSteps[4]){
            CollectButton.SetActive(true);
            ZoneButton.SetActive(true);
        }
    }

    public void save(){
        string json = JsonUtility.ToJson(t);
        Debug.Log("Saving Tutorial Progress");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/TutorialStep.json", json);
    }

    public void checkStep(int i){
        if (!t.completedSteps[i]){
            TutorialScreen.SetActive(true);
            tutorialText.text = steps[i].description;
            t.completedSteps[i] = true;
            save();
        }
    }
}

public class tutorialProgress {
    public bool[] completedSteps;

    public tutorialProgress(int steps){
        completedSteps = new bool[steps];
        int i = 0;
        while (i < steps){
            completedSteps[i] = false;
            i += 1;
        }
    }
}
