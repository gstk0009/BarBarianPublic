using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] GameObject skillObejct;
    [SerializeField] GameObject HPbar;

    public List<GameObject> Tutorials;
    private void Start()
    {
        if (DataManager.Instance.currentPlayer.SetTutorial)
        {
            for (int i = 0; i < Tutorials.Count; i++)
            {
                if (!DataManager.instance.currentPlayer.tutorialClearInfo[i])
                    Tutorials[i].gameObject.SetActive(true);
                else
                    Tutorials[i].gameObject.SetActive(false);

            }
        }
        else
        {
            for (int i = 0; i < Tutorials.Count; i++)
            {
                Tutorials[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetUI(bool active)
    {
        skillObejct.SetActive(active);
        HPbar.SetActive(active);
    }
}