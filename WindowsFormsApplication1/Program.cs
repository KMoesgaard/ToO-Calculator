using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace TooCalculator
{
    static class Program
    {
        //Uses last active character if 0!
        static int characternr = 0;
        static string membershipId;
        static string characterId;
        static string activityId;
        static string className;
        static string trialsKD;
        static string accountName;

        public static List<string> A = new List<string>();
        public static List<string> B = new List<string>();
        public static List<string> fireTeam = new List<string>();
        public static List<playerInfo> info = new List<playerInfo>();

        public static bool validUserName = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrimaryWindow());

        }

        public static void getMembershipId(string accountName)
        {

            using (var client = new HttpClient())
            {
                // Input Your own Api Key Here
                client.DefaultRequestHeaders.Add("X-API-Key", "INPUT YOUR API KEY HERE!!");

                //Get Membership ID
                var response = client.GetAsync("https://www.bungie.net/platform/Destiny/2/Stats/GetMembershipIdByDisplayName/" + accountName).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                membershipId = item.Response;

                //if username is not valid membership id is returned as a string "0"
                if (membershipId == "0") {
                    validUserName = false;
                }
                // need this to flip the bool on second input!
                if (membershipId != "0")
                {
                    validUserName = true;
                }

                // Only run if user name is valid!
                if (validUserName == true) { 
                    //Get Account characterid
                    var response1 = client.GetAsync("https://www.bungie.net/platform/Destiny/2/Account/" + membershipId + "/Summary/").Result;
                    var content1 = response1.Content.ReadAsStringAsync().Result;
                    dynamic item1 = Newtonsoft.Json.JsonConvert.DeserializeObject(content1);
                    characterId = item1.Response.data.characters[characternr].characterBase.characterId;

                    //find activityId
                    var response4 = client.GetAsync("http://www.bungie.net/Platform/Destiny/Stats/ActivityHistory/2/" + membershipId + "/" + characterId + "/?count=1&mode=14").Result;
                    var content4 = response4.Content.ReadAsStringAsync().Result;
                    dynamic item4 = Newtonsoft.Json.JsonConvert.DeserializeObject(content4);
                    activityId = item4.Response.data.activities[0].activityDetails.instanceId;

                    //find other people in fireteam
                    var response5 = client.GetAsync("http://www.bungie.net/Platform/Destiny/Stats/PostGameCarnageReport/" + activityId).Result;
                    var content5 = response5.Content.ReadAsStringAsync().Result;
                    dynamic item5 = Newtonsoft.Json.JsonConvert.DeserializeObject(content5);

                    // Add Players to A and B lists - indication of team!
                    for (int i = 0; i < 6; i++)
                    {
                        string playerName = item5.Response.data.entries[i].player.destinyUserInfo.displayName;

                        if (item5.Response.data.entries[i].values.team.basic.displayValue == "Alpha")
                        {
                            A.Add(playerName);
                        }

                        if (item5.Response.data.entries[i].values.team.basic.displayValue == "Bravo")
                        {
                            B.Add(playerName);
                        }
                    }

                    //Iterate each team list to determine which team you are on! when found the team is added to a fireteam list
                    foreach (string i in B)
                    {
                        if (accountName == i)
                        {
                            fireTeam.AddRange(B);
                        }
                    }

                    foreach (string i in A)
                    {
                        if (accountName == i)
                        {
                            fireTeam.AddRange(A);
                        }
                    }

                    // Membership ID, characterid and alltime stats for ToO retrieved for each item i fireteam
                    foreach (string i in fireTeam)
                    {

                        //Get Membership ID
                        var response6 = client.GetAsync("https://www.bungie.net/platform/Destiny/2/Stats/GetMembershipIdByDisplayName/" + i).Result;
                        var content6 = response6.Content.ReadAsStringAsync().Result;
                        dynamic item6 = Newtonsoft.Json.JsonConvert.DeserializeObject(content6);
                        membershipId = item6.Response;

                        //Get Account characterid
                        var response7 = client.GetAsync("https://www.bungie.net/platform/Destiny/2/Account/" + membershipId + "/Summary/").Result;
                        var content7 = response7.Content.ReadAsStringAsync().Result;
                        dynamic item7 = Newtonsoft.Json.JsonConvert.DeserializeObject(content7);
                        characterId = item7.Response.data.characters[characternr].characterBase.characterId;

                        string classHash = item7.Response.data.characters[characternr].characterBase.classHash;

                        ////produce alltime stats for ToO modes=14 is ToO
                        var response3 = client.GetAsync("https://www.bungie.net/Platform/Destiny/Stats/2/" + membershipId + "/" + characterId + "/?modes=14").Result;
                        var content3 = response3.Content.ReadAsStringAsync().Result;
                        dynamic item3 = Newtonsoft.Json.JsonConvert.DeserializeObject(content3);

                        trialsKD = item3.Response.trialsOfOsiris.allTime.killsDeathsRatio.basic.displayValue;

                        // determine the active character class based on the Hash code for class
                        switch (classHash)
                        {
                            case "2271682572":
                                className = "Warlock";
                                break;
                            case "3655393761":
                                className = "Titan";
                                break;
                            case "671679327":
                                className = "Hunter";
                                break;
                        }

                        //instansiate new object of player class with the retrived data, which is then added to a list
                        playerInfo obj = new playerInfo(i, trialsKD, className);
                        info.Add(obj);

                    }
                    //clear all list!
                    fireTeam.Clear();
                    A.Clear();
                    B.Clear();
                   
                }

            }
        }

    }
}


