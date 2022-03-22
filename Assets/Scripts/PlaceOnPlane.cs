using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.EventSystems;

public class PlaceOnPlane : MonoBehaviour
{
    // AR ������ ��ġ�� ������
    [SerializeField] private GameObject placeDispenser;

    // AR ����ĳ��Ʈ �Ŵ���
    [SerializeField] private ARRaycastManager arRaycastManager;

    [SerializeField]
    private PlayUIManager playUIManager;

    // ��ġ�� ���ѷ���ĳ��Ʈ ���� ����Ʈ (�����迭)
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
            // ù��° ��ġ ����Ʈ ��ġ
            touchPosition = Input.GetTouch(0).position;

            // ȭ�� ��ġ ������ Plane�� ���� ����ĳ��Ʈ�� ������
            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                // Plane�� �浹�� Plane ��ġ ������ �ľ�
                Pose hitPose = hits[0].pose;

                if (spawnObject == null)
                {
                    // ��ġ�� ��ġ�� AR ������Ʈ�� ����
                    spawnObject = Instantiate(placeDispenser, hitPose.position, hitPose.rotation);
                    playUIManager.enabled = true;
                }
            }
        }
    }
}