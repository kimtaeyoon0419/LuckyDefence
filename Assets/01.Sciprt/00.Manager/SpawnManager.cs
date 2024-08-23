// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap unitTilemap; // 타일맵 참조

    [Header("Grid Settings")]
    [SerializeField] private int horizontalSize; // 가로 크기
    [SerializeField] private int verticalSize;   // 세로 크기
    [SerializeField] private GameObject SlotObject; // 빈 오브젝트 프리팹
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
                // 타일맵의 셀 좌표 설정
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // 셀 좌표를 월드 좌표로 변환
                Vector3 worldPosition = unitTilemap.CellToWorld(cellPosition);

                // 빈 오브젝트 생성
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
        if (unitSlots.Count <= 0)
        {
            return;
        }

        foreach (UnitSlot slot in unitSlots)
        {
            if (slot.currentUnit == null)
            {
                unitNullSlot.Add(slot);
            }
        }

        // unitNullSlot이 비어 있는지 확인
        if (unitNullSlot.Count <= 0)
        {
            unitNullSlot.Clear(); // 비어 있을 경우, 리스트를 비워준 후 리턴
            return;
        }

        int unitIndex = Random.Range(0, UnitManager.Instance.units.Count);
        int slotIndex = Random.Range(0, unitNullSlot.Count);

        unitNullSlot[slotIndex].currentUnit = Instantiate(UnitManager.Instance.units[unitIndex].unitObject, unitNullSlot[slotIndex].transform.position, Quaternion.identity);

        unitNullSlot.Clear();
    }

}
