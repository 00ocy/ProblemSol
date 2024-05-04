using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera Camera;
    public Transform target; // �ٶ� ��� ������Ʈ
    public float moveSpeed = 5f; // �̵� �ӵ�



    public Transform objectToRotate; // ȸ����ų ������Ʈ

    public float rotationSpeed = 90f;
    public float rotationDuration = 1f; // ���濡 �ɸ��� �ð� (��)

    private bool isRotating = false;

    void Start()
    {

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


        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartCoroutine(RotateObject(1));
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(RotateObject(-1));
            }
        }
    }


    IEnumerator RotateObject(int direction)
    {
        isRotating = true;
        float elapsedTime = 0;
        Quaternion startRotation = objectToRotate.rotation;
        Quaternion targetRotation = Quaternion.Euler(objectToRotate.eulerAngles + new Vector3(0, direction * rotationSpeed, 0));

        while (elapsedTime < rotationDuration)
        {
            objectToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, (elapsedTime / rotationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectToRotate.rotation = targetRotation;
        isRotating = false;
    }

}
