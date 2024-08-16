using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//IPointerEnterHandler - 마우스의 포인터가 특정 충돌 범위 안에 들어올 때 발생하는 이벤트
//IPointerExitHandler - 마우스의 포인터가 특정 충돌 범위 밖으로 나갈 때 발생하는 이벤트
public class ButtonColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color highlightColor = Color.red; 

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>(); 
        buttonImage.color = Color.white; 
    }

    // 마우스가 버튼 위에 있을 때 색 변경
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = highlightColor; 
    }

    // 마우스가 버튼에서 벗어났을 때 색 변경
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = Color.white; 
    }
}
