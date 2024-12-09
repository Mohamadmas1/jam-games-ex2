using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public GameObject zippyWinImage;  
    public GameObject shlomoWinImage; 

    public void SetWinner(string winner)
    {
        zippyWinImage.SetActive(false);
        shlomoWinImage.SetActive(false);

        if (winner == "Zippy")
        {
            zippyWinImage.SetActive(true);
        }
        else if (winner == "Shlomo")
        {
            shlomoWinImage.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene"); 
    }
}
