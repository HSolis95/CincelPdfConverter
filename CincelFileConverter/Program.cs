//File converter in cincel
using System.Net.Http;
using System.Net.Http.Headers;

var richEdit = File.ReadAllBytes("./doc.docx");
var responseBytes = await ConvertToPdf(richEdit);
File.WriteAllBytes(Environment.CurrentDirectory + "/doc.pdf", responseBytes);

static async Task<byte[]> ConvertToPdf(byte[] data)
{
    string apiEndpoint = "https://sandbox.api.cincel.digital/v3/convert-to-pdf";
    try
    {
        var stream = new MemoryStream(data);

        // Create an HTTP client
        using var httpClient = new HttpClient();

        // Create a POST request to the "convert-to-pdf" endpoint
        using var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint);

        // Create a MultipartFormDataContent and add the file content
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(stream), "file", "doc.docx" }
        };
        request.Content = content;

        // Send the request and get the response
        var response = await httpClient.SendAsync(request);

        // Check if the response is successful
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }
        else
        {
            throw new Exception("request code failed");
            // Handle the case where the response is not successful
        }
    }
    catch (Exception ex)
    {
        Console.Error.Write(ex.ToString());
        Console.ReadLine();
        throw;
    }
}