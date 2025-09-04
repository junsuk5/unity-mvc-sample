using Common.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Feature.Search.View
{
    public struct OnClickSearchButton : IEvent
    {
        public string SearchText { get; }

        public OnClickSearchButton(string searchText)
        {
            SearchText = searchText;
        }
    }

    public class SearchCanvas : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private TMP_InputField searchInputField;
        [SerializeField] private Button searchButton;

        private void Awake()
        {
            searchButton.onClick.AddListener(OnSearch);
        }

        private void OnSearch()
        {
            if (string.IsNullOrEmpty(searchInputField.text)) return;

            this.Emit(new OnClickSearchButton(searchInputField.text));
        }
    }
}