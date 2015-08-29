using UnityEngine;
using System.Collections;

namespace Memoria
{
    public static class GameData
    {
        public static int silling
        {
            get
            {
                if (!PlayerPrefs.HasKey("silling"))
                {
                    PlayerPrefs.SetInt("silling", 0);
                }

                return PlayerPrefs.GetInt("silling");
            }
            
            set { PlayerPrefs.SetInt("silling", value); }
        }
        
        public static int floorMax
        {
            get 
            {
                if (!PlayerPrefs.HasKey("floorMax"))
                {
                    PlayerPrefs.SetInt("floorMax", 0);
                }
                
                return PlayerPrefs.GetInt("floorMax");
            }
            
            set { PlayerPrefs.SetInt("floorMax", value); }
        }
    }
}