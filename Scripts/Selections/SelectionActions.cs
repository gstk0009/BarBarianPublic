using UnityEngine;
public class SelectionActions : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance.SelectionActions != null) return;
            GameManager.Instance.SelectionActions = this;
    }

    public void CallMethod(string method, float delayTime = 0.3f)
    {
        Invoke(method, delayTime);
    }
    public void SpawnNPC()
    {
        GameManager.Instance.SpawnersManager.NPCSpawner.SpawnFollowingNPC();
    }

    public void SpawnEnemyNPC()
    {
        GameManager.Instance.SpawnersManager.NPCSpawner.SpawnEnemyNPC();
    }
    public void SaveGame()
    {
        DataManager.Instance.SaveGame();
    }
    public void PayTax()
    {
         GameManager.Instance.TaxManager.PayTax();
    }

  
    public void SetEnchantUI()
    {
        GameManager.Instance.Uis.SetEnchantUI(true);
    }
    public void DeactiveEnchantUI()
    {
        GameManager.Instance.Uis.SetEnchantUI(false);
    }

    public void SetStoreUI()
    {
        GameManager.Instance.Uis.SetShopUI(true);
    }
    public void DeactiveStoreUI()
    {
        GameManager.Instance.Uis.SetShopUI(false);
    }
}
