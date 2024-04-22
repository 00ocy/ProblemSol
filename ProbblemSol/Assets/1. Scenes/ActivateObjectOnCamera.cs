using UnityEngine;

public class ActivateObjectOnCamera : MonoBehaviour
{
    public Camera mainCamera; // 카메라를 가리키는 참조
    public GameObject objectToActivate; // 활성화할 게임 오브젝트

    void Start()
    {
        // 물체를 렌더링하지 않음
        objectToActivate.SetActive(false);
    }

    void Update()
    {
        // 카메라의 시야에 물체가 있는지 확인
        if (IsObjectInCameraView())
        {
            // 물체를 활성화
            objectToActivate.SetActive(true);
        }
    }

    bool IsObjectInCameraView()
    {
        // 물체의 위치를 카메라 시야에서의 위치로 변환
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(objectToActivate.transform.position);

        // 변환된 위치가 카메라 시야 내에 있는지 확인
        return (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && viewportPoint.z > 0);
    }
}
