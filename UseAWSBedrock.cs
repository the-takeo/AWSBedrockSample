using System;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Amazon;
using Amazon.Util;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.BedrockAgentRuntime;
using Amazon.BedrockAgentRuntime.Model;

namespace AWSBedrockSample
{
    internal class UseAWSBedrock
    {
        /// <summary>
        /// モデルにクエリを送信する。
        /// </summary>
        /// <param name="prompt">プロンプト</param>
        /// <returns></returns>
        public static async Task<string> InvokeModelAsync(string prompt)
        {
            var enclosedPrompt = "\n\nHuman:" + prompt + "\n\nAssistant:";
            var payload = new JsonObject
                {
                    { "prompt",enclosedPrompt },
                    { "max_tokens_to_sample",4000 },
                    { "temperature",0.5 }
                    //{ "stop_sequences",new JsonArray("\n\nHuman:") }
                }.ToJsonString();

            var request = new InvokeModelRequest
            {
                ModelId = "anthropic.claude-v2:1",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload),
                ContentType = "application/json",
                Accept = "application/json"
            };

            var bedrockClient = new AmazonBedrockRuntimeClient(Credentials.Credential, RegionEndpoint.USEast1);
            try
            {
                var response = await bedrockClient.InvokeModelAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    using var reader = new StreamReader(response.Body);
                    return await reader.ReadToEndAsync();
                }
                else
                {
                    return "Model invocation failed with status code: " + response.HttpStatusCode;
                }
            }
            catch (Exception e)
            {
                return "Error invoking model: " + e.Message;
            }
        }


        /// <summary>
        /// ナレッジベースにクエリを送信する。
        /// </summary>
        /// <param name="prompt">プロンプト</param>
        /// <returns></returns>
        public static async Task<string> InvokeAgentAsync(string prompt)
        {
            var bedrockAgentClient = new AmazonBedrockAgentRuntimeClient(Credentials.Credential, RegionEndpoint.USEast1);

            var invokeAgentRequest = new InvokeAgentRequest
            {
                AgentId = AgentInfo.AgentId,
                AgentAliasId = AgentInfo.AgentAliasId,
                InputText = prompt,
                SessionId = Guid.NewGuid().ToString()
            };

            try
            {
                var response = await bedrockAgentClient.InvokeAgentAsync(invokeAgentRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    // ToDo:forerachの流用ではなく、直接レスポンスを取得できるようにする。
                    foreach (Amazon.BedrockAgentRuntime.Model.PayloadPart item in response.Completion)
                    {
                        using var reader = new StreamReader(item.Bytes);
                        return await reader.ReadToEndAsync();
                    }

                    // ここには来ない想定。
                    return "Error response";
                }
                else
                {
                    return "Agent invocation failed with status code: " + response.HttpStatusCode;
                }
            }
            catch (Exception ex)
            {
                return $"Error invoking Agent: {ex.Message}";
            }
        }
    }
}
