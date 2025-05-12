using UnityEngine;

public class EnvironmentController : MonoBehaviour, INeedLoding
{
    public void LodingHandle(DataController dataController)
    {
        // Skybox 교체
        if (dataController.stageData.skyBoxMat != null)
            RenderSettings.skybox = dataController.stageData.skyBoxMat;

        // 맵 생성
        if (dataController.stageData.mapPrefab != null)
            SetStageMap(dataController.stageData.mapPrefab);

        // 날씨 파티클 생성
        if (dataController.stageData.weatherParticle != null)
            Instantiate(dataController.stageData.weatherParticle, new Vector3(0, 10, 0), Quaternion.identity, transform);

        dataController.SuccessGiveData();
    }

    protected virtual void SetStageMap(GameObject mapPrefab)
    {
        Instantiate(mapPrefab, transform);
    }
}
