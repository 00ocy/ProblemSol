using TMPro;
using UnityEngine;
public class EnemyFrustum : MonoBehaviour
{
    public Camera thisCamera;
    public GameObject player;
    public float followSpeed = 5f;
    public Transform detectionRange;
    public int width = 2; // ������� ���� ũ��
    public int height = 2; // ������� ���� ũ��

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
                // �÷��̾ �Ѱ� �ִ� ���¿��� detectionRange �ۿ� ���� ��
                // isChasingPlayer ���� false�� �����Ͽ� �ٽ� detectionRange�� ��ġ�� ���ư����� ��
                isChasingPlayer = false;
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
                Debug.Log("dd");
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
    void SetRandomTargetPosition()
    {
        // ���� Ÿ�� ������ ����
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
