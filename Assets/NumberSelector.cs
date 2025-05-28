using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberSelector : MonoBehaviour
{
    [SerializeField] private int currentValue = 0;
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 9;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private List<Button> increaseButtons;
    [SerializeField] private List<Button> decreaseButtons;
    [SerializeField] private bool wrapAround = false;

    public int CurrentValue
    {
        get => currentValue;
        set
        {
            if(wrapAround)
            {
                if (value < minValue)
                {
                    currentValue = maxValue;
                }
                else if (value > maxValue)
                {
                    currentValue = minValue;
                }
                else
                {
                    currentValue = value;
                }
            }
            else
            {
                // Clamp the value to the min and max range
                currentValue = Mathf.Clamp(value, minValue, maxValue);
            }
            UpdateValueText();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button increaseButton in increaseButtons)
        {
            if (increaseButton != null)
            {
                increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
            }
        }
        
        foreach (Button decreaseButton in decreaseButtons)
        {
            if (decreaseButton != null)
            {
                decreaseButton.onClick.AddListener(OnDecreaseButtonClicked);
            }
        }
        
        UpdateValueText();
    }

    private void UpdateValueText()
    {
        if (valueText != null)
        {
            valueText.text = CurrentValue.ToString();
        }
    }


    private void OnDecreaseButtonClicked()
    {
        CurrentValue--;
    }

    private void OnIncreaseButtonClicked()
    {
        CurrentValue++;
    }

}
