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
    }
}