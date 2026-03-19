// // Source - https://stackoverflow.com/a/48817315
// // Posted by 3Dynamite, modified by community. See post 'Timeline' for change history
// // Retrieved 2026-03-03, License - CC BY-SA 3.0

// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEditor.SceneManagement;

// [InitializeOnLoadAttribute]
// public static class DefaultSceneLoader
// {
//   static DefaultSceneLoader()
//   {
//     EditorApplication.playModeStateChanged += LoadDefaultScene;
//   }

//   static void LoadDefaultScene(PlayModeStateChange state)
//   {
//     if (state == PlayModeStateChange.ExitingEditMode)
//     {
//       EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
//     }

//     if (state == PlayModeStateChange.EnteredPlayMode)
//     {
//       EditorSceneManager.LoadScene(0);
//     }
//   }
// }
// #endif
