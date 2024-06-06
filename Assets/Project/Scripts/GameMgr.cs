using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public class GameMgr : MonoBehaviour
    {
        public static GameMgr Instance;

        public GameConsts Consts;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (this != Instance)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
