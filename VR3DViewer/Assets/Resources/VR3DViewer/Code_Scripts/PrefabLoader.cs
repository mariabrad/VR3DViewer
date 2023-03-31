using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
public class PrefabLoader : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;
   public Transform prefabsParent; // Parent transform for instantiated prefabs
    public GameObject cabinet;
    private GameObject instantiatedPrefab;
    private Vector3 y;
    void Start()
    {
        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        Object[] prefabObjects = Resources.LoadAll("Prefabs", typeof(GameObject));


        float buttonVerticalOffset = 5f;
        foreach (Object prefabObject in prefabObjects)
        {
            GameObject prefab = (GameObject)prefabObject;
            string prefabName = prefab.name;

            Debug.Log("Loading prefab: " + prefabName);

            GameObject button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName;

            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, buttonVerticalOffset); // Set button's vertical position
            buttonVerticalOffset -= 5f;

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Loading " + prefabName + " prefab.");

                GameObject newPrefab = Instantiate(prefab, Vector3.one, Quaternion.identity, prefabsParent);
                //newPrefab.AddComponent<Collider>();
                Collider collider = newPrefab.AddComponent<BoxCollider>();

                //newPrefab.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                //if(newPrefab != null)
                cabinet = GameObject.Find("sizetofit");
                //newPrefab.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                //newPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Vector3 cabinetSize = cabinet.GetComponent<BoxCollider>().bounds.size;
                Vector3 prefabSize = newPrefab.GetComponent<BoxCollider>().bounds.size;


                if (prefabSize != null)
                {
                    Debug.Log("Prefab has a collider");
                }




                if (prefabSize.x <= cabinetSize.x &&
                    prefabSize.y <= cabinetSize.y &&
                    prefabSize.z <= cabinetSize.z)
                {
                    //GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity, prefabsParent);
                    //newPrefab.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3f;
                    newPrefab.transform.localPosition = new Vector3(0.52f, 3f, 3f);
                }
                else
                {
                    Debug.Log("Prefab is too big to fit inside the cabinet. Resizing...");

                    float scaleFactor = Mathf.Min((cabinetSize.x / prefabSize.x)/100, (cabinetSize.y / prefabSize.y)/100, (cabinetSize.z / prefabSize.z)/100);
                    newPrefab.transform.localScale *= scaleFactor;

                    //GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity, prefabsParent);
                    //newPrefab.transform.position = new Vector3(0.52f, 6f, 6f);
                   y = new Vector3(0.52f, 6f, 6f);

                }
            });
        }
    }

        }
    


