
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GiveValue : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI timerText;

  // Start is called before the first frame update
  void Start()
  {
    string newText = StaticData.valueToKeep;
    timerText.text = newText;
  }
}
