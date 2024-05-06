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
    public int width = 2; // ������� ���� ũ��
    public int height = 2; // ������� ���� ũ��

    //private bool isChasingPlayer = false;
    private bool isbox = false;
    public Vector3 targetPosition;

    private bool playerhide = false; // ������Ʈ�� ������ ���θ� ��Ÿ���� ����
    Coroutine rotationCoroutine1;
   private bool go =true;

    private void Start()
    {
        InvokeRepeating("SetRandomTargetPosition", 0f, 3f);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // ����� ����
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
            Debug.LogError("ī�޶� ������Ʈ�� ã�� �� �����ϴ�.");
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
            if(playerhide) // �÷��̾� ������ �߰��߾���
            {
                playerhide = false;
                go = false;
                rotationCoroutine1 = StartCoroutine(RotateOverTime(new Vector3(0, 90, 0), 2f));
            }
       

                Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * Vector3.Distance(transform.position, player.transform.position), Color.green);
           
                     
                // detectionRange �ۿ� �ִٸ� detectionRange�� ��ġ�� �̵�
                Vector3 directionToRange = detectionRange.position - transform.position;
                directionToRange.y = 0; // Y�� �̵��� ����
                if (directionToRange.magnitude > 0.1f && !isbox) // �̵��� �Ÿ��� ���� �̻��� ���� �̵��ϵ��� ���� �߰�
                {
                    transform.position = Vector3.MoveTowards(transform.position, detectionRange.position, followSpeed * Time.deltaTime);
                    transform.LookAt(new Vector3(detectionRange.position.x, transform.position.y, detectionRange.position.z)); // Y�� �̵��� �����ϹǷ� LookAt ��� Y���� ���� ��ġ�� Y������ ����
                }
                else
                {
                    isbox = true;
                }

                if (isbox)
                {
                    Vector3 targetPositionToRange = targetPosition - transform.position;
                    targetPositionToRange.y = 0; // Y�� �̵��� ����
                    if (targetPositionToRange.magnitude > 0.1f) // �̵��� �Ÿ��� ���� �̻��� ���� �̵��ϵ��� ���� �߰�
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
        // ���� Ÿ�� ������ ����
        targetPosition = new Vector3(detectionRange.transform.position.x + Random.Range(-width / 2, width / 2), 0f, detectionRange.transform.position.z + Random.Range(-height / 2, height / 2));
    }
    IEnumerator RotateOverTime(Vector3 targetRotation, float duration)
    {
        rotationCoroutine1 = null; // �ڷ�ƾ �ʱ�ȭ

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation) * startRotation; // ��ǥ ȸ������ ���� ȸ���� ����

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            elapsedTime = Time.time - startTime;

            // �÷��̾ �����ϸ� �����ϰ� �÷��̾ �Ѿư���
            if (PlayerDetected())
            {
                go = true;
                if (rotationCoroutine1 != null)
                    StopCoroutine(rotationCoroutine1);
                yield break; // �ڷ�ƾ ����
            }

            yield return null;
        }

        // ȸ���� ������ 180�� ȸ��
        Quaternion originalRotation = transform.rotation;
        Quaternion target180Rotation = originalRotation * Quaternion.Euler(0, 180, 0);
        float rotateDuration = 3.0f; // ȸ���ϴµ� �ɸ��� �ð�
        float rotateStartTime = Time.time;

        while (Time.time < rotateStartTime + rotateDuration)
        {
            float t = (Time.time - rotateStartTime) / rotateDuration;
            transform.rotation = Quaternion.Slerp(originalRotation, target180Rotation, t);

            // �÷��̾ �����ϸ� �����ϰ� �÷��̾ �Ѿư���
            if (PlayerDetected())
            {
                go = true;
                if (rotationCoroutine1 != null)
                    StopCoroutine(rotationCoroutine1);
                yield break; // �ڷ�ƾ ����
            }

            yield return null;
        }

        // �ڷ�ƾ�� �����ϰ� ���� �ݺ��� ����
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
                    return false; // �÷��̾� ������
                }
                
            }
            if (player != null)
            {
                    return true; // �÷��̾� ������
                
            }
        }
        return false; // �÷��̾� �������� ����
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
