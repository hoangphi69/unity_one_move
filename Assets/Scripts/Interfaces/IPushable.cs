using UnityEngine;
using System.Threading.Tasks;

public interface IPushable
{
    Task Push(Vector3 direction);
}