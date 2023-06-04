using UnityEngine;

/// <summary>
/// progress bar helper class, basic, hardcoded max size
/// needs to be redone beyond the prototype
/// </summary>
public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _bar = null;

    public void SetProgress(float val)
    {
        _bar.sizeDelta = new Vector2(val * 150, _bar.sizeDelta.y);
    }
}
