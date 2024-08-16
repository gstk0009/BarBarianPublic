using TMPro;
using UnityEngine;
using System.Collections;
public class ExitBtnGuideUI : MonoBehaviour
{
    [SerializeField] GameObject guidePanel;
    [SerializeField] TextMeshProUGUI text;

    WaitForSeconds wfs_1 = new WaitForSeconds(1);

    public void ShowGuidePanel()
    {
        guidePanel.SetActive(true);
        StartCoroutine(ClosePanelAfterTime(3));
    }

    // 3초 카운트다운 후 패널을 비활성화하는 코루틴
    IEnumerator ClosePanelAfterTime(int countdownTime)
    {
        for (int i = countdownTime; i > 0; i--)
        {
            text.text = $"해당 창은 <color=#6CF6FF>{i}<color=#ffffff>초 뒤 자동으로 닫힙니다.";
            yield return wfs_1;
        }

        guidePanel.SetActive(false);
    }
}
