using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft_OxygenRelay : ACraftable
{
    private int _maximumNumberOfElementesConnected = 1;
    private List<GameObject> _playersConnected = new List<GameObject>();
    private List<GameObject> _plantsConnected = new List<GameObject>();

    public bool CanConnectNewObject()
    {
        return _playersConnected.Count + _playersConnected.Count < _maximumNumberOfElementesConnected;
    }

    public void SetMaximumOfConnection(int newMax)
    {
        _maximumNumberOfElementesConnected = newMax;
    }

    public void ConnectPlayer(bool isConnecting, GameObject player)
    {
        if (!CanConnectNewObject())
        {
            return;
        }

        if (isConnecting)
        {
            _playersConnected.Add(player);
        }
        else
        {
            _playersConnected.Remove(player);
        }
    }

    public void ConnectPlant(bool isConnecting, GameObject plant)
    {
        if (!CanConnectNewObject())
        {
            return;
        }

        if (isConnecting)
        {
            _plantsConnected.Add(plant);
        }
        else
        {
            _plantsConnected.Remove(plant);
        }
    }
}
