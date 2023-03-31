using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

using TMPro;
public class objbuttoninside : MonoBehaviour
{
    public ScrollRect scrollView = null;
    public static string nameOfSelected;
    public string[] files = null;


    private GameObject buttonTemplate = null;
    private GameObject newButton = null;
    public UnityEngine.Object[] prefabObjects;
    public GameObject cabinet1;

    // Start is called before the first frame update
    void Start()
    {

        int i = 0;
        buttonTemplate = transform.GetChild(0).gameObject;

        prefabObjects = Resources.LoadAll("Prefabs", typeof(GameObject));

        if (prefabObjects != null)
        {
            foreach (UnityEngine.Object prefabObject in prefabObjects)
            {
                GameObject prefab = (GameObject)prefabObject;
                string prefabName = prefab.name;
                newButton = Instantiate(buttonTemplate, transform);
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{prefabName}";

                newButton.GetComponent<Button>().AddEventListener(i, ButtonClicked);
                i++;

            }
            Destroy(buttonTemplate);
        }
        scrollView.verticalNormalizedPosition = 1;



    }


    //LOAD CLICKED DATASET
    void ButtonClicked(int idx)
    {
        GameObject prefab = (GameObject)prefabObjects[idx];
        SharedResources.selectedPrefab = prefab;
        //SceneManager.LoadScene("cabinetwithobj");
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
            }

            foreach (Transform child in newPrefab.transform)
            {
                if (child.gameObject.GetComponent<MeshFilter>())
                {
                    AddGrabbableComponents(child.gameObject);
                    child.SetParent(null, true);
                }
            }

            Destroy(newPrefab);
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
        }

        if (!obj.GetComponent<XRGrabInteractable>())
        {
            XRGrabInteractable grabInteractable = obj.AddComponent<XRGrabInteractable>();
            grabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
            grabInteractable.attachTransform = GameObject.Find("Camera Offset/RightHand Controller").transform;
        }
    }
}



