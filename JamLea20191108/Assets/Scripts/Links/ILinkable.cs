using UnityEngine;

public interface ILinkable
{
    void OnLink(Linker owner);
    void OnUnlink(Linker owner);
    Vector3 GetPosition();
    bool IsLinked();
}
