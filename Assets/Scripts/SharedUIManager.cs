using UnityEngine;

// ReSharper disable once HollowTypeName
public class SharedUIManager : Singleton<SharedUIManager>
{
    [SerializeField] private FadeEffectPanel _fadeEffectPanel;
    [SerializeField] private PopUpPanel _popUpPanel;

    public static PopUpPanel PopUpPanel => Instance._popUpPanel;
    public static FadeEffectPanel FadeEffectPanel => Instance._fadeEffectPanel;
}