using UnityEngine;

public class ActivateObjectsInView : MonoBehaviour
{
    private Camera thisCamera;

    private void Start()
    {
        // �ش� ���� ������Ʈ�� ����� ī�޶� ������Ʈ ��������
        thisCamera = GetComponent<Camera>();
        if (thisCamera == null)
        {
            Debug.LogError("ī�޶� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void Update()
    {
        if (thisCamera == null) return;

        // ��� ���� �ִ� ��� GameObject ��������
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // ī�޶�� ���� ����
            if (obj.GetComponent<Camera>() != null || obj.GetComponent<Light>() != null)
                continue;

            // ������Ʈ�� ī�޶� �þ߿� �ִ��� Ȯ��
            if (IsObjectInView(obj))
            {
                // �þ� �ȿ� ������ Ȱ��ȭ
                obj.SetActive(true);
            }
            else
            {
                // �þ� �ۿ� ������ ��Ȱ��ȭ
                obj.SetActive(false);
            }
        }
    }

    // ������Ʈ�� ī�޶� �þ߿� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsObjectInView(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        // Renderer�� ���� ��� �þ߿� ���� �ʴٰ� ����
        if (renderer == null)
        {
            return false;
        }

        // Renderer�� �߽����� ī�޶� �þ� ���� �ִ��� Ȯ��
        return thisCamera.WorldToViewportPoint(renderer.bounds.center).z > 0;
    }
}
