using System.Collections.Generic;

namespace Data.Dto
{
    [System.Serializable]
    public class ImageResultDto
    {
        public int total;
        public int totalHits;
        public List<ImageDto> hits;
    }

    [System.Serializable]
    public class ImageDto
    {
        public int id;
        public string pageURL;
        public string type;
        public string tags;
        public string previewURL;
        public int previewWidth;
        public int previewHeight;
        public string webformatURL;
        public int webformatWidth;
        public int webformatHeight;
        public string largeImageURL;
        public int imageWidth;
        public int imageHeight;
        public int imageSize;
        public int views;
        public int downloads;
        public int collections;
        public int likes;
        public int comments;
        public int user_id;
        public string user;
        public string userImageURL;
        public bool noAiTraining;
        public bool isAiGenerated;
        public bool isGRated;
        public bool isLowQuality;
        public string userURL;
    }
}