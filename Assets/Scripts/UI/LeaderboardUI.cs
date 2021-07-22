using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{

    [SerializeField] private List<LeaderboardElementUI> leaderboardElements = new List<LeaderboardElementUI>();

    public void SetLeaderboard(in List<Player> players, in Player mainPlayer)
    {
        bool bMainPlayerIn = false;

        for (int i = 0; i < leaderboardElements.Count && i < players.Count; i++)
        {

            bMainPlayerIn |= players[i].IsMainPlayer;
            leaderboardElements[i].SetElement(players[i].PlayerName, i, players[i].IsMainPlayer);
        }



        if (!bMainPlayerIn && mainPlayer!=null)
        {
            int mainPlayerIndex = -1;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsMainPlayer)
                {
                    mainPlayerIndex = i;
                    break;
                }

            }
            leaderboardElements[leaderboardElements.Count-1].SetElement(mainPlayer.PlayerName, mainPlayerIndex, true);
        }
    }
}
