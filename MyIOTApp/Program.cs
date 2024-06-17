using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MyIOTApp;
using System.Net;
using static System.Environment;

string modelId = GetEnvironmentVariable("OPENAI_MODEL");
string apiKey = GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(apiKey))
{
    Exit(0);
}

var builder = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(modelId, apiKey);
builder.Plugins.AddFromType<LightPlugin>();
Kernel kernel = builder.Build();
// Create chat history
var history = new ChatHistory();

// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Start the conversation
Console.Write("User > ");
string? userInput;
while ((userInput = Console.ReadLine()) != null)
{
    // Add user input
    history.AddUserMessage(userInput);

    // Enable auto function calling
    OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);

    // Get user input again
    Console.Write("User > ");
}