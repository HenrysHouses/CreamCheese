//Charlie SCRIPT

using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Map
{
    public class Map_Manager : MonoBehaviour
    {
        public Map_Configuration configuration;
        public Map_View view;

        public Map CurrentMap { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            if (PlayerPrefs.HasKey("Map")) //needs to be added into the playerprefs
            {
                var mapJson = PlayerPrefs.GetString("Map");
                var map = JsonConvert.DeserializeObject<Map>(mapJson);

                if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
                {
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;
                    view.MapShow(map);
                }
            }
            else
            {
                GenerateNewMap(); //fix
            }
            
        }

        public void GenerateNewMap()
        {
            var map = Map_Generator.GetMap(configuration);
            CurrentMap = map;
            Debug.Log(map.ToJson());
            view.MapShow(map); //fix
        }

        public void SavingMap()
        {
            if(CurrentMap == null)
            {
                return;
            }

            var json = JsonConvert.SerializeObject(CurrentMap);
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SavingMap();
        }
    }
}