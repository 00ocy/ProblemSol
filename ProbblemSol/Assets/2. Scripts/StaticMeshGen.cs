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
            /*// �Ʒ� ū������
            0, 1, 2,  // �� ���� ���ϴ�
            0, 2, 3,  // �� ���ϴ� ���ϴ�
            0, 3, 4,  // �� ���ϴ� �»��*/

            // �Ʒ� ����������
            5, 7, 6,  // �Ʒ� �»�� ���ϴ� 
            5, 8, 7,  // �Ʒ� ���� �»�� 
            5, 9, 8,  // �Ʒ� ���ϴ� ���� 
        
            // �Ʒ� ��
            7, 8, 0,  // ���»�� �ۿ��� ū�� 
            8, 9, 1,  // �ۿ��� �ۿ��ϴ� ū���� 
            9, 5, 2,  // �ۿ��ϴ� �۾Ʒ� ū���ϴ� 
            5, 6, 3,  // �۾Ʒ� �����ϴ� ū���ϴ� 
            6, 7, 4,  // �����ϴ� ���»�� ū�»�� 
       

            /*// �� ū������
            10, 11, 12,  // �� ���� ���ϴ�
            10, 12, 13,  // �� ���ϴ� ���ϴ�
            10, 13, 14,  // �� ���ϴ� �»��*/

            // �� ����������
            15, 16, 17,  // �Ʒ� ���ϴ� �»��
            15, 17, 18,  // �Ʒ� �»�� ����
            15, 18, 19,  // �Ʒ� ���� ���ϴ�
        
            // �� ��
            17, 10, 18,  // ���»�� ū�� �ۿ���
            18, 11, 19,  // �ۿ��� ū���� �ۿ��ϴ�
            19, 12, 15,  // �ۿ��ϴ� ū���ϴ� �۾Ʒ�
            15, 13, 16,  // �۾Ʒ� ū���ϴ� �����ϴ�
            16, 14, 17,  // �����ϴ� ū�»�� ���»��
            
            // ��� 
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