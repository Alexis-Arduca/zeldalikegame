using UnityEngine;
using System.Collections;

public class VillageWindowUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float displayDuration = 3f;
    private Vector2 initialPosition;
    private Vector2 targetPosition;

    private void Start()
    {
        initialPosition = panel.anchoredPosition;
        targetPosition = initialPosition + new Vector2(0, -100);
    }

    public void StartAnimation()
    {
        panel.anchoredPosition = initialPosition;
        panel.gameObject.SetActive(true);

        StartCoroutine(AnimatePanel());
    }

    private IEnumerator AnimatePanel()
    {
        float elapsed = 0f;
        Vector2 startPos = initialPosition;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPosition, t);
            yield return null;
        }
        panel.anchoredPosition = targetPosition;

        yield return new WaitForSeconds(displayDuration);

        elapsed = 0f;
        startPos = targetPosition;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            panel.anchoredPosition = Vector2.Lerp(startPos, initialPosition, t);
            yield return null;
        }
        panel.anchoredPosition = initialPosition;

        panel.gameObject.SetActive(false);
    }
}
