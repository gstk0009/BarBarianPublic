using UnityEngine;

public class SpawnersManager : MonoBehaviour
{
    public MonsterSpawner MonsterSpawner;
    public NPCSpawner NPCSpawner;

    private void Awake()
    {
        if (GameManager.Instance.SpawnersManager != null) return;

        GameManager.Instance.SpawnersManager = this;

        MonsterSpawner = GetComponent<MonsterSpawner>();
        NPCSpawner = GetComponent<NPCSpawner>();
    }
}