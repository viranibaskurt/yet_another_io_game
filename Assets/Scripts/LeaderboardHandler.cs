using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardHandler
{
    private Player currentLeader;
    private List<Player> leaderboard = new List<Player>();

    public Player CurrentLeader { get { return leaderboard.Count>0? leaderboard[0]:null; } }
    public List<Player> Leaderboard { get { return leaderboard; } }

    public void RefreshLeaderboard(List<Player> players)
    {
        leaderboard = new List<Player>();
        players.ForEach(item => leaderboard.Add(item));

        leaderboard.Sort((a, b) => b.Level.CompareTo(a.Level));
        if (leaderboard.Count > 0)
        {
            if (currentLeader)
                currentLeader.IsLeader = false;
            currentLeader = leaderboard[0];
            currentLeader.IsLeader = true;
        }
    }
}
