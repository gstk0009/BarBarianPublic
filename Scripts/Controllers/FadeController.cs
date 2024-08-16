using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private  float fadeTime = .7f; // 이 값이 작을수록 fade가 빠름 
    public  Image fadeImage;

    private void Start()
    {
        fadeImage = GetComponent<Image>();
    }

    public  IEnumerator FadeOut()
    {
        if (Player.Instance != null)
            Player.Instance.isPlayerInteracting = true;

        if (fadeImage !=null)
        {
            fadeImage.gameObject.SetActive(true);

            float currentTime = 0f;
            Color alpha = fadeImage.color;

            // 화면이 점점 어두워짐 
            while (alpha.a < 1f)
            {
                currentTime += Time.unscaledDeltaTime / fadeTime;
                alpha.a = Mathf.Lerp(0, 1, currentTime);
                fadeImage.color = new Color(alpha.r, alpha.g, alpha.b, alpha.a);
                yield return null;
            }

            fadeImage.color = new Color(alpha.r, alpha.g, alpha.b, 1f);
        }
        
    }

    public  IEnumerator FadeIn()
    {
        yield return new WaitUntil(() => fadeImage != null);
        yield return new WaitForSeconds(.5f);

        if (fadeImage != null)
        {
            float currentTime = 0;
            Color alpha = fadeImage.color;

            fadeImage.color = new Color(alpha.r, alpha.g, alpha.b, .9f);

            while (alpha.a > 0f)
            {
                currentTime += Time.deltaTime / fadeTime;
                alpha.a = Mathf.Lerp(1, 0, currentTime);
                fadeImage.color = new Color(alpha.r, alpha.g, alpha.b, alpha.a);
                yield return null;
            }

            fadeImage.color = new Color(alpha.r, alpha.g, alpha.b, 0f);

            yield return null;
        }

        if (Player.Instance != null)
            Player.Instance.isPlayerInteracting = false;
    }
}
