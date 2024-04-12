using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    //��ư����� ����
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

//�޽������ ����
public class StaticMeshGen : MonoBehaviour
{
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // �Ʒ� ū������
            new Vector3 (0.0f, 0.0f, 1.1f),      // ��        0
            new Vector3 (1.1f, 0.0f, 0.33f),     // �� ���   1
            new Vector3 (0.55f, 0.0f, -0.88f),   // �� �ϴ�   2
            new Vector3 (-0.55f, 0.0f, -0.88f),  // �� �ϴ�   3
            new Vector3 (-1.1f, 0.0f, 0.33f),    // �� ���   4

            // �Ʒ� ����������
            new Vector3 (0.0f, 0.0f, -0.5f),     // �Ʒ�      5
            new Vector3 (-0.5f, 0.0f, -0.15f),   // �� �ϴ�   6
            new Vector3 (-0.25f, 0.0f, 0.4f),    // �� ���   7
            new Vector3 (0.25f, 0.0f, 0.4f),     // �� ���   8
            new Vector3 (0.5f, 0.0f, -0.15f),    // �� �ϴ�   9


            // �� ū������
            new Vector3 (0.0f, 2.0f, 1.1f),      // ��        10
            new Vector3 (1.1f, 2.0f, 0.33f),     // �� ���   11
            new Vector3 (0.55f, 2.0f, -0.88f),   // �� �ϴ�   12
            new Vector3 (-0.55f, 2.0f, -0.88f),  // �� �ϴ�   13
            new Vector3 (-1.1f, 2.0f, 0.33f),    // �� ���   14

            // �� ����������
            new Vector3 (0.0f, 2.0f, -0.5f),     // �Ʒ�      15
            new Vector3 (-0.5f, 2.0f, -0.15f),   // �� �ϴ�   16
            new Vector3 (-0.25f, 2.0f, 0.4f),    // �� ���   17
            new Vector3 (0.25f, 2.0f, 0.4f),     // �� ���   18
            new Vector3 (0.5f, 2.0f, -0.15f),    // �� �ϴ�   19
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



        // ������ �ﰢ�� �ε����� �޽ÿ� ����
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;

        // �ﰢ���� ������ ����Ͽ� ������ ������ ���ϰ�, ����ȭ
        Vector3[] normals = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int index0 = mesh.triangles[i];
            int index1 = mesh.triangles[i + 1];
            int index2 = mesh.triangles[i + 2];

            Vector3 side1 = mesh.vertices[index1] - mesh.vertices[index0];
            Vector3 side2 = mesh.vertices[index2] - mesh.vertices[index0];

            Vector3 triangleNormal = Vector3.Cross(side1, side2).normalized;

            normals[index0] += triangleNormal;
            normals[index1] += triangleNormal;
            normals[index2] += triangleNormal;
        }

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        // �޽ÿ� ���� ����
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