using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GetValue : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI timerText;

  public void LoadSceneAndKeepValue()
  {
    string dataToKeep = timerText.text;
    StaticData.valueToKeep = dataToKeep;
    SceneManager.LoadScene("WinScreen");
  }
}
