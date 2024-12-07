using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("UpdatedMainScene"); 
    }

    public void SetWinner(string winnerName)
    {
        GameObject.Find("WinnerText").GetComponent<UnityEngine.UI.Text>().text = "go" + winnerName + "! you are the winner";
    }
}
