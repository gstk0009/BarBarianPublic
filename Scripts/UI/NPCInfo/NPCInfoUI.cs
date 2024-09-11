using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInfoUI : MonoBehaviour
{
    public NPC npc;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcType;
    [SerializeField] private TextMeshProUGUI npcHp;

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        if (npc != null)
        {
            npcIcon.sprite = npc.npcIcon;
            npcName.text = npc.npcIcon.name;
            npcType.text = "Type: " + npc.npcType.ToString();
        }
            
    }

    void Update()
    {
        if (npc == null) Destroy(gameObject);
        if(npc != null) npcHp.text = (int)npc.npcStat.HP.curValue + " / " + npc.npcStat.HP.maxValue;
    }
}
