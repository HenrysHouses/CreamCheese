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
            if (PlayerPrefs.HasKey("Map"))
            {
                string mapJson = PlayerPrefs.GetString("Map");
                Map map = JsonConvert.DeserializeObject<Map>(mapJson);

                // if (map.path.Contains(map.GetBossNode().point))
                // {
                //     GenerateNewMap();
                // }



                // Debug.Log(map.GetBossNode());

                if (map.path.Any(p => p.Equals(map.GetBossNode().point)) || GameManager.instance.shouldGenerateNewMap)
                {
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;

                    //the player has not reached the boss yet so lets load the current map
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
            Map map = Map_Generator.GetMap(configuration);
            CurrentMap = map;
            view.MapShow(map);
            GameManager.instance.shouldGenerateNewMap = false;
        }

        public void SavingMap()
        {
            if(CurrentMap == null)
            {
                return;
            }
            // scuffed fix for now, should def check if there a better way of fixing this
            string json = JsonConvert.SerializeObject(CurrentMap, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();

            Debug.Log("map has been saved");
        }

        private void OnApplicationQuit()
        {
            SavingMap();

            Debug.Log("map has been saved on quit");
        }
    }
}