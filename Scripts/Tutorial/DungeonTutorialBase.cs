using TMPro;
using UnityEngine;

public abstract class DungeonTutorialBase : MonoBehaviour
{
    [SerializeField] protected GameObject TutorialGudieIMG;
    [SerializeField] protected TextMeshProUGUI tutorialText;

    public static bool isTutorialing = false;
    public abstract void Enter();

    public abstract void Execute(DungeonTutorialController dtc);

    public abstract void Exit();
}