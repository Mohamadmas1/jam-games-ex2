using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    public GameObject tzipiWinScreen;
    public GameObject shlomoWinScreen; 

    public void SetWinner(string winner)
    {
        tzipiWinScreen.SetActive(false);
        shlomoWinScreen.SetActive(false);

        if (winner == "Zippy")
        {
            tzipiWinScreen.SetActive(true);
        }
        else if (winner == "Shlomo")
        {
            shlomoWinScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene"); 
    }
}