using System.Net.Http.Headers;
using System.Text.Json;
using AzureAIPromptFlowLibrary.Models;

namespace AzureAIPromptFlowLibrary;

public class AzureAIPromptFlowService : IAzureAIPromptFlowService
{
    // Method to invoke the prompt flow
    // Returns a ChatHistoryItem, which contains both the question and the answer
    // This method is built off the code provided by Azure AI Studio, under the Consume tab of the prompt flow deployment
    public async Task<ChatHistoryItem> InvokeRequestResponseService(string question, ChatHistoryItem[] chatHistory, string promptFlowBaseAddress, string promptFlowApiKey)
    {
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };
        using (var client = new HttpClient(handler))
        {
            // Request data goes here
            // The example below assumes JSON formatting which may be updated
            // depending on the format your endpoint expects.
            // More information can be found here:
            // https://docs.microsoft.com/azure/machine-learning/how-to-deploy-advanced-entry-script
            string chatHistoryString = JsonSerializer.Serialize(chatHistory);
            var requestBody = $@"
                {{
                    ""chat_history"": {chatHistoryString},
                    ""question"": ""{question}""
                }}";

            // Replace this with the primary/secondary key, AMLToken, or Microsoft Entra ID token for the endpoint
            string apiKey = promptFlowApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("A key should be provided to invoke the endpoint");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(promptFlowBaseAddress);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // WARNING: The 'await' statement below can result in a deadlock
            // if you are calling this code from the UI thread of an ASP.Net application.
            // One way to address this would be to call ConfigureAwait(false)
            // so that the execution does not attempt to resume on the original context.
            // For instance, replace code such as:
            //      result = await DoSomeTask()
            // with the following:
            //      result = await DoSomeTask().ConfigureAwait(false)
            HttpResponseMessage response = await client.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ChatHistoryItemOutputs? chatHistoryItemOutputs = JsonSerializer.Deserialize<ChatHistoryItemOutputs>(result);
                ChatHistoryItemInputs? chatHistoryItemInputs = new ChatHistoryItemInputs(question);
                ChatHistoryItem? chatHistoryItem = new ChatHistoryItem(chatHistoryItemInputs, chatHistoryItemOutputs);

                return chatHistoryItem;
            }
            else
            {
                Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                // Print the headers - they include the requert ID and the timestamp,
                // which are useful for debugging the failure
                Console.WriteLine(response.Headers.ToString());

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                return null;
            }
        }
    }
}