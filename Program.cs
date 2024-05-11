using AWSBedrockSample;

Console.Write("モデルに質問する場合は1と入力してください: ");
var isModel = Console.ReadLine() == "1";

Console.Write("質問を入力してください: ");
var userQuestion = Console.ReadLine();

string answer;

if (isModel)
{ answer = await UseAWSBedrock.InvokeModelAsync(userQuestion); }
else
{ answer = await UseAWSBedrock.InvokeAgentAsync(userQuestion); }

Console.WriteLine("回答: " + answer);
Console.ReadKey();