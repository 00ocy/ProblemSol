using TMPro;
using UnityEngine;
public class EnemyFrustum : MonoBehaviour
{
    public Camera thisCamera;
    public GameObject player;
    public float followSpeed = 5f;
    public Transform detectionRange;
    public int width = 2; // 기즈모의 가로 크기
    public int height = 2; // 기즈모의 세로 크기

    private bool isChasingPlayer = false;
    private bool isbox = false;
    public Vector3 targetPosition;

    private void Start()
    {
        InvokeRepeating("SetRandomTargetPosition", 0f, 3f);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // 기즈모 생성
        Gizmos.color = Color.yellow;
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;
        Vector3 position = detectionRange.position;
        Gizmos.DrawWireCube(position, new Vector3(width, 0, height));
    }

    private void Update()
    {
        if (thisCamera == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        FrustumPlanes frustum = new FrustumPlanes(thisCamera);

        if (frustum.IsInsideFrustum(player.GetComponent<Renderer>().bounds))
        {
            isChasingPlayer = true;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position));

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform != player.transform && hit.transform != transform)
                {
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * hit.distance, Color.red);
                    return;
                }
            }

            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);
                transform.LookAt(player.transform);
                isbox = false;
            }
        }
        else
        {
            if (isChasingPlayer)
            {
                // 플레이어를 쫓고 있는 상태에서 detectionRange 밖에 있을 때
                // isChasingPlayer 값을 false로 설정하여 다시 detectionRange의 위치로 돌아가도록 함
                isChasingPlayer = false;
            }

            Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * Vector3.Distance(transform.position, player.transform.position), Color.green);

            // detectionRange 밖에 있다면 detectionRange의 위치로 이동
            Vector3 directionToRange = detectionRange.position - transform.position;
            directionToRange.y = 0; // Y축 이동을 무시
            if (directionToRange.magnitude > 0.1f && !isbox) // 이동할 거리가 일정 이상일 때만 이동하도록 조건 추가
            {
                transform.position = Vector3.MoveTowards(transform.position, detectionRange.position, followSpeed * Time.deltaTime);
                transform.LookAt(new Vector3(detectionRange.position.x, transform.position.y, detectionRange.position.z)); // Y축 이동을 무시하므로 LookAt 대상도 Y값을 현재 위치의 Y값으로 설정
            }
            else
            {
                isbox = true;
            }

            if (isbox)
            {
                Debug.Log("dd");
                Vector3 targetPositionToRange = targetPosition - transform.position;
                targetPositionToRange.y = 0; // Y축 이동을 무시
                if (targetPositionToRange.magnitude > 0.1f) // 이동할 거리가 일정 이상일 때만 이동하도록 조건 추가
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 1f);
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                }
            }
        }
    }
    void SetRandomTargetPosition()
    {
        // 랜덤 타겟 포지션 생성
        targetPosition = new Vector3(detectionRange.transform.position.x + Random.Range(-width / 2, width / 2), 0f, detectionRange.transform.position.z + Random.Range(-height / 2, height / 2));
    }
}



public class FrustumPlanes
{
    private readonly Plane[] planes;

    public FrustumPlanes(Camera camera)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
    }

    public bool IsInsideFrustum(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
    
}
