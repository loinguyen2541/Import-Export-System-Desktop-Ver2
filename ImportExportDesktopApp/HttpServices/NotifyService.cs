using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/9/2021 9:10:06 AM 
*/

namespace ImportExportDesktopApp.HttpServices
{
    class NotifyService
    {
        private HttpClient Client = new HttpClient();

        public NotifyService()
        {
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void NotifyAndroid()
        {
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=AAAAhgjnK7U:APA91bHjz9YtWR8BJWIuMKVIOWqsvQOIsb4WB52O2pBNzqcgwp2EDLaaQ33214CQYGHEA3sCosIJhexlgsdx-39UAyTI_tnq902zUdBMO0EEE8-aqEReGWbhn0b3UvoeQgFSPzhwJO1i");

            StringContent stringContent = new StringContent("{\"notification\": {" +
                "\"title\": \"update completed\"," +
            "\"body\": \"Updated\"}," +
            "\"to\" : \"e1z9ciPGQCC7ZcDe5CiCH7:APA91bHjrMryex-cv-vNB3m9db2kwwVtkhAq7kBJPsDBenM8FWhsYY4xkK7nesH8PszNkWOM72QlAt7Cu85idhlTJdMzIk4wLXTXcOfVz4y4h6RfoLUz5iiWGJOHLZk8XkYLpAz8tjxa\"}\"", Encoding.UTF8,
                                    "application/json");
            var response = Client.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent).Result;
        }

        public void NotifyWeb()
        {
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=AAAAMz0s7oc:APA91bFhXNj2-4FAh5yeMu2oNKRvbBlqXHHXGu5UiHZ5epGLh5i-VuZ-NR2W3407RaLIgMq2HJGUN5NX1JzwgaI7Iq0OOC6dRnFByxzhHWSEqXIJ8hpIydinIVukyiYmTMVTgdHBl8B8");

            StringContent stringContent = new StringContent("{\"to\":\"cHibh-60NM1cJTSisDPayM:APA91bEoeWepPfCCWMH5PhGqBZYHoD2ZkRp0rNNCGBYagKrgKYq4lNhSd5c-e_b784BgswUaf8G2SC0ZbcGrXrxMBJ2ZT2itU7ojCk4twVFOp-ejwo6dgj2JwFbeH9tRdOrTv1JDM18M\", \"notification\" : {\"body\" : \"IScale System \nThere is new transaction\",\"OrganizationId\":\"2\",\"content_available\" : true,\"priority\" : \"high\",\"subtitle\":\"Elementary School\",\"Title\":\"ISacle System\"},\"data\" : {\"priority\" : \"high\",\"sound\":\"app_sound.wav\",\"content_available\" : true,\"bodyText\" : \"New Announcement assigned\",\"organization\" :\"FPT school\"}}", Encoding.UTF8, "application/json");
            var response = Client.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent).Result;
        }
    }
}
