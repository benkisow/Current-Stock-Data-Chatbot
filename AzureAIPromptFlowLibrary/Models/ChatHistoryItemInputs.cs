namespace AzureAIPromptFlowLibrary.Models;

public class ChatHistoryItemInputs
{
    public ChatHistoryItemInputs(string? question)
    {
        this.question = question;
    }

    public string? question { get; set; }
}
