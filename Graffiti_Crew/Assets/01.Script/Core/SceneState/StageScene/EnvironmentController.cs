using UnityEngine;

public class EnvironmentController : MonoBehaviour, INeedLoding
{
    public void LodingHandle(DataController dataController)
    {
        // Skybox ��ü
        if (dataController.stageData.skyBoxMat != null)
            RenderSettings.skybox = dataController.stageData.skyBoxMat;

        // �� ����
        if (dataController.stageData.mapPrefab != null)
            SetStageMap(dataController.stageData.mapPrefab);

        // ���� ��ƼŬ ����
        if (dataController.stageData.weatherParticle != null)
            Instantiate(dataController.stageData.weatherParticle, new Vector3(0, 10, 0), Quaternion.identity, transform);

        dataController.SuccessGiveData();
    }

    protected virtual void SetStageMap(GameObject mapPrefab)
    {
        Instantiate(mapPrefab, transform);
    }
}
