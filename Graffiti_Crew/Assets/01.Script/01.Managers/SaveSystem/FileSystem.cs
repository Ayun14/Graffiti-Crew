using System;
using System.IO;
using UnityEngine;

namespace AH.Save {
    public class FileSystem : MonoBehaviour {
        public static bool WriteToFile(string saveSlotName, string fileName, string fileContents) {
            var folderPath = Path.Combine(Application.persistentDataPath, saveSlotName);
            var fullPath = Path.Combine(folderPath, fileName); // Combine 두 문자열 연결
            try {
                File.WriteAllText(fullPath, fileContents); // 파일 써주고
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }
        public static string FindDataValue(string slotName, string fileName, string dataName) {
            var folderPath = Path.Combine(Application.persistentDataPath, slotName);
            var fullPath = Path.Combine(folderPath, fileName);

            if (!File.Exists(fullPath)) return "";

            string text = File.ReadAllText(fullPath);
            if (string.IsNullOrWhiteSpace(text)) return "";

            string dataNameSearch = $"\"dataName\": \"{dataName}\"";
            int nameIndex = text.IndexOf(dataNameSearch, StringComparison.Ordinal);
            if (nameIndex == -1) return "";

            // dataName 찾기
            int objectStart = text.LastIndexOf("{", nameIndex, StringComparison.Ordinal);
            int objectEnd = text.IndexOf("}", nameIndex, StringComparison.Ordinal);
            if (objectStart == -1 || objectEnd == -1) return "";

            string objectText = text.Substring(objectStart, objectEnd - objectStart);

            // "data": " 찾기
            string dataKey = "\"data\": \"";
            int dataIndex = objectText.IndexOf(dataKey, StringComparison.Ordinal);
            if (dataIndex == -1) return "";

            int valueStart = dataIndex + dataKey.Length;
            int valueEnd = objectText.IndexOf("\"", valueStart, StringComparison.Ordinal);
            if (valueEnd == -1) return "";

            return objectText.Substring(valueStart, valueEnd - valueStart);
        }


        public static bool CheckToSlotFolder(string saveSlotName) {
            var folderPath = Path.Combine(Application.persistentDataPath, saveSlotName);
            if (!Directory.Exists(folderPath)) { // 슬롯 찾기
                Directory.CreateDirectory(folderPath); // 플롯에 해당하는 폴더를 생성하고
                return true;
            }
            return false;
        }
        public static bool CheckToSaveFile(string slotName, string fileName) {
            var slotPath = Path.Combine(Application.persistentDataPath, slotName);
            var filePath = Path.Combine(slotPath, fileName);
            if (File.Exists(filePath)) { // 파일 찾기
                return true;
            }
            return false;
        }
        private static bool CheckInText(out string result, string fullPath) {
            if (!File.Exists(fullPath)) { // 경로에 파일이 없다면
                File.WriteAllText(fullPath, "");
            }
            try {
                result = File.ReadAllText(fullPath); /// 읽고
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

