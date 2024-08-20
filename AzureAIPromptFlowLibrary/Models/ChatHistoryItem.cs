namespace AzureAIPromptFlowLibrary.Models;

public class ChatHistoryItem
{
    public ChatHistoryItem(ChatHistoryItemInputs inputs, ChatHistoryItemOutputs outputs)
    {
        this.inputs = inputs;
        this.outputs = outputs;
    }

    public ChatHistoryItemInputs inputs { get; set; }
    public ChatHistoryItemOutputs outputs { get; set; }
}