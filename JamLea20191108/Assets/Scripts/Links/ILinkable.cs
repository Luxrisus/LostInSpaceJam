using UnityEngine;

public interface ILinkable
{
    //void OnLinkadded(Linker owner);
    //void OnLinkDestroyed(Linker owner);
    Vector3 GetPosition();
    bool IsLinked();

    //bool CanMove(); apply restrictions based on distance and max count
}
