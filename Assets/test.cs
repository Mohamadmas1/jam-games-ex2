using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class test : MonoBehaviour
{
    public int PlayerIndex;
    public void GetInupt(CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log(PlayerIndex + " " + input);
    }
}