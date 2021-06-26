using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour
{
    [SerializeField] private GameObject currentPanel;
    [SerializeField] private GameObject goBackToPanel;

    public void OnGoBack()
    {
        currentPanel.SetActive(false);
        goBackToPanel.SetActive(true);
    }
}
