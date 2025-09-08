using UnityEngine;
using UnityEngine.UI;

namespace Feature.SearchDetail.View
{
    public class DetailImageView : MonoBehaviour
    {
        [SerializeField] private RawImage detailImage;
        
        private void Awake()
        {
        }
        
        public void SetImage(Texture2D texture)
        {
            if (detailImage != null)
            {
                detailImage.texture = texture;
            }
        }
        
        public void SetImage(Texture texture)
        {
            if (detailImage != null)
            {
                detailImage.texture = texture;
            }
        }
    }
}