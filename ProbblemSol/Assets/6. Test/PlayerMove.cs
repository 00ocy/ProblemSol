using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player_Move : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private Camera mainCamera;


    public float moveSpeed = 10f;                     // 이동 속도
    public float rofSpeed = 15f;                      // 방향전환 속도
    private bool isMoving = false;                    // 움직임 여부를 나타내는 변수


    private Vector3 dir = Vector3.zero;               // 이동 방향 계산에 필요한 벡터 (사용자 입력)
    private Vector3 cameraForward;                    // 이동 방향 계산에 필요한 벡터 (카메라의 전방 벡터)

    [Header("Slope Handling")]
    public float maxSlopeAngle;                       // 올라갈 수 있는 언덕의 최대 각도
    private RaycastHit slopeHit;                      // 언덕 위에 있는지 확인할 레이캐스트

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

    // 캐릭터 이동 함수를 물리 업데이트에서 다룸
    private void FixedUpdate()
    {
        cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;        // 카메라의 전방 벡터를 y 축을 제외한 값으로 정규화
        Vector3 moveDirection = cameraForward * dir.z + mainCamera.transform.right * dir.x;                  // 카메라의 전방 벡터와 오른쪽 벡터를 이용하여 이동 방향 설정

        if (OnSlope())
        {
            // 경사면 위에서의 이동 방향 계산
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();
            // 경사면을 따라 플레이어를 이동시킴
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

    // 언덕 움직임
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
        Vector3 moveDirection = cameraForward * dir.z + mainCamera.transform.right * dir.x; // 카메라의 전방 벡터와 오른쪽 벡터를 이용하여 이동 방향 설정
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    // 움직임 감지 함수
    private void MovementDetection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        dir.x = Mathf.Abs(horizontalInput) > 0.1f ? Mathf.Sign(horizontalInput) : 0f;
        dir.z = Mathf.Abs(verticalInput) > 0.1f ? Mathf.Sign(verticalInput) : 0f;

        dir.Normalize();                                                                                     // 방향을 유지하고 1로 정규화
        isMoving = dir != Vector3.zero;                                                                      // 움직임 여부 감지
    }

    // 애니메이션 관리 함수
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
