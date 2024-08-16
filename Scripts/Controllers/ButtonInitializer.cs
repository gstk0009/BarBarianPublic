using UnityEngine;
using UnityEngine.UI;

public class ButtonInitializer : MonoBehaviour
{
    [SerializeField] Button btn;
    public bool isExitBtn;
    public bool isSelectBtn;
    private void Start()
    {
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                if(isExitBtn)
                {
                    LoadSceneManager.instance.EndExitBtn();
                }
                else if(isSelectBtn)
                {
                    SceneTransitionManager.Instance.LoadScene((int)SceneNumber.SelectSeene);
                }
                else
                {
                    LoadSceneManager.instance.StartBtn();
                }
            });
        }
    }
}
