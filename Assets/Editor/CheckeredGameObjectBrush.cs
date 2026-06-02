using UnityEngine;
using UnityEditor.Tilemaps;

namespace UnityEditor
{
  [CustomGridBrush(true, false, false, "Checkered Game Object Brush")]
  public class CheckeredGameObjectBrush : GridBrushBase
  {
    public GameObject prefabEven;
    public GameObject prefabOdd;

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
      // Do not paint if prefabs are missing
      if (prefabEven == null || prefabOdd == null) return;

      // Determine if the cell is even or odd using X and Y grid coordinates
      bool isEven = (Mathf.Abs(position.x) + Mathf.Abs(position.y)) % 2 == 0;
      GameObject selectedPrefab = isEven ? prefabEven : prefabOdd;

      // Prevent stacking multiple objects in the exact same cell
      Transform existingObject = GetObjectInCell(grid, brushTarget, position);
      if (existingObject != null) return;

      // Instantiate and align the GameObject to the grid cell
      GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
      if (instance != null)
      {
        instance.transform.SetParent(brushTarget.transform);
        instance.transform.position = grid.CellToWorld(position) + grid.GetComponent<Grid>().cellGap;

        // Register the action for Unity's Undo system
        Undo.RegisterCreatedObjectUndo(instance, "Paint Checkered GameObject");
      }
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
      Transform existingObject = GetObjectInCell(grid, brushTarget, position);
      if (existingObject != null)
      {
        Undo.DestroyObjectImmediate(existingObject.gameObject);
      }
    }

    private Transform GetObjectInCell(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
      Vector3 targetWorldPos = grid.CellToWorld(position);
      foreach (Transform child in brushTarget.transform)
      {
        // Verify if the object matches the cell position
        if (Vector3.Distance(child.position, targetWorldPos) < 0.01f)
        {
          return child;
        }
      }
      return null;
    }
  }
}
