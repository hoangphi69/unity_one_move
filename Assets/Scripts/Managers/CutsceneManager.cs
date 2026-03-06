using System.Threading.Tasks;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
  public static CutsceneManager Instance { get; private set; }

  // Configs
  [SerializeField] private SceneField cutscene;

  private TaskCompletionSource<bool> _cutsceneTask;

  void Awake()
  {
    Instance = this;
  }

  public async Task StartCutscene(string knot)
  {
    _cutsceneTask = new();
    await Utility.LoadAdditiveAsync(cutscene);
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(knot, DialogueMode.Cutscene);
    await _cutsceneTask.Task;
  }

  public void EndCutscene()
  {
    _cutsceneTask?.TrySetResult(true);
  }

  public async Task HideCutscene()
  {
    await Utility.UnloadAsync(cutscene);
  }
}