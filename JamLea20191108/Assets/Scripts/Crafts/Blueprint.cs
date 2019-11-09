using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "Craftable Elements", order = 1)]
public class Blueprint : ScriptableObject
{
    public List<string> m_elementsNeeded;

}
