using NUnit.Framework;

public class PadMsgTests
{
	[TestCase("", "***0")]
	[TestCase("x", "x**1")]
	[TestCase("ab", "ab*2")]
	[TestCase("abc", "abc3")]
	[TestCase("abcd", "abcd***4")]
	[TestCase("abcdedghi", "abcdedghi**9")]
	[TestCase("abcdedghij", "abcdedghij10")]
	[TestCase("abcdedghijk", "abcdedghijk***11")]
	public void TestPadMsg(string msg, string paddedMsg)
	{
		string actualPaddedMsg = SimpleCrypto.PadMsg(msg);
		Assert.That(actualPaddedMsg, Is.EqualTo(paddedMsg));
	}
}
