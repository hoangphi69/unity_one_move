using UnityEngine;
using System.Threading.Tasks;

public interface IPushable
{
    Task<bool> Push(Vector3 direction);
}