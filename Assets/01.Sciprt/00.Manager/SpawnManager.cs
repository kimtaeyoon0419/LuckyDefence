// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using DG.Tweening;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap unitTilemap; // Ÿ�ϸ� ����

    [Header("Grid Settings")]
    [SerializeField] private int horizontalSize; // ���� ũ��
    [SerializeField] private int verticalSize;   // ���� ũ��
    [SerializeField] private GameObject SlotObject; // �� ������Ʈ ������
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateEmptyObjects();
        SetSpawnUI();
    }

    private void GenerateEmptyObjects()
    {
        for (int y = 0; y < verticalSize; y++)
        {
            for (int x = 0; x < horizontalSize; x++)
            {
                // Ÿ�ϸ��� �� ��ǥ ����
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // �� ��ǥ�� ���� ��ǥ�� ��ȯ
                Vector3 worldPosition = unitTilemap.CellToWorld(cellPosition);

                // �� ������Ʈ ����
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

        // unitNullSlot�� ��� �ִ��� Ȯ��
        if (unitNullSlot.Count <= 0)
        {
            unitNullSlot.Clear(); // ��� ���� ���, ����Ʈ�� ����� �� ����
            return;
        }

        int unitIndex = Random.Range(0, UnitManager.Instance.units.Count);
        int slotIndex = Random.Range(0, unitNullSlot.Count);

        unitNullSlot[slotIndex].currentUnit = Instantiate(UnitManager.Instance.units[unitIndex].unitObject, unitNullSlot[slotIndex].transform.position, Quaternion.identity);

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
        spawnLevel_Text.text = "��ȯ���� : " + spawnLevel.ToString();
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
}
