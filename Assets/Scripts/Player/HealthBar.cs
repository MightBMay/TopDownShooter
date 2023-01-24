using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    Stats stats;
    Slider healthBar;
    [SerializeField]TextMeshProUGUI healthText;
    [SerializeField]Image fill;
    // Start is called before the first frame update
    void Start()
    {
        //stats = GetComponent<Stats>();
        stats = GetComponentInParent<Stats>();
        healthBar = GetComponent<Slider>();
        healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = stats.cStats.GetHealthPercent();
        healthText.text = stats.cStats.GetHealthString();
        ChangeFillColour();
    }
    void ChangeFillColour()
    {
        float h, s, v;
        Color.RGBToHSV(Color.red, out h, out s, out v);
        float startV = v;
        Color.RGBToHSV(Color.green, out h, out s, out v);
        float endV = v;
        v = Mathf.Lerp(startV, endV, healthBar.value);
        fill.color = Color.HSVToRGB(Mathf.Lerp(0f, 120f / 360f, healthBar.value), 1, v);
    }
}
