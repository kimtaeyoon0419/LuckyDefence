using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    [SerializeField] public List<GameObject> units;

    private void Awake()
    {
        Instance = this;
    }
}
