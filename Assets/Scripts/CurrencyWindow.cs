using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _diamondText;
    [SerializeField] private TextMeshProUGUI _woodText;

    public static CurrencyWindow Instance { get; private set; }

    private void Start()
    {
        RefreshText();        
    } 

    private int Wood
    {
        get => PlayerPrefs.GetInt(PrefsKeys.WoodKey);
        set => PlayerPrefs.SetInt(PrefsKeys.WoodKey, value);
    }

    private int Diamond
    {
        get => PlayerPrefs.GetInt(PrefsKeys.DiamondKey);
        set => PlayerPrefs.SetInt(PrefsKeys.DiamondKey, value);
    }

    public void AddDiamond(int count)
    {
        Diamond += count;
        RefreshText();
    }

    public void AddWood(int count)
    {
        Wood += count;
        RefreshText();
    }

    private void RefreshText()
    {
        if (_diamondText != null)
            _diamondText.text = Diamond.ToString();
        if (_woodText != null)
            _woodText.text = Wood.ToString();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
