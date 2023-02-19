using NUnit.Framework;

public class EncryptMsgTests
{
	[TestCase("", "", "CE90AD4C")]
	[TestCase("a", "b", "F4D06C9E6760")]
	[TestCase("Msg", "pass", "546DBAB0DE8B849D70DA")]
	[TestCase("I am a message", "simple@password1",
		"9B200BF162D01A2A2662795E148D72E732C1E227C2CEF56413CF020D1415EDF1")]
	[TestCase("This message holds Unicode chars: здрасти 你好", "passАБВ你好",
		"BCC67A21B39BA8DBF3C99B19CC9F29384F79047468CC1472B0B85376D9D8455CAF5739B6CDA89ADC7377454FECC7809ADFBC1F22804DD912AE3F5B4339B191EFF9D39115C92FF2E13B582F7AFADFEBFB98B65BC5A400B77D599A91D6")]
	public void EncryptMsgMsg(string msg, string password, string encryptedMsg)
	{
		string actualEncryptedMsg = SimpleCrypto.EncryptMsg(msg, password);
		Assert.That(actualEncryptedMsg, Is.EqualTo(encryptedMsg));
	}
}
