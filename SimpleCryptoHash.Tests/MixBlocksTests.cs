using NUnit.Framework;

public class MixBlocksTests
{
	[TestCase(0x00000000u, 0x00000000u, 0x1A9CF280u)]
	[TestCase(0x12345678u, 0x23456789u, 0xC03EBA44u)]
	[TestCase(0x89ABCDEFu, 0x45678901u, 0xF0F9AB2Du)]
	[TestCase(0x11111111u, 0x22222222u, 0x66E3E41Cu)]
	[TestCase(0xFFFFFFFFu, 0xFFFFFFFFu, 0x0757E01Bu)]
	public void TestMixBlocks(uint block1, uint block2, uint mixedBlocks)
	{
		uint actualMixedBlocks = SimpleCrypto.MixBlocks(block1, block2);
		Assert.That(actualMixedBlocks, Is.EqualTo(mixedBlocks));
	}
}
