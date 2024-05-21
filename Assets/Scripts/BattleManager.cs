using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BattleManager : Singleton<BattleManager>
{
    // �ӽ� ���丮 �ڵ�
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
    /// ���� ���� ���� �Ŵ�¡
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

    // �ӽ� ���� ���ݿ�
    ////////////////////////
    public void AttackAreaFromPlayer(Player player, float skillRange, float damage)
    {
        // skill Range �� ��� monster �˻��ؼ�
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
    /// ������ ������ �Ŵ�¡
    /// 
    /// ������ �־ ����ȭ : ���� �Ÿ� ���͸� :: 
    /// ��� ��ü���� �Ÿ��� ����ϱ� ����, �뷫���� �Ÿ� ���͸��� �����մϴ�. ���� ���, ���� �Ÿ� �̻� ������ ��ü�� ó������ ���ܽ�ŵ�ϴ�.
    /// </summary>

    private List<Monster> monsters = new List<Monster>();
    private List<Player> players = new List<Player>();

    // ������ monster�� �����ϱ� ���� ... ���� monster pool ����� �� �����丵 �����Ҽ���
    public void RegisterMonster(Monster monster)
    {
        monsters.Add(monster);
    }
    // ������ player ���� ����
    public void RegisterPlayer(Player player)
    {
        players.Add(player);
    }

    public void DeregisterMonster(Monster monster)
    {
        monsters.Remove(monster);
    }
    // ������ player ���� ����
    public void DeregisterPlayer(Player player)
    {
        players.Remove(player);
    }

    // ���� ����� ����� Ž���ϵ�, Ž�� ���� ���� �ִ� ��� ����. Ž�� ��� �ȵȴٸ� null ����.

    public Player FindNearestPlayer(Monster monster)
    {
        float shortestDistance = Mathf.Infinity;
        Player nearest = null;
        foreach (Player player in players)
        {
            float distanceToPlayer = Vector2.Distance(monster.transform.position, player.transform.position);
            if (distanceToPlayer > monster.sightRange) continue; // ���� ���͸�

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
            if (distanceToPlayer > player.sightRange) continue; // ���� ���͸�

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearest = monster;
            }
        }
        return nearest;
    }
}
