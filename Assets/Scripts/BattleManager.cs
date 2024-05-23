using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

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
        DebugOpt.Log("BattleManager : RegisterPlayer : " + (player == null) + " :: " + (int)player.playerClass + " :: " + player.name);
        if (player == null) return;
        players[(int)player.playerClass] = player;
    }
    public void DeregisterMonster(Monster monster)
    {
        DebugOpt.Log("BattleManager : DeregisterMonster : " + monster.name);
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
    public bool isAnyPlayerAlive()
    {
        // 한 명이라도 플레이어 캐릭터 생존해있는지
        foreach (Player player in players)
        {
            if (player != null) return true;
        }
        return false;
    }
    public bool isAllMonstersCleared()
    {
        // 한 명이라도 플레이어 캐릭터 생존해있는지
        return monsters.Count == 0;
    }
    public int test()
    {
        return monsters.Count;

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
            if (monster == null) continue;
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
            if (monster == null) continue;

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
    private Player GetPlayerLeftmost()
    {
        Player leftmostPlayer = null;
        foreach (Player player in players)
        {
            if (player == null) continue;

            if (leftmostPlayer == null) leftmostPlayer = player;
            else
            {
                Vector3 curPos = player.transform.position;
                Vector3 leftPos = leftmostPlayer.transform.position;
                if (curPos.x < leftPos.x)
                {
                    leftmostPlayer = player;
                }
            }
        }
        return leftmostPlayer;
    }
    private Player GetPlayerRightmost()
    {
        Player rightmostPlayer = null;
        foreach (Player player in players)
        {
            if (player == null) continue;

            if (rightmostPlayer == null) rightmostPlayer = player;
            else
            {
                Vector3 curPos = player.transform.position;
                Vector3 leftPos = rightmostPlayer.transform.position;
                if (curPos.x < leftPos.x)
                {
                    rightmostPlayer = player;
                }
            }
        }
        return rightmostPlayer;
    }
    public Vector3 SpawnOnRandomPosition(float minDistance, float maxDistance)
    {
        int flag = Random.Range(0, 2);
        if (flag == 0) return SpawnOnRandomLeftmost(minDistance, maxDistance);
        else return SpawnOnRandomRightmost(minDistance, maxDistance);
    }
    private Vector3 SpawnOnRandomLeftmost(float minDistance, float maxDistance)
    {
        // 플레이어들 각각에 대해 minDistance 이상 maxDistance 이하 거리에 떨어진 랜덤 좌표 반환
        // 거의 찾지 못해 무한대기가 일어나기에 레거시 코드는 폐기.
        Vector3 randomPosition = Vector3.zero;
        bool isValid = false;
        Transform leftmostPlayerTransform = GetPlayerLeftmost().transform;
        while (!isValid)
        {
            randomPosition = new Vector3(
                Random.Range((-1) * maxDistance, 0),
                Random.Range((-1) * maxDistance, maxDistance),
                0.0f
            );
            isValid = true;

            float distance = Vector3.Distance(Vector3.zero, randomPosition);
            if (!(distance >= minDistance && distance <= maxDistance))
            {
                isValid = false;
                break;
            }
        }
        Vector3 result = new Vector3(randomPosition.x + leftmostPlayerTransform.position.x,
            randomPosition.y + leftmostPlayerTransform.position.y, 0);
        return result;
    }
    private Vector3 SpawnOnRandomRightmost(float minDistance, float maxDistance)
    {
        // 플레이어들 각각에 대해 minDistance 이상 maxDistance 이하 거리에 떨어진 랜덤 좌표 반환
        // 거의 찾지 못해 무한대기가 일어나기에 레거시 코드는 폐기.
        Vector3 randomPosition = Vector3.zero;
        bool isValid = false;
        Transform rightmostPlayerTransform = GetPlayerRightmost().transform;
        while (!isValid)
        {
            randomPosition = new Vector3(
                Random.Range(0, maxDistance),
                Random.Range((-1) * maxDistance, maxDistance),
                0.0f
            );
            isValid = true;

            float distance = Vector3.Distance(Vector3.zero, randomPosition);
            if (!(distance >= minDistance && distance <= maxDistance))
            {
                isValid = false;
                break;
            }
        }
        Vector3 result = new Vector3(randomPosition.x + rightmostPlayerTransform.position.x,
            randomPosition.y + rightmostPlayerTransform.position.y, 0);
        return result;
    }

    public Vector3 SpawnOnRandomPosition_Legacy(float minDistance, float maxDistance)
    {
        // 플레이어들 각각에 대해 minDistance 이상 maxDistance 이하 거리에 떨어진 랜덤 좌표 반환
        // 거의 찾지 못해 무한대기가 일어나기에 레거시 코드는 폐기.
        Vector3 randomPosition = Vector3.zero;
        bool isValid = false;
        while (!isValid)
        {
            randomPosition = new Vector3(
                Random.Range(-10, 11),
                Random.Range(-10, 11),
                0.0f
            );
            isValid = true;
            foreach (Player player in players)
            {
                if (player == null) continue;

                float distance = Vector3.Distance(player.transform.position, randomPosition);
                if (!(distance >= minDistance && distance <=  maxDistance))
                {
                    isValid = false;
                    break;
                }
            }
        }
        return randomPosition;
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

    public void PlayerVictory()
    {
        DebugOpt.Log("BattleManager:  PlayerVictory");
        foreach (var player in players)
        {
            player.SetAnimTrigger("Victory");
        }
    }
}
