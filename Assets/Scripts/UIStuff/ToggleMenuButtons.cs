using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private bool showingButtons = true;

    // Start is called before the first frame update
    void Start()
    {
        ToggleButtons();
    }

    public void ToggleButtons()
    {
        showingButtons = !showingButtons;
        panel.SetActive(showingButtons);
    }
}
