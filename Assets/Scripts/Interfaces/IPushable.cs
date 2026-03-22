using UnityEngine;
using System.Threading.Tasks;

public interface IPushable
{
    bool CanPush(Vector3 direction);
    Task Push(Vector3 direction);
}