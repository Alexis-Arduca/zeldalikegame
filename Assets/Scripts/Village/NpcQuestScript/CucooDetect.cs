using UnityEngine;

public class CucooDetect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cucoo"))
        {
            GameEventsManager.instance.cocoricoQuestEvents.OnCucooDetect();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Cucoo"))
        {
            GameEventsManager.instance.cocoricoQuestEvents.OnCucooDetect();
        }
    }
}