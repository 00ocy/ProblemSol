using UnityEngine;

public class EnemyFrustum : MonoBehaviour
{
    public Material material1; // �������� ���ο� �ִ� ������Ʈ�� ����� ��Ƽ����
    public Material material2; // �������� �ۿ� �ִ� ������Ʈ�� ����� ��Ƽ����
    public Material material3; // �������� �ۿ� �ִ� ������Ʈ�� ����� ��Ƽ����

    public Camera thisCamera;
    public GameObject player; // �÷��̾� ���� ������Ʈ

    public float followSpeed = 5f; // �÷��̾ ���󰡴� �ӵ�

    private void Update()
    {
        if (thisCamera == null)
        {
            Debug.LogError("ī�޶� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // ī�޶��� ���������� ��������
        FrustumPlanes frustum = new FrustumPlanes(thisCamera);

        // �������� ���� �ִ� ��� Material1 ����
        player.GetComponent<Renderer>().material = material1;

        if (frustum.IsInsideFrustum(player.GetComponent<Renderer>().bounds))
        {
            // �÷��̾�� �� ������ ��ֹ� �˻�
            RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position));
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform != player.transform && hit.transform != transform)
                {
                    // ��ֹ��� �÷��̾ ���� �ƴ� �ٸ� ������Ʈ�� ���� ó��
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * hit.distance, Color.red);
                    player.GetComponent<Renderer>().material = material2;
                    return;
                }
            }


            // �÷��̾ ���󰡴� ���� �߰�
            if (player != null)
            {
                // �÷��̾� �������� ���� �ӵ��� �̵�
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);

                // �÷��̾ �ٶ󺸵��� ȸ��
                transform.LookAt(player.transform);
            }
        }
        else
        {
            // �������� �ۿ� �ִ� ��� Material2 ����
            player.GetComponent<Renderer>().material = material3;
        }

        // ����� ���̸� �׸���
        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * Vector3.Distance(transform.position, player.transform.position), Color.green);
    }
}

// �������� �÷��� Ŭ����
public class FrustumPlanes
{
    private readonly Plane[] planes;

    public FrustumPlanes(Camera camera)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
    }

    // �Լ� ���ڸ� Bounds�� �����Ͽ� �ٿ�� �ڽ��� �������� ���ο� �ִ��� Ȯ��
    public bool IsInsideFrustum(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
