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

        public static bool hasPassiveItem1
        {
            get
            {
                if (!PlayerPrefs.HasKey(keyHasPassiveItem1))
                {
                    PlayerPrefs.SetInt(keyHasPassiveItem1, 0);
                }

                return PlayerPrefs.GetInt(keyHasPassiveItem1) != 0;
            }

            set { PlayerPrefs.SetInt(keyHasPassiveItem1, value ? 1 : 0); }
        }

        public static bool hasPassiveItem2
        {
            get
            {
                if (!PlayerPrefs.HasKey(keyHasPassiveItem2))
                {
                    PlayerPrefs.SetInt(keyHasPassiveItem2, 0);
                }

                return PlayerPrefs.GetInt(keyHasPassiveItem2) != 0;
            }

            set { PlayerPrefs.SetInt(keyHasPassiveItem2, value ? 1 : 0); }
        }

        public static void Reset()
        {
            silling = 0;
            floorMax = 0;
            hasPassiveItem1 = false;
            hasPassiveItem2 = false;
        }
    }
}