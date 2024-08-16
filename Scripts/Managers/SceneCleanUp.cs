using UnityEngine;
public class SceneCleanup : MonoBehaviour
{
    private void Start()
    {
        DestroyObjectsWithLayer();
    }

    private void DestroyObjectsWithLayer()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.layer == (int)LayerType.Managers || obj.layer == (int)LayerType.Player)
            {
                Destroy(obj);
            }
        }

    }
}
