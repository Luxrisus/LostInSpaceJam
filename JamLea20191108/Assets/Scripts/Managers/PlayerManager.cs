using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : AManager
{
    [SerializeField]
    private List<Color> _playerColors = new List<Color>();

    private List<bool> _colorUsed = new List<bool>();

    private List<Player> _players = new List<Player>();

    public override void Initialize()
    {
        _players.Clear();

        for(int i = 0; i < _playerColors.Count; ++i)
        {
            _colorUsed.Add(false);
        }
    }

    public void PlayerJoined(Player player)
    {
        if (player == null)
        {
            return;
        }

        int currentIndex = 0;
        foreach(var colorUsed in _colorUsed)
        {
            if (!colorUsed)
            {
                break;
            }

            currentIndex++;
        }

        Color color = _playerColors[currentIndex];
        _colorUsed[currentIndex] = true;

        _players.Add(player);

        player.SetColor(color);
        var playerHud = ManagersManager.Instance.Get<UIManager>().PlayersPanel.AddPlayerHud(color);
        player.AddPlayerHud(playerHud);
        player.transform.SetParent(FindObjectOfType<LevelLayoutHook>().transform);
    }

    public void PlayerDied(Player player)
    {
        int index = 0;

        for(int i = 0; i < _players.Count; ++i)
        {
            if (_players[i] == player)
            {
                index = i;
                break;
            }
        }

        _players.Remove(player);
        _colorUsed[index] = false;

        if (_players.Count == 0)
        {
            ManagersManager.Instance.Get<LevelManager>().EndOfLevel(false);
        }
    }
}
