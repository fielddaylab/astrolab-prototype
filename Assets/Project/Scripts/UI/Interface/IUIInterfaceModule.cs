using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public interface IUIInterfaceModule
    {
        public void Init();

        public void Open();

        public void Close();

    }
}