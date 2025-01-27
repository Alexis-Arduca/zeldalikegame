using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RupeeUI : MonoBehaviour
{
    public RupeeManager rupeeBag;
    public TMP_Text rupeeText;

    // Start is called before the first frame update
    void Start()
    {
        if (rupeeBag == null)
        {
            Debug.LogError("RupeeManager not assigned.");
        }

        if (rupeeText == null)
        {
            Debug.LogError("Text not assigned.");
        }
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
