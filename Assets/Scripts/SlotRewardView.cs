using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotRewardView : MonoBehaviour
{
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private Image _highlightImage;
    [SerializeField] private TMP_Text _textDays;
    [SerializeField] private TMP_Text _countText;

    public void SetData(Reward reward, string CountType, int CountNum, bool isSelect)
    {
        _rewardIcon.sprite = reward.Icon;
        _textDays.text = $"{CountType} {CountNum}";
        _countText.text = reward.Count.ToString();
        _highlightImage.gameObject.SetActive(isSelect);
    }
    public void SetData(bool isSelect)
    {
        _highlightImage.gameObject.SetActive(isSelect);
    }
}
