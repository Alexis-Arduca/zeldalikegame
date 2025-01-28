using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewLamp", menuName = "Inventory/Lamp")]
public class Lamp : Item
{
    public GameObject lightPrefab;
    public float lightRadius = 15f;
    private GameObject activeLight;
    private Material lightMaterial;
    public LayerMask ignitableLayer;
    public GameObject mask;

    public Lamp() : base("Lamp", null)
    {
    }

    public override void Use()
    {
        if (isEquipped)
        {
            if (activeLight == null)
            {
                ActivateLight();
            }
            else
            {
                DeactivateLight();
            }

            LightNearbyObjects();
        }
    }

    private void ActivateLight()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && lightPrefab != null)
        {
            activeLight = Instantiate(lightPrefab, player.transform);
            activeLight.transform.localPosition = Vector3.zero;

            lightMaterial = mask.GetComponent<Image>().material;
            
            if (lightMaterial != null)
            {
                lightMaterial.SetFloat("_Radius", 0.2f);
                Debug.Log("Lamp activated!");
            }
            else
            {
                Debug.LogError("Material not found on lightPrefab!");
            }
        }
        else
        {
            Debug.LogError("Player or lightPrefab not found!");
        }
    }

    private void DeactivateLight()
    {
        if (activeLight != null)
        {
            if (lightMaterial != null)
            {
                lightMaterial.SetFloat("_Radius", 0.1f);
            }

            Destroy(activeLight);
            activeLight = null;
            Debug.Log("Lamp deactivated!");
        }
    }

    private void LightNearbyObjects()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, lightRadius, ignitableLayer);

            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Testing");
                var ignitable = hitCollider.GetComponent<Ignitable>();
                if (ignitable != null)
                {
                    ignitable.Ignite();
                }
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
