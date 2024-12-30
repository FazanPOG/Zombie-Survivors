using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class LevelScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void SetScoreText(string text) => _scoreText.text = text;
    }
}