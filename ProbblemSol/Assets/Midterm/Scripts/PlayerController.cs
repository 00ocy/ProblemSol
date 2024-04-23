using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera Camera;
    public Transform target; // �ٶ� ��� ������Ʈ
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float rotationTime = 1f; // ȸ�� �ð�
    private Vector3[] cameraPositions; // ī�޶� ��ġ��
    private int currentCameraIndex = 0; // ���� ī�޶� ��ġ �ε���
    private bool isCameraRotating = false; // ī�޶� ȸ�� ������ ����

    void Start()
    {
        // ī�޶� ��ġ�� �ʱ�ȭ
        cameraPositions = new Vector3[]
        {
            new Vector3(45f, 45f, 0f),
            new Vector3(45f, -45f, 0f),
            new Vector3(45f, -135f, 0f),
            new Vector3(45f, -225f, 0f)
        };

        // �ʱ� ī�޶� ��ġ ����
        SetCameraPosition(currentCameraIndex);
    }

    void Update()
    {
        // �̵� �Է� �ޱ�
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // ī�޶��� ���� ���͸� �������� �̵� ���� ����
        Vector3 cameraForward = Camera.transform.forward;
        cameraForward.y = 0f; // ���� �̵��� ����
        Vector3 moveDirection = cameraForward.normalized * moveZ + Camera.transform.right * moveX;

        // �̵�
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            // �̵� �������� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // O Ű �Ǵ� P Ű�� ������ �� ī�޶� ȸ��
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isCameraRotating) // ī�޶� ȸ�� ���� �ƴ� ��쿡�� ȸ�� ����
            {
                int newIndex = currentCameraIndex;
                if (Input.GetKeyDown(KeyCode.P))
                    newIndex = (currentCameraIndex + 1) % cameraPositions.Length; // ���� ��ġ�� �̵�
                else
                    newIndex = (currentCameraIndex - 1 + cameraPositions.Length) % cameraPositions.Length; // ���� ��ġ�� �̵�

                // ī�޶� ��ġ ����
                SetCameraPosition(newIndex);
            }
        }
    }

    // ī�޶��� ��ġ�� �����ϴ� �Լ�
    void SetCameraPosition(int index)
    {
        isCameraRotating = true; // ī�޶� ȸ�� �� ���·� ����

        // ���� ��ġ���� ���� ��ġ�� ȸ��
        Quaternion startRotation = Camera.transform.rotation;
        // Vector3 startPosition = Camera.transform.position;
        Quaternion endRotation = Quaternion.Euler(cameraPositions[index]);
        // Vector3 endPosition = target.position - startRotation * Vector3.forward * 10f; // ��� ������Ʈ�� �ٶ󺸵��� ����

        // ȸ���� �̵� �ִϸ��̼� �ڷ�ƾ ����
        StartCoroutine(RotateCamera(startRotation, endRotation));

        // ���� ī�޶� ��ġ �ε��� ����
        currentCameraIndex = index;
    }

    // ī�޶��� ȸ�� �ִϸ��̼��� ó���ϴ� �ڷ�ƾ
    IEnumerator RotateCamera(Quaternion startRotation, Quaternion endRotation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < rotationTime)
        {
            // ȸ��
            Camera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȸ�� �ִϸ��̼� ���� �� ȸ�� ���� ����
        isCameraRotating = false;
    }
}
