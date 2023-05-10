using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GUIManager : MonoBehaviour {

#region Public Fields
    public static GUIManager Instance;
#endregion


#region Private Serializable Fields
    [SerializeField] private GameObject blockInfoPanel;
    [SerializeField] private TextMeshProUGUI txtGradeAndDomain, txtCluster, txtStandardIDAndDescription;
    [SerializeField] private Button buttonTestMyStack, button6thGradeStack, button7thGradeStack, button8thGradeStack, buttonExit;
#endregion


#region Private Fields

#endregion


#region MonoBehaviour CallBacks

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
    }

    void Start(){
        blockInfoPanel.SetActive(false);
        buttonTestMyStack.onClick.AddListener(() => EventManager.Instance.TriggerEvent(EventManager.Events.OnTestMyStackButtonPressed));
        button6thGradeStack.onClick.AddListener(() => EventManager.Instance.TriggerEvent(EventManager.Events.On6thGradeStackButtonPressed));
        button7thGradeStack.onClick.AddListener(() => EventManager.Instance.TriggerEvent(EventManager.Events.On7thGradeStackButtonPressed));
        button8thGradeStack.onClick.AddListener(() => EventManager.Instance.TriggerEvent(EventManager.Events.On8thGradeStackButtonPressed));
        buttonExit.onClick.AddListener(() => Application.Quit());
    }


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            HideBlockInfo();
        }
    }
#endregion


#region Private Methods
 
#endregion


#region Public Methods
    public void DisplayBlockInfo(string grade_, string domain_, string cluster_, string id_, string description_){
        txtGradeAndDomain.text = $"{grade_}: {domain_}";
        txtCluster.text = $"{cluster_}";
        txtStandardIDAndDescription.text = $"{id_}: {description_}";
        blockInfoPanel.SetActive(true);
    }

    public void HideBlockInfo(){
        blockInfoPanel.SetActive(false);
    }
#endregion
}