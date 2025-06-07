using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class House : MonoBehaviour
{
    public Transform houseTeleportPoint;
    public Image fadeImage;
    private bool isTransitioning;
    private GameObject player;

    void Start()
    {
        isTransitioning = false;

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        fadeImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            player = other.gameObject;
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        fadeImage.gameObject.SetActive(true);
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f, 1f));

        player.transform.position = houseTeleportPoint.position;

        yield return StartCoroutine(Fade(1f, 0f, 1f));

        isTransitioning = false;
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, endAlpha);
    }
}