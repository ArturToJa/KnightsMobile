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
    [SerializeField] private InputField registerUsername;
    [SerializeField] private InputField registerPassword;

    public string email { get; private set; }
    public string username { get; private set; }
    public string password { get; private set; }
    public bool isRegister { get; private set; }

    [Header("Switch panels on successful login")]
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject gameplayPanel;

    [Header("Main info")]
    [SerializeField] private Text health;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text experience;
    [SerializeField] private Image experienceBar;
    [SerializeField] private Text gold;
    [SerializeField] private Text rubies;
    [SerializeField] private Text playername;
    [SerializeField] private Text level;


    [Header("Character info")]
    [SerializeField] private Text strength;
    [SerializeField] private Button strengthButton;
    [SerializeField] private Text aptitude;
    [SerializeField] private Button aptitudeButton;
    [SerializeField] private Text constitution;
    [SerializeField] private Button constitutionButton;
    [SerializeField] private Text luck;
    [SerializeField] private Button luckButton;
    [SerializeField] private Text weaponSkill;
    [SerializeField] private Button weaponSkillButton;
    [SerializeField] private Text defenceSkill;
    [SerializeField] private Button defenceSkillButton;

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
        strengthButton.onClick.AddListener(() => { this.player.UpgradeStatistic(KnightsMobile.Statistic.Strength); });
        aptitudeButton.onClick.AddListener(() => { this.player.UpgradeStatistic(KnightsMobile.Statistic.Aptitude); });
        constitutionButton.onClick.AddListener(() => { this.player.UpgradeStatistic(KnightsMobile.Statistic.Constitution); });
        luckButton.onClick.AddListener(() => { this.player.UpgradeStatistic(KnightsMobile.Statistic.Luck); });
        weaponSkillButton.onClick.AddListener(() => { this.player.UpgradeStatistic(KnightsMobile.Statistic.WeaponSkill); });
        defenceSkillButton.onClick.AddListener(() => { this.player.UpgradeStatistic(KnightsMobile.Statistic.DefenceSkill); });
    }

    public void Register()
    {
        isRegister = true;
        email = registerEmail.text;
        username = registerUsername.text;
        password = registerPassword.text;
        manager.StartClient();
        manager.networkAddress = "localhost";
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

    public void UpdateStatistic(KnightsMobile.Statistic whichStat, int value, int bonusValue, int cost)
    {
        switch (whichStat)
        {
            case KnightsMobile.Statistic.Strength:
                strength.text = value.ToString() + " + " + bonusValue.ToString();
                strengthButton.GetComponentInChildren<Text>().text = cost.ToString();
                break;
            case KnightsMobile.Statistic.Aptitude:
                aptitude.text = value.ToString() + " + " + bonusValue.ToString();
                aptitudeButton.GetComponentInChildren<Text>().text = cost.ToString();
                break;
            case KnightsMobile.Statistic.Constitution:
                constitution.text = value.ToString() + " + " + bonusValue.ToString();
                constitutionButton.GetComponentInChildren<Text>().text = cost.ToString();
                break;
            case KnightsMobile.Statistic.Luck:
                luck.text = value.ToString() + " + " + bonusValue.ToString();
                luckButton.GetComponentInChildren<Text>().text = cost.ToString();
                break;
            case KnightsMobile.Statistic.WeaponSkill:
                weaponSkill.text = value.ToString() + " + " + bonusValue.ToString();
                weaponSkillButton.GetComponentInChildren<Text>().text = cost.ToString();
                break;
            case KnightsMobile.Statistic.DefenceSkill:
                defenceSkill.text = value.ToString() + " + " + bonusValue.ToString();
                defenceSkillButton.GetComponentInChildren<Text>().text = cost.ToString();
                break;
        }

    }

    public void UpdateMainData(KnightsMobile.MainData whichData, int value)
    {
        switch(whichData)
        {
            case KnightsMobile.MainData.Experience:
                this.experience.text = value.ToString() + "/" + player.GetMaxExperience().ToString();
                float expDifference = value >= player.GetMinExperience() ? (float)value - (float)player.GetMinExperience() : 0.0f;
                experienceBar.fillAmount = expDifference / (float)player.GetMaxExperience() - (float)player.GetMinExperience();
                break;
            case KnightsMobile.MainData.Gold:
                this.gold.text = value.ToString();
                break;
            case KnightsMobile.MainData.Health:
                this.health.text = value.ToString() + "/" + player.GetMaxHealth().ToString();
                healthBar.fillAmount = (float)value / (float)player.GetMaxHealth();
                break;
            case KnightsMobile.MainData.Level:
                this.level.text = "Level " + value.ToString();
                break;
            case KnightsMobile.MainData.Rubies:
                this.rubies.text = value.ToString();
                break;
        }
    }

    public void UpdatePlayername(string name)
    {
        this.playername.text = name;
    }
}
