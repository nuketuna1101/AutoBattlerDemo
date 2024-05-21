using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // �ӽ� �׽�Ʈ �ڵ� : �¸� Ʈ���� �ߵ�

    public void Victory()
    {
        DebugOpt.Log("GameManager: Victory clicked");
        BattleManager.Instance.PlayerVictory();
    }

    public void PlayerDeath()
    {
        DebugOpt.Log("GameManager: PlayerDeath clicked");
        BattleManager.Instance.KillPlayers();
    }

    public void PlayerCastSkill()
    {
        DebugOpt.Log("GameManager: PlayerCastSkill clicked");
        BattleManager.Instance.PlayersCastSkill();
    }
}
