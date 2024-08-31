// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using DG.Tweening;
using LuckyDefence.Unit;

public class UnitSlotManager : MonoBehaviour
{
    public static UnitSlotManager Instance;

    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap unitTilemap; // 타일맵 참조

    [Header("Grid Settings")]
    [SerializeField] private int horizontalSize; // 가로 크기
    [SerializeField] private int verticalSize;   // 세로 크기
    [SerializeField] private GameObject SlotObject; // 빈 오브젝트 프리팹
    [SerializeField] private List<UnitSlot> unitSlots;

    [Header("NullSlot")]
    List<UnitSlot> unitNullSlot = new List<UnitSlot>();

    [Header("SpawnCost")]
    [SerializeField] private int havePoint;
    [SerializeField] private TextMeshProUGUI havePoint_Text;
    [SerializeField] private int spawnPoint;
    [SerializeField] private TextMeshProUGUI spawnPoint_Text;
    [SerializeField] private int spawnLevel;
    [SerializeField] private float currentSpawnLevelExp;
    [SerializeField] private float maxSpawnLevelExp;
    [SerializeField] private TextMeshProUGUI spawnLevel_Text;
    [SerializeField] private Slider spawnLevelExpBar;
    [SerializeField] private Ease ease;

    [Header("SelectSlot")]
    [SerializeField] private LayerMask slotLayer;
    [SerializeField] private UnitSlot startSlot;
    [SerializeField] private UnitSlot changeSlot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateEmptyObjects();
        SetSpawnUI();
    }

    private void Update()
    {
        ClickSelectSlot();
        EndDragSlot();
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
        if (unitSlots.Count <= 0 || havePoint < spawnPoint)
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

        int unitIndex = Random.Range(0, UnitDataManager.Instance.units.Count);
        int slotIndex = Random.Range(0, unitNullSlot.Count);

        unitNullSlot[slotIndex].currentUnit = Instantiate(UnitDataManager.Instance.units[unitIndex].unitObject, unitNullSlot[slotIndex].transform.position, Quaternion.identity);

        havePoint -= spawnPoint;
        currentSpawnLevelExp += 10;
        if(currentSpawnLevelExp >= maxSpawnLevelExp)
        {
            float leftoverExp = currentSpawnLevelExp - maxSpawnLevelExp;
            currentSpawnLevelExp = 0;
            currentSpawnLevelExp += leftoverExp;
            spawnLevel++;
        }

        SetSpawnUI();

        unitNullSlot.Clear();
    }

    private void SetSpawnUI()
    {
        spawnLevel_Text.text = "소환레벨 : " + spawnLevel.ToString();
        havePoint_Text.text = "Sp : " + havePoint.ToString();
        spawnPoint_Text.text = "SpCost : " + spawnPoint.ToString();
        spawnLevelExpBar.value = currentSpawnLevelExp / maxSpawnLevelExp;
    }

    public void GetSp(int spValue)
    {
        havePoint += spValue;
        StartCoroutine(SpTextSizeAnim());
        SetSpawnUI();
    }

    IEnumerator SpTextSizeAnim()
    {
        havePoint_Text.transform.DOScale(1.2f, 0.1f).SetEase(ease);
        yield return new WaitForSeconds(0.1f);
        havePoint_Text.transform.DOScale(1, 0.1f).SetEase(ease);
    }

    private void ClickSelectSlot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, slotLayer);

            if (hit.collider != null)
            {
                UnitSlot slot = hit.collider.GetComponent<UnitSlot>();
                if (slot != null)
                {
                    startSlot = slot;
                    Debug.Log("슬롯 선택됨: " + slot.gameObject.name);
                }
            }
        }
    }


    private void EndDragSlot()
    {
        if (Input.GetMouseButtonUp(0) && startSlot != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, slotLayer);

            if (hit.collider != null)
            {
                UnitSlot endSlot = hit.collider.GetComponent<UnitSlot>();
                if (endSlot != null && endSlot != startSlot)
                {
                    SwapUnits(startSlot, endSlot);
                    Debug.Log("드래그 종료 슬롯 선택됨: " + endSlot.gameObject.name);
                }
            }

            startSlot = null; // 드래그 작업 초기화
        }
    }

    private void SwapUnits(UnitSlot slot1, UnitSlot slot2)
    {
        GameObject tempUnit = slot1.currentUnit;
        slot1.currentUnit = slot2.currentUnit;
        slot2.currentUnit = tempUnit;

        if (slot1.currentUnit != null)
        {
            slot1.currentUnit.GetComponent<Unit>().MoveSlot(slot1.transform);
        }

        if (slot2.currentUnit != null)
        {
            slot2.currentUnit.GetComponent<Unit>().MoveSlot(slot2.transform);
        }
    }

}
