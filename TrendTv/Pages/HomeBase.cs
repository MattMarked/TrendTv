using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using ZapTube.Models;
using System.Text;
using System.Net.Http;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Blazor;

namespace TrendTv.Pages
{
    public class HomeBase : BlazorComponent
    {
        [Inject] public HttpClient _httpClient { get; set; }
        public YouTubeModel model;
        //TODO why this won't work??
        //public string YOUTUBE_API_KEY = Environment.GetEnvironmentVariable("youtube_key");
        public string YOUTUBE_API_KEY = "your_youtube_api_key";
        public string text, err, currentVideoUrl = "", regionCode = "IT";
        public int currentVideo = 0, offset = 0, categoryId = 0;
        public const int MAX_NUMBER_OF_VIDEO = 50;
        public bool[] viewedVideos = new bool[MAX_NUMBER_OF_VIDEO];
        public bool collapseNavMenu = true, autoRegionCode = true;
        public Random rnd = new Random();
        public StringBuilder sb = new StringBuilder();
        public Dictionary<string, int> categories = new Dictionary<string, int>
        {
            { "Trending", 0 },
            { "Music", 10 },
            {"Sport",17 },
            {"Gaming",20 },
            {"Science",28 },
            {"Trailers",44 }
        };  

        /// <summary>
        /// Generate our api url with some filters
        /// </summary>
        /// <returns></returns>
        public string GenerateUrl()
        {
            sb.Clear();
            //First we set most popular and regionCode
            sb.Append("videos?part=id&chart=mostPopular&regionCode=");
            sb.Append(regionCode);
            //Then the maximum number of results
            sb.Append("&maxResults=");
            sb.Append(MAX_NUMBER_OF_VIDEO);
            //Then the video category
            sb.Append("&videoCategoryId=");
            sb.Append(categoryId);
            //And at last our youtube api key
            sb.Append("&key=");
            sb.Append(YOUTUBE_API_KEY);
            return sb.ToString();
        }

        /// <summary>
        /// Get randomly a new unwatched video from our list and set url and offset
        /// </summary>
        public void RandomVideo()
        {
            rnd = new Random();
            CheckResetViews();
            int i = rnd.Next(MAX_NUMBER_OF_VIDEO);
            while (viewedVideos[i])
            {
                i = rnd.Next(MAX_NUMBER_OF_VIDEO);

            }
            currentVideo = i;
            currentVideoUrl = model.items.ElementAt(currentVideo).id;
            viewedVideos[currentVideo] = true;
            offset = 60 + i;
        }

        /// <summary>
        /// Check if all videos are watched. If it's true, reset the viewedVideos array
        /// </summary>
        public void CheckResetViews()
        {
            if (!viewedVideos.Any(x => !x))
                viewedVideos = new bool[MAX_NUMBER_OF_VIDEO];
        }
        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
      
        protected override async Task OnInitAsync()
        {

            await FetchData();
            RandomVideo();
        }

        /// <summary>
        /// Change the region code and re-fetch the videos list. The load a new video
        /// </summary>
        /// <param name="newRegionCode"></param>
        /// <returns></returns>
        public async Task ChangeRegionCode(string newRegionCode)
        {
            regionCode = newRegionCode;
            autoRegionCode = false;
            await FetchData();
            RandomVideo();
        }

        /// <summary>
        /// Change the category id and re-fetch the videos list. The load a new video
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ChangeCategory(int id)
        {
            categoryId = id;
            await FetchData();
            RandomVideo();
        }
        /// <summary>
        /// Fetch a list of videos from youtube
        /// </summary>
        /// <returns></returns>
        protected async Task FetchData()
        {
            _httpClient.BaseAddress = new System.Uri("https://www.googleapis.com/youtube/v3/");

            if (autoRegionCode)
            {
                string tempregionCode = await JSRuntime.Current.InvokeAsync<string>("myFunction.local");
                if (tempregionCode.Contains("-"))
                    regionCode = tempregionCode.Split('-')[1];
            }

            text = await _httpClient.GetStringAsync(GenerateUrl());
            try
            {
                model = Json.Deserialize<YouTubeModel>(text);
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }
        }
    }
}
