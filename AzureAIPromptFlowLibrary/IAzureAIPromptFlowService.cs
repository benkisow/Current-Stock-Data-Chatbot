using AzureAIPromptFlowLibrary.Models;

namespace AzureAIPromptFlowLibrary;

public interface IAzureAIPromptFlowService
{
    public Task<ChatHistoryItem> InvokeRequestResponseService(string question, ChatHistoryItem[] chatHistory, string promptFlowBaseAddress, string promptFlowApiKey);
}
