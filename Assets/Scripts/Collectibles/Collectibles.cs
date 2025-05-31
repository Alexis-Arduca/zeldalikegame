using UnityEngine;

public class Collectibles : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Will be override in the class
    }

    public virtual void OnBuy()
    {
        // Will be override in the class
    }
}
