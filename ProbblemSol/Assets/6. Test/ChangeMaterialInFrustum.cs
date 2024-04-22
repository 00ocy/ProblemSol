using UnityEngine;

public class ChangeMaterialInViewport : MonoBehaviour
{
    public Material material1; // ���� ���ο� �ִ� ������Ʈ�� ����� ��Ƽ����
    public Material material2; // ���� �ܺο� �ִ� ������Ʈ�� ����� ��Ƽ����
    public Material material3; // ���� ��� ��и鿡 ����� ��Ƽ����
    public Material material4; // ���� �ϴ� ��и鿡 ����� ��Ƽ����
    public Material materialout; // ���� �ϴ� ��и鿡 ����� ��Ƽ����

    private Camera thisCamera;
    public int maxRecursionDepth = 0; // �ִ� ��� ȣ�� ����
 
    private void Start()
    {
        // �ش� ���� ������Ʈ�� ����� ī�޶� ������Ʈ ��������
        thisCamera = GetComponent<Camera>();
        if (thisCamera == null)
        {
            Debug.LogError("ī�޶� ������Ʈ�� ã�� �� �����ϴ�.");
        }

    }

    private void Update()
    {
        if (thisCamera == null) return;

        // ī�޶� ������ ���� ���� (0, 0)�� �� ���� (1, 1)�� ����
        Vector2 viewportStart = new Vector2(0, 0);
        Vector2 viewportEnd = new Vector2(1, 1);

        // ����Ʈ ������ �������� ��Ƽ������ ����
        UpdateMaterialsInViewport(viewportStart, viewportEnd);

        // ���۰� �� ������ �̿��Ͽ� ������ 4�����ϸ� ��Ƽ������ ����
        CheckViewportRecursive(viewportStart, viewportEnd, 0);
    }

    private void UpdateMaterialsInViewport(Vector2 start, Vector2 end)
    {
        // ��� ���� �ִ� ��� Renderer ��������
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        Rect viewportRect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);

        // ���� ���ο� ���� ���� Renderer�� material2 ����
        foreach (Renderer renderer in renderers)
        {
            if (!ViewportContainsBounds(viewportRect, renderer.bounds))
            {
                renderer.material = materialout;
            }
        }
    }

    private void CheckViewportRecursive(Vector2 start, Vector2 end, int recursionDepth)
    {
        // ���۰� �� ������ �̿��Ͽ� ����Ʈ ������ ����
        Rect viewportRect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);

        // ��� ���� �ִ� ��� Renderer ��������
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        bool objectsFound = false;

        foreach (Renderer renderer in renderers)
        {
            // Renderer�� �߽����� ����Ʈ ���� �ִ��� Ȯ��
            if (ViewportContainsBounds(viewportRect, renderer.bounds))
            {
                objectsFound = true;
                break;
            }
        }

        // ��������� ó���� �� ������Ʈ�� �߰ߵǾ���, �ִ� ��� ȣ�� ���̿� �������� ���� ���
        if (objectsFound && recursionDepth < maxRecursionDepth)
        {
            // 4���ҵ� ����Ʈ�� �߽����� ����
            Vector2 midPoint = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);

            // ��� ȣ��
            CheckViewportRecursive(start, midPoint, recursionDepth + 1); // ���� �Ʒ� ��и�
            CheckViewportRecursive(new Vector2(midPoint.x, start.y), new Vector2(end.x, midPoint.y), recursionDepth + 1); // ������ �Ʒ� ��и�
            CheckViewportRecursive(midPoint, end, recursionDepth + 1); // ������ �� ��и�
            CheckViewportRecursive(new Vector2(start.x, midPoint.y), new Vector2(midPoint.x, end.y), recursionDepth + 1); // ���� �� ��и�
        }
        // ��� ȣ���� ���� �� ���� ���� ������ ��
        else if (recursionDepth == maxRecursionDepth)
        {
            // ���� ���� �������� �������� ������Ʈ�� �ִ��� Ȯ���ϰ�, �׿� ���� ��Ƽ������ ����
            foreach (Renderer renderer in renderers)
            {

                if (ViewportContainsBounds(viewportRect, renderer.bounds))
                {
                    // �ش� ��и鿡 ���� �ٸ� ��Ƽ���� ����
                    if (start.x == 0 && start.y == 0) // ���� �ϴ� ��и�
                    {
                        renderer.material = material3;
                    }
                    else if (start.x == 0 && end.y == 1) // ���� ��� ��и�
                    {
                        renderer.material = material1;
                    }
                    else if (end.x == 1 && start.y == 0) // ���� �ϴ� ��и�
                    {
                        renderer.material = material2;
                    }
                    else if (end.x == 1 && end.y == 1) // ���� ��� ��и�
                    {
                        renderer.material = material4;
                    }

                }
            }


        }
    }

    // ����Ʈ ������ Renderer�� �ٿ�� �ڽ��� �����ϴ��� Ȯ���ϴ� �Լ�
    private bool ViewportContainsBounds(Rect viewportRect, Bounds bounds)
    {
        // ī�޶��� �þ� ������ �������� ������Ʈ�� ��ġ�� Ȯ��
        Vector3 objectDirection = bounds.center - thisCamera.transform.position;
        bool isObjectInFront = Vector3.Dot(thisCamera.transform.forward, objectDirection) > 0;

        // ������Ʈ�� ī�޶� �þ� ���ʿ� ���� ���� Ȯ��
        if (isObjectInFront)
        {
            Vector3 minViewport = thisCamera.WorldToViewportPoint(bounds.min);
            Vector3 maxViewport = thisCamera.WorldToViewportPoint(bounds.max);

            // Renderer�� �ٿ�� �ڽ��� ����Ʈ ���� �ִ��� Ȯ��
            return viewportRect.Contains(minViewport) || viewportRect.Contains(maxViewport);
        }

        // ������Ʈ�� ī�޶� �ڿ� ������ ������ false ��ȯ
        return false;
    }
}