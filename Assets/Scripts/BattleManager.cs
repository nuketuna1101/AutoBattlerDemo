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

    public void AttackFromPlayerToMonster(Player _Player, Monster _Monster, int damage)
    {
        if (_Player == null || _Monster == null)    return;

        //_Monster.BeAttacked(damage);
    }

    public void AttackFromMonsterToPlayer(Monster _Monster, Player _Player, int damage)
    {
        if (_Player == null || _Monster == null) return;

        //_Player.BeAttacked(damage);
    }

    /// <summary>
    /// ������ ������ �Ŵ�¡
    /// 
    /// ������ �־ ����ȭ : ���� �Ÿ� ���͸� :: 
    /// ��� ��ü���� �Ÿ��� ����ϱ� ����, �뷫���� �Ÿ� ���͸��� �����մϴ�. ���� ���, ���� �Ÿ� �̻� ������ ��ü�� ó������ ���ܽ�ŵ�ϴ�.
    /// </summary>

    private List<Monster> monsters = new List<Monster>();
    private List<Player> players = new List<Player>();
    private float monsterSightRange = 5;
    private float playerSightRange = 5;

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
    /*
    void FixedUpdate()
    {
        foreach (Player player in players)
        {
            Monster nearest = FindNearestMonster(player);
            //player.SetTargetMonster(nearest);
        }

        foreach (Monster monster in monsters)
        {
            Player nearest = FindNearestPlayer(monster);
            //monster.SetTargetPlayer(nearest);
        }
    }
    */
    // ���� ����� ����� Ž���ϵ�, Ž�� ���� ���� �ִ� ��� ����. Ž�� ��� �ȵȴٸ� null ����.

    public Player FindNearestPlayer(Monster monster)
    {
        float shortestDistance = Mathf.Infinity;
        Player nearest = null;
        foreach (Player player in players)
        {
            float distanceToPlayer = Vector2.Distance(monster.transform.position, player.transform.position);
            if (distanceToPlayer > monsterSightRange) continue; // ���� ���͸�

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
            if (distanceToPlayer > playerSightRange) continue; // ���� ���͸�

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearest = monster;
            }
        }
        return nearest;
    }

}
