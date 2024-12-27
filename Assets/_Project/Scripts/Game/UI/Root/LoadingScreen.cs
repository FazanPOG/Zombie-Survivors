using UnityEngine;

namespace _Project.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}