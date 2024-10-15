using System.Collections;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    public float floatHeight = 0.5f;
    public float floatDuration = 1.0f;

    public void StartFloating()
    {
        StartCoroutine(FloatAndDisappear());
    }

    private IEnumerator FloatAndDisappear()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, floatHeight, 0);

        float elapsedTime = 0;

        while (elapsedTime < floatDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / floatDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        float fadeDuration = 0.5f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;

        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
