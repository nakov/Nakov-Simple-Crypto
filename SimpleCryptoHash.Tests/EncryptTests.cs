using NUnit.Framework;

public class EncryptMsgTests
{
	[TestCase("", "", "CE90AD4C")]
	[TestCase("a", "b", "F4D06C9EEC76")]
	[TestCase("Msg", "pass", "546DBAB04138759743F6")]
	[TestCase("I am a message", "simple@password1",
		"9B200BF1C7B6EE41AFB9EA7A770B15E96E558752749983E150E23AC3D77284D6")]
	[TestCase("This message holds Unicode chars: здрасти 你好", "passАБВ你好",
		"BCC67A2141CFE1C43F78C78165EBE53C59DA76454B5AFADB8AEC33526D619B5E16AF92D695D15A80BD8231EFC2DF52B4DD13385F09B8C9BE9C5C4D4E568F72218F01528E102EDE22AFB721AC65DC4A07DF1564904C1C6DFBA3F0416B")]
	public void EncryptMsgMsg(string msg, string password, string encryptedMsg)
	{
		string actualEncryptedMsg = SimpleCrypto.EncryptMsg(msg, password);
		Assert.That(actualEncryptedMsg, Is.EqualTo(encryptedMsg));
	}
}
