using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SnowHorse.Utils
{
    public class WarningTool : MonoBehaviour
    {
        public static void Print(string message, GameObject gameObject)
        {
            print($"{message}\nGameobject: {gameObject.name}; Parent: {gameObject.transform.parent.name}");
        }
    }
}

