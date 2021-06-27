using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkManager))]
public class UIManager : MonoBehaviour
{
    NetworkManager manager;
    public static UIManager instance { get; protected set; }

    [Header("Login/register credentials")]
    [SerializeField] private InputField loginEmail;
    [SerializeField] private InputField loginPassword;
    [SerializeField] private InputField registerEmail;
    [SerializeField] private InputField registerPassword;

    [NonSerialized] public string email;
    [NonSerialized] public string password;
    [NonSerialized] public bool isRegister;

    [Header("Switch panels on successful login")]
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject gameplayPanel;

    private KnightsMobile.Player player;

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
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
    }

    public void SetPlayer(KnightsMobile.Player player)
    {
        this.player = player;
    }

    public void Register()
    {
        isRegister = true;
        email = registerEmail.text;
        password = registerPassword.text;
        manager.StartClient();
        manager.networkAddress = "localhost";
        /*NetworkClient.Ready();
        if (NetworkClient.localPlayer == null)
        {
            NetworkClient.AddPlayer();
        }*/
    }

    public void Login()
    {
        isRegister = false;
        email = loginEmail.text;
        password = loginPassword.text;
        manager.StartClient();
        manager.networkAddress = "localhost";
    }

    public void OnSuccessfulLogin()
    {
        startGamePanel.SetActive(false);
        gameplayPanel.SetActive(true);
    }    
}
