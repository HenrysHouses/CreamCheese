//Charlie SCRIPT
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Map
{
    public class Map_Manager : MonoBehaviour
    {
        public Map_Configuration configuration;
        public Map_View view;

        public Map currentMap { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            var m_map = PlayerPrefs.GetString("Map");
            var map = JsonUtility.FromJson<Map>(m_map);
            if (PlayerPrefs.HasKey("Map")) //needs to be added into the playerprefs
            {
                if (map.path.Any(p => p.Equals(map.GetBossNode().point))) //dont know how to fix this yet
                {
                    GenerateNewMap();
                }
                else
                {
                    currentMap = map;
                    view.MapShow(map);
                }
            }
            else
            {
                GenerateNewMap();
            }
            
        }

        public void GenerateNewMap()
        {
            var map = Map_Generator.GetMap(configuration);
            currentMap = map;
            view.MapShow(map);
        }

        public void SavingMap()
        {
            if(currentMap == null)
            {
                return;
            }

            var json = JsonUtility.ToJson(currentMap);
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
            Debug.Log("map has been saved");
        }

        private void OnApplicationQuit()
        {
            SavingMap();
        }
    }
}*/