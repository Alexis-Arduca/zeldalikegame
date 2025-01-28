using UnityEngine;

public class Ignitable : MonoBehaviour
{
    public GameObject fireEffect;
    private bool isLit = false;

    public void Ignite()
    {
        Debug.Log("Hola");
        if (!isLit)
        {
            isLit = true;

            if (fireEffect != null)
            {
                Instantiate(fireEffect, transform.position, Quaternion.identity, transform);
            }

            Debug.Log($"{gameObject.name} is now lit!");
        }
    }
}
