using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public class PrefabLoader
    {
        /// <summary>
        ///     Get all prefab files in Resources
        /// </summary>
        /// <returns>Array with all prefabs</returns>
        public static IEnumerable<string> GETPrefab()
        {
            //Get default resource folder
            String folderPath = Application.dataPath + "\\Resources\\";
            
            //Get every .prefab file in folder
            String[] files = Directory.GetFiles(folderPath, "*.prefab");
            return files;
        }
    }
}