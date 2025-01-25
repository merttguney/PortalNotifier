using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

class Program
{
    static async Task Main(string[] args)
    {
        var url = "https://ois.atlas.edu.tr/sistem/mesaj/mesajliste/tip/gelen";

        try
        {
            var html = await GetHtmlAsync(url);
            var messages = await GetMessagesFromHtml(html);
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata: " + ex.Message);
        }
    }

    static async Task<string> GetHtmlAsync(string url)
    {
        using (var client = new HttpClient())
        {
            return await client.GetStringAsync(url);
        }
    }

    static async Task<string[]> GetMessagesFromHtml(string html)
    {
        var doc = new XmlDocument();
        try
        {
            await doc.LoadXmlAsync(html); // Hata yönetimi eklendi
        }
        catch (Exception ex)
        {
            Console.WriteLine("HTML içeriği yüklenirken hata: " + ex.Message);
            return new string[0]; // Hata durumunda boş dizi döndür
        }

        // Daha spesifik bir XPath ifadesi kullanabilirsiniz (örnek)
        var xpath = "//div[contains(@class, 'message')]";

        var navigator = doc.CreateNavigator();
        var nodes = navigator.Select(xpath);

        var messages = new List<string>();
        foreach (var node in nodes)
        {
            var messageText = node.SelectSingleNode(".//p[@class='message-text']")?.InnerText;
            if (messageText != null)
            {
                messages.Add(messageText);
            }
        }

        return messages.ToArray();
    }
}