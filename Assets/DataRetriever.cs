using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class DataRetriever : MonoBehaviour {

#region Public Fields
    public static DataRetriever Instance;
    public bool IsRetrieving { get => _isRetrieving; private set{} }
    public List<MasteryData> ListOfMasteries { get => _listOfMasteries; private set{} }
    public List<MasteryData> ListOf6thGradeMasteries { get => _listOf6thGradeMasteries; private set{} }
    public List<MasteryData> ListOf7thGradeMasteries { get => _listOf7thGradeMasteries; private set{} }
    public List<MasteryData> ListOf8thGradeMasteries { get => _listOf8thGradeMasteries; private set{} }
#endregion

#region Private Serializable Fields
    [SerializeField] private string _endpoint;
#endregion

#region Private Fields
    private List<MasteryData> _listOfMasteries = new();
    private List<MasteryData> _listOf6thGradeMasteries = new();
    private List<MasteryData> _listOf7thGradeMasteries = new();
    private List<MasteryData> _listOf8thGradeMasteries = new();
    private bool _isRetrieving = true;
#endregion


    #region MonoBehaviour CallBacks
    void Awake(){
        if (Instance == null)
        {
            Instance = this;
        }else{
            Destroy(this);
            return;
        }
    }
#endregion


#region Private Methods
private IEnumerator RetrieveDataCor(){
    _isRetrieving = true;

    Debug.Log($"Retrieving data...");
    using (UnityWebRequest webRequest = UnityWebRequest.Get(_endpoint))
    {
        // Send the request and wait for the response
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            // Handle the error
            Debug.Log(webRequest.error);
            _isRetrieving = false;
        }
        else
        {
            // Process the response
            string response = webRequest.downloadHandler.text;
            _listOfMasteries = JsonConvert.DeserializeObject<List<MasteryData>>(response);
            SplitAndSortRetrievedData();
            _isRetrieving = false;
        }
        webRequest.Dispose();
    }
    // Debug.Log($"{_listOfMasteries.Count} items retrieved");
}

    private void SplitAndSortRetrievedData(){

        //Split in 3 Lists based on Student Grade
        foreach(var data in _listOfMasteries){
            switch(data.grade){
                case "6th Grade":
                    _listOf6thGradeMasteries.Add(data);
                break;
                case "7th Grade":
                    _listOf7thGradeMasteries.Add(data);
                break;
                case "8th Grade":
                    _listOf8thGradeMasteries.Add(data);
                break;
            }
        }

        //Now sort them according to specification
        //"Order the blocks in the stack starting from the bottom up, by domain name ascending, then by cluster name ascending, then by standard ID ascending"

        //Sort by domain name ascending
        _listOf6thGradeMasteries.Sort((p1, p2) => string.Compare(p1.domain, p2.domain));
        _listOf7thGradeMasteries.Sort((p1, p2) => string.Compare(p1.domain, p2.domain));
        _listOf8thGradeMasteries.Sort((p1, p2) => string.Compare(p1.domain, p2.domain));

        //Sort by cluster name ascending
        _listOf6thGradeMasteries.Sort((p1, p2) => string.Compare(p1.cluster, p2.cluster));
        _listOf7thGradeMasteries.Sort((p1, p2) => string.Compare(p1.cluster, p2.cluster));
        _listOf8thGradeMasteries.Sort((p1, p2) => string.Compare(p1.cluster, p2.cluster));

        //Finally, sort by stardard ID ascending
        _listOf6thGradeMasteries.Sort((p1, p2) => string.Compare(p1.standardid, p2.standardid));
        _listOf7thGradeMasteries.Sort((p1, p2) => string.Compare(p1.standardid, p2.standardid));
        _listOf8thGradeMasteries.Sort((p1, p2) => string.Compare(p1.standardid, p2.standardid));

        Debug.Log($"_listOf6thGradeMasteries contains {_listOf6thGradeMasteries.Count} items");
        Debug.Log($"_listOf7thGradeMasteries contains {_listOf7thGradeMasteries.Count} items");
        Debug.Log($"_listOf8thGradeMasteries contains {_listOf8thGradeMasteries.Count} items");
    }
#endregion


#region Public Methods
    public void RetrieveData(){
        _isRetrieving = true;
        StartCoroutine(RetrieveDataCor());
    }
#endregion

#region Helper Classes
    public class MasteryData{
        public int id { get; set; }
        public string subject { get; set; }
        public string grade { get; set; }
        public int mastery { get; set; }
        public string domainid { get; set; }
        public string domain { get; set; }
        public string cluster { get; set; }
        public string standardid { get; set; }
        public string standarddescription { get; set; }
    }
#endregion
}

