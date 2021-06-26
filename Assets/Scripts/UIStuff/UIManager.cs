using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; protected set; }

    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    private KnightsMobile.Player player;

    private void Awake()
    {
        if(instance != null && instance != this)
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
}
