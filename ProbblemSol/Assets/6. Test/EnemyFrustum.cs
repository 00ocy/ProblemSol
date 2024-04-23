using OCY_ProblemSol;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemyFrustum : MonoBehaviour
{
    public Material material1; // 프러스텀 내부에 있는 오브젝트에 적용될 머티리얼
    public Material material2; // 프러스텀 밖에 있는 오브젝트에 적용될 머티리얼
    public Material material3; // 프러스텀 밖에 있는 오브젝트에 적용될 머티리얼

    public Camera thisCamera;
    public GameObject player; // 플레이어 게임 오브젝트
    private MQ mQ;
    private bool inboke = false;
    public float followSpeed = 5f; // 플레이어를 따라가는 속도

    private void Start()
    {
        mQ = this.GetComponent<MQ>();
        mQ.enabled = false;
    }
    private void Update()
    {
        if (thisCamera == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 카메라의 프러스텀을 가져오기
        FrustumPlanes frustum = new FrustumPlanes(thisCamera);

        // 프러스텀 내에 있는 경우 Material1 적용
        player.GetComponent<Renderer>().material = material1;

        if (frustum.IsInsideFrustum(player.GetComponent<Renderer>().bounds))
        {
            // 플레이어와 적 사이의 장애물 검사
            RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position));
            
            foreach (RaycastHit hit in hits)
            {
                
                if (hit.transform != player.transform && hit.transform != transform)
                {
                    // 장애물이 플레이어나 적이 아닌 다른 오브젝트일 때만 처리
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * hit.distance, Color.red);
                    player.GetComponent<Renderer>().material = material2;
                    if (inboke == true)
                    {
                        mQ.CancelInvoke("Shoot");
                        inboke = false;

                    }
                    return;
                }
            }


            // 플레이어를 따라가는 로직 추가
            if (player != null)
            {
                if(inboke==false)
                {
                    mQ.InvokeRepeating("Shoot", 1f, 1f);
                    inboke = true;
                }

                // 플레이어 방향으로 일정 속도로 이동
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);

                // 플레이어를 바라보도록 회전
                transform.LookAt(player.transform);
            }
        }
        else
        {
            // 프러스텀 밖에 있는 경우 Material2 적용
            player.GetComponent<Renderer>().material = material3;

            if (inboke==true)
            {
            mQ.CancelInvoke("Shoot");
            inboke = false;

            }
        }

        // 디버그 레이를 그리기
        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * Vector3.Distance(transform.position, player.transform.position), Color.green);
    }
}

// 프러스텀 플레인 클래스
public class FrustumPlanes
{
    private readonly Plane[] planes;

    public FrustumPlanes(Camera camera)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
    }

    // 함수 인자를 Bounds로 변경하여 바운딩 박스가 프러스텀 내부에 있는지 확인
    public bool IsInsideFrustum(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
