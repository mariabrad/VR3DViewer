using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MenuBoard : MonoBehaviour
{
    public Transform prefabContainer;
    public GameObject prefabButton;
    public string prefabFolder = "Prefabs";

    private void Start()
    {
        // Load prefabs from the prefab folder
        string[] prefabFiles = Directory.GetFiles(Application.dataPath + "/" + prefabFolder, "*.prefab");

        Debug.Log("Found " + prefabFiles.Length + " prefabs in " + prefabFolder + " folder.");

        // Loop through the prefab files and create buttons for them
        foreach (string prefabFile in prefabFiles)
        {
            string prefabName = Path.GetFileNameWithoutExtension(prefabFile);

            // Create a button for the prefab
            GameObject prefabButtonObj = Instantiate(prefabButton, prefabContainer);
            prefabButtonObj.GetComponentInChildren<Text>().text = prefabName;

            // Set up a click event for the button
            prefabButtonObj.GetComponent<Button>().onClick.AddListener(() => LoadPrefab(prefabName));

            Debug.Log("Created button for prefab: " + prefabName);
        }
    }

    void LoadPrefab(string prefabName)
    {
        Debug.Log("Loading prefab: " + prefabName);

        // Load the prefab and instantiate it in the scene
        GameObject prefab = Resources.Load<GameObject>(prefabFolder + "/" + prefabName);
        Instantiate(prefab);
    }
}
