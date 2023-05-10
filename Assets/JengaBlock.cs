using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class JengaBlock : MonoBehaviour {

#region Public Fields
    public enum BlockTypes{
        Glass,
        Wood,
        Stone
    }
    public DataRetriever.MasteryData BlockData { get => _blockData; private set{} }
    public BlockTypes BlockType { get => _blockType; private set{} }
#endregion

#region Private Serializable Fields
    [SerializeField] private MeshRenderer blockMeshRenderer;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material glassMaterial;
    [SerializeField] private Material woodMaterial;
    [SerializeField] private Material stoneMaterial;    
#endregion

#region Private Fields
    private DataRetriever.MasteryData _blockData;
    private Material blockMaterial;
    private BlockTypes _blockType;
    private Rigidbody rb;
#endregion


#region MonoBehaviour CallBacks
    private void OnEnable(){
        EventManager.Instance.StartListening(EventManager.Events.OnTestMyStackButtonPressed, () => rb.isKinematic = false);
        EventManager.Instance.StartListening(EventManager.Events.OnTestMyStackButtonPressed, DestroyGlassBlocks);
    }


    private void OnDisable(){
        EventManager.Instance.StopListening(EventManager.Events.OnTestMyStackButtonPressed, () => rb.isKinematic = false);
        EventManager.Instance.StopListening(EventManager.Events.OnTestMyStackButtonPressed, DestroyGlassBlocks);
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
#endregion


#region Private Methods
    private void ChangeMaterialBasedOnMastery()
    {
        //according to specifications:
        //mastery = 0 → Glass
        //mastery = 1 → Wood
        //mastery = 2 →  Stone
        switch(_blockData.mastery){
            case 0:
                blockMaterial = glassMaterial;
                _blockType = BlockTypes.Glass;
            break;
            case 1:
                blockMaterial = woodMaterial;
                _blockType = BlockTypes.Wood;
            break;
            case 2:
                blockMaterial = stoneMaterial;
                _blockType = BlockTypes.Stone;
            break;
        }

        //right now the mesh uses just 1 material, but if it's updated in the future, we might have to change this
        blockMeshRenderer.material = blockMaterial;
    }

    private void OnMouseEnter()
    {
    }

    private void OnMouseDown()
    {
        GUIManager.Instance.DisplayBlockInfo(_blockData.grade, _blockData.domain, _blockData.cluster, _blockData.standardid, _blockData.standarddescription);
        blockMeshRenderer.material = highlightMaterial;
    }

    private void OnMouseExit() {
        blockMeshRenderer.material = blockMaterial;
    }
    
    private void DestroyGlassBlocks()
    {
        if(_blockType == BlockTypes.Glass){
            Destroy(gameObject);
        }
    }
#endregion


#region Public Methods
    public void Initialize(DataRetriever.MasteryData masteryData_){
        _blockData = masteryData_;
        ChangeMaterialBasedOnMastery();
    }
#endregion
}