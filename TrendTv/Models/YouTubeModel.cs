using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapTube.Models
{
    public class YouTubeModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        //public string pageInfo { get; set; }
        public List<item> items { get; set; }
    }

    public class item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public snippet snippet { get; set; }
    }

    public class snippet
    {
        //public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public thumbnail thumbnails { get; set; }
        public string channelTitle { get; set; }
        public List<string> tags { get; set; }
    }

    public class thumbnail
    {
        public thumbnailObj high { get; set; }
        public thumbnailObj medium { get; set; }
        public thumbnailObj standard { get; set; }
        public thumbnailObj maxres { get; set; }

    }

    public class thumbnailObj
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
