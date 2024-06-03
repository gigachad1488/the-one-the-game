using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeySettingsItem : MonoBehaviour
{
    public string title;
    public InputActionReference actionReference;

    public TextMeshProUGUI titleText;

    public Button firstKeyButton;
    public TextMeshProUGUI firstKeyButtonText;
    public int firstBindingCount = 1;
    public bool firstBindingComposite;

    [Space(2)]
    public Button secondKeyButton;
    public TextMeshProUGUI secondKeyButtonText;
    public int secondBindingCount = 1;
    public bool secondBindingComposite;

    public Button resetButton;

    private int bindingIndex;

    private void Start()
    {
        titleText.text = title;

        if (firstBindingCount > 0)
        {
            firstKeyButtonText.text = actionReference.action.GetBindingDisplayString(0);
        }

        if (firstBindingComposite)
        {
            firstKeyButton.onClick.AddListener(delegate { KeyOverride(firstKeyButton, firstKeyButtonText, 1, firstBindingComposite, 0, firstBindingCount); });

            if (secondBindingCount > 0)
            {
                secondKeyButtonText.text = actionReference.action.GetBindingDisplayString(firstBindingCount + 1);
            }
        }
        else
        {
            firstKeyButton.onClick.AddListener(delegate { KeyOverride(firstKeyButton, firstKeyButtonText, 0, firstBindingComposite); });

            if (secondBindingCount > 0)
            {
                secondKeyButtonText.text = actionReference.action.GetBindingDisplayString(firstBindingCount);
            }
        }

        if (secondBindingComposite)
        {          
            secondKeyButton.onClick.AddListener(delegate { KeyOverride(secondKeyButton, secondKeyButtonText, firstBindingCount + 2, secondBindingComposite, firstBindingCount + 1, firstBindingCount); });
        }
        else
        {
            secondKeyButton.onClick.AddListener(delegate { KeyOverride(secondKeyButton, secondKeyButtonText, firstBindingCount, secondBindingComposite); });
        }

        resetButton.onClick.AddListener(ResetKey);
    }

    public void KeyOverride(Button button, TextMeshProUGUI buttonText, int index, bool composite, int bindingCompositeStart = 0, int compositeBindingsCount = 0)
    {
        button.interactable = false;

        actionReference.action.Disable();
        var rebind = actionReference.action.PerformInteractiveRebinding(index);

        rebind.OnComplete((op) =>
        {
            actionReference.action.Enable();
            op.Dispose();
            
            
            if (composite)
            {
                buttonText.text = op.action.GetBindingDisplayString(index);
                int nextIndex = index + 1;
                if (nextIndex <= bindingCompositeStart + compositeBindingsCount)
                {                    
                    KeyOverride(button, buttonText, nextIndex, composite, bindingCompositeStart, compositeBindingsCount);
                }
                else
                {
                    Debug.Log("KEY INDEX = " + bindingCompositeStart);
                    buttonText.text = op.action.GetBindingDisplayString(bindingCompositeStart);
                    button.interactable = true;
                }
            }
            else
            {
                buttonText.text = op.action.GetBindingDisplayString(index);
                button.interactable = true;
            }

            SaveBindings();
        });

        rebind.OnCancel((op) =>
        {
            actionReference.action.Enable();
            op.Dispose();
            buttonText.text = actionReference.action.GetBindingDisplayString(index);
            button.interactable = true;
        });

        rebind.Start();
    }

    public void ResetKey()
    {
        actionReference.action.RemoveAllBindingOverrides();

        if (firstBindingCount > 0)
        {
            firstKeyButtonText.text = actionReference.action.GetBindingDisplayString(0);
        }

        if (secondBindingCount > 0 && firstBindingComposite)
        {
            secondKeyButtonText.text = actionReference.action.GetBindingDisplayString(firstBindingCount + 1);
        }
        else if (firstBindingCount > 0)
        {
            secondKeyButtonText.text = actionReference.action.GetBindingDisplayString(firstBindingCount);
        }

        SaveBindings();
    }

    public void SaveBindings()
    {
        string actions = actionReference.action.actionMap.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("actions", actions);

    }
}
