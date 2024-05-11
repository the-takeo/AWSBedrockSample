# AWSBedrockSample
Sample code to ask questions to AWS Bedrock models.
You can also ask questions to AWS Bedrock agents. Agents can be tied to the knowledge base, making it easy to create a RAG chat.

Because I excluded `Credentials.cs`, you should add following class.

    using Amazon.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace AWSBedrockSample
    {
        internal class Credentials
        {
            // クレデンシャルを明示的に指定。
            // stakedaのIAMのアクセスキー。
            // これは外部には絶対に公開しないこと。
            public static readonly BasicAWSCredentials Credential = new("XXXXXXXXXXXXXXXXXXXX", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        }
        internal static class AgentInfo
        {
            // SactonaSupportのAgentID
            // これは外部には絶対に公開しないこと。
            public static readonly string AgentId = "XXXXXXXXXX";
    
            // SactonaSupportのAgentAliasID
            // これは外部には絶対に公開しないこと。
            public static readonly string AgentAliasId = "XXXXXXXXXX";
        }
    }
