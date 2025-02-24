using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumpLevel : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            LevelManager.Instance.StartGame(int.Parse(inputField.text));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
