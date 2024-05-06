using System.Collections;
using Unity.VisualScripting;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("end"))
        {
            // "Clear" �̸��� ���� ĵ���� ã��
            GameObject clearCanvas = GameObject.Find("Clear");
            // ���� ������Ʈ�� �����ϴ��� Ȯ���մϴ�.
            if (clearCanvas != null)
            {
                // ���� ������Ʈ�� ��� �����ͼ� Ȱ��ȭ�մϴ�.
                foreach (Transform child in clearCanvas.transform)
                {
                    child.gameObject.SetActive(true);
                    Time.timeScale = 0f;

                }
            }
            else
            {
                Debug.LogError("Target game object not found!");
            }
        }
        if (collision.gameObject.CompareTag("enemy"))
        {
            GameObject overCanvas = GameObject.Find("Over");
            // ���� ������Ʈ�� �����ϴ��� Ȯ���մϴ�.
            if (overCanvas != null)
            {
                // ���� ������Ʈ�� ��� �����ͼ� Ȱ��ȭ�մϴ�.
                foreach (Transform child in overCanvas.transform)
                {
                    child.gameObject.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
            else
            {
                Debug.LogError("Target game object not found!");
            }
        }
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
