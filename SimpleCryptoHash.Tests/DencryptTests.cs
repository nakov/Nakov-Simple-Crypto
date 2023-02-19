using NUnit.Framework;

public class DecryptMsgTests
{
	[TestCase("CE90AD4C", "", "")]
	[TestCase("F4D06C9EEC76", "b", "a")]
	[TestCase("546DBAB04138759743F6", "pass", "Msg")]
	[TestCase("9B200BF1C7B6EE41AFB9EA7A770B15E96E558752749983E150E23AC3D77284D6",
		"simple@password1", "I am a message")]
	[TestCase("BCC67A2141CFE1C43F78C78165EBE53C59DA76454B5AFADB8AEC33526D619B5E16AF92D695D15A80BD8231EFC2DF52B4DD13385F09B8C9BE9C5C4D4E568F72218F01528E102EDE22AFB721AC65DC4A07DF1564904C1C6DFBA3F0416B",
		"passАБВ你好", "This message holds Unicode chars: здрасти 你好")]
	public void DeccryptMsgMsg(string encryptedMsg, string password, string originalMsg)
	{
		string decryptedMsg = SimpleCrypto.DecryptMsg(encryptedMsg, password);
		Assert.That(decryptedMsg, Is.EqualTo(originalMsg));
	}
}
