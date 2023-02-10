using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Slider healtSlider;
    [SerializeField] private Image healthFiller;
    public int currentHealth;
    // Start is called before the first frame update

    public void SetInitiallyHealth(int maxhealth)
    {
        healtSlider.maxValue = maxhealth;
        healtSlider.value = maxhealth;
    }

    void SetCurrentHealth(int currenthealth)
    {
        healtSlider.value = currenthealth;
    }
}
