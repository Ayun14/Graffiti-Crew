using System;
using System.IO;
using UnityEngine;

namespace AH.SaveSystem {
    public class FileSystem : MonoBehaviour {
        public static bool WriteToFile(string fileName, string fileContents) {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName); // Combine 두 문자열 연결
            try {
                File.WriteAllText(fullPath, fileContents); // 파일 써주고
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out string result) {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log($"SaveFile Path : {fullPath}");
            if (!File.Exists(fullPath)) { // 경로에 파일이 없다면
                File.WriteAllText(fullPath, ""); // 비워주고
            }
            try {
                result = File.ReadAllText(fullPath); /// 읽고 
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
    }
}

