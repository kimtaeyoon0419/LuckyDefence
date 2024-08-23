using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class UnitInfo
{
    public string unitName;
    public GameObject unitObject;
}

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    [SerializeField] public List<UnitInfo> units;

    private void Awake()
    {
        Instance = this;
    }
}
