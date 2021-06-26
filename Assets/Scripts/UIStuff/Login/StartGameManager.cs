using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;

    void Start()
    {
        welcomePanel.SetActive(true);
        loginPanel.SetActive(false);
    }


    public void LoginOption()
    {
        welcomePanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void RegisterOption()
    {
        welcomePanel.SetActive(false);
        registerPanel.SetActive(true);
    }
}
