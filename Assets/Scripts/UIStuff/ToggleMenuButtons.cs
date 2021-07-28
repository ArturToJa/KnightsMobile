using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenuButtons : MonoBehaviour
{
    public static ToggleMenuButtons instance { get; protected set; }

    [SerializeField] private GameObject panel;
    public GameObject currentPanel;

    private bool showingButtons = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleButtons();
    }

    public void ToggleButtons()
    {
        currentPanel.SetActive(showingButtons);
        showingButtons = !showingButtons;
        panel.SetActive(showingButtons);
    }
}
