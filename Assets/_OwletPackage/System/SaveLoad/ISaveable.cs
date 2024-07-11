using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Systems.SaveLoad
{
    public interface ISaveable
    {
        /// <summary>
        /// Return the json string of the saved data
        /// </summary>
        /// <returns></returns>
        public string Save();

        /// <summary>
        /// Load the json string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool Load(string json);

        /// <summary>
        /// Create new save file
        /// </summary>
        public void CreateNewSaveFile();

        /// <summary>
        /// Return a unique ID for the object
        /// </summary>
        /// <returns></returns>
        public string ID();
    }
}
