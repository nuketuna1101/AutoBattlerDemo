using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private const int level = 1;             // 라운드 단계: 현재는 시간상의 이유로 1단계만으로 고정.
    public const float spawnInterval = 0.5f; // 몬스터 스폰 간격
    private int remainMonstersToSpawn;

    private Coroutine checkWinConditionCoroutine;
    private Coroutine spawnMonsterCoroutine;

    private new void Awake()
    {
        // 씬 진입시 호출 이벤트 등록
        base.Awake();
        SceneManager.sceneLoaded += CallWithSceneLoaded;
    }
    private void OnDestroy()
    {
        // 이벤트 해제 : memory 우히ㅐ
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
    private void InitLevel()        // 레벨 디자인이 아직 안되었지만 임시 코드. 레벨 비례 몬스터 수
    {
        remainMonstersToSpawn = 8 + 4 * level;
    }
    private void SpawnPlayers()
    {
        // 지정위치에 플레이어 생성
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
            // 풀에서 1개 가져오기 요청
            GameObject goblinObj = MonsterPoolManager.GetFromPool();
            // 풀에서 가져오지 못하면 잠시 대기 후 재반복
            if (goblinObj == null) continue;
            // 풀에서 가져왔다면 카운트 줄여주고 제대로 배치
            //goblinObj.transform.position = new Vector3(-1, 0, 0.0f); // 레거시
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

        // 승패 조건을 확인
        while (true)
        {
            yield return null;
            // 승리 조건: 모든 몬스터 전멸
            DebugOpt.Log("remainMonstersToSpawn <= 0 : " + (remainMonstersToSpawn <= 0));
            DebugOpt.Log("BattleManager.Instance.isAllMonstersCleared() : " + (BattleManager.Instance.isAllMonstersCleared()));
            DebugOpt.Log("test " + BattleManager.Instance.test());

            if (remainMonstersToSpawn <= 0 && BattleManager.Instance.isAllMonstersCleared())
            {
                // 승리

                VictoryEnd();
                yield break;
            }
            // 패배 조건: 플레이어 전멸
            if (!BattleManager.Instance.isAnyPlayerAlive())
            {
                // 패배
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
        Time.timeScale = 0; // 게임 일시 정지
        UIManager.Instance.ShowPopupWindow("Defeat");
    }
    public void ExitGame()         // exit 버튼 누를 경우의 플로우
    {  
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit(); // 어플리케이션 종료
    #endif 
    }
}
