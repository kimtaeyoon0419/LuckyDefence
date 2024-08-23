// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap unitTilemap; // ≈∏¿œ∏  ¬¸¡∂

    [Header("Grid Settings")]
    [SerializeField] private int horizontalSize; // ∞°∑Œ ≈©±‚
    [SerializeField] private int verticalSize;   // ºº∑Œ ≈©±‚
    [SerializeField] private GameObject SlotObject; // ∫Û ø¿∫Í¡ß∆Æ «¡∏Æ∆’
    [SerializeField] private List<UnitSlot> unitSlots;

    [Header("NullSlot")]
    List<UnitSlot> unitNullSlot = new List<UnitSlot>();

    private void Start()
    {
        GenerateEmptyObjects();
    }

    private void GenerateEmptyObjects()
    {
        for (int y = 0; y < verticalSize; y++)
        {
            for (int x = 0; x < horizontalSize; x++)
            {
                // ≈∏¿œ∏ ¿« ºø ¡¬«• º≥¡§
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // ºø ¡¬«•∏¶ ø˘µÂ ¡¬«•∑Œ ∫Ø»Ø
                Vector3 worldPosition = unitTilemap.CellToWorld(cellPosition);

                // ∫Û ø¿∫Í¡ß∆Æ ª˝º∫
                unitSlots.Add(Instantiate(SlotObject, worldPosition, Quaternion.identity).GetComponent<UnitSlot>());

                foreach (UnitSlot slot in unitSlots)
                {
                    slot.transform.SetParent(transform);
                }
            }
        }
    }

    public void SpawnUnit()
    {
        foreach(UnitSlot slot in unitSlots)
        {
            if(slot.currentUnit == null)
            {
                unitNullSlot.Add(slot);
            }
        }

        int unitIndex = Random.Range(0, UnitManager.Instance.units.Count);
        int slotIndex = Random.Range(0, unitNullSlot.Count);

        unitNullSlot[slotIndex].currentUnit = Instantiate(UnitManager.Instance.units[unitIndex], unitNullSlot[slotIndex].transform.position, Quaternion.identity);

        unitNullSlot.Clear();
    }
}
