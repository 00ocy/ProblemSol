using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenetator: MonoBehaviour
{
    [SerializeField] int Width;
    [SerializeField] int Height;
    public GameObject tilePrefab; // 타일 프리팹
    public Transform parentTransform; // 타일들을 담을 부모 트랜스폼
    public Transform parentBottomLeft;
    public GameObject lowWallPrefab;
    public GameObject highWallPrefab;

    private int[,] mapData; // 맵 데이터 배열
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

    // 맵을 생성
    public void GenerateMap()
    {
        if (mapData == null)
        {
            Debug.LogError("Map data is not loaded.");
            return;
        }

        // 기존에 생성된 타일들 삭제
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }

        // 맵 데이터에 따라 타일 생성
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                //Vector3 BottomLeft = parentTransform.position - new Vector3(parentTransform.localScale.x / 2f, 0f, parentTransform.localScale.z / 2f);
                //parentBottomLeft.position = BottomLeft;
                Vector3 parentScale = parentTransform.localScale; // 부모의 스케일 가져오기
                // 타일의 종류 설정
                switch (mapData[x, y])
                {
                    case 0:
                        // 타일이 없음
                        break;
                    case 1:
                        // 낮은 벽
                        // 벽 오브젝트를 생성하여 타일의 자식으로 추가
                        Vector3 L_tilePosition = new Vector3((x* parentScale.x)- (4.5f* parentTransform.localScale.x), 0.25f*parentScale.y, (y* parentScale.z)-(4.5f* parentTransform.localScale.z)); // 타일의 위치 계산
                        GameObject L_tile = Instantiate(tilePrefab, L_tilePosition, Quaternion.identity, parentTransform);
                        GameObject lowWall = Instantiate(lowWallPrefab, L_tilePosition, Quaternion.identity, L_tile.transform);
                        break;
                    case 2:
                        // 높은 벽
                        // 높은 벽 오브젝트를 생성하여 타일의 자식으로 추가
                        Vector3 H_tilePosition = new Vector3((x* parentScale.x)- (4.5f* parentTransform.localScale.x), 0.5f*parentScale.y, (y* parentScale.z)-(4.5f* parentTransform.localScale.z)); // 타일의 위치 계산
                        GameObject H_tile = Instantiate(tilePrefab, H_tilePosition, Quaternion.identity, parentTransform);
                        GameObject highWall = Instantiate(highWallPrefab, H_tilePosition, Quaternion.identity, H_tile.transform);
                        break;
                }

            }
        }
    }

    // 맵 데이터 초기화
    public void ClearMapData()
    {
        mapData = new int[Width, Height];
    }

    // 맵 데이터에서 특정 위치의 값 설정
    public void SetTileType(int x, int y, int type)
    {
        mapData[x, y] = type;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
