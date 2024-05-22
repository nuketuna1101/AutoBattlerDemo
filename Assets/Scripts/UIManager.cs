using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// 배틀 매니저를 기반으로 Player에 관한 UI를 관리해주자.
    /// </summary>
    public Sprite[] sprites = new Sprite[4];
    private Transform CharacterSlotsParent;
    private Coroutine updateHPbarCoroutine;

    private void Awake()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        CharacterSlotsParent = canvas.transform.GetChild(0).GetChild(1).transform;
        for(int i = 0; i < 4; i++)
        {
            Transform slot = CharacterSlotsParent.GetChild(i).transform;
            slot.GetChild(1).GetComponent<Image>().sprite = sprites[i];
            slot.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = string.Format("{0}", (i + 1));
            Slider slider = slot.GetChild(3).GetComponent<Slider>();
            slider.maxValue = BattleManager.Instance.Players[i].maxHealth;
            slider.value = BattleManager.Instance.Players[i].health;
            Debug.Log("BattleManager.Instance.Players[i] : " + (BattleManager.Instance.Players[i] == null));
        }
    }

    private IEnumerator UpdateHPbarRoutine()
    {
        // 체력바 HP UI가 오브젝트 따라다니는 코루틴. 그리고 현재 체력에 따라 슬라이더 업데이트
        while (true)
        {
            yield return null;
            for (int i = 0; i < 4; i++)
            {
                //if (BattleManager.Instance.players)

                Transform slot = CharacterSlotsParent.GetChild(i).transform;
                Slider slider = slot.GetChild(3).GetComponent<Slider>();
                //slider.value = 
            }
        }
    }
}
