using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetBoy : MonoBehaviour
{
    public void RestartGame() {
        this.transform.GetComponent<PlayerData>().eraseData();
        this.transform.GetComponent<FishingManager>().eraseData();
        this.transform.GetComponent<PoleManager>().eraseData();


        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
