using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GamaManager : MonoBehaviour
{
    
    
    public static GamaManager instance;
    [SerializeField] private List<Transform> playerSpawnPoints;
    [SerializeField] private List<AnimatorController> playerAnimators;
    
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
    }
    
    public void SpawnPlayer(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        playerInput.transform.position = playerSpawnPoints[playerIndex].position;
        playerInput.GetComponent<PlayerController>().spriteAnimator.runtimeAnimatorController = playerAnimators[playerIndex];
    }
    
    public void CheckGameOver(int looserInd)
    {
        if (looserInd == 2) 
        {
            FindObjectOfType<EndGameManager>().SetWinner("Shlomo");
            SceneManager.LoadScene("EndScene"); 
        }
        else if (looserInd == 1)
        {
            FindObjectOfType<EndGameManager>().SetWinner("Zippy");
            SceneManager.LoadScene("EndScene"); 
        }
    }

}
