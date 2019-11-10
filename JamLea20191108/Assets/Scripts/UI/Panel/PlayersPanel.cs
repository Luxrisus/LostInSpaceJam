using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersPanel : MonoBehaviour
{
    [SerializeField]
    PlayerHud _playerHud = null;

    [SerializeField]
    GameObject _playersHud = null;

    private List<PlayerHud> _playerHuds = new List<PlayerHud>();

    public PlayerHud AddPlayerHud(Color color)
    {
        PlayerHud newPlayer = Instantiate(_playerHud, _playersHud.transform);
        newPlayer.SetColor(color);
        _playerHuds.Add(newPlayer);

        return newPlayer;
    }

    public void PlayerDied(PlayerHud hud)
    {
        _playerHuds.Remove(hud);
        Destroy(hud.gameObject);
    }
}
