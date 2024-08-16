using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField]private AudioMixer MasterMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BgmSlider;
    [SerializeField] private Slider SfxSlider;

    public void MasterAudioControl()
    {
        float sound = MasterSlider.value;

        if (sound == -40f) MasterMixer.SetFloat("Master", -80f);
        else MasterMixer.SetFloat("Master", sound);
    }

    public void BgmAudioControl()
    {
        float sound = BgmSlider.value;

        if (sound == -40f) MasterMixer.SetFloat("BGM", -80f);
        else MasterMixer.SetFloat("BGM", sound);
    }

    public void SfxAudioControl()
    {
        float sound = SfxSlider.value;

        if (sound == -40f) MasterMixer.SetFloat("SFX", -80f);
        else MasterMixer.SetFloat("SFX", sound);
    }
}
