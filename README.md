# Current Stock Data Chatbot
## Overview
This is a .NET application that uses a console UI for a chatbot that is based on a Prompt Flow from Azure AI Studio. It is built for a Prompt Flow that looks up current stock market data, but the code will work with any Prompt Flow.

## Usage
This repository can be forked and will work with any Azure AI Prompt Flow that has "chat_history" and "question" inputs, and an "answer" output. The only change that must be made is to replace the information in `appsettings.json` with your Prompt Flow's base address and API key.
```json
{
  "PromptFlow": {
    "BaseAddress": "YOUR_PROMPT_FLOW_BASE_ADDRESS",
    "ApiKey": "YOUR_PROMPT_FLOW_API_KEY"
  }
}
```
