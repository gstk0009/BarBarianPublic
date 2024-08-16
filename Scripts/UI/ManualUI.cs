using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private int infoIndex = 0;
    public List<GameObject> infoes = new List<GameObject>();
    
    [SerializeField]
    Button PrevBtn, NextBtn;

    private void OnEnable()
    {
        infoIndex = 0;
        infoes[0].SetActive(true);
        for (int i=1;i<infoes.Count;i++)
            infoes[i].SetActive(false);

        PrevBtn.interactable = false;
        NextBtn.interactable = true;
    }
    public void TurnLeft()
    {
        if (infoIndex >= 1)
        {
            infoes[infoIndex].SetActive(false);
            infoes[--infoIndex].SetActive(true);
        }

        if (infoIndex == 0)
        {
            PrevBtn.interactable = false;
        }
        else if (infoIndex == infoes.Count - 2)
        {
            NextBtn.interactable = true;
        }
    }

    public void TurnRight()
    {
        if (infoIndex < infoes.Count - 1)
        {
            infoes[infoIndex].SetActive(false);
            infoes[++infoIndex].SetActive(true);
        }

        if (infoIndex == infoes.Count - 1)
        {
            NextBtn.interactable = false;
        }
        else if (infoIndex == 1)
        {
            PrevBtn.interactable = true;
        }
    }
}
