using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantEffectController : MonoBehaviour
{
    [SerializeField] GameObject EnchantingEffect;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI enchantingText;

    public float duration = 1f;
    WaitForSeconds wfs_05 = new WaitForSeconds(0.5f);
    private void Start()
    {
        if (GameManager.Instance.EnchantEffectController != null) return;

        GameManager.Instance.EnchantEffectController = this;
        EnchantingEffect.SetActive(false);
        progressBar.fillAmount = 0f;

        
    }

    public void StartEnchanting()
    {
        StartCoroutine(EnchantingCoroutine());
    }

    private IEnumerator EnchantingCoroutine()
    {
        EnchantEffect();

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            progressBar.fillAmount = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        progressBar.fillAmount = 1f;

        yield return wfs_05;
        CompleteEnchanting();
        yield return null;
    }

    
    void EnchantEffect()
    {
        EnchantingEffect.SetActive(true);
        SoundManager.Instance.PlaySoundEffect((int)SoundEffects.Hammer, 1, 2, false);
        progressBar.fillAmount = 0f;
        enchantingText.text = "강화중...";
        enchantingText.gameObject.SetActive(true);
    }

    private void CompleteEnchanting()
    {
        EnchantingEffect.SetActive(false);
        progressBar.fillAmount = 0f;
        enchantingText.gameObject.SetActive(false);
    }
}
