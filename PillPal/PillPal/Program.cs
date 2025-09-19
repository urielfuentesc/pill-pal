using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.OpenAI;
using OpenAI.Chat;

string endpoint = "https://ai-resource-vision.cognitiveservices.azure.com/";
string key = "API_KEY";
AzureKeyCredential credential = new AzureKeyCredential(key);
DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), credential);

string filePath = "C:\\Users\\urielf\\OneDrive - Microsoft\\Documents\\Tasks\\2025\\September\\Hackathon\\rx.png";

if (!File.Exists(filePath))
{
	Console.WriteLine($"File not found: {filePath}");
	return;
}

// Validate basic size (optional; service has limits ~50 MB for PDF/images; adjust if needed)
FileInfo fi = new FileInfo(filePath);
if (fi.Length == 0)
{
	Console.WriteLine("File is empty.");
	return;
}

Console.WriteLine($"Analyzing file: {filePath} ({fi.Length / 1024.0:F1} KB).");

var endpointGpt = new Uri("https://test-gpt-hackathon.openai.azure.com/");
var model = "gpt-4.1";
var deploymentName = "gpt-4.1";
var apiKey = "API_KEY";

AzureOpenAIClient azureClient = new(
	endpointGpt,
	new AzureKeyCredential(apiKey));
ChatClient chatClient = azureClient.GetChatClient(deploymentName);

// Stream -> BinaryData (avoids loading twice)
using FileStream fs = File.OpenRead(filePath);
BinaryData data = BinaryData.FromStream(fs);

Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", data);
AnalyzeResult result = operation.Value;

string fullDocumentText = result.Content ?? string.Empty;
//Console.WriteLine(fullDocumentText);

string example = "{\n  \"drugs\": [\n    {\n      \"name\": \"aspirin\",\n      \"dosage\": \"1 capsule of 500mg\",\n      \"frequency\": [\"Morning\", \"Evening\"]\n    }\n  ]\n}";
List<ChatMessage> messages = new List<ChatMessage>()
{
	new SystemChatMessage("The following is the text extracted from a drug prescription. Return this data for each drug prescibed: name, dosage, frequency (array with options: Morning, Midday, Evening). Return a data in a JSON array." +
						  "Example: " + example + ". This is the drug prescription text: " + fullDocumentText),
};

var response = chatClient.CompleteChat(messages);
System.Console.WriteLine(response.Value.Content[0].Text);