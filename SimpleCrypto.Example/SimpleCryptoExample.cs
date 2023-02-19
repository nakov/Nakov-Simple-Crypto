string msg = "I am a secret message for encryption";
string password = "s3cret~p@ss";
Console.WriteLine("msg = " + msg);
Console.WriteLine("hash(msg) = " + SimpleCrypto.Hash(msg).ToString("X8"));
string encryptedMsg = SimpleCrypto.EncryptMsg(msg, password);
Console.WriteLine("encrypted = " + encryptedMsg);
string decryptedMsg = SimpleCrypto.DecryptMsg(encryptedMsg, password);
Console.WriteLine("decrypted = " + decryptedMsg);
Console.WriteLine($"Decrypted correctly? {msg == decryptedMsg}");
