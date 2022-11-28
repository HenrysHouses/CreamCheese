//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class TEST_LoadingFile : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Loading();
        }
    }

    private void Loading()
    {
        string file = "C:/Users/somme/AppData/LocalLow/DefaultCompany/A_Gods_Draw";

        using(StreamWriter streamWriter = new StreamWriter(file))
        {
            streamWriter.WriteLine("Herllow");
        }

        using(StreamReader streamReader = new StreamReader(file))
        {
            Debug.Log(streamReader.ReadToEnd());
        }
    }
}
