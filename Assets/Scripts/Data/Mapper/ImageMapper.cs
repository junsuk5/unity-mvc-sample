using System.Collections.Generic;
using Data.Dto;
using Newtonsoft.Json;

namespace Data.Mapper
{
    public static class ImageMapperExtensions
    {
        public static ImageResultDto JsonToImageResultDto(this string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            try
            {
                return JsonConvert.DeserializeObject<ImageResultDto>(json);
            }
            catch (JsonSerializationException e)
            {
                return null;
            }
        }

        public static List<ImageDto> ToImageDTOs(this ImageResultDto imageResultDto)
        {
            if (imageResultDto == null || imageResultDto.hits == null) return new List<ImageDto>();

            return imageResultDto.hits;
        }
    }
}