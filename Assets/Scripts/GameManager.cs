using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{


    // 
    public void SpawnPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            var playerObj = PlayerPoolManager.GetFromPool((PlayerClass)i);
            playerObj.transform.position = new Vector3( (i + 1), (i % 2) * (-2), 0.0f);
        }
    }

    public void SpawnGoblin()
    {
        var goblinObj = MonsterPoolManager.GetFromPool();
        goblinObj.transform.position = new Vector3(-1, 0, 0.0f);
    }

    // 임시 테스트 코드 : 승리 트리거 발동

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
