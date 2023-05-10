using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class MyCameraController : MonoBehaviour {

#region Public Fields

#endregion


#region Private Serializable Fields
    [SerializeField] private float xSpeed = 300f;
    [SerializeField] private float ySpeed = 11f;
    [SerializeField] private float minZoom = -5f;
    [SerializeField] private float maxZoom = 5f;
    [SerializeField] private  float zoomSpeed = 10f;
    [SerializeField] private CinemachineFreeLook camTower6;
    [SerializeField] private CinemachineFreeLook camTower7;
    [SerializeField] private CinemachineFreeLook camTower8;
    #endregion


    #region Private Fields

    #endregion


    #region MonoBehaviour CallBacks

    private void OnEnable(){
        EventManager.Instance.StartListening(EventManager.Events.On6thGradeStackButtonPressed, () => FocusOnStack(1));
        EventManager.Instance.StartListening(EventManager.Events.On7thGradeStackButtonPressed, () => FocusOnStack(2));
        EventManager.Instance.StartListening(EventManager.Events.On8thGradeStackButtonPressed, () => FocusOnStack(3));
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(EventManager.Events.On6thGradeStackButtonPressed, () => FocusOnStack(1));
        EventManager.Instance.StopListening(EventManager.Events.On7thGradeStackButtonPressed, () => FocusOnStack(2));
        EventManager.Instance.StopListening(EventManager.Events.On8thGradeStackButtonPressed, () => FocusOnStack(3));
    }

    void Start()
    {
        camTower6.Priority = 0;
        camTower7.Priority = 1;
        camTower8.Priority = 0;
        ZeroCameraSpeed();
    }

    private void ZeroCameraSpeed()
    {
        camTower6.m_XAxis.m_MaxSpeed = 0;
        camTower6.m_YAxis.m_MaxSpeed = 0;
        camTower7.m_XAxis.m_MaxSpeed = 0;
        camTower7.m_YAxis.m_MaxSpeed = 0;
        camTower8.m_XAxis.m_MaxSpeed = 0;
        camTower8.m_YAxis.m_MaxSpeed = 0;
    }

    void Update(){

        //if RMB is pressed we can rotate around the Stack
        if(Input.GetMouseButton(1)){
            camTower6.m_XAxis.m_MaxSpeed = xSpeed;
            camTower6.m_YAxis.m_MaxSpeed = ySpeed;
            camTower7.m_XAxis.m_MaxSpeed = xSpeed;
            camTower7.m_YAxis.m_MaxSpeed = ySpeed;
            camTower8.m_XAxis.m_MaxSpeed = xSpeed;
            camTower8.m_YAxis.m_MaxSpeed = ySpeed;
        }
        else if(Input.GetMouseButtonUp(1)){
            ZeroCameraSpeed();
        }

        //Control the focus on different stacks
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            FocusOnStack(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            FocusOnStack(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            FocusOnStack(3);
        }

        //Control the Zoom
        float scrollDelta = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        if(scrollDelta != 0){
            Debug.Log($"scrollDelta: {scrollDelta}");
            camTower6.m_Orbits[0].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower6.m_Orbits[1].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower6.m_Orbits[2].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower7.m_Orbits[0].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower7.m_Orbits[1].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower7.m_Orbits[2].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower8.m_Orbits[0].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower8.m_Orbits[1].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
            camTower8.m_Orbits[2].m_Radius -= Mathf.Clamp(scrollDelta, minZoom, maxZoom);
        }
    }
#endregion


#region Private Methods
    void FocusOnStack(int num_){
        switch (num_)
        {
            case 1:
                camTower6.Priority = 1;
                camTower7.Priority = 0;
                camTower8.Priority = 0;
            break;
            case 2:
                camTower6.Priority = 0;
                camTower7.Priority = 1;
                camTower8.Priority = 0;
            break;
            case 3:
                camTower6.Priority = 0;
                camTower7.Priority = 0;
                camTower8.Priority = 1;
            break;
        }
    }
#endregion


#region Public Methods

#endregion
}