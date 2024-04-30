using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenetator: MonoBehaviour
{
    [SerializeField] int Width;
    [SerializeField] int Height;
    public GameObject tilePrefab; // Ÿ�� ������
    public Transform parentTransform; // Ÿ�ϵ��� ���� �θ� Ʈ������
    public Transform parentBottomLeft;
    public GameObject lowWallPrefab;
    public GameObject highWallPrefab;

    private int[,] mapData; // �� ������ �迭
    void Start()
    {
        
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "map.txt");
        LoadMapDataFromCSV(filePath);
        GenerateMap();
    }
    public void LoadMapDataFromCSV(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            Width = lines[0].Split(',').Length;
            Height = lines.Length;
            mapData = new int[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                string[] entries = lines[y].Split(',');
                for (int x = 0; x < Width; x++)
                {
                    mapData[x, y] = int.Parse(entries[x]);
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    // ���� ����
    public void GenerateMap()
    {
        if (mapData == null)
        {
            Debug.LogError("Map data is not loaded.");
            return;
        }

        // ������ ������ Ÿ�ϵ� ����
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }

        // �� �����Ϳ� ���� Ÿ�� ����
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                //Vector3 BottomLeft = parentTransform.position - new Vector3(parentTransform.localScale.x / 2f, 0f, parentTransform.localScale.z / 2f);
                //parentBottomLeft.position = BottomLeft;
                Vector3 parentScale = parentTransform.localScale; // �θ��� ������ ��������
                // Ÿ���� ���� ����
                switch (mapData[x, y])
                {
                    case 0:
                        // Ÿ���� ����
                        break;
                    case 1:
                        // ���� ��
                        // �� ������Ʈ�� �����Ͽ� Ÿ���� �ڽ����� �߰�
                        Vector3 L_tilePosition = new Vector3((x* parentScale.x)- (4.5f* parentTransform.localScale.x), 0.25f*parentScale.y, (y* parentScale.z)-(4.5f* parentTransform.localScale.z)); // Ÿ���� ��ġ ���
                        GameObject L_tile = Instantiate(tilePrefab, L_tilePosition, Quaternion.identity, parentTransform);
                        GameObject lowWall = Instantiate(lowWallPrefab, L_tilePosition, Quaternion.identity, L_tile.transform);
                        break;
                    case 2:
                        // ���� ��
                        // ���� �� ������Ʈ�� �����Ͽ� Ÿ���� �ڽ����� �߰�
                        Vector3 H_tilePosition = new Vector3((x* parentScale.x)- (4.5f* parentTransform.localScale.x), 0.5f*parentScale.y, (y* parentScale.z)-(4.5f* parentTransform.localScale.z)); // Ÿ���� ��ġ ���
                        GameObject H_tile = Instantiate(tilePrefab, H_tilePosition, Quaternion.identity, parentTransform);
                        GameObject highWall = Instantiate(highWallPrefab, H_tilePosition, Quaternion.identity, H_tile.transform);
                        break;
                }

            }
        }
    }

    // �� ������ �ʱ�ȭ
    public void ClearMapData()
    {
        mapData = new int[Width, Height];
    }

    // �� �����Ϳ��� Ư�� ��ġ�� �� ����
    public void SetTileType(int x, int y, int type)
    {
        mapData[x, y] = type;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
