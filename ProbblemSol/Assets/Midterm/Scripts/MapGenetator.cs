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
            Debug.LogWarning("File not found: " + filePath + ". Creating a new file with default map data.");

            // Create a new file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string defaultMapData =
                    "2,2,2,2,2,2,2,2,2,2\n" +
                    "2,0,0,0,0,0,0,0,2,2\n" +
                    "2,2,0,0,0,0,0,0,2,2\n" +
                    "2,0,0,2,2,2,2,0,2,2\n" +
                    "2,0,1,2,0,0,2,0,2,2\n" +
                    "2,0,0,0,0,0,0,0,2,2\n" +
                    "2,0,2,2,2,2,2,0,2,2\n" +
                    "2,0,0,0,2,0,0,0,2,2\n" +
                    "2,0,2,0,0,0,1,1,2,2\n" +
                    "2,2,2,2,2,2,2,2,2,2";
                writer.Write(defaultMapData);
            }

            // Reload map data
            LoadMapDataFromCSV(filePath);
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

  

        // �� �����Ϳ� ���� Ÿ�� ����
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                //Vector3 BottomLeft = parentTransform.position - new Vector3(parentTransform.localScale.x / 2f, 0f, parentTransform.localScale.z / 2f);
                //parentBottomLeft.position = BottomLeft;
                Vector3 parentScale = parentTransform.localScale; // �θ��� ������ ��������
                parentScale.x = parentScale.x / (Width / 10f);
                parentScale.z = parentScale.z / (Height / 10f);
                // Ÿ���� ���� ����
                switch (mapData[x, y])
                {
                    case 0:
                        // Ÿ���� ����
                        break;
                    case 1:
                        // ���� ��
                        // �� ������Ʈ�� �����Ͽ� Ÿ���� �ڽ����� �߰�
                        // ���� ���� Ÿ�� ��ġ ���
                        Vector3 L_tilePosition = new Vector3(
                            (x * parentScale.x) - (parentScale.x * (Width - 1) * 0.5f),
                            0.05f * parentScale.y,
                            (y * parentScale.z) - (parentScale.z * (Height - 1) * 0.5f)
                        );

                        GameObject L_tile = Instantiate(tilePrefab, L_tilePosition, Quaternion.identity, parentTransform);
                        GameObject lowWall = Instantiate(lowWallPrefab, L_tilePosition, Quaternion.identity, L_tile.transform);
                        lowWall.transform.localScale = new Vector3(AdjustToUnityScale(Width / 10f), 0.1f, AdjustToUnityScale(Height / 10f));
                        break;
                    case 2:
                        // ���� ��
                        // ���� �� ������Ʈ�� �����Ͽ� Ÿ���� �ڽ����� �߰�
                        // ���� ���� Ÿ�� ��ġ ���
                        Vector3 H_tilePosition = new Vector3(
                            (x * parentScale.x) - (parentScale.x * (Width - 1) * 0.5f),
                            0.25f * parentScale.y,
                            (y * parentScale.z) - (parentScale.z * (Height - 1) * 0.5f)
                        );

                        GameObject H_tile = Instantiate(tilePrefab, H_tilePosition, Quaternion.identity, parentTransform);
                        GameObject highWall = Instantiate(highWallPrefab, H_tilePosition, Quaternion.identity, H_tile.transform);
                        highWall.transform.localScale = new Vector3(AdjustToUnityScale(Width / 10f), 0.5f, AdjustToUnityScale(Height / 10f));
                        break;
                }

            }
        }
    }
    public float AdjustToUnityScale(float value)
    {
        if (value < 1f)
        {
            return 1f + (1f-value)+0.01f;
        }
        else
        {
            return 1f - (value - 1f)+0.01f;
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
