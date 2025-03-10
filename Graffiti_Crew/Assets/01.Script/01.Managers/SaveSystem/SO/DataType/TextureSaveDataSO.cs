using System.Collections;
using System.IO;
using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/TextureSaveDataSO")]
    public class TextureSaveDataSO : SaveDataSO {
        [Space]
        public string textureFilePath; // ���� ���
        public Texture2D data;
        [Space]
        [SerializeField] private Texture2D _defaultData;

        private void Awake() {
            dataType = DataType.Texture;
        }
        public override string GetDataType() {
            SaveTexture(data, textureFilePath);
            return dataType.ToString();
        }
        public override string GetData() {
            return textureFilePath;
        }
        public override void SetValueFromString(string value) {
            textureFilePath = value;
            data = TextureSaveLoadUtility.LoadTextureFromFile(textureFilePath);
        }
        public override void ResetData() {
            data = _defaultData;
        }

        public Texture2D GetTexture() {
            if (data == null && !string.IsNullOrEmpty(textureFilePath)) {
                data = TextureSaveLoadUtility.LoadTextureFromFile(textureFilePath);
            }
            return data;
        }
        public void SaveTexture(Texture2D textureToSave, string savePath) {
            textureFilePath = TextureSaveLoadUtility.SaveTextureToFile(textureToSave, savePath);
            data = textureToSave;
        }

    }
    public static class TextureSaveLoadUtility {
        // ����� Texture2D�� ���� �� �ֵ���
        public static Texture2D DeCompress(this Texture2D source) {
            RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);

            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            return readableText;
        }
        // Texture2D�� PNG ���Ϸ� ����
        public static string SaveTextureToFile(Texture2D texture, string savePath) {
            Texture2D decompressedTex = texture.DeCompress();
            Texture2D gammaTexture = GammaUtility.ConvertToGamma(decompressedTex); // �̰� ���ϸ� ��ο���

            byte[] bytes = gammaTexture.EncodeToJPG();
            File.WriteAllBytes(savePath, bytes);
            return savePath;
        }
        // PNG ������ Texture2D�� �ε�
        public static Texture2D LoadTextureFromFile(string filePath) {
            if (!File.Exists(filePath)) {
                Debug.LogWarning($"File not found at {filePath}");
                return null;
            }

            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2028, 2028);
            texture.LoadImage(fileData); // ���� �����͸� �ؽ�ó�� �ε�
            return texture;
        }

        // Texture2D�� Sprite�� ��ȯ
        public static Sprite TextureToSprite(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
    public static class GammaUtility {
        // ���� ���� -> ����
        public static Color LinearToGamma(Color color) {
            return new Color(
                Mathf.Pow(color.r, 1.0f / 2.2f),
                Mathf.Pow(color.g, 1.0f / 2.2f),
                Mathf.Pow(color.b, 1.0f / 2.2f),
                color.a
            );
        }

        // Texture2D �����͸� ���� -> ����
        public static Texture2D ConvertToGamma(Texture2D texture) {
            Texture2D correctedTexture = new Texture2D(texture.width, texture.height, texture.format, false);

            for (int y = 0; y < texture.height; y++) {
                for (int x = 0; x < texture.width; x++) {
                    Color linearColor = texture.GetPixel(x, y);
                    Color gammaColor = LinearToGamma(linearColor);
                    correctedTexture.SetPixel(x, y, gammaColor);
                }
            }
            correctedTexture.Apply();
            return correctedTexture;
        }
    }
}