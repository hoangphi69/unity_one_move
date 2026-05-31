using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SyncOrthographicSize : MonoBehaviour
{
  [Tooltip("Drag your Main Camera (with the Cinemachine Brain) here")]
  public Camera mainCamera;

  private Camera childCamera;

  private void Awake()
  {
    // Cache the child camera component attached to this GameObject
    childCamera = GetComponent<Camera>();
  }

  // Using LateUpdate ensures this runs after standard movement logic
  private void LateUpdate()
  {
    if (mainCamera != null && childCamera.orthographicSize != mainCamera.orthographicSize)
    {
      // Update the projection size strictly, ignoring the Transform entirely
      childCamera.orthographicSize = mainCamera.orthographicSize;
    }
  }
}