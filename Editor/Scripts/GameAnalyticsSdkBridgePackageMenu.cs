#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class GameAnalyticsSdkBridgePackageMenu
{
    private const string MenuRoot = "Tools/Package/GameAnalytics SDK Bridge/";
    private const string ReadmePath = "Packages/com.actionfit.sdk.gameanalytics/README.md";
    private const int ReadmePriority = 901;

    [MenuItem(MenuRoot + "README", false, ReadmePriority)]
    private static void OpenReadme()
    {
        var readme = AssetDatabase.LoadAssetAtPath<TextAsset>(ReadmePath);
        if (readme == null)
        {
            EditorUtility.DisplayDialog("GameAnalytics SDK Bridge", $"README was not found.\n{ReadmePath}", "OK");
            return;
        }

        Selection.activeObject = readme;
        AssetDatabase.OpenAsset(readme);
    }
}
#endif
