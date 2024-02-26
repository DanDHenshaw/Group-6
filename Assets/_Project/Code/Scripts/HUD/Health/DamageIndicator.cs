using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
  private GameObject[] healthPrefabs;

  private float MinX, MaxX, MinY, MaxY;
  private Vector3 pos;

  void Awake()
  {
    healthPrefabs = Resources.LoadAll<GameObject>("Health");
  }

  void SpawnOnCanvas()
  {
    SpawnHealthDamage();
    SetMinAndMax();
  }

  private void SetMinAndMax()
  {
    Vector3 Bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    MinX = -Bounds.x;
    MaxX = Bounds.x;
    MinY = -Bounds.y;
    MaxY = Bounds.y;
  }

  public void SpawnHealthDamage()
  {
    int randomPrefab = Random.Range(0, healthPrefabs.Length - 1);
    pos = new Vector3(Random.Range(0, 1920), Random.Range(0, 1080), 0);
    GameObject healthPrefab = Instantiate(healthPrefabs[randomPrefab], pos, Quaternion.Euler(0, 0,65));
    healthPrefab.transform.SetParent(gameObject.transform);
  }
}