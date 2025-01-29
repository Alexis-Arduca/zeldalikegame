using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[CreateAssetMenu(fileName = "NewLamp", menuName = "Inventory/Lamp")]
public class Lamp : Item
{
    public GameObject lightPrefab;
    public float lightRadius = 15f;
    private GameObject activeLight;
    private Material lightMaterial;
    public LayerMask ignitableLayer;
    public GameObject mask;
    private int magicCost = 1;
    private Coroutine magicDrainRoutine;

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
        PlayerMagic playerMagic = player?.GetComponent<PlayerMagic>();

        if (player != null && lightPrefab != null && playerMagic != null)
        {
            if (!playerMagic.CanUse(magicCost))
            {
                Debug.Log("Not enough magic to activate the lamp.");
                return;
            }

            activeLight = Instantiate(lightPrefab, player.transform);
            activeLight.transform.localPosition = Vector3.zero;

            lightMaterial = mask.GetComponent<Image>().material;
            
            if (lightMaterial != null)
            {
                lightMaterial.SetFloat("_Radius", 0.2f);
                Debug.Log("Lamp activated!");
                magicDrainRoutine = player.GetComponent<MonoBehaviour>().StartCoroutine(DrainMagic(playerMagic));
            }
            else
            {
                Debug.LogError("Material not found on lightPrefab!");
            }
        }
        else
        {
            Debug.LogError("Player, PlayerMagic component, or lightPrefab not found!");
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

            if (magicDrainRoutine != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.GetComponent<MonoBehaviour>().StopCoroutine(magicDrainRoutine);
                }
                magicDrainRoutine = null;
            }
        }
    }

    private IEnumerator DrainMagic(PlayerMagic playerMagic)
    {
        while (activeLight != null)
        {
            if (!playerMagic.CanUse(magicCost))
            {
                DeactivateLight();
                yield break;
            }
            
            playerMagic.ConsumeMagic(magicCost);
            yield return new WaitForSeconds(1f);
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
