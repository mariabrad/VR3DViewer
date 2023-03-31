using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Inst1 : MonoBehaviour
{
    public GameObject cabinet1;

    void Start()
    {
        if (SharedResources.selectedPrefab != null)
        {
            GameObject newPrefab = Instantiate(SharedResources.selectedPrefab, Vector3.zero, Quaternion.identity);

            cabinet1 = GameObject.Find("sizetofit");

            Vector3 cabinetSize = cabinet1.GetComponent<BoxCollider>().bounds.size;
            Vector3 prefabSize = newPrefab.GetComponent<BoxCollider>().bounds.size;

            if (prefabSize.x <= cabinetSize.x &&
                prefabSize.y <= cabinetSize.y &&
                prefabSize.z <= cabinetSize.z)
            {
                Debug.Log("Prefab fits inside the cabinet");
            }
            else
            {
                Debug.Log("Prefab is too big to fit inside the cabinet. Resizing...");

                Vector3 originalScale = newPrefab.transform.localScale;
                float minScaleFactor = Mathf.Min(
                    (cabinetSize.x / prefabSize.x) / 100,
                    (cabinetSize.y / prefabSize.y) / 100,
                    (cabinetSize.z / prefabSize.z) / 100
                );

                newPrefab.transform.localScale = originalScale * minScaleFactor;
            }

            GameObject container = new GameObject("Container");
            newPrefab.transform.SetParent(container.transform);

            BoxCollider containerCollider = container.AddComponent<BoxCollider>();
            containerCollider.size = newPrefab.GetComponent<BoxCollider>().size * newPrefab.transform.localScale.x;
            containerCollider.center = newPrefab.GetComponent<BoxCollider>().center;

            Rigidbody rigidbody = container.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.useGravity = true;
            rigidbody.angularDrag = float.PositiveInfinity;
            rigidbody.drag = float.PositiveInfinity;

            XRGrabInteractable grabInteractable = container.AddComponent<XRGrabInteractable>();
            grabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
            grabInteractable.attachTransform = GameObject.Find("Camera Offset/RightHand Controller").transform;
        }
    }
}
