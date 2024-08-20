using Microsoft.Extensions.Configuration;
using AzureAIPromptFlowLibrary;
using AzureAIPromptFlowLibrary.Models;

namespace AzureAIConsole;

public class App
{
    private readonly IAzureAIPromptFlowService _azureAIPromptFlowService;
    private readonly IConfiguration _configuration;

    public App(IAzureAIPromptFlowService azureAIPromptFlowService,
               IConfiguration configuration)
    {
        _azureAIPromptFlowService = azureAIPromptFlowService;
        _configuration = configuration;
    }

    public void Run(string[] args)
    {
        // Set the prompt flow variables
        string? promptFlowBaseAddress = _configuration["PromptFlow:BaseAddress"];
        string? promptFlowApiKey = _configuration["PromptFlow:ApiKey"];

        // Open the chat and create a blank chat history
        bool chatOpen = true;
        List<ChatHistoryItem> chatHistory = new List<ChatHistoryItem>();

        // Print an introductory message
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("How can I help?\n");

        while (chatOpen == true)
        {
            // Take in a question
            Console.ForegroundColor = ConsoleColor.White;
            string? question = Console.ReadLine();

            // If the user typed "Exit", close the chat
            if (question == "Exit")
            {
                Console.ForegroundColor = ConsoleColor.White;
                chatOpen = false;
                break;
            }

            // Define variables for the response
            string answer = string.Empty;
            ChatHistoryItem chatHistoryItem = null;

            // If the question is not blank, then invoke the prompt flow
            if (question != null && question != "")
            {
                chatHistoryItem = _azureAIPromptFlowService.InvokeRequestResponseService(question, chatHistory.ToArray<ChatHistoryItem>(), promptFlowBaseAddress, promptFlowApiKey).Result;
                chatHistory.Add(chatHistoryItem);
                answer = chatHistoryItem.outputs.answer;
            }

            // Print the answer to the question
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{answer}\n");

            // Ask for another question or to exit
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ask another question or type 'Exit' to exit\n");
        }
    }
}
