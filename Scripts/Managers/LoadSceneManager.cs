using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// LoadScene을 이용해 실제로 씬 이동을 수행하는 클래스 
public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;

    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] TextMeshProUGUI guide;

    [SerializeField] string csv_TutorialTips; // 로딩 팁 파일 
    Dictionary<int, TutorialTips> tipsDic = new Dictionary<int, TutorialTips>();

    string sceneName = "디폴트";

    private void Awake()
    {
        Application.targetFrameRate = 60;
        SetTipsDictionary();
    }
    private void Start()
    {
        instance = this;
    }
    public void LoadScene(int sceneNum)
    {
        StartCoroutine(LoadSceneCoroutine(sceneNum));
    }
    public IEnumerator LoadSceneCoroutine(int sceneNum)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneNum);
        op.allowSceneActivation = false;

        setNameText(sceneNum);

        progressBar.enabled = true;

        tipText.text = "Tip! " + GetRandomTutorialTip();
        tipText.enabled = true;
        guide.enabled = true;

        progressText.enabled = true;

        float time = 0.0f;
        float minLoadTime = 1.5f;  // 최소 로딩 시간

        while (!op.isDone)
        {
            yield return null;
            time += Time.deltaTime;

            // 최소 로딩 시간 동안 진행 바를 일정하게 증가
            if (time < minLoadTime)
            {
                float p = time / minLoadTime;
                progressBar.fillAmount = time / minLoadTime;
                progressText.text =$"{sceneName}(으)로 가는 중..." + (p * 100f).ToString("F0") + "%";
            }
            else if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = 1f;
                progressText.text = "Loading Complete! 100%";
                op.allowSceneActivation = true;
            }
        }

        yield return new WaitForSeconds(.5f);

        InitUI();
    }

    void InitUI()
    {
        progressBar.fillAmount = 0f;
        progressText.text = "";
        sceneName = "디폴트";
        progressBar.enabled = false;
        progressText.enabled = false;
        tipText.enabled = false;
        guide.enabled = false;

    }

    void setNameText(int sceneNum)
    {
        switch (sceneNum)
        {
            case (int)SceneNumber.DugeonScene_1:
                sceneName = "던전 1층";
                break;
            case (int)SceneNumber.DugeonScene_2:
                sceneName = "던전 2층";
                break;
            case (int)SceneNumber.VillageScene:
                sceneName = "마을";
                break;
            case (int)SceneNumber.EndScene:
                sceneName = "엔딩";
                break;
            case (int)SceneNumber.SelectSeene:
                EquiptmentSlotUIs.isFirstOpen = true;
                sceneName = "캐릭터 선택 창";
                break;
            default:
                break;
        }
    }

    string GetRandomTutorialTip()
    {
        TutorialTips[] tips = GetTutorialTips();

        if (tips.Length == 0)
        {
            return "고블린은 끊임없는 커피를 좋아합니다. ";
        }

        int idx = Random.Range(0, tips.Length);

        return tips[idx].Context;
    }

    void SetTipsDictionary()
    {
        TutorialTipParser parser = GetComponent<TutorialTipParser>();
        if (parser == null)
        {
            return;
        }
        TutorialTips[] tutorialTips = parser.Parse(csv_TutorialTips); // 여기서 csv_TutorialTips 사용

        for (int i = 0; i < tutorialTips.Length; i++)
        {
            tipsDic.Add(tutorialTips[i].ID, tutorialTips[i]);
        }
    }
    public TutorialTips[] GetTutorialTips()
    {
        List<TutorialTips> tutorialTips = new List<TutorialTips>();

        for (int i = 1; i <= tipsDic.Count; i++)
        {
            if (tipsDic.ContainsKey(i))
            {
                tutorialTips.Add(tipsDic[i]);
            }
        }
        return tutorialTips.ToArray();
    }

   
    public void StartExitBtn()
    {
        Application.Quit();
    }
    public void StartBtn()
    {
        SceneManager.LoadScene((int)SceneNumber.SelectSeene);
    }
    public void EndExitBtn()
    {
        SceneManager.LoadScene((int)SceneNumber.StartScene);
    }
}
