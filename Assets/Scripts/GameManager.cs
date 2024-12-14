using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private const int NumPlayers = 2;
    
    static public GameManager instance;
    
    [Tooltip("The zoom level at the start of the game")]
    [SerializeField] private float startZoom = 1.7f;
    [Tooltip("The zoom level during the game")]
    [SerializeField] private float gameZoom = 16f;
    [Tooltip("The spawn points for the players at the start of the game")]
    [SerializeField] private List<Transform> playerStartSpawnPoints;
    [Tooltip("The spawn points for the players during the game")]
    [SerializeField] private List<Transform> playerGameSpawnPoints;
    [SerializeField] private List<GameObject> playersText;
    [SerializeField] private List<AnimatorController> playerAnimators;
    [SerializeField] private List<PlayerLifeUI> playerLives;
    [SerializeField] private GameObject FloorScreen;
    [SerializeField] private GameObject shlomoWinScreen;
    [SerializeField] private GameObject tzipiWinScreen;
    [SerializeField] private GameObject gameUICanvas;
    
    private List<PlayerController> playerControllers;
    private PlayerInputManager playerInputManager;
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

    private void Start()
    {
        gameUICanvas.SetActive(false);
        playerInputManager = GetComponent<PlayerInputManager>();
        playerControllers = new List<PlayerController>(NumPlayers);
        Camera.main.orthographicSize = startZoom;
    }

    public void SpawnPlayer(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        playerInput.transform.position = playerStartSpawnPoints[playerIndex].position;
        playerControllers.Add(playerInput.GetComponent<PlayerController>());
        playerControllers[playerIndex].DisableMovement();
        playerControllers[playerIndex].spriteAnimator.runtimeAnimatorController = playerAnimators[playerIndex];
        playersText[playerIndex].SetActive(false);
        playerLives[playerIndex].health = playerInput.GetComponent<Health>();
        
        if (playerControllers.Count == NumPlayers)
        {
            PrepareGame();
        }
        
    }

    private void PrepareGame()
    {
        gameUICanvas.SetActive(true);
        for (int idx = 0; idx < NumPlayers; idx++)
        {
            playerControllers[idx].EnableMovement();
            playerControllers[idx].transform.position = playerGameSpawnPoints[idx].position;
        }
        Camera.main.orthographicSize = gameZoom;
    }

    public void CheckGameOver(int looserInd)
    {
        if (looserInd == 0) 
            tzipiWinScreen.SetActive(true);
        else if (looserInd == 1) 
            shlomoWinScreen.SetActive(true);
        
        gameUICanvas.SetActive(false);
        FloorScreen.SetActive(false);
        playerInputManager.enabled = false;
        DestroyPlayers();

    }
    
    private void DestroyPlayers()
    {
        foreach (var player in playerControllers)
        {
            if (player != null)
                Destroy(player.gameObject);
        }
        playerControllers.Clear();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
