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

namespace UnityVolumeRendering
{


    public class VolumeRenderingSelecting : MonoBehaviour
    {
        public ScrollRect scrollView = null;

        public string[] savedDirectoryNames = null;

        private string dicomAssetPath = "MediVR/Textures/Dicom 3D Textures";
        private static string savedDirectoryFileName = "importedDirectoryNames";
        private string savedDirectoryFilePath = null;
        private string savedDirectoryFileNames = null;

        private initialImportDicom initialImportDicomScript = null;

        private GameObject buttonTemplate = null;
        private GameObject newButton1 = null;
 
        // Start is called before the first frame update
        void Start()
        {
            initialImportDicomScript = this.GetComponent<initialImportDicom>();

            savedDirectoryFilePath = Path.Combine(dicomAssetPath, savedDirectoryFileName);
            //Debug.Log(savedDirectoryFilePath);

            //LOAD DIRECTORY PATH
            if (savedDirectoryFilePath != null)
            {
                var resource = Resources.Load<TextAsset>(savedDirectoryFilePath);
                if (resource != null)
                {
                    savedDirectoryFileNames = resource.text;
                    //Debug.Log(savedDirectoryFileNames);
                }
            }

            //DESERIALIZE DIRECTORY NAMES
            if (savedDirectoryFileNames != null)
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(string[]));

                using (StringReader reader = new StringReader(savedDirectoryFileNames))
                {
                    object obj = deserializer.Deserialize(reader);

                    savedDirectoryNames = (string[])obj;
                    //Debug.Log(savedDirectoryNames);
                }
            }

            buttonTemplate = transform.GetChild(0).gameObject;

            //CREATE ONE BUTTON IN LIST FOR EVERY IMPORTED DIRECTORY
            if (savedDirectoryNames != null)
            {
                for (int i = 0; i < savedDirectoryNames.Length; i++)
                {
                    newButton1 = Instantiate(buttonTemplate, transform);
                    newButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{savedDirectoryNames[i]}";

                    newButton1.GetComponent<Button>().AddEventListener(i, ButtonClicked);
                }

                Destroy(buttonTemplate);
            }

            scrollView.verticalNormalizedPosition = 1;
        }

        //LOAD CLICKED DATASET
        async void ButtonClicked(int idx)
        {
            setCurrentDirectory.currentDirectory = savedDirectoryNames[idx];

            Debug.Log($"Clicked folder name: {setCurrentDirectory.currentDirectory}.");

            //Debug.Log($"Loading Cabinet.");
            //initialImportDicomScript.GoToNextScene();*

            bool recursive = true;
            string pathtoDicom = "Asset/Ressources";
            // Read all files
          IEnumerable<string> fileCandidates = Directory.EnumerateFiles(pathtoDicom, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(p => p.EndsWith(".dcm", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".dicom", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".dicm", StringComparison.InvariantCultureIgnoreCase));

            // Import the dataset
          IImageSequenceImporter importer = ImporterFactory.CreateImageSequenceImporter(ImageSequenceFormat.DICOM);
           IEnumerable<IImageSequenceSeries> seriesList = await importer.LoadSeriesAsync(fileCandidates);
            float numVolumesCreated = 0;
            foreach (IImageSequenceSeries series in seriesList)
            {
                VolumeDataset dataset = await importer.ImportSeriesAsync(series);
                // Spawn the object
                if (dataset != null)
                {
                    VolumeRenderedObject obj = await VolumeObjectFactory.CreateObjectAsync(dataset);
                    obj.transform.position = new Vector3(numVolumesCreated, 0, 0);
                    numVolumesCreated++;


                }
            }
        }
        //MODIFY BUTTON ONCLICK METHOD

    }
}