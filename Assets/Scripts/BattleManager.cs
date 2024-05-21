using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BattleManager : Singleton<BattleManager>
{
    // 임시 빅토리 코드
    public void PlayerVictory()
    {
        DebugOpt.Log("BattleManager:  PlayerVictory");
        foreach (var player in players)
        {
            player.SetAnimTrigger("Victory");
        }
    }

    public void KillPlayers()
    {
        DebugOpt.Log("BattleManager:  KillPlayers");
        foreach (var player in players)
        {
            player.SetAnimTrigger("Death");
        }
    }
    public void PlayersCastSkill()
    {
        DebugOpt.Log("BattleManager:  PlayersCastSkill");
        foreach (var player in players)
        {
            player.SetAnimTrigger("CastSkill");
        }
    }

    /// <summary>
    /// 공격 판정 관한 매니징
    /// </summary>

    public void AttackFromPlayerToMonster(Player player, Monster monster, float damage)
    {
        if (player == null || monster == null)    return;
        
        monster.BeAttacked(damage);
    }

    public void AttackFromMonsterToPlayer(Monster monster, Player player, float damage)
    {
        if (player == null || monster == null) return;

        player.BeAttacked(damage);
    }

    // 임시 범위 공격용
    ////////////////////////
    public void AttackAreaFromPlayer(Player player, float skillRange, float damage)
    {
        // skill Range 내 모든 monster 검색해서
        foreach (Monster monster in monsters)
        {
            if (isInArea(monster, player.transform.position, skillRange))
                monster.BeAttacked(damage);
        }
    }

    private bool isInArea(Monster monster, Vector3 centerPoint, float range)
    {
        float distance = Vector2.Distance(monster.transform.position, centerPoint);
        return (distance <= range);
    }
    ////////////////////////

    /// <summary>
    /// 추적과 관련한 매니징
    /// 
    /// 추적에 있어서 최적화 : 사전 거리 필터링 :: 
    /// 모든 객체와의 거리를 계산하기 전에, 대략적인 거리 필터링을 수행합니다. 예를 들어, 일정 거리 이상 떨어진 객체는 처음부터 제외시킵니다.
    /// </summary>

    private List<Monster> monsters = new List<Monster>();
    private List<Player> players = new List<Player>();

    // 씬에서 monster를 관리하기 위해 ... 차후 monster pool 사용할 때 리팩토링 가능할수도
    public void RegisterMonster(Monster monster)
    {
        monsters.Add(monster);
    }
    // 씬에서 player 관리 위해
    public void RegisterPlayer(Player player)
    {
        players.Add(player);
    }

    public void DeregisterMonster(Monster monster)
    {
        monsters.Remove(monster);
    }
    // 씬에서 player 관리 위해
    public void DeregisterPlayer(Player player)
    {
        players.Remove(player);
    }

    // 가장 가까운 대상을 탐지하되, 탐지 범위 내에 있는 대상에 한함. 탐지 모두 안된다면 null 리턴.

    public Player FindNearestPlayer(Monster monster)
    {
        float shortestDistance = Mathf.Infinity;
        Player nearest = null;
        foreach (Player player in players)
        {
            float distanceToPlayer = Vector2.Distance(monster.transform.position, player.transform.position);
            if (distanceToPlayer > monster.sightRange) continue; // 사전 필터링

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearest = player;
            }
        }
        return nearest;
    }

    public Monster FindNearestMonster(Player player)
    {
        float shortestDistance = Mathf.Infinity;
        Monster nearest = null;
        foreach (Monster monster in monsters)
        {
            float distanceToPlayer = Vector2.Distance(monster.transform.position, player.transform.position);
            if (distanceToPlayer > player.sightRange) continue; // 사전 필터링

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearest = monster;
            }
        }
        return nearest;
    }
}
