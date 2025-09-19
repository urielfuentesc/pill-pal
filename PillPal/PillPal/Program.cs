using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.OpenAI;
using OpenAI.Chat;

//set `<your-endpoint>` and `<your-key>` variables with the values from the Azure portal to create your `AzureKeyCredential` and `DocumentIntelligenceClient` instance
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

//foreach (DocumentPage page in result.Pages)
//{
//	Console.WriteLine($"Document Page {page.PageNumber} has {page.Lines.Count} line(s), {page.Words.Count} word(s)," +
//		$" and {page.SelectionMarks.Count} selection mark(s).");

//	for (int i = 0; i < page.Lines.Count; i++)
//	{
//		DocumentLine line = page.Lines[i];

//		Console.WriteLine($"  Line {i}:");
//		Console.WriteLine($"    Content: '{line.Content}'");

//		Console.Write("    Bounding polygon, with points ordered clockwise:");
//		for (int j = 0; j < line.Polygon.Count; j += 2)
//		{
//			Console.Write($" ({line.Polygon[j]}, {line.Polygon[j + 1]})");
//		}

//		Console.WriteLine();
//	}

//	for (int i = 0; i < page.SelectionMarks.Count; i++)
//	{
//		DocumentSelectionMark selectionMark = page.SelectionMarks[i];

//		Console.WriteLine($"  Selection Mark {i} is {selectionMark.State}.");
//		Console.WriteLine($"    State: {selectionMark.State}");

//		Console.Write("    Bounding polygon, with points ordered clockwise:");
//		for (int j = 0; j < selectionMark.Polygon.Count; j++)
//		{
//			Console.Write($" ({selectionMark.Polygon[j]}, {selectionMark.Polygon[j + 1]})");
//		}

//		Console.WriteLine();
//	}
//}

//for (int i = 0; i < result.Paragraphs.Count; i++)
//{
//	DocumentParagraph paragraph = result.Paragraphs[i];

//	Console.WriteLine($"Paragraph {i}:");
//	Console.WriteLine($"  Content: {paragraph.Content}");

//	if (paragraph.Role != null)
//	{
//		Console.WriteLine($"  Role: {paragraph.Role}");
//	}
//}

//foreach (DocumentStyle style in result.Styles)
//{
//	// Check the style and style confidence to see if text is handwritten.
//	// Note that value '0.8' is used as an example.

//	bool isHandwritten = style.IsHandwritten.HasValue && style.IsHandwritten == true;

//	if (isHandwritten && style.Confidence > 0.8)
//	{
//		Console.WriteLine($"Handwritten content found:");

//		foreach (DocumentSpan span in style.Spans)
//		{
//			var handwrittenContent = result.Content.Substring(span.Offset, span.Length);
//			Console.WriteLine($"  {handwrittenContent}");
//		}
//	}
//}

//for (int i = 0; i < result.Tables.Count; i++)
//{
//	DocumentTable table = result.Tables[i];

//	Console.WriteLine($"Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");

//	foreach (DocumentTableCell cell in table.Cells)
//	{
//		Console.WriteLine($"  Cell ({cell.RowIndex}, {cell.ColumnIndex}) is a '{cell.Kind}' with content: {cell.Content}");
//	}
//}