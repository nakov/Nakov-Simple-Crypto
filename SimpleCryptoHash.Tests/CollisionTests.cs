using NUnit.Framework;

public class CollisionTests
{
	[Test]
	public void TestCheckCollissionsCount()
	{
		// Ensure that for 1,000,000 keys we have less than 150 collisions
		// This collision ratio is similar for SHA-256 crypto hash function

		const int MsgCount = 1_000_000;

		int collisionsCount = 0;
		var map = new Dictionary<uint, string>();
		for (int i = 0; i < MsgCount; i++)
		{
			string str = "msg" + i;
			uint hash = SimpleCrypto.Hash(str);
			//uint hash = HashSHA256(str);
			if (map.ContainsKey(hash))
			{
				Console.WriteLine("Collision: ");
				Console.WriteLine("  " + str + " --> " + hash.ToString("X8"));
				Console.WriteLine("  " + map[hash] + " --> " + hash.ToString("X8"));
				collisionsCount++;
			}
			else
			{
				map.Add(hash, str);
			}
		}
		Console.WriteLine("Total collisions found: " + collisionsCount);

		Assert.That(collisionsCount, Is.LessThan(150));
	}

	//uint HashSHA256(string msg)
	//{
	//	byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
	//	byte[] hash = SHA256.HashData(msgBytes);
	//	uint result = BitConverter.ToUInt32(hash);
	//	return result;
	//}
}
