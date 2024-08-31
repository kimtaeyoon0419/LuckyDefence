// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.StartBGM("Stage");
    }
}
