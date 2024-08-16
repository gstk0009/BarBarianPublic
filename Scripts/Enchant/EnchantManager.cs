using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantManager : MonoBehaviour
{
    public static bool isEnchanting = false;

    [SerializeField] MaterialItemData[] materials;
    [SerializeField] GameObject requireUIObj;
    [SerializeField] Image requireMaterialImage;
    [SerializeField] TextMeshProUGUI amountText;

    [HideInInspector] public bool isMaterialItemLeft = false;

    private void Start()
    {
        if (GameManager.Instance.EnchantManager != null) return;

        GameManager.Instance.EnchantManager = this;
        requireUIObj.SetActive(false);
    }

    public void EnchantItem(EnchantItemSlot weaponSlot, EnchantItemSlot materialSlot, EquipmentItem ei)
    {
        int requireID = GetRequireMaterial(ei);
        if (requireID == -1)
        {
            return;
        }

        int requireAmount = (ei.EnchantLevel % 3) + 1;

        if (materialSlot.itemData.ID == requireID && materialSlot.GetItemAmount() >= requireAmount)
        {
            GameManager.Instance.EnchantEffectController.StartEnchanting();

            if (ei.EnchantLevel > 9)
            {
                EnchantMenuUI.instance.guideText.text = "장비가 이미 최대 강화 수치에 달했습니다!";
                return;
            }

            ei.Enchant();

            weaponSlot.RemoveItem();
            ToggleUI(false);
            EnchantDragAndDrop.instance.enchantSlotItems[0] = null;

            materialSlot.DecreaseItem(requireAmount);

            if (materialSlot.GetItemAmount() <= 0)
            {
                materialSlot.RemoveItem();
                EnchantDragAndDrop.instance.enchantSlotItems[1] = null;
                isMaterialItemLeft = false;
            }
            else
            {
                isMaterialItemLeft = true;
            }

            ei.ItemName = ei.Data.Name + $" <color=#6CF6FF>(+{ei.EnchantLevel}강)</color>";
            UpdateResultSlot(ei, ei.sprite);
            EnchantDragAndDrop.instance.enchantSlotItems[2] = ei;
        }
        else
        {
            EnchantMenuUI.instance.guideText.text = "합성을 위한 재료가 충족되지 않았습니다.";
        }
    }

    public void UpdateResultSlot(EquipmentItem item, Sprite icon)
    {
        EnchantMenuUI.instance.resultSlot.SetItem(icon, item.Data);
        EnchantMenuUI.instance.resultSlot.SetItemAmount(1);
    }

    int GetRequireMaterial(EquipmentItem ei)
    {
        if (ei.EnchantLevel >= 9)
        {
            return materials[3].ID;
        }

        int i = ei.EnchantLevel / 3;
        return materials[i].ID;
    }

    public void ToggleUI(bool active)
    {
        requireUIObj.SetActive(active);
    }

    public void SetRequireMentUI(EquipmentItem item)
    {
        int level = item.EnchantLevel;
        int amount = (level % 3) + 1;

        requireMaterialImage.sprite = materials[level / 3].IconSprite;
        amountText.text = " x" + amount.ToString();
    }
}
