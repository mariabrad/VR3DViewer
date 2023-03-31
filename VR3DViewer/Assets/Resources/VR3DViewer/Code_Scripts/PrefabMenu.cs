using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrefabMenu : MonoBehaviour
{
    // Path to the folder where your prefabs are located
    public string prefabFolder = "Prefabs";

    // Reference to the menu board object
    public GameObject menuBoard;

    // Reference to the prefab button prefab
    public GameObject buttonPrefab;

    // List to store references to the prefabs
    private List<GameObject> prefabs = new List<GameObject>();

    void Start()
    {
        // Load prefabs from the specified folder
        LoadPrefabs();

        // Create menu items for each prefab
        CreateMenuItems();
    }

    // Load prefabs from the specified folder
    private void LoadPrefabs()
    {
        // Get the path to the specified folder
        string path = "Assets/" + prefabFolder;

        // Get an array of all prefab assets in the folder
        Object[] prefabAssets = Resources.LoadAll(path, typeof(GameObject));

        // Loop through the assets and add them to the prefabs list
        foreach (Object asset in prefabAssets)
        {
            if (asset is GameObject)
            {
                prefabs.Add(asset as GameObject);
            }
        }
    }

    // Create menu items for each prefab
    private void CreateMenuItems()
    {
        // Loop through the prefabs list
        for (int i = 0; i < prefabs.Count; i++)
        {
            // Get a reference to the current prefab
            GameObject prefab = prefabs[i];

            // Create a new button object using the button prefab
            GameObject button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            // Set the parent of the button to the menu board
            button.transform.SetParent(menuBoard.transform);

            // Set the position of the button based on its index in the list
            button.transform.localPosition = new Vector3(0, -i * 30, 0);

            // Set the text of the button to the name of the prefab
            button.GetComponentInChildren<Text>().text = prefab.name;

            // Add a listener to the button component that instantiates the prefab when clicked
            button.GetComponent<Button>().onClick.AddListener(() => Instantiate(prefab));
        }
    }
}
