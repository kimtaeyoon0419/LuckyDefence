using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap unitTilemap; // ≈∏¿œ∏  ¬¸¡∂

    [Header("Grid Settings")]
    [SerializeField] private int horizontalSize; // ∞°∑Œ ≈©±‚
    [SerializeField] private int verticalSize;   // ºº∑Œ ≈©±‚
    [SerializeField] private GameObject SlotObject; // ∫Û ø¿∫Í¡ß∆Æ «¡∏Æ∆’
    [SerializeField] private List<UnitSlot> unitSlots;

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
}
