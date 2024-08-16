using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSizeUI : MonoBehaviour
{
    FullScreenMode screenMode = FullScreenMode.FullScreenWindow;
    [SerializeField] private Toggle fullScreenBtn;
    [SerializeField] private TMP_Dropdown screenSizeOption;
    List<Resolution> resolutions = new List<Resolution>();
    private int screenSizeOptionNum = 0;

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value == 60f)
                resolutions.Add(Screen.resolutions[i]);
        }
        //resolutions.AddRange(Screen.resolutions);

        screenSizeOption.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRateRatio + " hz";
            screenSizeOption.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                screenSizeOption.value = optionNum;
                Screen.SetResolution(resolutions[optionNum].width, resolutions[optionNum].height, screenMode);
            }
            optionNum++;
        }
        TMP_Dropdown.OptionData emptyOption = new TMP_Dropdown.OptionData();
        emptyOption.text = "";
        screenSizeOption.options.Add(emptyOption);
        screenSizeOption.RefreshShownValue();
    }

    // Dropbox 연결
    public void DropboxOptionChange(int x)
    {
        screenSizeOptionNum = x;
        IsChange();
    }

    // ToggleBtn 연결
    public void FullScreenBtn()
    {
        screenMode = fullScreenBtn.isOn ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
        IsChange();
    }

    private void IsChange()
    {
        Screen.SetResolution(resolutions[screenSizeOptionNum].width,
            resolutions[screenSizeOptionNum].height, screenMode);
    }
}
