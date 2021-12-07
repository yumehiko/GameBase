using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using yumehiko.Resident;



public class SceneDummy : MonoBehaviour
{
    private void Awake()
    {
        int currentBuildID = SceneManager.GetActiveScene().buildIndex;
        ReactiveInput.OnMaru.Where(isOn => isOn).Subscribe(_ => LoadManager.RequireLoadScene(currentBuildID + 1)).AddTo(this);
    }
}
