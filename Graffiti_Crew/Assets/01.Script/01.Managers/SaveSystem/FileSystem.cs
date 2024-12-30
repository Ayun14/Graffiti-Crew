using System;
using System.IO;
using UnityEngine;

namespace AH.SaveSystem {
    public class FileSystem : MonoBehaviour {
        public static bool WriteToFile(string fileName, string fileContents) {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName); // Combine �� ���ڿ� ����
            try {
                File.WriteAllText(fullPath, fileContents); // ���� ���ְ�
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
            if (!File.Exists(fullPath)) { // ��ο� ������ ���ٸ�
                File.WriteAllText(fullPath, ""); // ����ְ�
            }
            try {
                result = File.ReadAllText(fullPath); /// �а� 
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

