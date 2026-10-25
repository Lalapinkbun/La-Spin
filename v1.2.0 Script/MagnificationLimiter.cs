using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnificationLimiter : MonoBehaviour
{
    public Dropdown _magnification;
    public int _slotCount;
    private List<string> _allOptions = new List<string> { "x1", "x2", "x3", "x4", "x5" };

    public void Awake()
    {
        //基于_slotCount，对_magnification设置限制：_slotCoutn = 1; _magnification 最高只能 = 1； 依此类推最高值为5。
        LimitMagnification();
    }

    private void LimitMagnification()
    {
        // 限制在 1 ~ 5 之间，避免 slotCount 超过范围
        int max = Mathf.Clamp(_slotCount, 1, 5);

        // 截取前 max 个倍率
        List<string> limitedOptions = _allOptions.GetRange(0, max);

        // 更新 Dropdown
        _magnification.ClearOptions();
        _magnification.AddOptions(limitedOptions);

        // 强制默认选到最后一个（最高倍率）
        _magnification.value = 0;
        _magnification.RefreshShownValue();
    }
}
