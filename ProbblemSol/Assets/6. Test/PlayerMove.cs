using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player_Move : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private Camera mainCamera;


    public float moveSpeed = 10f;                     // �̵� �ӵ�
    public float rofSpeed = 15f;                      // ������ȯ �ӵ�
    private bool isMoving = false;                    // ������ ���θ� ��Ÿ���� ����


    private Vector3 dir = Vector3.zero;               // �̵� ���� ��꿡 �ʿ��� ���� (����� �Է�)
    private Vector3 cameraForward;                    // �̵� ���� ��꿡 �ʿ��� ���� (ī�޶��� ���� ����)

    [Header("Slope Handling")]
    public float maxSlopeAngle;                       // �ö� �� �ִ� ����� �ִ� ����
    private RaycastHit slopeHit;                      // ��� ���� �ִ��� Ȯ���� ����ĳ��Ʈ

    public float currenty;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

    }

    void Update()
    {
        MovementDetection();
        Animation();

    }

    // ĳ���� �̵� �Լ��� ���� ������Ʈ���� �ٷ�
    private void FixedUpdate()
    {
        cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;        // ī�޶��� ���� ���͸� y ���� ������ ������ ����ȭ
        Vector3 moveDirection = cameraForward * dir.z + mainCamera.transform.right * dir.x;                  // ī�޶��� ���� ���Ϳ� ������ ���͸� �̿��Ͽ� �̵� ���� ����

        if (OnSlope())
        {
            // ���� �������� �̵� ���� ���
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();
            // ������ ���� �÷��̾ �̵���Ŵ
            rb.velocity = slopeMoveDirection * moveSpeed;
        }
        else
        {
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
        }


        if (dir != Vector3.zero)
        {
            if (Mathf.Sign(dir.x) != Mathf.Sign(transform.position.x) || Mathf.Sign(dir.z) != Mathf.Sign(transform.position.z))
            {
                transform.Rotate(0, 1, 0);
            }
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rofSpeed * Time.deltaTime);
        }
        rb.useGravity = !OnSlope();
    }

    // ��� ������
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.7f))
        {
            Debug.DrawRay(transform.position, Vector3.down * 0.7f, Color.red);
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        Vector3 moveDirection = cameraForward * dir.z + mainCamera.transform.right * dir.x; // ī�޶��� ���� ���Ϳ� ������ ���͸� �̿��Ͽ� �̵� ���� ����
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    // ������ ���� �Լ�
    private void MovementDetection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        dir.x = Mathf.Abs(horizontalInput) > 0.1f ? Mathf.Sign(horizontalInput) : 0f;
        dir.z = Mathf.Abs(verticalInput) > 0.1f ? Mathf.Sign(verticalInput) : 0f;

        dir.Normalize();                                                                                     // ������ �����ϰ� 1�� ����ȭ
        isMoving = dir != Vector3.zero;                                                                      // ������ ���� ����
    }

    // �ִϸ��̼� ���� �Լ�
    private void Animation()
    {
        if (isMoving)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }

    }



}
