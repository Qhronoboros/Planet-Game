using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStages : MonoBehaviour
{
    public List<GameObject> stages = new List<GameObject>();
    public List<Vector2> stagesPos = new List<Vector2>();
    public Text stageText;

    private void Awake()
    {
        foreach (GameObject stage in stages)
        {
            stagesPos.Add(new Vector2(stage.transform.position.x, stage.transform.position.y));

            if (SaveGameManager.Instance.check_level_unlocked(stage.GetComponent<MenuStageIndex>().stageIndex))
            {
                stage.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                stage.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        UpdateOrder();
    }

    private void Update()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            stages[i].transform.position = Vector2.MoveTowards(stages[i].transform.position, stagesPos[i], 3000.0f * Time.deltaTime);
        }
    }

    public void UpdateOrder()
    {
        for (int i = stages.Count / 2; i >= 0; i--)
        {
            stages[i].transform.SetSiblingIndex(i);
            stages[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

            if (i > stages.Count/2 -1)
            {
                stageText.text = stages[i].GetComponent<MenuStageIndex>().stageName;
                main_menu.selected_stage_index = stages[i].GetComponent<MenuStageIndex>().stageIndex;
                main_menu.sceneName = stages[i].GetComponent<MenuStageIndex>().sceneName;
                stages[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                stages[stages.Count - 1 - i].transform.SetSiblingIndex(i);
                stages[stages.Count - 1 - i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
    }

    public void NextPressed()
    {
        GameObject stage = stages[stages.Count-1];
        stages.RemoveAt(stages.Count - 1);
        stages.Insert(0, stage);
        UpdateOrder();
        Debug.Log(ConcatenateList(stages));
    }

    public void PrevPressed()
    {
        GameObject stage = stages[0];
        stages.RemoveAt(0);
        stages.Add(stage);
        UpdateOrder();
        Debug.Log(ConcatenateList(stages));
    }

    static public string ConcatenateList(List<GameObject> list)
    {
        string export = "";

        foreach (GameObject stage in list)
        {
            export += stage.name + " ";
        }

        return export;
    }
}
