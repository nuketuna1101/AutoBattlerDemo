using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BattleManager : Singleton<BattleManager>
{
    /// <summary>
    /// ���� ���� ���� �Ŵ�¡
    /// </summary>

    public void AttackFromPlayerToMonster(Player player, Monster monster, int damage)
    {
        if (player == null || monster == null)    return;

        monster.BeAttacked(damage);
    }

    public void AttackFromMonsterToPlayer(Monster monster, Player player, int damage)
    {
        if (player == null || monster == null) return;

        player.BeAttacked(damage);
    }

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


    // pool�� ȸ����ų �� battlemanager������ ȸ�� �۾�
}
