using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.EventSystems;

public class PlaceOnPlane : MonoBehaviour
{
    // AR 공간에 배치할 프리팹
    [SerializeField] private GameObject placeDispenser;

    // AR 레이캐스트 매니저
    [SerializeField] private ARRaycastManager arRaycastManager;

    [SerializeField]
    private PlayUIManager playUIManager;

    // 터치를 통한레이캐스트 정보 리스트 (동적배열)
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnObject;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            return;
        }

        Vector2 touchPosition = default; // Vector2 touchPosition = Vector2.zero;

        if (Input.touchCount > 0)
        {
            // 첫번째 터치 포인트 위치
            touchPosition = Input.GetTouch(0).position;

            // 화면 터치 위에서 Plane을 향한 레이캐스트를 수행함
            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                // Plane과 충돌한 Plane 위치 정보를 파악
                Pose hitPose = hits[0].pose;

                if (spawnObject == null)
                {
                    // 터치한 위치에 AR 오브젝트를 생성
                    spawnObject = Instantiate(placeDispenser, hitPose.position, hitPose.rotation);
                    playUIManager.enabled = true;
                }
            }
        }
    }
}