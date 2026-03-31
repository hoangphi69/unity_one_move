using UnityEngine;

public class Obstacle : MonoBehaviour
{
  [field: SerializeField] public bool BlockPlayer { get; private set; } = true;
  [field: SerializeField] public bool BlockEnemy { get; private set; } = true;
  [field: SerializeField] public bool BlockEnemySight { get; private set; } = false;
}