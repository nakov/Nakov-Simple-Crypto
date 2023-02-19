using NUnit.Framework;

public class DecryptMsgTests
{
	[TestCase("CE90AD4C", "", "")]
	[TestCase("F4D06C9E6760", "b", "a")]
	[TestCase("546DBAB0DE8B849D70DA", "pass", "Msg")]
	[TestCase("9B200BF162D01A2A2662795E148D72E732C1E227C2CEF56413CF020D1415EDF1",
		"simple@password1", "I am a message")]
	[TestCase("BCC67A21B39BA8DBF3C99B19CC9F29384F79047468CC1472B0B85376D9D8455CAF5739B6CDA89ADC7377454FECC7809ADFBC1F22804DD912AE3F5B4339B191EFF9D39115C92FF2E13B582F7AFADFEBFB98B65BC5A400B77D599A91D6",
		"passАБВ你好", "This message holds Unicode chars: здрасти 你好")]
	public void DecryptMsgMsg(string encryptedMsg, string password, string originalMsg)
	{
		string decryptedMsg = SimpleCrypto.DecryptMsg(encryptedMsg, password);
		Assert.That(decryptedMsg, Is.EqualTo(originalMsg));
	}
}
