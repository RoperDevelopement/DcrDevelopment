using Azure;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//https://www.bing.com/search?q=butte
//string query = "emotionally adaptive UI design";
//string url = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";/
//await Launcher.Default.OpenAsync(url);
//AIzaSyBRN-bLNuDESYkWM_-eUYjaTWEnXrKQO98
//cx=46a3924c7cf73491e
//ttps://cse.google.com/cse.js?cx=46a3924c7cf73491e
namespace BMRMobileApp.Utilites
{
   public class SearchWeb
    {
        public async Task SearchGoogleAsync(string query)
        {
            string url = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchBingAsync(string query)
        {
            string url = $"https://www.bing.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchCustomSearchGoogleAsync(string query)
        {
            string apiKey = "YOUR_API_KEY";
            string cx = "YOUR_CSE_ID";
            string url = "https://cse.google.com/cse.js?cx=46a3924c7cf73491e&q={Uri.EscapeDataString(query)}";
              //  string url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchGoogleAsyncApiKe(string query)
        {
            string apiKey = "AIzaSyBRN-bLNuDESYkWM_-eUYjaTWEnXrKQO98";
            string cx = "46a3924c7cf73491e";
            string requestUrl = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={Uri.EscapeDataString(query)}";
            using var client = new HttpClient();
            var response = await client.GetAsync(requestUrl);
            var json = await response.Content.ReadAsStringAsync();
            client.Dispose();


        }
        public async Task SearchDuckDuckGoAsync(string query)
        {
            string url = $"https://duckduckgo.com/?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchYahooAsync(string query)
        {
            string url = $"https://search.yahoo.com/search?p={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchBaiduAsync(string query)
        {
            string url = $"https://www.baidu.com/s?wd={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchYandexAsync(string query)
        {
            string url = $"https://yandex.com/search/?text={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchEcosiaAsync(string query)
        {
            string url = $"https://www.ecosia.org/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchQwantAsync(string query)
        {
            string url = $"https://www.qwant.com/?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchStartpageAsync(string query)
        {
            string url = $"https://www.startpage.com/do/dsearch?query={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchSwisscowsAsync(string query)
        {
            string url = $"https://swisscows.com/web?query={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchBraveAsync(string query)
        {
            string url = $"https://search.brave.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchGigablastAsync(string query)
        {
            string url = $"https://www.gigablast.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchInfoSpaceAsync(string query)
        {
            string url = $"https://www.infospacelabs.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchMojeekAsync(string query)
        {
            string url = $"https://www.mojeek.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchMetaGerAsync(string query)
        {
            string url = $"https://metager.org/meta/meta.ger3?eingabe={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchOscoboAsync(string query)
        {
            string url = $"https://www.oscobo.com/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchExaleadAsync(string query)
        {
            string url = $"https://www.exalead.com/search/web/results/?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchYippyAsync(string query)
        {
            string url = $"https://yippy.com/search?query={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchBoardreaderAsync(string query)
        {
            string url = $"https://boardreader.com/?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchInternetArchiveAsync(string query)
        {
            string url = $"https://archive.org/search.php?query={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchWaybackMachineAsync(string query)
        {
            string url = $"https://web.archive.org/web/*/{Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }   
        public async Task SearchWolframAlphaAsync(string query)
        {
            string url = $"https://www.wolframalpha.com/input/?i={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchInternetMovieDatabaseAsync(string query)
        {
            string url = $"https://www.imdb.com/find?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchScholarGoogleAsync(string query)
        {
            string url = $"https://scholar.google.com/scholar?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchPubMedAsync(string query)
        {
            string url = $"https://pubmed.ncbi.nlm.nih.gov/?term={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }   
        public async Task SearchJSTORAsync(string query)
        {
            string url = $"https://www.jstor.org/action/doBasicSearch?Query={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchResearchGateAsync(string query)
        {
            string url = $"https://www.researchgate.net/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchAcademiaEduAsync(string query)
        {
            string url = $"https://www.academia.edu/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchSemanticScholarAsync(string query)
        {
            string url = $"https://www.semanticscholar.org/search?q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchScienceDirectAsync(string query)
        {
            string url = $"https://www.sciencedirect.com/search?qs={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchSpringerLinkAsync(string query)
        {
            string url = $"https://link.springer.com/search?query={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchIEEEExploreAsync(string query)
        {
            string url = $"https://ieeexplore.ieee.org/search/searchresult.jsp?queryText={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }   
       public async Task SearchArXivAsync(string query)
        {
            string url = $"https://arxiv.org/search/?query={Uri.EscapeDataString(query)}&searchtype=all&source=header";
            await Launcher.Default.OpenAsync(url);
        }
        public async Task SearchGoogleBooksAsync(string query)
        {
            string url = $"https://www.google.com/search?tbm=bks&q={Uri.EscapeDataString(query)}";
            await Launcher.Default.OpenAsync(url);
        }
    }
}
