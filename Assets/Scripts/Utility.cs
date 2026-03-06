using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utility
{
  public static async Task LoadAdditiveAsync(string sceneName)
  {
    if (!SceneManager.GetSceneByName(sceneName).isLoaded)
    {
      await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
  }

  public static async Task UnloadAsync(string sceneName)
  {
    if (SceneManager.GetSceneByName(sceneName).isLoaded)
    {
      await SceneManager.UnloadSceneAsync(sceneName);
    }
  }
}