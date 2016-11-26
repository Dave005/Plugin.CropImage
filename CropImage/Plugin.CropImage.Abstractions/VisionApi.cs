﻿
using Stannieman.HttpQueries;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace Plugin.CropImage.Abstractions
{
    /// <summary>
    /// Class that holds static property with key for VisionApi.
    /// </summary>
    public class VisionApi
    {
        /// <summary>
        /// Vision Subscription Key
        /// </summary>
        public static string Key{get;set;}

        /// <summary>
        /// Returns an image byte[] smartcropped ...
        /// </summary>
        /// <param name="originalSource"></param>
        /// <param name="subscriptionKey"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="smartCrop">default true</param>
        /// <returns></returns>
      async  public static Task<byte []> GetThumbNail(byte[] originalSource ,string subscriptionKey,int width,int height, bool smartCrop=true)
        {
            var client = new HttpClient();
            var queryString = new Query();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey );

            // Request parameters
            queryString.AddParameter("width", width);
            queryString.AddParameter("height", height);
            queryString.AddParameter("smartCropping", smartCrop.ToString());
            
            var uri = "https://api.projectoxford.ai/vision/v1.0/generateThumbnail?" + queryString.QueryString;

            HttpResponseMessage response;

            // Request body

            Debug.WriteLine("Vision Api Request Uri = " + uri);
            


            using (var content = new ByteArrayContent(originalSource))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                Debug.WriteLine("Headers ToString = "+content.Headers.ToString());
                response = await client.PostAsync(uri, content);
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();

        }
    }
}
