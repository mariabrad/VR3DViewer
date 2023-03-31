/*

    MediVR, a medical Virtual Reality application for exploring 3D medical datasets on the Oculus Quest.

    Copyright (C) 2020  Dimitar Tahov

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    This script serves to create scrollable list of imported directories at beginning menu.

*/

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

using TMPro;

public class objButtonSelect : MonoBehaviour
{
    public ScrollRect scrollView = null;
    public static string nameOfSelected;
    public string[] files = null;


    private GameObject buttonTemplate = null;
    private GameObject newButton = null;
    public UnityEngine.Object[] prefabObjects;

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
        SceneManager.LoadScene("cabinetwithobj");
    }
}