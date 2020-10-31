using System;
using System.IO;
using UnityEngine;

namespace Skripte
{
    public class PrefabLoader
    {
        public String[] getPrefab()
        {
            //Get default resource folder
            String folderPath = Application.dataPath + "\\Resources\\";
            //Get every .prefab file in folder
            String[] files = Directory.GetFiles(folderPath, "*.prefab");
            return files;
        }
    }
}