using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
  private GameObject[] healthPrefabs;

  [SerializeField] private CanvasScaler scaler;
  [SerializeField] private float offset = 250f;

  void Awake()
  {
    healthPrefabs = Resources.LoadAll<GameObject>("Health");

    scaler = GetComponent<CanvasScaler>();
  }

    Vector3 UnscalePosition(Vector3 randomPos)
    {
        Vector2 referenceRes = scaler.referenceResolution;
        Vector2 currentRes = new Vector2(Screen.width, Screen.height);

        float widthRatio = currentRes.x / referenceRes.x;
        float heightRatio = currentRes.y / referenceRes.y;

        float ratio = Mathf.Lerp(heightRatio, widthRatio, scaler.matchWidthOrHeight);

        Debug.Log(ratio);

        return randomPos / ratio;
    }

  public void SpawnHealthDamage()
  {
    int randomPrefab = Random.Range(0, healthPrefabs.Length);
    Vector3 pos = UnscalePosition(new Vector3(Random.Range(0, Screen.width / 2), Random.Range(0, Screen.height / 2), 0));
    int randomRotZ = Random.Range(0, 360);
    GameObject healthPrefab = Instantiate(healthPrefabs[randomPrefab], pos, Quaternion.Euler(0, 0, randomRotZ));
    healthPrefab.transform.SetParent(gameObject.transform);
   }
}