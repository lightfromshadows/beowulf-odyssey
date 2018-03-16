using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] CharacterStatsObj myStats;
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField] Color lowHealth;
    [SerializeField] Color highHealth;

	// Update is called once per frame
	void Update () {
        float t = myStats.Health / myStats.MaxHealth;

        fillImage.color = Color.Lerp(lowHealth, highHealth, t);
        slider.value = t;
	}
}
