using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Instantiate : MonoBehaviour
{
    public GameObject cabinet1;
    public GameObject instantiatedPrefab;

    void Start()
    {
        ResetPrefab();
    }

    public void ResetPrefab()
    {
        if (instantiatedPrefab != null)
        {
            Destroy(instantiatedPrefab);
        }


        if (SharedResources.selectedPrefab != null)
        {
            GameObject newPrefab = Instantiate(SharedResources.selectedPrefab, Vector3.zero, Quaternion.identity);

            cabinet1 = GameObject.Find("sizetofit");

            Bounds combinedBounds = new Bounds(newPrefab.transform.position, Vector3.zero);
            Renderer[] renderers = newPrefab.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }

            Vector3 cabinetSize = cabinet1.GetComponent<BoxCollider>().bounds.size;
            Vector3 prefabSize = combinedBounds.size;

            if (prefabSize.x <= cabinetSize.x &&
                prefabSize.y <= cabinetSize.y &&
                prefabSize.z <= cabinetSize.z)
            {
                Debug.Log("Prefab fits inside the cabinet");
            }
            else
            {
                Debug.Log("Prefab is too big to fit inside the cabinet. Resizing...");

                float minScaleFactor = Mathf.Min(
                    (cabinetSize.x / prefabSize.x),
                    (cabinetSize.y / prefabSize.y),
                    (cabinetSize.z / prefabSize.z)
                );

                newPrefab.transform.localScale = newPrefab.transform.localScale * minScaleFactor;
                newPrefab.transform.position += new Vector3(0, 1, 0);
            }

            foreach (Transform child in newPrefab.transform)
            {
                if (child.gameObject.GetComponent<MeshFilter>())
                {
                    AddGrabbableComponents(child.gameObject);
                    child.SetParent(newPrefab.transform, true);
                }
            }

            instantiatedPrefab = newPrefab;

            resetObjectsButton resetButton = FindObjectOfType<resetObjectsButton>();
            if (resetButton != null)
            {
                resetButton.instantiateScript = this;
            }
        }
    }



    private void AddGrabbableComponents(GameObject obj)
    {
        if (!obj.GetComponent<BoxCollider>())
        {
            BoxCollider collider = obj.AddComponent<BoxCollider>();
        }

        if (!obj.GetComponent<Rigidbody>())
        {
            Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.useGravity = false;
            rigidbody.angularDrag = float.PositiveInfinity;
            rigidbody.drag = float.PositiveInfinity;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (!obj.GetComponent<XRGrabInteractable>())
        {
            XRGrabInteractable grabInteractable = obj.AddComponent<XRGrabInteractable>();
            grabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
            grabInteractable.attachTransform = GameObject.Find("Camera Offset/RightHand Controller").transform;
            //grabInteractable.alignPosition = false;
            grabInteractable.smoothPosition = true; // enable smooth movement when pulling objects
            grabInteractable.throwOnDetach = false; // prevent the object from being thrown when released

            // adjust these values to customize the behavior when pulling objects
            


            grabInteractable.attachEaseInTime = 0.2f;

            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            StartCoroutine(HandleGrabState(grabInteractable));
        }
    }
    private IEnumerator HandleGrabState(XRGrabInteractable grabInteractable)
    {
        Rigidbody rigidbody = grabInteractable.GetComponent<Rigidbody>();

        while (true)
        {
            if (grabInteractable.isSelected)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }

            yield return null;
        }
    }
}