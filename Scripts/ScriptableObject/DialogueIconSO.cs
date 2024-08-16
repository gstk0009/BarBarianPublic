using UnityEngine;

[CreateAssetMenu(fileName = "faceIcon", menuName = "faceIcon/Icon")]
public class DialogueIconSO : ScriptableObject
{
    [field: SerializeField] public string speaker;
    [field: SerializeField] public Sprite faceIcon;
}