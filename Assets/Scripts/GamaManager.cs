using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GamaManager : MonoBehaviour
{
    
    
    static public GamaManager instance;
    [SerializeField] private List<Transform> playerSpawnPoints;
    [SerializeField] private List<AnimatorController> playerAnimators;
    [SerializeField] private GameObject FloorScreen;
    [SerializeField] private GameObject shlomoWinScreen;
    [SerializeField] private GameObject tzipiWinScreen;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        FloorScreen.SetActive(true);
        shlomoWinScreen.SetActive(false);
        tzipiWinScreen.SetActive(false);
    }
    
    public void SpawnPlayer(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        playerInput.transform.position = playerSpawnPoints[playerIndex].position;
        playerInput.GetComponent<PlayerController>().spriteAnimator.runtimeAnimatorController = playerAnimators[playerIndex];
    }
    
    public void CheckGameOver(int looserInd)
    {
        if (looserInd == 0) 
        {
            FloorScreen.SetActive(false);
            shlomoWinScreen.SetActive(true);
            SceneManager.LoadScene("EndScene");
            
        }
        else if (looserInd == 1)
        {
            FloorScreen.SetActive(false);
            tzipiWinScreen.SetActive(true);
            SceneManager.LoadScene("EndScene");
        }
        //SceneManager.LoadScene("EndGameScene");

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
