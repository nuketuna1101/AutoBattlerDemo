using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BattleManager : Singleton<BattleManager>
{   
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 배틀매니저로 인게임에서 존재할 플레이어 캐릭터 및 몬스터에 관한 등록/해제 관리.
    /// 
    /// 플레이어든 몬스터든 풀에서 할당/해제와 함께 진행되어야 한다.
    /// </summary>

    private List<Monster> monsters = new List<Monster>();
    private Player[] players = new Player[4];       public Player[] Players { get { return players; } }
    public void RegisterMonster(Monster monster)
    {
        monsters.Add(monster);
    }
    public void RegisterPlayer(Player player)
    {
        if (player == null) return;
        players[(int)player.playerClass] = player;
    }
    public void DeregisterMonster(Monster monster)
    {
        monsters.Remove(monster);
    }
    public void DeregisterPlayer(Player player)
    {
        if (player == null) return;
        players[(int)player.playerClass] = null;
    }
    public bool isExistingPlayer(Player player)
    {
        return (players[(int)player.playerClass] != null);
    }
    public bool isExistingMonster(Monster monster)
    {
        return (monsters.Contains(monster));
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 공격 판정 관한 매니징
    /// </summary>
    public void AttackFromPlayerToMonster(Player player, Monster targetMonster, float damage)
    {
        if (player == null || targetMonster == null) return;
        targetMonster.BeAttacked(damage);
    }
    public void AttackFromMonsterToPlayer(Monster monster, Player targetPlayer, float damage)
    {
        if (targetPlayer == null || monster == null) return;
        targetPlayer.BeAttacked(damage);
    }
    public void AttackAreaFromPlayer(Player player, float skillRange, float damage)
    {
        // 범위공격: skill Range 내 모든 monster 검색해서
        foreach (Monster monster in monsters)
        {
            if (isInArea(monster, player.transform.position, skillRange))
                monster.BeAttacked(damage);
        }
    }
    public void HealPlayer(Player healer, Player targetPlayer, float amount)
    {
        if (healer == null || targetPlayer == null) return;
        targetPlayer.BeHealed(amount);
    }
    public void HealAnyPlayerInRange(Player healer, float skillRange, float amount)
    {
        foreach (Player player in players)
        {
            if (player == null
                || !isInArea(player, healer.transform.position, skillRange)
                || player.health == player.maxHealth)
                continue;
            player.BeHealed(amount);
            break;
        }
    }
    public void GiveStunned(Player player, Monster targetMonster, float duration)
    {
        targetMonster.GetStunned(duration);
    }
    // 사용하진 않았지만 넉백 관련 코드
    private void KnockBackFromPlayerToMonster(Player player, Monster monster)
    {
        Vector3 direction = (monster.transform.position - player.transform.position).normalized;
        monster.transform.position = monster.transform.position + direction * 0.25f;
    }
    private void KnockBackFromMonsterToPlayer(Monster monster, Player player)
    {
        Vector3 direction = (player.transform.position - monster.transform.position).normalized;
        player.transform.position = player.transform.position + direction * 0.25f;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 대상 탐지 관련
    public Player FindNearestPlayer(Monster monster)
    {
        float shortestDistance = Mathf.Infinity;
        Player nearest = null;
        foreach (Player player in players)
        {
            if (player == null) continue;

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
    private bool isInArea(Monster monster, Vector3 centerPoint, float range)
    {
        float distance = Vector2.Distance(monster.transform.position, centerPoint);
        return (distance <= range);
    }
    private bool isInArea(Player player, Vector3 centerPoint, float range)
    {
        float distance = Vector2.Distance(player.transform.position, centerPoint);
        return (distance <= range);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 배틀 씬에서의 플레이어 캐릭터의 부활 관련
    /// </summary>
    private Coroutine[] respawnCoroutines = new Coroutine[4];
    public void RespawnPlayer(Player player)
    {
        DebugOpt.Log("BattleManager - RespawnPlayer called " + player.name);
        int index = (int)player.playerClass;
        CoroutineHelper.RestartCor(this, ref respawnCoroutines[index], RespawnRoutine(player));
    }
    private IEnumerator RespawnRoutine(Player player)
    {
        DebugOpt.Log("BattleManager - RespawnRoutine called " + player.name);
        yield return new WaitForSeconds(player.respawnCycle);
        var playerObj = PlayerPoolManager.GetFromPool(player.playerClass);
        int index = (int)player.playerClass;
        playerObj.transform.position = new Vector3((index + 1), (index % 2) * (-2), 0.0f);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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
}
