using UnityEngine;

public class ChangeMaterialInViewport : MonoBehaviour
{
    public Material material1; // 시점 내부에 있는 오브젝트에 적용될 머티리얼
    public Material material2; // 시점 외부에 있는 오브젝트에 적용될 머티리얼
    public Material material3; // 좌측 상단 사분면에 적용될 머티리얼
    public Material material4; // 우측 하단 사분면에 적용될 머티리얼
    public Material materialout; // 우측 하단 사분면에 적용될 머티리얼

    private Camera thisCamera;
    public int maxRecursionDepth = 0; // 최대 재귀 호출 깊이
 
    private void Start()
    {
        // 해당 게임 오브젝트에 연결된 카메라 컴포넌트 가져오기
        thisCamera = GetComponent<Camera>();
        if (thisCamera == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
        }

    }

    private void Update()
    {
        if (thisCamera == null) return;

        // 카메라 시점의 시작 지점 (0, 0)과 끝 지점 (1, 1)을 구함
        Vector2 viewportStart = new Vector2(0, 0);
        Vector2 viewportEnd = new Vector2(1, 1);

        // 뷰포트 영역을 기준으로 머티리얼을 변경
        UpdateMaterialsInViewport(viewportStart, viewportEnd);

        // 시작과 끝 지점을 이용하여 시점을 4분할하며 머티리얼을 변경
        CheckViewportRecursive(viewportStart, viewportEnd, 0);
    }

    private void UpdateMaterialsInViewport(Vector2 start, Vector2 end)
    {
        // 모든 씬에 있는 모든 Renderer 가져오기
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        Rect viewportRect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);

        // 시점 내부에 있지 않은 Renderer에 material2 적용
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
        // 시작과 끝 지점을 이용하여 뷰포트 영역을 구함
        Rect viewportRect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);

        // 모든 씬에 있는 모든 Renderer 가져오기
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        bool objectsFound = false;

        foreach (Renderer renderer in renderers)
        {
            // Renderer의 중심점이 뷰포트 내에 있는지 확인
            if (ViewportContainsBounds(viewportRect, renderer.bounds))
            {
                objectsFound = true;
                break;
            }
        }

        // 재귀적으로 처리할 때 오브젝트가 발견되었고, 최대 재귀 호출 깊이에 도달하지 않은 경우
        if (objectsFound && recursionDepth < maxRecursionDepth)
        {
            // 4분할된 뷰포트의 중심점을 구함
            Vector2 midPoint = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);

            // 재귀 호출
            CheckViewportRecursive(start, midPoint, recursionDepth + 1); // 왼쪽 아래 사분면
            CheckViewportRecursive(new Vector2(midPoint.x, start.y), new Vector2(end.x, midPoint.y), recursionDepth + 1); // 오른쪽 아래 사분면
            CheckViewportRecursive(midPoint, end, recursionDepth + 1); // 오른쪽 위 사분면
            CheckViewportRecursive(new Vector2(start.x, midPoint.y), new Vector2(midPoint.x, end.y), recursionDepth + 1); // 왼쪽 위 사분면
        }
        // 재귀 호출이 끝난 후 최종 하위 영역일 때
        else if (recursionDepth == maxRecursionDepth)
        {
            // 최종 하위 영역에서 렌더링할 오브젝트가 있는지 확인하고, 그에 따라 머티리얼을 적용
            foreach (Renderer renderer in renderers)
            {

                if (ViewportContainsBounds(viewportRect, renderer.bounds))
                {
                    // 해당 사분면에 따라 다른 머티리얼 적용
                    if (start.x == 0 && start.y == 0) // 좌측 하단 사분면
                    {
                        renderer.material = material3;
                    }
                    else if (start.x == 0 && end.y == 1) // 좌측 상단 사분면
                    {
                        renderer.material = material1;
                    }
                    else if (end.x == 1 && start.y == 0) // 우측 하단 사분면
                    {
                        renderer.material = material2;
                    }
                    else if (end.x == 1 && end.y == 1) // 우측 상단 사분면
                    {
                        renderer.material = material4;
                    }

                }
            }


        }
    }

    // 뷰포트 영역이 Renderer의 바운딩 박스를 포함하는지 확인하는 함수
    private bool ViewportContainsBounds(Rect viewportRect, Bounds bounds)
    {
        // 카메라의 시야 방향을 기준으로 오브젝트의 위치를 확인
        Vector3 objectDirection = bounds.center - thisCamera.transform.position;
        bool isObjectInFront = Vector3.Dot(thisCamera.transform.forward, objectDirection) > 0;

        // 오브젝트가 카메라 시야 앞쪽에 있을 때만 확인
        if (isObjectInFront)
        {
            Vector3 minViewport = thisCamera.WorldToViewportPoint(bounds.min);
            Vector3 maxViewport = thisCamera.WorldToViewportPoint(bounds.max);

            // Renderer의 바운딩 박스가 뷰포트 내에 있는지 확인
            return viewportRect.Contains(minViewport) || viewportRect.Contains(maxViewport);
        }

        // 오브젝트가 카메라 뒤에 있으면 무조건 false 반환
        return false;
    }
}