using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AlwaysSelectedButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        button.Select();
    }
}
