using UnityEngine;

public class Uis : MonoBehaviour
{
    [Header("EnchantUI")]
    [SerializeField] GameObject enchantUI;

    [Header("ShopUI")]
    [SerializeField] GameObject shopUI;

    private void Start()
    {
        if (GameManager.Instance.Uis != null) return;

        GameManager.Instance.Uis = this;
        enchantUI.SetActive(false);
    }


    public void SetShopUI(bool active)
    {
        Player.Instance.isPlayerInteracting = active;
        GameManager.Instance.canOpenInventory = !active;
        shopUI.SetActive(active);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.playerStateMachine.IdleState);
    }
    public void SetEnchantUI(bool active)
    {
        Player.Instance.isPlayerInteracting = active;
        GameManager.Instance.canOpenInventory = !active;
        enchantUI.SetActive(active);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.playerStateMachine.IdleState);
    }
   
}
