using OCY_ProblemSol;
using System.Collections;
using TMPro;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    public Camera thisCamera;
    public GameObject player;
    public float followSpeed = 5f;
    public Transform detectionRange;
    public int width = 2; // 기즈모의 가로 크기
    public int height = 2; // 기즈모의 세로 크기

    //private bool isChasingPlayer = false;
    private bool isbox = false;
    public Vector3 targetPosition;

    private bool playerhide = false; // 오브젝트를 숨길지 여부를 나타내는 변수
    Coroutine rotationCoroutine1;
   private bool go =true;

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

            if(go)
            {
        if (frustum.IsInsideFrustum(player.GetComponent<Renderer>().bounds))
        {
            //isChasingPlayer = true;
            playerhide = false;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position));

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform != player.transform && hit.transform != transform)
                {
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * hit.distance, Color.red);
                    playerhide = false;
                    go = false;
                    rotationCoroutine1 = StartCoroutine(RotateOverTime(new Vector3(0, 90, 0), 2f));
                    return;
                }
            }

            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);
                transform.LookAt(player.transform);
                playerhide = true;
                isbox = false;
            }


        }
        else
        {
            if(playerhide) // 플레이어 숨었고 발견했었음
            {
                playerhide = false;
                go = false;
                rotationCoroutine1 = StartCoroutine(RotateOverTime(new Vector3(0, 90, 0), 2f));
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
    }
    void SetRandomTargetPosition()
    {
        // 랜덤 타겟 포지션 생성
        targetPosition = new Vector3(detectionRange.transform.position.x + Random.Range(-width / 2, width / 2), 0f, detectionRange.transform.position.z + Random.Range(-height / 2, height / 2));
    }
    IEnumerator RotateOverTime(Vector3 targetRotation, float duration)
    {
        rotationCoroutine1 = null; // 코루틴 초기화

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation) * startRotation; // 목표 회전량을 현재 회전에 더함

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            elapsedTime = Time.time - startTime;

            // 플레이어를 감지하면 중지하고 플레이어를 쫓아가기
            if (PlayerDetected())
            {
                go = true;
                if (rotationCoroutine1 != null)
                    StopCoroutine(rotationCoroutine1);
                yield break; // 코루틴 종료
            }

            yield return null;
        }

        // 회전이 끝나면 180도 회전
        Quaternion originalRotation = transform.rotation;
        Quaternion target180Rotation = originalRotation * Quaternion.Euler(0, 180, 0);
        float rotateDuration = 3.0f; // 회전하는데 걸리는 시간
        float rotateStartTime = Time.time;

        while (Time.time < rotateStartTime + rotateDuration)
        {
            float t = (Time.time - rotateStartTime) / rotateDuration;
            transform.rotation = Quaternion.Slerp(originalRotation, target180Rotation, t);

            // 플레이어를 감지하면 중지하고 플레이어를 쫓아가기
            if (PlayerDetected())
            {
                go = true;
                if (rotationCoroutine1 != null)
                    StopCoroutine(rotationCoroutine1);
                yield break; // 코루틴 종료
            }

            yield return null;
        }

        // 코루틴을 중지하고 무한 반복을 막음
        go = true;
        if (rotationCoroutine1 != null)
            StopCoroutine(rotationCoroutine1);
    }

    bool PlayerDetected()
    {
        FrustumPlanes frustum = new FrustumPlanes(thisCamera);

        if (frustum.IsInsideFrustum(player.GetComponent<Renderer>().bounds))
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position));

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform != player.transform && hit.transform != transform)
                {
                    return false; // 플레이어 감지됨
                }
                
            }
            if (player != null)
            {
                    return true; // 플레이어 감지됨
                
            }
        }
        return false; // 플레이어 감지되지 않음
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
