using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    private SerializedProperty _iosAppId;
//    private SerializedProperty _iosLeadersboardSetting;
//    private SerializedProperty _androidLeadersboardSetting;
    private SerializedProperty _inAppSetting;
//    private SerializedProperty _notificationSetting;
//    private SerializedProperty _dailyRewardSetting;


    private SerializedProperty _adsSettings;
    private SerializedProperty _privatePolicySetting;

    private void OnEnable()
    {
//        _iosLeadersboardSetting = serializedObject.FindProperty(GameSettings.IOS_LEADERSBOARD_SETTINGS);
//        _androidLeadersboardSetting = serializedObject.FindProperty(GameSettings.ANDROID_LEADERSBOARD_SETTINGS);
        _iosAppId = serializedObject.FindProperty(GameSettings.IOS_APP_ID);
        _inAppSetting = serializedObject.FindProperty(GameSettings.IN_APP_SETTING);
//        _notificationSetting = serializedObject.FindProperty(GameSettings.NOTIFICATION_SETTING);
//        _dailyRewardSetting = serializedObject.FindProperty(GameSettings.DAILY_REWARD_SETTING);
        _privatePolicySetting = serializedObject.FindProperty(GameSettings.PRIVATE_POLICY_SETTING);
        _adsSettings = serializedObject.FindProperty(GameSettings.ADS_SETTINGS);
    }

    public override void OnInspectorGUI()
    {
        DrawAppId();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DrawAdsSettings();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
//        DrawLeadersBoard();
//        EditorGUILayout.Space();
//        EditorGUILayout.Space();
        DrawInApp();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
//        DrawNotificationSetting();
//        EditorGUILayout.Space();
//        EditorGUILayout.Space();
//        DrawDailyReward();
//        EditorGUILayout.Space();

        DrawPrivatePolicy();
        EditorGUILayout.Space();
        DrawFixIfNeeded();


        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawFixIfNeeded()
    {
        if (MissingSymbols())
        {
            if (GUILayout.Button("Fix Missing Symbols"))
            {
                FixMissingSymbols();
            }
        }
    }

    private void FixMissingSymbols()
    {
        var adsSettings = _adsSettings.ToObjectValue<AdsSettings>();

        HandleScriptingSymbol(NamedBuildTarget.iOS, adsSettings.iosAdmobSetting.enable, "ADMOB");
        HandleScriptingSymbol(NamedBuildTarget.Android, adsSettings.androidAdmobSetting.enable, "ADMOB");

        var inAppSetting = _inAppSetting.ToObjectValue<InAppSetting>();
        HandleScriptingSymbol(NamedBuildTarget.iOS, inAppSetting.enable, "IN_APP");
        HandleScriptingSymbol(NamedBuildTarget.Android, inAppSetting.enable, "IN_APP");
    }
    private bool MissingSymbols()
    {
        var adsSettings = _adsSettings.ToObjectValue<AdsSettings>();
        if (adsSettings.iosAdmobSetting.enable && !HaveBuildSymbol(NamedBuildTarget.iOS, "ADMOB"))
            return true;

        if (adsSettings.androidAdmobSetting.enable && !HaveBuildSymbol(NamedBuildTarget.Android, "ADMOB"))
            return true;

        var inAppSetting = _inAppSetting.ToObjectValue<InAppSetting>();
        if (inAppSetting.enable && (!HaveBuildSymbol(NamedBuildTarget.Android, "IN_APP") ||
                                    !HaveBuildSymbol(NamedBuildTarget.iOS, "IN_APP")))
            return true;

        return false;
    }

    private void DrawAdsSettings()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ads Settings", EditorStyles.boldLabel);
        _adsSettings.isExpanded = EditorGUILayout.ToggleLeft("", _adsSettings.isExpanded);
        EditorGUILayout.EndHorizontal();
        if (_adsSettings.isExpanded)
        {
            EditorGUI.indentLevel++;

//            EditorGUILayout.BeginVertical(GUI.skin.box);
//            EditorGUILayout.LabelField("Other Settings", EditorStyles.boldLabel);
            _adsSettings.DrawChildrenDefault(nameof(AdsSettings.iosAdmobSetting)
                , nameof(AdsSettings.androidAdmobSetting)
//                , nameof(AdsSettings.iosAdColonySettings)
//                , nameof(AdsSettings.androidAdColonySettings)
            );
//            EditorGUILayout.EndVertical();


            DrawAdmobSetting(_adsSettings.FindPropertyRelative(nameof(AdsSettings.iosAdmobSetting)),
                _adsSettings.FindPropertyRelative(nameof(AdsSettings.androidAdmobSetting)));
//            DrawAdColonySetting(_adsSettings.FindPropertyRelative(nameof(AdsSettings.iosAdColonySettings)),
//                _adsSettings.FindPropertyRelative(nameof(AdsSettings.androidAdColonySettings)));

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
    }

//    private void DrawNotificationSetting()
//    {
////        var enableProperty = _notificationSetting.FindPropertyRelative(nameof(NotificationSetting.enable));
//
//        EditorGUILayout.BeginVertical(GUI.skin.box);
//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Notification Setting", EditorStyles.boldLabel);
//        var enable = EditorGUILayout.ToggleLeft("", enableProperty.boolValue);
//        EditorGUILayout.EndHorizontal();
//        EditorGUI.indentLevel++;
//
//        if (enable != enableProperty.boolValue)
//        {
//            enableProperty.boolValue = enable;
//            if (enableProperty.boolValue)
//            {
//                AddBuildSymbol(BuildTargetGroup.iOS, "NOTIFICATION");
//                AddBuildSymbol(BuildTargetGroup.Android, "NOTIFICATION");
//            }
//            else
//            {
//                RemoveBuildSymbol(BuildTargetGroup.iOS, "NOTIFICATION");
//                RemoveBuildSymbol(BuildTargetGroup.Android, "NOTIFICATION");
//            }
//        }
//
//        if (enableProperty.boolValue)
//        {
//            _notificationSetting.DrawChildrenDefault(nameof(NotificationSetting.enable));
//        }
//
//        EditorGUI.indentLevel--;
//        EditorGUILayout.EndVertical();
//    }

//    private void DrawDailyReward()
//    {
//        EditorGUILayout.BeginVertical(GUI.skin.box);
//        var enableProperty = _dailyRewardSetting.FindPropertyRelative(nameof(DailyRewardSetting.enable));
//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Daily Reward Setting", EditorStyles.boldLabel);
//        var enable = EditorGUILayout.ToggleLeft("", enableProperty.boolValue);
//        EditorGUILayout.EndHorizontal();
//        EditorGUI.indentLevel++;
//        if (enable != enableProperty.boolValue)
//        {
//            enableProperty.boolValue = enable;
//            if (enableProperty.boolValue)
//            {
//                AddBuildSymbol(BuildTargetGroup.iOS, "DAILY_REWARD");
//                AddBuildSymbol(BuildTargetGroup.Android, "DAILY_REWARD");
//            }
//            else
//            {
//                RemoveBuildSymbol(BuildTargetGroup.iOS, "DAILY_REWARD");
//                RemoveBuildSymbol(BuildTargetGroup.Android, "DAILY_REWARD");
//            }
//        }
//
//        if (enableProperty.boolValue)
//        {
//            _dailyRewardSetting.DrawChildrenDefault(nameof(DailyRewardSetting.enable));
//        }
//
//        EditorGUI.indentLevel--;
//        EditorGUILayout.EndVertical();
//    }

    private void DrawPrivatePolicy()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        var enableProperty = _privatePolicySetting.FindPropertyRelative(nameof(PrivatePolicySetting.enable));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Private Policy", EditorStyles.boldLabel);
        enableProperty.boolValue = EditorGUILayout.ToggleLeft("", enableProperty.boolValue);
        EditorGUILayout.EndHorizontal();
      
  

        if (enableProperty.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_privatePolicySetting.FindPropertyRelative(nameof(PrivatePolicySetting.url)));
            EditorGUI.indentLevel--;
        }

       
        EditorGUILayout.EndVertical();
    }


    private void DrawInApp()
    {
        var enableProperty = _inAppSetting.FindPropertyRelative(nameof(InAppSetting.enable));

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("In App", EditorStyles.boldLabel);
        var enable = EditorGUILayout.ToggleLeft("", enableProperty.boolValue);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        if (enable != enableProperty.boolValue)
        {
            enableProperty.boolValue = enable;
            if (enableProperty.boolValue)
            {
                AddBuildSymbol(NamedBuildTarget.iOS, "IN_APP");
                AddBuildSymbol(NamedBuildTarget.Android, "IN_APP");
            }
            else
            {
                RemoveBuildSymbol(NamedBuildTarget.iOS, "IN_APP");
                RemoveBuildSymbol(NamedBuildTarget.Android, "IN_APP");
            }
        }


        if (enableProperty.boolValue)
        {
            _inAppSetting.DrawChildrenDefault(nameof(InAppSetting.enable));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

//    private void DrawLeadersBoard()
//    {
//        var iosEnableProperty = _iosLeadersboardSetting.FindPropertyRelative(nameof(LeadersboardSetting.enable));
//        var androidEnableProperty =
//            _androidLeadersboardSetting.FindPropertyRelative(nameof(LeadersboardSetting.enable));
//
//        EditorGUILayout.BeginVertical(GUI.skin.box);
//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Leadersboard Setting", EditorStyles.boldLabel);
//        var toggleValue =
//            EditorGUILayout.ToggleLeft("", iosEnableProperty.boolValue || androidEnableProperty.boolValue);
//
//        if (toggleValue != (androidEnableProperty.boolValue || iosEnableProperty.boolValue))
//        {
//            androidEnableProperty.boolValue = toggleValue;
//            iosEnableProperty.boolValue = toggleValue;
//        }
//
//        EditorGUILayout.EndHorizontal();
//        if (toggleValue)
//        {
//            EditorGUILayout.Space();
//
//            EditorGUILayout.BeginVertical(GUI.skin.box);
//            EditorGUI.indentLevel++;
//            var iosEnable = EditorGUILayout.Toggle("Ios", iosEnableProperty.boolValue);
//
//            if (iosEnable != iosEnableProperty.boolValue)
//            {
//                iosEnableProperty.boolValue = iosEnable;
//
//                if (iosEnable)
//                    AddBuildSymbol(BuildTargetGroup.iOS, "GAME_SERVICE");
//                else
//                {
//                    RemoveBuildSymbol(BuildTargetGroup.iOS, "GAME_SERVICE");
//                }
//
//                //            HandleScriptingSymbol(BuildTargetGroup.iOS, iosEnable);
//            }
//
//            if (iosEnableProperty.boolValue)
//            {
//                EditorGUI.indentLevel++;
//                _iosLeadersboardSetting.DrawChildrenDefault(nameof(LeadersboardSetting.enable));
//                EditorGUI.indentLevel--;
//            }
//
//            EditorGUI.indentLevel--;
//            EditorGUILayout.EndVertical();
//            EditorGUILayout.BeginVertical(GUI.skin.box);
//            EditorGUI.indentLevel++;
//
//            var androidEnable = EditorGUILayout.Toggle("Android", androidEnableProperty.boolValue);
//
//            if (androidEnable != androidEnableProperty.boolValue)
//            {
//                androidEnableProperty.boolValue = androidEnable;
//
//                if (androidEnable)
//                    AddBuildSymbol(BuildTargetGroup.Android, "GAME_SERVICE");
//                else
//                {
//                    RemoveBuildSymbol(BuildTargetGroup.Android, "GAME_SERVICE");
//                }
//
//                //            HandleScriptingSymbol(BuildTargetGroup.iOS, iosEnable);
//            }
//
//            if (androidEnableProperty.boolValue)
//            {
//                EditorGUI.indentLevel++;
//                _androidLeadersboardSetting.DrawChildrenDefault(nameof(LeadersboardSetting.enable));
//                EditorGUI.indentLevel--;
//            }
//
//            EditorGUI.indentLevel--;
//            EditorGUILayout.EndVertical();
//        }
//
//        EditorGUILayout.EndVertical();
//    }

    private void DrawAdmobSetting(SerializedProperty iosAdmobSetting, SerializedProperty androidAdmobSetting)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        iosAdmobSetting.isExpanded = EditorGUILayout.Foldout(iosAdmobSetting.isExpanded, "Admob Setting");

        if (iosAdmobSetting.isExpanded)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            var iosEnableProperty = iosAdmobSetting.FindPropertyRelative(nameof(AdmobSetting.enable));
            var iosEnable = EditorGUILayout.Toggle("Ios", iosEnableProperty.boolValue);

            if (iosEnable != iosEnableProperty.boolValue)
            {
                iosEnableProperty.boolValue = iosEnable;
                HandleScriptingSymbol(NamedBuildTarget.iOS, iosEnable, "ADMOB");
            }

            if (iosEnableProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                iosAdmobSetting.DrawChildrenDefault(nameof(AdmobSetting.enable));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            var androidEnableProperty = androidAdmobSetting.FindPropertyRelative(nameof(AdmobSetting.enable));
            var androidEnable = EditorGUILayout.Toggle("Android", androidEnableProperty.boolValue);

            if (androidEnable != androidEnableProperty.boolValue)
            {
                androidEnableProperty.boolValue = androidEnable;
                HandleScriptingSymbol(NamedBuildTarget.Android, androidEnable, "ADMOB");
            }

            if (androidEnableProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                androidAdmobSetting.DrawChildrenDefault(nameof(AdmobSetting.enable));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }



//    private void DrawAdColonySetting(SerializedProperty iosAdColonySetting, SerializedProperty androidAdColonySetting)
//    {
//        EditorGUILayout.BeginVertical(GUI.skin.box);
//        iosAdColonySetting.isExpanded = EditorGUILayout.Foldout(iosAdColonySetting.isExpanded, "AdColony Setting");
//        if (iosAdColonySetting.isExpanded)
//        {
//            EditorGUILayout.Space();
//
//            EditorGUILayout.BeginVertical(GUI.skin.box);
//            EditorGUI.indentLevel++;
//            var iosEnableProperty = iosAdColonySetting.FindPropertyRelative(nameof(AdColonySettings.enable));
//
//            var iosEnable = EditorGUILayout.Toggle("Ios", iosEnableProperty.boolValue);
////
//            if (iosEnable != iosEnableProperty.boolValue)
//            {
//                iosEnableProperty.boolValue = iosEnable;
//                HandleScriptingSymbol(BuildTargetGroup.iOS, iosEnable, "ADCLONY");
//            }
//
////
//            if (iosEnableProperty.boolValue)
//            {
//                EditorGUI.indentLevel++;
//                iosAdColonySetting.DrawChildrenDefault(nameof(AdColonySettings.enable));
//
////            _iosAdColonySetting.NextVisible(false);
//                EditorGUI.indentLevel--;
//            }
//
//            EditorGUI.indentLevel--;
//            EditorGUILayout.EndVertical();
//            EditorGUILayout.BeginVertical(GUI.skin.box);
//            EditorGUI.indentLevel++;
//            var androidEnableProperty = androidAdColonySetting.FindPropertyRelative(nameof(AdColonySettings.enable));
//            var androidEnable = EditorGUILayout.Toggle("Android", androidEnableProperty.boolValue);
//
//            if (androidEnable != androidEnableProperty.boolValue)
//            {
//                androidEnableProperty.boolValue = androidEnable;
//                HandleScriptingSymbol(BuildTargetGroup.Android, androidEnable, "ADCLONY");
//            }
//
//            if (androidEnableProperty.boolValue)
//            {
//                EditorGUI.indentLevel++;
//                androidAdColonySetting.DrawChildrenDefault(nameof(AdColonySettings.enable));
//                EditorGUI.indentLevel--;
//            }
//
//            EditorGUI.indentLevel--;
//            EditorGUILayout.EndVertical();
//        }
//
//        EditorGUILayout.EndVertical();
//    }


    private void DrawAppId()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("App Ids", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_iosAppId);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private static bool HaveBuildSymbol(NamedBuildTarget target, string symbol)
    {
        var defines = PlayerSettings.GetScriptingDefineSymbols(target);
        var strings = defines.Split(';').ToList();
        return strings.Contains(symbol);
    }

    private static void AddBuildSymbol(NamedBuildTarget target, string symbol)
    {
        if (HaveBuildSymbol(target, symbol))
            return;
        var defines = PlayerSettings.GetScriptingDefineSymbols(target);
        var strings = defines.Split(';').ToList();
        strings.Add(symbol);
        PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", strings.Distinct()));
    }

    private static void RemoveBuildSymbol(NamedBuildTarget target, string symbol)
    {
        if (!HaveBuildSymbol(target, symbol))
            return;
        var defines = PlayerSettings.GetScriptingDefineSymbols(target);
        var strings = defines.Split(';').ToList();
        strings.Remove(symbol);
        PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", strings.Distinct()));
    }

    private static void HandleScriptingSymbol(NamedBuildTarget target, bool enable, string key)
    {
        var defines = PlayerSettings.GetScriptingDefineSymbols(target);
        var strings = defines.Split(';').ToList();
        if (enable)
            strings.Add(key);
        else
            strings.Remove(key);
        PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", strings.Distinct()));
    }
}


public static class EditorExtensions
{
    public static void DrawChildrenDefault(this SerializedProperty property,
        params string[] exceptChildren)
    {
        var exceptList = exceptChildren?.ToList() ?? new List<string>();

        property = property.Copy();

        var parentDepth = property.depth;
        if (property.NextVisible(true) && parentDepth < property.depth)
        {
            do
            {
                if (exceptList.Contains(property.name))
                    continue;
                EditorGUILayout.PropertyField(property, true);
            } while (property.NextVisible(false) && parentDepth < property.depth);
        }
    }
}
