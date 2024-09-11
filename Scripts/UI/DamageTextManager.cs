using UnityEngine;
using TMPro;

public class DamageTextManager : MonoBehaviour
{
    public float textSpeed; //텍스트 이동속도
    public float alphaSpeed; //투명도 변환속도
    public float destroyTime;
    TextMeshPro text;
    Color alpha;
    public int damage;

    private Color initColor = new Color(1f, 0.3f, 0.3f, 1f);
    private Color playerColor = new Color(0.25f, 0.4f, 1f, 1f);

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, textSpeed *  Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }
    
    private void DestroyObject()
    {
        gameObject.SetActive(false);
    }

    public void SetTextColor(EnchantType type = EnchantType.None, bool isPlayer = false)
    {
        if (isPlayer)
            text.color = playerColor;
        else
            text.color = initColor;

        switch (type)
        {
            case  EnchantType.None:
                break;
            case EnchantType.Poison:
                text.color = Color.magenta;
                break;
            default:
                break;
        }

        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }
}
