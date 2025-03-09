using TMPro;
using UnityEngine;

public class ChestCountText : MonoBehaviour
{
    [Header("Elements")]
    private TextMeshProUGUI text;

    public void UpdateText(string chestCountString)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        text.text = chestCountString;
    }
}
