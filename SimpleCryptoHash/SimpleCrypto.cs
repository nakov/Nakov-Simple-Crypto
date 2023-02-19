using System.Text;

/// <summary>
/// Simple 32-bit crypto hashing and symmetric encryption algorithms,
/// based on a simple Merkle–Damgård construction with 32-bit blocks:
/// https://en.wikipedia.org/wiki/Merkle%E2%80%93Damg%C3%A5rd_construction
/// 
/// Note: This library is not cryptographically secure. Don't use in production!
/// For educational purposes only: to demonstrate some ideas about
/// how hashing and symetric encryption algorithms may be designed.
/// </summary>
public class SimpleCrypto
{
	public static string PadMsg(string msg, int blockSize = sizeof(uint), char padLetter = '*')
	{
		string msgLenStr = msg.Length.ToString();
		int totalLen = msg.Length + msgLenStr.Length;
		int padLen = (blockSize - (totalLen % blockSize)) % blockSize;
		string paddedMsg = msg + new string(padLetter, padLen) + msgLenStr;
		return paddedMsg;
	}

	static uint RotateLeft(uint value, int bits)
	{
		return (value << bits) | (value >> (32 - bits));
	}

	static uint RotateRight(uint value, int bits)
	{
		return (value >> bits) | (value << (32 - bits));
	}

	public static uint MixBlocks(uint block, uint state)
	{
		for (int i = 0; i < 16; i++)
		{
			state = state * 28657 + block * 514229 + 2971215073;
			block = block * 1597 + 433494437;
			state = RotateLeft(state, 7);
			block = RotateRight(block, 13);
		}
		return state;
	}

	// Calculates a hash code by a simple 32-bit Merkle–Damgård construction
	public static uint Hash(string msg)
	{
		msg = PadMsg(msg);

		uint state = 0xA1B2C3D4; // Initial Vector (IV): a magic number

		// Loop over the message blocks (32-bits at-a-time)
		int offset = 0;
		while (offset + 3 < msg.Length)
		{
			uint block = (uint)(
				msg[offset + 3] ^
				RotateLeft(msg[offset + 2], 8) ^
				RotateLeft(msg[offset + 1], 16) ^
				RotateLeft(msg[offset + 0], 24)
			);
			state = MixBlocks(block, state);
			offset += sizeof(uint);
		}

		return state;
	}

	public static string EncryptMsg(string msg, string password)
	{
		StringBuilder encryptedMsg = new StringBuilder();
		string msgHash = Hash(msg).ToString("X8");
		encryptedMsg.Append(msgHash);

		for (int index = 0; index < msg.Length; index++)
		{
			ushort nextChar = msg[index];
			string nextKey = index + " | " + password + " | " + msgHash;
			ushort nextSecret = (ushort)Hash(nextKey);
			ushort encryptedChar = (ushort)(nextChar ^ nextSecret);
			string encryptedCharHex = encryptedChar.ToString("X4");
			encryptedMsg.Append(encryptedCharHex);
		}

		return encryptedMsg.ToString();
	}

	public static string DecryptMsg(string encryptedMsg, string password)
	{
		string msgHash = encryptedMsg.Substring(0, 8);

		StringBuilder msg = new StringBuilder();
		int offset = 0;
		for (int i = 8; i < encryptedMsg.Length; i += 4)
		{
			string nextEncryptedCharHex = encryptedMsg.Substring(i, 4);
			ushort nextEncryptedChar = Convert.ToUInt16(nextEncryptedCharHex, 16);
			string nextKey = offset + " | " + password + " | " + msgHash;
			ushort nextSecret = (ushort)Hash(nextKey);
			char decryptedChar = (char)(nextEncryptedChar ^ nextSecret);
			msg.Append(decryptedChar);
			offset++;
		}

		return msg.ToString();
	}
}
