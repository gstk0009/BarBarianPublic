using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float textSpeed; //텍스트 이동속도
    public float alphaSpeed; //투명도 변환속도
    public float destroyTime;
    TextMeshPro text;
    Color alpha;
    public int damage;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
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
        Destroy(gameObject);
    }

    public void SetTextColor(EnchantType type = EnchantType.None)
    {
        switch(type)
        {
            case  EnchantType.None:
                    break;
            case EnchantType.Poison:
                text.color = Color.magenta;
                break;
            default:
                break;
        }
    }
}
