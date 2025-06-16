using System;
using System.IO;
using UnityEngine;

namespace AH.SaveSystem {
    public class FileSystem : MonoBehaviour {
        public static bool WriteToFile(string saveSlotName, string fileName, string fileContents) {
            var folderPath = Path.Combine(Application.persistentDataPath, saveSlotName);
            Debug.Log("WRITE : " + fileName);
            var fullPath = Path.Combine(folderPath, fileName); // Combine �� ���ڿ� ����\
            try {
                File.WriteAllText(fullPath, fileContents); // ���� ���ְ�
                Debug.Log(fileContents);
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool CheckToSlotFolder(string saveSlotName) {
            var folderPath = Path.Combine(Application.persistentDataPath, saveSlotName);
            if (!Directory.Exists(folderPath)) { // ���� ã��
                Directory.CreateDirectory(folderPath); // �÷Կ� �ش��ϴ� ������ �����ϰ�
                return true;
            }
            return false;
        }
        public static bool CheckToSaveFile(string slotName, string fileName) {
            var slotPath = Path.Combine(Application.persistentDataPath, slotName);
            var filePath = Path.Combine(slotPath, fileName);
            if (File.Exists(filePath)) { // ���� ã��
                return true;
            }
            return false;
        }
        private static bool CheckInText(out string result, string fullPath) {
            if (!File.Exists(fullPath)) { // ��ο� ������ ���ٸ�
                File.WriteAllText(fullPath, "");
            }
            try {
                result = File.ReadAllText(fullPath); /// �а�
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");
                result = "";
                Debug.Log("return false");
                return false;
            }
        }

        public static bool LoadFromFile(string saveSlotName, string fileName, out string result) {
            var folderPath = Path.Combine(Application.persistentDataPath, saveSlotName);
            var fullPath = Path.Combine(folderPath, fileName);
            Debug.Log($"SaveFile Path : {fullPath}");

            return CheckInText(out result, fullPath);
        }
        public static void DeleteFolder(string saveSlotName) {
            var folderPath = Path.Combine(Application.persistentDataPath, saveSlotName);
            if (Directory.Exists(folderPath)) {
                Directory.Delete(folderPath, true);
            }
        }
    }
}

