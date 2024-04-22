using UnityEngine;

public class ActivateObjectOnCamera : MonoBehaviour
{
    public Camera mainCamera; // ī�޶� ����Ű�� ����
    public GameObject objectToActivate; // Ȱ��ȭ�� ���� ������Ʈ

    void Start()
    {
        // ��ü�� ���������� ����
        objectToActivate.SetActive(false);
    }

    void Update()
    {
        // ī�޶��� �þ߿� ��ü�� �ִ��� Ȯ��
        if (IsObjectInCameraView())
        {
            // ��ü�� Ȱ��ȭ
            objectToActivate.SetActive(true);
        }
    }

    bool IsObjectInCameraView()
    {
        // ��ü�� ��ġ�� ī�޶� �þ߿����� ��ġ�� ��ȯ
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(objectToActivate.transform.position);

        // ��ȯ�� ��ġ�� ī�޶� �þ� ���� �ִ��� Ȯ��
        return (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && viewportPoint.z > 0);
    }
}
