using UnityEngine;

public class TaxOffice : MonoBehaviour
{


    int[] lineX = new int[] { 0, 17, 18, 19 };
    int[] lineY = new int[] { 0, 17, 18, 19 };
    public InteractionEvent interaction;

    private void Start()
    {
        interaction = GetComponent<InteractionEvent>();
        DialogueChange();
    }
    private void OnEnable()
    {
        ClockSystem.TaxDialogueEvent += DialogueChange;
    }

    private void OnDisable()
    {
        ClockSystem.TaxDialogueEvent -= DialogueChange;
    }

    private int DialogueNum()
    {
        if (ClockSystem.Dday % GameManager.Instance.TaxManager.TaxDue == 0) // 세금 당일
        {
            return 3;
        }
        else if (ClockSystem.Dday % GameManager.Instance.TaxManager.TaxDue < GameManager.Instance.TaxManager.TaxDue - 1) // 평상 시
            return 1;

        else if (ClockSystem.Dday % GameManager.Instance.TaxManager.TaxDue == GameManager.Instance.TaxManager.TaxDue - 1) // 세금 전 날
           return 2;


        return 0;
    }

    public int SetLineX()
    {
        return lineX[DialogueNum()];
    }

    public int SetLineY()
    {
        return lineY[DialogueNum()];
    }

    public  void DialogueChange()
    {
        int x = SetLineX();
        int y = SetLineY();

        interaction.UpdateDialogueLines(x, y);
        GameManager.Instance.DialogueManager.GetDialogues(interaction.GetDialogue());
    }
}
