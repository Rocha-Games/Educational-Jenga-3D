using Microsoft.CSharp.RuntimeBinder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour {

#region Public Fields
    public static GameManager Instance;
#endregion

#region Private Serializable Fields
    [SerializeField] private float horizontalBlockSpacing = 1f;
    [SerializeField] private float verticalBlockSpacing = 1f;

    [SerializeField] private Transform tower6thGradeLocation;
    [SerializeField] private Transform tower7thGradeLocation;
    [SerializeField] private Transform tower8thGradeLocation;
#endregion


#region Private Fields
    private List<GameObject> listOf6thGradeBlocks = new();
    private List<GameObject> listOf7thGradeBlocks = new();
    private List<GameObject> listOf8thGradeBlocks = new();
#endregion


#region MonoBehaviour CallBacks
    void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
    }

    IEnumerator Start(){
        //Retrieve Mastery Data from the API
        yield return StartCoroutine(RetrieveDataCor());

        //Create the Jenga Towers
        yield return StartCoroutine(CreateJengaTowersCor());
    }
    
    void Update(){
        
    }
#endregion


#region Private Methods
private IEnumerator RetrieveDataCor(){
    DataRetriever.Instance.RetrieveData();
    
    // wait for data retrieval to complete
    yield return new WaitWhile(() => DataRetriever.Instance.IsRetrieving);
}

private IEnumerator CreateJengaTowersCor()
{
    Debug.Log($"Creating Towers...");
    int blockIndex = 0;
    Vector3 blockPosition = new();
    bool rotated = false;
    var blockRotation = new Quaternion();

    
    var list = DataRetriever.Instance.ListOf6thGradeMasteries;
    for (int i = 0; i < 3; i++)
    {
        blockIndex = 0;

        switch(i){
            case 0:
                list = DataRetriever.Instance.ListOf6thGradeMasteries;
            break;
            case 1:
                list = DataRetriever.Instance.ListOf7thGradeMasteries;
            break;
            case 2:
                list = DataRetriever.Instance.ListOf8thGradeMasteries;
            break;
        }

        foreach (var itemData in list)
        {
            //determine block position
            blockPosition = DetermineBlockPosition(6 + i, blockIndex);
            rotated = ShouldBeRotated(blockIndex);
            blockRotation = Quaternion.Euler(0f, rotated ? 90f : 0f, 0f);

            //Get Block from the Object Pool
            var block = ObjectPoolManager.Instance.GetPooledObject();
            if(block == null){
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            block.transform.position = blockPosition;
            block.transform.rotation = blockRotation;            
            block.name = $"Block {blockIndex}";
            block.GetComponent<JengaBlock>().Initialize(itemData);
            block.SetActive(true);

            switch(i){
                case 0:
                    listOf6thGradeBlocks.Add(block);
                break;
                case 1:
                    listOf7thGradeBlocks.Add(block);
                break;
                case 2:
                    listOf8thGradeBlocks.Add(block);
                break;
            }

            blockIndex++;
            yield return new WaitForSeconds(0);
        }
    }
    

    yield return null;
}

    private Vector3 DetermineBlockPosition(int towerGrade, int blockIndex)
    {
        Vector3 position = new();

        var xOrigin = 0f;
        var yOrigin = 0f;
        var zOrigin = 0f;

        var posX = 0f;
        var posY = 0f;
        var posZ = 0f;

        //depending on the current row, the block will be rotated, so we must calculate either X or Z depending on that
        var currentRow = blockIndex / 3;
        
        Transform towerBaseOrigin;
        switch(towerGrade){
            case 6:
                towerBaseOrigin = tower6thGradeLocation;
            break;
            case 7:
                towerBaseOrigin = tower7thGradeLocation;
            break;
            case 8:
                towerBaseOrigin = tower8thGradeLocation;
            break;
            default:
                towerBaseOrigin = tower6thGradeLocation;
            break;
        }

        xOrigin = towerBaseOrigin.position.x;
        yOrigin = towerBaseOrigin.position.y;
        zOrigin = towerBaseOrigin.position.z;

        //for even rows (block not rotated) we calculate the new X position
        if(!ShouldBeRotated(blockIndex)){
            posX =  xOrigin + (blockIndex % 3) * horizontalBlockSpacing;
            posZ = zOrigin;
        }else{
            //for odd rows (block rotated), X is a bit offset. Here we calculate the Z positions
            posX = xOrigin + horizontalBlockSpacing;

            zOrigin = zOrigin + horizontalBlockSpacing;
            posZ =  zOrigin - (blockIndex % 3) * horizontalBlockSpacing;
        }

        //calculate the height        
        posY =  yOrigin + (blockIndex / 3) * verticalBlockSpacing;

        position.x = posX;
        position.y = posY;
        position.z = posZ;

        return position;
    }

    private bool ShouldBeRotated(int blockIndex){
        var row = blockIndex / 3;
        var isOdd = row % 2 == 1;
        return isOdd; //odd rows will be rotated
    }
    #endregion


    #region Public Methods

    #endregion
}