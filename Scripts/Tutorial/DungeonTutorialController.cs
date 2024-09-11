using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DungeonTutorialController : MonoBehaviour 
{
    [SerializeField] List<DungeonTutorialBase> tutorials;

    DungeonTutorialBase currentTutorial = null;
    private int currentIdx = -1;

    void Start()
    {
        SetNextTutorial();
    }


    void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (currentIdx >= tutorials.Count - 1)
        {
            StartCoroutine(CompleteAllTutorials());
            DungeonTutorialBase.isTutorialing = false;
            return;
        }

        if (currentTutorial != null)
            currentTutorial.Exit();

        currentIdx++;
        currentTutorial = tutorials[currentIdx];

        // 다음 튜토리얼을 실행
        currentTutorial.Enter();
    }

    IEnumerator CompleteAllTutorials()
    {
        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());

        currentTutorial = null;

        yield return null;
    }

}