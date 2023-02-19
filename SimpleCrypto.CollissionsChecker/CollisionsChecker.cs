//uint HashSha256(string msg)
//{
//	byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
//	byte[] hash = SHA256.HashData(msgBytes);
//	uint result = BitConverter.ToUInt32(hash);
//	return result;
//}

void CheckForCollissions(int MsgCount = 1000000)
{
	int collisionsCount = 0;
	var map = new Dictionary<uint, string>();
	for (int i = 0; i < MsgCount; i++)
	{
		string str = "msg" + i;
		uint hash = SimpleCrypto.Hash(str);
		//Console.WriteLine(hash.ToString("X8"));
		//uint hash = HashSha256(str);
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
}

CheckForCollissions();
