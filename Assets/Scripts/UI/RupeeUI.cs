using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RupeeUI : MonoBehaviour
{
    private RupeeManager rupeeBag;
    public TMP_Text rupeeText;

    // Start is called before the first frame update
    void Start()
    {
        if (rupeeText == null)
        {
            Debug.LogError("Text 'RupeeText' missing.");
        }

        GameEventsManager.instance.playerEvents.onPlayerSpawn += LoadRupeeBag;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerSpawn -= LoadRupeeBag;
    }

    private void LoadRupeeBag()
    {
        rupeeBag = FindAnyObjectByType<RupeeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rupeeBag != null && rupeeText != null)
        {
            rupeeText.text = $"{rupeeBag.GetRupee()}";
        }
    }
}
