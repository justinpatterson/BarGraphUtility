using UnityEngine;
namespace Prisms.Assignment
{
    public interface IMoveable
    {
        void Click(Vector3 ScreenPos);
        void Drag(Vector3 WorldPos);
        void Release(Vector3 ScreenPos);
    }
}