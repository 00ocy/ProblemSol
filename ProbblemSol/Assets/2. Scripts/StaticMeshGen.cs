using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    //버튼만들기 예제
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }

    }
}

//메쉬만들기 예제
public class StaticMeshGen : MonoBehaviour
{
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // 아래 큰오각형
            new Vector3 (0.0f, 0.0f, 1.1f),      // 위        0
            new Vector3 (1.1f, 0.0f, 0.33f),     // 우 상단   1
            new Vector3 (0.55f, 0.0f, -0.88f),   // 우 하단   2
            new Vector3 (-0.55f, 0.0f, -0.88f),  // 좌 하단   3
            new Vector3 (-1.1f, 0.0f, 0.33f),    // 좌 상단   4

            // 아래 작은오각형
            new Vector3 (0.0f, 0.0f, -0.5f),     // 아래      5
            new Vector3 (-0.5f, 0.0f, -0.15f),   // 좌 하단   6
            new Vector3 (-0.25f, 0.0f, 0.4f),    // 좌 상단   7
            new Vector3 (0.25f, 0.0f, 0.4f),     // 우 상단   8
            new Vector3 (0.5f, 0.0f, -0.15f),    // 우 하단   9


            // 위 큰오각형
            new Vector3 (0.0f, 2.0f, 1.1f),      // 위        10
            new Vector3 (1.1f, 2.0f, 0.33f),     // 우 상단   11
            new Vector3 (0.55f, 2.0f, -0.88f),   // 우 하단   12
            new Vector3 (-0.55f, 2.0f, -0.88f),  // 좌 하단   13
            new Vector3 (-1.1f, 2.0f, 0.33f),    // 좌 상단   14

            // 위 작은오각형
            new Vector3 (0.0f, 2.0f, -0.5f),     // 아래      15
            new Vector3 (-0.5f, 2.0f, -0.15f),   // 좌 하단   16
            new Vector3 (-0.25f, 2.0f, 0.4f),    // 좌 상단   17
            new Vector3 (0.25f, 2.0f, 0.4f),     // 우 상단   18
            new Vector3 (0.5f, 2.0f, -0.15f),    // 우 하단   19
        };

        mesh.vertices = vertices;

        int[] triangleIndices = new int[]
        {
            /*// 아래 큰오각형
            0, 1, 2,  // 위 우상단 우하단
            0, 2, 3,  // 위 우하단 좌하단
            0, 3, 4,  // 위 좌하단 좌상단*/

            // 아래 작은오각형
            5, 7, 6,  // 아래 좌상단 좌하단 
            5, 8, 7,  // 아래 우상단 좌상단 
            5, 9, 8,  // 아래 우하단 우상단 
        
            // 아래 별
            7, 8, 0,  // 작좌상단 작우상단 큰위 
            8, 9, 1,  // 작우상단 작우하단 큰우상단 
            9, 5, 2,  // 작우하단 작아래 큰우하단 
            5, 6, 3,  // 작아래 작좌하단 큰좌하단 
            6, 7, 4,  // 작좌하단 작좌상단 큰좌상단 
       

            /*// 위 큰오각형
            10, 11, 12,  // 위 우상단 우하단
            10, 12, 13,  // 위 우하단 좌하단
            10, 13, 14,  // 위 좌하단 좌상단*/

            // 위 작은오각형
            15, 16, 17,  // 아래 좌하단 좌상단
            15, 17, 18,  // 아래 좌상단 우상단
            15, 18, 19,  // 아래 우상단 우하단
        
            // 위 별
            17, 10, 18,  // 작좌상단 큰위 작우상단
            18, 11, 19,  // 작우상단 큰우상단 작우하단
            19, 12, 15,  // 작우하단 큰우하단 작아래
            15, 13, 16,  // 작아래 큰좌하단 작좌하단
            16, 14, 17,  // 작좌하단 큰좌상단 작좌상단
            
            // 위 왼쪽 기둥 
            10, 17, 7,   // 위쪽큰위 위쪽작좌상단 아래쪽작좌상단 
            10, 7, 0,    // 위쪽큰위 아래쪽작좌상단 아래쪽큰위

            // 위 오른쪽 기둥
            18, 10, 0,   // 위쪽작은우상단 위쪽큰위 아래쪽큰위
            18, 0, 8,    // 위쪽작은우상단 아래쪽큰위 아래쪽작은우상단

            // 우상단 왼쪽 기둥
            11, 18, 8,   // 위쪽큰우상단 위쪽작은우상단 아래쪽작은우상단
            11, 8, 1,    // 위쪽큰우상단 아래쪽작은우상단 아래쪽큰우상단

            // 우상단 오른쪽 기둥
            19, 11, 1,   // 위쪽작은우하단 위쪽큰우상단 아래쪽작은우상단
            19, 1, 9,    // 위쪽작은우하단 아래쪽작은우상단 위쪽작은우하단

            // 우하단 왼쪽 기둥
            12, 19, 9,   // 위쪽큰우하단 위쪽작은우하단 아래쪽작은우하단
            12, 9, 2,    // 위쪽큰우하단 아래쪽작은우하단 아래쪽큰우하단 

            15, 12, 2,   // 위쪽작은아래 위쪽큰우하단 아래쪽큰우하단
            15, 2, 5,    // 위쪽작은아래 아래쪽큰우하단 아래쪽작은아래

            13, 15, 5,   // 위쪽큰좌하단 위쪽작은아래 아래쪽작은아래
            13, 5, 3,    // 위쪽큰좌하단 아래쪽작은아래 아래쪽큰좌하단

            16, 13, 3,   // 위쪽작은좌하단 위쪽큰좌하단 아래쪽큰좌하단
            16, 3, 6,    // 위쪽작은좌하단 아래쪽큰좌하단 아래쪽작은좌하단

            14, 16, 6,   // 위쪽큰좌상단 위쪽작은좌하단 아래쪽작은좌하단
            14, 6, 4,    // 위쪽큰좌상단 아래쪽작은좌하단 아래쪽큰좌상단

            17, 14, 4,   // 위쪽작은좌상단 위쪽큰좌상단 아래쪽큰좌상단
            17, 4, 7,    // 위쪽작은좌상단 아래쪽큰좌상단 아래쪽작은좌상단

                
        };

        mesh.triangles = triangleIndices;

        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();

        mf.mesh = mesh;
    }



}   