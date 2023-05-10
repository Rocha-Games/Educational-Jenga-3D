using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrbitalCameraController : MonoBehaviour {

#region Public Fields

#endregion


#region Private Serializable Fields

    [SerializeField] private List<Transform> listOfTowers;
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float xSpeed = 250.0f;
    [SerializeField] private float ySpeed = 120.0f;
    [SerializeField] private float yMinLimit = -20.0f;
    [SerializeField] private float yMaxLimit = 80.0f;
#endregion


#region Private Fields
    private float x = 0.0f;
    private float y = 0.0f;
    private Transform currentTower;
    private int towerIndex = 1;
    private bool isChangingTowers = false;

#endregion


#region MonoBehaviour CallBacks
    void Start(){
        currentTower = listOfTowers[towerIndex];
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        CalculatePosition();
    }
    
    void LateUpdate(){

        if(isChangingTowers){
            return;
        }

        if(Input.GetMouseButton(1)){
            CalculatePosition();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            towerIndex = 0;
            currentTower = listOfTowers[towerIndex];
            isChangingTowers = true;
        }else if(Input.GetKeyDown(KeyCode.Alpha2)){
            towerIndex = 1;
            currentTower = listOfTowers[towerIndex];
            isChangingTowers = true;
        }else if(Input.GetKeyDown(KeyCode.Alpha3)){
            towerIndex = 2;
            currentTower = listOfTowers[towerIndex];
            isChangingTowers = true;
        }
    }
#endregion

#region Private Methods

    private void CalculatePosition(){
        if (!isChangingTowers)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + currentTower.position;

        transform.rotation = rotation;
        transform.position = position;

    }

    private float ClampAngle(float angle, float min, float max)  
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
#endregion


#region Public Methods

#endregion
}