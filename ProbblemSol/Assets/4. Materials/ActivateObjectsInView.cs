using UnityEngine;

public class ActivateObjectsInView : MonoBehaviour
{
    private Camera thisCamera;

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

        // 모든 씬에 있는 모든 GameObject 가져오기
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 카메라와 빛은 제외
            if (obj.GetComponent<Camera>() != null || obj.GetComponent<Light>() != null)
                continue;

            // 오브젝트가 카메라 시야에 있는지 확인
            if (IsObjectInView(obj))
            {
                // 시야 안에 있으면 활성화
                obj.SetActive(true);
            }
            else
            {
                // 시야 밖에 있으면 비활성화
                obj.SetActive(false);
            }
        }
    }

    // 오브젝트가 카메라 시야에 있는지 확인하는 함수
    private bool IsObjectInView(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        // Renderer가 없는 경우 시야에 있지 않다고 가정
        if (renderer == null)
        {
            return false;
        }

        // Renderer의 중심점이 카메라 시야 내에 있는지 확인
        return thisCamera.WorldToViewportPoint(renderer.bounds.center).z > 0;
    }
}
