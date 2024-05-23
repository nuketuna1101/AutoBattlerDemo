using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private const int level = 1;             // ���� �ܰ�: ����� �ð����� ������ 1�ܰ踸���� ����.
    public const float spawnInterval = 0.5f; // ���� ���� ����
    private int remainMonstersToSpawn;

    private Coroutine checkWinConditionCoroutine;
    private Coroutine spawnMonsterCoroutine;

    private new void Awake()
    {
        // �� ���Խ� ȣ�� �̺�Ʈ ���
        base.Awake();
        SceneManager.sceneLoaded += CallWithSceneLoaded;
    }
    private void OnDestroy()
    {
        // �̺�Ʈ ���� : memory ������
        SceneManager.sceneLoaded -= CallWithSceneLoaded;
    }
    private void CallWithSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "2.Ingame")
            StartBattle();
    }
    public void StartBattle()
    {
        Presetting();
        CheckWinCondition();
    }
    private void Presetting()
    {
        InitLevel();
        SpawnPlayers();
        SpawnMonster();
    }
    private void InitLevel()        // ���� �������� ���� �ȵǾ����� �ӽ� �ڵ�. ���� ��� ���� ��
    {
        remainMonstersToSpawn = 8 + 4 * level;
    }
    private void SpawnPlayers()
    {
        // ������ġ�� �÷��̾� ����
        for (int i = 0; i < 4; i++)
        {
            var playerObj = PlayerPoolManager.GetFromPool((PlayerClass)i);
            playerObj.transform.position = new Vector3((i + 1), (i % 2) * (-2), 0.0f);
        }
    }
    private void SpawnMonster()
    {
        DebugOpt.Log("SpawnMonster called");
        CoroutineHelper.RestartCor(this, ref spawnMonsterCoroutine, SpawnMonstersRoutine());
    }
    private IEnumerator SpawnMonstersRoutine()
    {
        while (remainMonstersToSpawn > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            // Ǯ���� 1�� �������� ��û
            GameObject goblinObj = MonsterPoolManager.GetFromPool();
            // Ǯ���� �������� ���ϸ� ��� ��� �� ��ݺ�
            if (goblinObj == null) continue;
            // Ǯ���� �����Դٸ� ī��Ʈ �ٿ��ְ� ����� ��ġ
            //goblinObj.transform.position = new Vector3(-1, 0, 0.0f); // ���Ž�
            goblinObj.transform.position = BattleManager.Instance.SpawnOnRandomPosition(5f, 7f);
            remainMonstersToSpawn--;
        }
    }

    private void CheckWinCondition()
    {
        CoroutineHelper.RestartCor(this, ref checkWinConditionCoroutine, CheckWinConditionCoroutine());
    }
    private IEnumerator CheckWinConditionCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        // ���� ������ Ȯ��
        while (true)
        {
            yield return null;
            // �¸� ����: ��� ���� ����
            DebugOpt.Log("remainMonstersToSpawn <= 0 : " + (remainMonstersToSpawn <= 0));
            DebugOpt.Log("BattleManager.Instance.isAllMonstersCleared() : " + (BattleManager.Instance.isAllMonstersCleared()));
            DebugOpt.Log("test " + BattleManager.Instance.test());

            if (remainMonstersToSpawn <= 0 && BattleManager.Instance.isAllMonstersCleared())
            {
                // �¸�

                VictoryEnd();
                yield break;
            }
            // �й� ����: �÷��̾� ����
            if (!BattleManager.Instance.isAnyPlayerAlive())
            {
                // �й�
                DefeatEnd();
                yield break;
            }
        }
    }

    private void VictoryEnd()
    {
        DebugOpt.Log("GameManager: Victory");
        BattleManager.Instance.PlayerVictory();
        UIManager.Instance.ShowPopupWindow("Victory");
    }
    private void DefeatEnd()
    {
        DebugOpt.Log("GameManager: Defeat");
        Time.timeScale = 0; // ���� �Ͻ� ����
        UIManager.Instance.ShowPopupWindow("Defeat");
    }
    public void ExitGame()         // exit ��ư ���� ����� �÷ο�
    {  
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit(); // ���ø����̼� ����
    #endif 
    }
}
