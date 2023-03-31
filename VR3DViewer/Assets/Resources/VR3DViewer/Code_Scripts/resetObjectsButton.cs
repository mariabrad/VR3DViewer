using UnityEngine;

public class resetObjectsButton : MonoBehaviour
{
    public Instantiate instantiateScript;

    public void ResetObjects()
    {
        if (instantiateScript != null)
        {
            instantiateScript.ResetPrefab();
        }
    }
}
