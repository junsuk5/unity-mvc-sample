using System.Collections.Generic;

namespace Data.Dto
{
    public class ImageResponse
    {
        private readonly int _statusCode;
        private readonly Dictionary<string, string> _headers;
        private readonly string _body;

        public int StatusCode => _statusCode;

        public Dictionary<string, string> Headers => new(_headers);

        public string Body => _body;

        public ImageResponse(int statusCode, Dictionary<string, string> headers, string body)
        {
            _statusCode = statusCode;
            _headers = headers;
            _body = body;
        }
    }
}