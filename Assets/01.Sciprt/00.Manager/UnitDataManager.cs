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

public class UnitDataManager : MonoBehaviour
{
    public static UnitDataManager Instance;

    [SerializeField] public List<UnitInfo> units;

    private void Awake()
    {
        Instance = this;
    }
}
