using UnityEngine;
using System.Collections;

namespace Memoria
{
    public static class GameData
    {
        private static readonly string keySilling = "siling";
        private static readonly string keyFloorMax = "floorMax";
        private static readonly string keyHasPassiveItem1 = "hasPassiveItem1";
        private static readonly string keyHasPassiveItem2 = "hasPassiveItem2";

        public static int silling
        {
            get
            {
                if (!PlayerPrefs.HasKey(keySilling))
                {
                    PlayerPrefs.SetInt(keySilling, 0);
                }

                return PlayerPrefs.GetInt(keySilling);
            }
            
            set { PlayerPrefs.SetInt(keySilling, value); }
        }
        
        public static int floorMax
        {
            get 
            {
                if (!PlayerPrefs.HasKey(keyFloorMax))
                {
                    PlayerPrefs.SetInt(keyFloorMax, 0);
                }
                
                return PlayerPrefs.GetInt(keyFloorMax);
            }
            
            set { PlayerPrefs.SetInt(keyFloorMax, value); }
        }
    }
}