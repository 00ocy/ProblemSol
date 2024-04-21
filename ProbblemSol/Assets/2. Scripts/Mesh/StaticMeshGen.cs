using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

namespace OCY_Editor
{

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
           
          
                5, 7, 6, 
                5, 8, 7,   
                5, 9, 8,  
        
                7, 8, 0, 
                8, 9, 1,   
                9, 5, 2,  
                5, 6, 3, 
                6, 7, 4,  
       


                15, 16, 17,  
                15, 17, 18,  
                15, 18, 19,  
        
                17, 10, 18,  
                18, 11, 19,  
                19, 12, 15,  
                15, 13, 16,  
                16, 14, 17,  
            
            
                10, 17, 7,   
                10, 7, 0,    

            
                18, 10, 0,   
                18, 0, 8,    

            
                11, 18, 8,   
                11, 8, 1,    

            
                19, 11, 1,   
                19, 1, 9,    

            
                12, 19, 9,   
                12, 9, 2,    

                15, 12, 2,   
                15, 2, 5,    

                13, 15, 5,   
                13, 5, 3,    

                16, 13, 3,   
                16, 3, 6,    

                14, 16, 6,   
                14, 6, 4,    

                17, 14, 4,   
                17, 4, 7,    

                
            };



            // 정점과 삼각형 인덱스를 메시에 설정
            mesh.vertices = vertices;
            mesh.triangles = triangleIndices;

            // 메시의 정점 수 만큼 배열을 만듦
            Vector3[] normals = new Vector3[mesh.vertices.Length];

            // 정점 수 만큼 for문 돌림 삼각형이니까 3칸 건너뜀
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                // 인덱스 012에 3개의 정점 부여 
                int index0 = mesh.triangles[i];
                int index1 = mesh.triangles[i + 1];
                int index2 = mesh.triangles[i + 2];

                // 정점 1에서 정점 0을 빼서 변1 을 구함
                Vector3 side1 = mesh.vertices[index1] - mesh.vertices[index0];
                // 정점 2에서 정점 0을 빼서 변2 를 구함
                Vector3 side2 = mesh.vertices[index2] - mesh.vertices[index0];
                // 변1, 변2를 외적해서 정규화 == 법선 벡터
                Vector3 triangleNormal = Vector3.Cross(side1, side2).normalized;

                // 공유하는 법선 벡터들을 누적
                normals[index0] += triangleNormal;
                normals[index1] += triangleNormal;
                normals[index2] += triangleNormal;
            }

            // 누적된 법선 벡터들을 정규화 해서 대각선(자칭) 법선을 구함 그래야 조명에 따라 적합한 계산이 가능
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].normalized;
            }

            // 메시에 법선 설정
            mesh.normals = normals;

            MeshFilter mf = GetComponent<MeshFilter>();
            MeshRenderer mr = GetComponent<MeshRenderer>();

            if (mf == null)
                mf = gameObject.AddComponent<MeshFilter>();
            if (mr == null)
                mr = gameObject.AddComponent<MeshRenderer>();

            mf.mesh = mesh;
        }
    }
}