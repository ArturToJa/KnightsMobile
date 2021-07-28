using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject nextPanel;

    public void OnMenuButton()
    {
        ToggleMenuButtons.instance.currentPanel = nextPanel;
        ToggleMenuButtons.instance.ToggleButtons();        
    }
}
