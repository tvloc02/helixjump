using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public static event Action<PlayerSkin> PlayerSkinSelectionChanged;

    public const string SKIN_LOCK_PREFIX = "skin_locked";
    private const string SELECTED_SKIN_KEY = "SelectedSkin";

    [SerializeField] private PlayerSkins _playerSkins;

    public static PlayerSkins PlayerSkins => Instance._playerSkins;

    // Ads luôn tắt (không có quảng cáo trong bài tập)
    public static bool EnableAds => false;
    public static bool AbleToRestore => false;

    public static GameSettings GameSettings => Resources.Load<GameSettings>(nameof(GameSettings));

    public static PlayerSkin GetSkinById(string id) => PlayerSkins.FirstOrDefault(skin => skin.id == id);

    public static void SetSelectedSkin(string id)
    {
        PrefManager.SetString(SELECTED_SKIN_KEY, id);
        PlayerSkinSelectionChanged?.Invoke(GetSkinById(id));
    }

    public static string GetSelectedSkin()
    {
        return PrefManager.GetString(SELECTED_SKIN_KEY, PlayerSkins.FirstOrDefault().id);
    }

    public static bool IsSkinLocked(string skinId)
    {
        var skin = GetSkinById(skinId);
        return skin.preLocked && skin.lockDetails.minLevel > GameManager.CurrentLevel;
    }
}