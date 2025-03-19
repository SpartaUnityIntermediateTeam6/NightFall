using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    public Image playerHpImage;
    public Image sanityImage;
    public Image beaconHpImage;

    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI sanityText;
    public TextMeshProUGUI beaconHpText;

    public void OnPlayerHpChanged(BoundedValue hp)
    {
        playerHpImage.fillAmount = hp.Ratio;
        playerHpText.text = $"{hp.Value} / {hp.Max}";
    }

    public void OnSanityChanged(BoundedValue sanity)
    {
        sanityImage.fillAmount = sanity.Ratio;
        sanityText.text = $"{sanity.Value} / {sanity.Max}";
    }

    public void OnBeaconHpChanged(BoundedValue hp)
    {
        beaconHpImage.fillAmount = hp.Ratio;
        beaconHpText.text = $"{hp.Value} / {hp.Max}";
    }
}
