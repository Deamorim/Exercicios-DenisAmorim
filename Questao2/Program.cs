using Newtonsoft.Json;

public class Program
{
    const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals($"{BaseUrl}?year={year}&team1={teamName}").GetAwaiter().GetResult();
        totalGoals += getTotalScoredGoals($"{BaseUrl}?year={year}&team2={teamName}").GetAwaiter().GetResult();

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals($"{BaseUrl}?year={year}&team1={teamName}").GetAwaiter().GetResult();
        totalGoals += getTotalScoredGoals($"{BaseUrl}?year={year}&team2={teamName}").GetAwaiter().GetResult();

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic jsonData = JsonConvert.DeserializeObject(responseBody);

                int totalGoals = 0;

                foreach (var match in jsonData.data)
                {
                    totalGoals += (int)match.team1goals;
                }

                int i = (int)jsonData.page;
                i++;

                while (i <= (int)jsonData.total_pages)
                {
                    HttpResponseMessage responseSecound = await client.GetAsync($"{url}&page={i}");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBodySecound = await responseSecound.Content.ReadAsStringAsync();
                        var jsonDataSecound = JsonConvert.DeserializeObject<dynamic>(responseBodySecound);

                        foreach (var match in jsonDataSecound.data)
                        {
                            totalGoals += (int)match.team1goals;
                        }
                    }
                    i++;
                }

                return totalGoals;
            }
            else
            {
                Console.WriteLine($"Erro ao acessar a API. Status Code: {response.StatusCode}");
                return -1; // Retorna um valor negativo para indicar um erro
            }
        }
    }

}