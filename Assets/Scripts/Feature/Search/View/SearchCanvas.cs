using Common.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private TMP_InputField _searchInputField;
        private Button _searchButton;

        private void Awake()
        {
            _searchInputField = GetComponentInChildren<TMP_InputField>();
            _searchButton = GetComponentInChildren<Button>();

            _searchButton.onClick.AddListener(OnSearch);
        }

        private void OnSearch()
        {
            if (string.IsNullOrEmpty(_searchInputField.text)) return;

            this.Emit(new OnClickSearchButton(_searchInputField.text));
        }
    }
}