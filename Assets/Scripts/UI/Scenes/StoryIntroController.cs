using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class StoryIntroController : MonoBehaviour
{
    public Sprite[] slides;
    public float displayDuration = 2f;
    private Image panelImage;
    // Start is called before the first frame update
    void Start()
    {
        panelImage = GetComponent<Image>();
        StartCoroutine(Playintro());

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    private IEnumerator Playintro()
    {
        foreach (Sprite slide in slides)
        {
            panelImage.sprite = slide;
            panelImage.SetNativeSize();
            yield return StartCoroutine(Fade(0f, 1f, 0.5f));
            yield return new WaitForSeconds(displayDuration);
            yield return StartCoroutine(Fade(1f, 0f, 0.5f));
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = panelImage.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            panelImage.color = color;
            yield return null;
        }
        color.a = endAlpha;
        panelImage.color = color;
    }


}
