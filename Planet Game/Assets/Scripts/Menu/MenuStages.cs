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

    private void Start()
    {
        foreach (GameObject stage in stages)
        {
            Vector2 position = new Vector2(stage.transform.localPosition.x, stage.transform.localPosition.y);

            stagesPos.Add(position);

            Debug.Log(stage.GetComponent<MenuStageIndex>().sceneName + " cleared: " + PlayerPrefs.GetInt(stage.GetComponent<MenuStageIndex>().sceneName));

            stage.transform.Find("lock").gameObject.SetActive(!SaveGameManager.Instance.check_level_unlocked(stage.GetComponent<MenuStageIndex>().stageIndex));

            if (stage.GetComponent<Animator>() != null)
            {
                stage.GetComponent<Animator>().SetBool("IsCleared", PlayerPrefs.GetInt(stage.GetComponent<MenuStageIndex>().sceneName) == 1 ? true : false);
            }
        }

        UpdateOrder();
    }

    private void Update()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            stages[i].transform.localPosition = Vector2.MoveTowards(stages[i].transform.localPosition, stagesPos[i], 3000.0f * Time.deltaTime);
        }

        GameObject stage = stages[stages.Count / 2];

        stage.transform.Rotate(0, 0, -20 * Time.deltaTime); //rotates 50 degrees per second around z axis
        for (int i=0; i < stage.transform.childCount; i++)
        {
            stage.transform.GetChild(i).transform.Rotate(0, 0, 20 * Time.deltaTime);
        }
    }

    public void UpdateOrder()
    {
        for (int i = stages.Count / 2; i >= 0; i--)
        {
            if (i > stages.Count/2 -1)
            {
                stageText.text = stages[i].GetComponent<MenuStageIndex>().stageName;
                main_menu.selected_stage_index = stages[i].GetComponent<MenuStageIndex>().stageIndex;
                main_menu.sceneName = stages[i].GetComponent<MenuStageIndex>().sceneName;

                ChangeOrder(i, i, true);
            }
            else
            {
                ChangeOrder(i, i);
                ChangeOrder(stages.Count - 1 - i, i);
            }
        }
    }

    // stageIndex = index in the stages list, orderIndex = order in hierarchy
    public void ChangeOrder(int stageIndex, int orderIndex, bool middle=false)
    {
        GameObject stage = stages[stageIndex];
        float colorAmount = Mathf.Max((float)orderIndex / (stages.Count / 2), 0.3f);
        Color color = new Color(colorAmount, colorAmount, colorAmount, 1.0f);

        stage.transform.SetSiblingIndex(orderIndex);
        stage.GetComponent<Image>().color = color;

        if (stage.transform.childCount > 1)
        {
            for (int i=0; i < stage.transform.childCount - 1; i++)
            {
                stage.transform.GetChild(i).GetComponent<Image>().color = color;

                if (stage.transform.GetChild(i).GetComponent<Animator>() != null)
                {
                    if (middle)
                    {
                        stage.transform.GetChild(i).GetComponent<Animator>().StopPlayback();
                    }
                    else
                    {
                        stage.transform.GetChild(i).GetComponent<Animator>().StartPlayback();
                    }
                }
            }
        }

        if (stage.GetComponent<Animator>() != null)
        {
            if (middle)
            {
                stage.GetComponent<Animator>().StopPlayback();
            }
            else
            {
                stage.GetComponent<Animator>().StartPlayback();
            }
        }
    }

    public void NextPressed()
    {
        GameObject stage = stages[stages.Count-1];
        stages.RemoveAt(stages.Count - 1);
        stages.Insert(0, stage);
        UpdateOrder();
        //Debug.Log(ConcatenateList(stages));
    }

    public void PrevPressed()
    {
        GameObject stage = stages[0];
        stages.RemoveAt(0);
        stages.Add(stage);
        UpdateOrder();
        //Debug.Log(ConcatenateList(stages));
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
