using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamaManager : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawnPoints;
    [SerializeField] private List<AnimatorController> playerAnimators;
    public void SpawnPlayer(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        playerInput.transform.position = playerSpawnPoints[playerIndex].position;
        playerInput.GetComponent<PlayerController>().spriteAnimator.runtimeAnimatorController = playerAnimators[playerIndex];
    }
}
