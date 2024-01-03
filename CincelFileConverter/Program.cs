//File converter in cincel
using System.Net.Http.Headers;

var inBytes = File.ReadAllBytes("./doc.docx");
var path = "./doc.docx";

var pathBytes = await ConvertFromPath(path);
var byteArrayBytes = await ConvertFromByteArray(inBytes);

File.WriteAllBytes("./bytes.pdf", byteArrayBytes);
File.WriteAllBytes("./path.pdf", pathBytes);



static async Task<byte[]> ConvertFromPath(string path)
{
    using (var httpClient = new HttpClient())
    using (var formData = new MultipartFormDataContent())
    using (var fileStream = File.OpenRead(path))
    using (var fileContent = new StreamContent(fileStream))
    {
        // Add the image file to the form data
        formData.Add(fileContent, "file", Path.GetFileName(path));

        // Set the content type header
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

        // Send the request and get the response
        var response = await httpClient.PostAsync("https://sandbox.api.cincel.digital/v3/convert-to-pdf", formData);

        // Check if the response is successful
        if (response.IsSuccessStatusCode)
        {
            // Handle the successful response
            return await response.Content.ReadAsByteArrayAsync();
        }
        else
        {
            // Handle the case where the response is not successful
            var resultString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Request failed with status code {response.StatusCode}. Response: {resultString}");
            throw new Exception("Failed");
        }
    }
}



static async Task<byte[]> ConvertFromByteArray(byte[] data)
{

    try
    {
        // Create an HTTP client
        using var httpClient = new HttpClient();
        using var formData = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(data);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

        formData.Add(fileContent, "file", "doc.docx");

        // Send the request and get the response
        var response = await httpClient.PostAsync("https://sandbox.api.cincel.digital/v3/convert-to-pdf", formData);

        // Check if the response is successful
        if (response.IsSuccessStatusCode)
        {
            // Handle the successful response
            return await response.Content.ReadAsByteArrayAsync();
        }
        else
        {
            throw new Exception("Http status code failed");
        }

    }
    catch (Exception ex)
    {
        throw;
    }
}