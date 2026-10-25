using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;
    public GameDataManager gameDataManager;

    public void Start()
    {
        BGMSlider.value = gameDataManager._intexPlayerData._playerData._volumeBGM;
        SFXSlider.value = gameDataManager._intexPlayerData._playerData._volumeSFX;
    }

    public void UpdateVolumeBGM()
    {
        gameDataManager.UpdateVolumeBGM(BGMSlider.value);
    }

    public void UpdateVolumeSFX()
    {
        gameDataManager.UpdateVolumeSFX(SFXSlider.value);
    }
}
