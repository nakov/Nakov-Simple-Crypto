# Nakov's Simple Crypto Algorithms

Simple crypto algorithms, designed for educational purposes:
  - 32-bit low-collision **hash function**
  - 32-bit **symmetric block cipher**


## Simple 32-Bit Hash + Symmetric Encryption Algorithm

This project demonstrates how to build a **simple 32-bit crypto hashing function** and a **symmetric 32-bit block cipher** (encrypt + decrypt).


### How Does It Work?

The algorithms are based on a simple **[Merkle–Damgård construction](https://en.wikipedia.org/wiki/Merkle%E2%80%93Damg%C3%A5rd_construction)**
using 32-bit blocks.


### Designing a Padding Scheme

Hashing and encryption typically work on **blocks of fixes size**, e.g. 32-bits (4 bytes).
When the input message size is not multiple of the block size, we need to **pad the message** with additional chars at the end.

We use the following **padding scheme**:
```
msg + 0...3 times "*" + msg_length
```

Examples:
  - `ab` --> `ab*2` (1 block)
  - `abcdefg` --> `abcdefg7` (2 blocks)
  - `a` --> `a**1` (1 block)
  - `a**` --> `a**3` (1 block)
  - `abcdedghi` --> `abcdedghi**9` (3 blocks)

We append the message length at the end to avoid trivial collision construction schemes.

Warning: more simple padding schemes (e.g. just add a few "*" at the end) may be insecure for using with hash functions,
due to **[collision attack](https://en.wikipedia.org/wiki/Collision_attack)** and
**[length-extension attack](https://en.wikipedia.org/wiki/Length_extension_attack)** vulnerabilities.


### Designing a Simple Crypto Hash Function

We follow the classical **[Merkle–Damgård construction scheme](https://en.wikipedia.org/wiki/Merkle%E2%80%93Damgard_construction)**
to build two cryptographic primitives `MixBlocks(block, state)` and `Hash(msg)`.

The **block mixing** primitive works as follows:

```
MixBlocks(block, state) --> new state:
    repeat 16 times:
        state = state * 28657 + block * 514229 + 2971215073
        block = block * 1597 + 433494437
        state = state <<< 7
        block = block >>>  13
    return state
```

Notes:
  - The magic numbers above are **prime numbers** (to reduce potential collissions).
  - The above design aims to make the function **irreversible**, while **preserving the entropy** from the input blocks.

The **hash calculation** primitive follows the classical **[Merkle–Damgård construction](https://en.wikipedia.org/wiki/Merkle%E2%80%93Damgard_construction)** over the input sequence of blocks using the **block mixing** primitive:

```
Hash(msg) --> int32:
    msg = PadMsg(msg)   // Make the message size a multiple of 32-bits
    state = 0xA1B2C3D4  // Initial Vector (IV): a magic number
    for each 32-bit block from the input message:
        state = MixBlocks(block, state)
    return state  
```


### Designing a Simple Symmetric Encryption Scheme

We propose a **simple symmetric block cipher**, based on `XOR` or each letter from the input message with an **unique sequence of hash values**, derived from the **encryption password**, the **character offset** in the input message and the **input message** itself.

### Encryption Algorithm

Once we have a collision-resistant cryptographic hash function, we may design a **simple symmetric encryption scheme** as follows:

```
Encrypt(msg, password):
    msgHash = Hash(msg)
    for i = 0 ... msg_length-1:
        encryptedChar[i] = msg[i] xor Hash(i + " | " + password + " | " + msgHash)
    return msgHash + encrypted chars
```

We encrypt each letter from the input sequence by an **unique and hard-to-predict hash value**,
derived from: `letter offset` + `input password` + `message hash`.

The letter encryption is based on **bitwise `XOR`**, so it is easy to revert back to the original letter
when we know the encrypted letter and the encryption hash value for this letter.

Example:
  - Suppose we have a message msg = `hello` to be encrypted by a password `p@ss`
  - The message hash will be calculated as `57A97ED8`
  - Each message letter will be encrypted by different hash, derived from the letter offset + password + msgHash:
    - encryptedChar[`0`] = `h` xor Hash(`0 | p@ss | 57A97ED8`) = `01FF`
    - encryptedChar[`1`] = `e` xor Hash(`1 | p@ss | 57A97ED8`) = `A0A9`
    - encryptedChar[`2`] = `l` xor Hash(`2 | p@ss | 57A97ED8`) = `79A5`
    - encryptedChar[`3`] = `l` xor Hash(`3 | p@ss | 57A97ED8`) = `8F72`
    - encryptedChar[`4`] = `o` xor Hash(`4 | p@ss | 57A97ED8`) = `B4DF`
  - encryptedMsg = `57A97ED8` + `01FF` + `A0A9` + `79A5` + `8F72` + `B4DF` = `57A97ED801FFA0A979A58F72B4DF`

The above encryption scheme design aims to generate an unique and hard-to-predict **encryption hash sequence**,
which depends highly on the input message and the encryption password. The security of the algorithm relies on
the difficulty to generate the same **encryption hash sequence** without knowing the encryption password.

The reason to include the **input message hash** in the calculation of the encryption hash sequence is to avoid
**revealing the encryption hash sequence** when we know a significant part of the encrypted message (e.g. its first paragraph)
and we use the same password to encrypt multiple messages (e.g. 50 text files in an encrypted ZIP archive).

### Decryption Algorithm

Decryption follows the same scheme, like the encryption:

```
Decrypt(encryptedMsg, password):
    msgHash = extract and remove the first 8 letters from encryptedMsg
    for i = 0 ... msg_length-1:
        decryptedChar[i] = substring(encryptedMsg, i*4, 4) xor Hash(i + " | " + password + " | " + msgHash)
    return decrypted chars
```

Example:
  - Suppose encryptedMsg = `57A97ED801FFA0A979A58F72B4DF` and password = `p@ss`
  - The encrypted message can be decomposed to:
    - `57A97ED8` (msgHash) + `01FF` (char 0) + `A0A9` (char 1) + `79A5` (char 2) + `8F72` (char 3) + `B4DF` (char 4)
  - To decrypt the encrypted message we XOR it with the same encryption hash sequence, used during the encryption:
    - decryptedChar[`0`] = `01FF` xor Hash(`0 | p@ss | 57A97ED8`) = `h`
    - decryptedChar[`1`] = `A0A9` xor Hash(`1 | p@ss | 57A97ED8`) = `e`
    - decryptedChar[`2`] = `79A5` xor Hash(`2 | p@ss | 57A97ED8`) = `l`
    - decryptedChar[`3`] = `8F72` xor Hash(`3 | p@ss | 57A97ED8`) = `l`
    - decryptedChar[`4`] = `B4DF` xor Hash(`4 | p@ss | 57A97ED8`) = `o`
  - The decrypted message = `h` + `e` + `l` + `l` + `o`

Note that the above encryption scheme cannot detect if the password is correct or not.
When we decrypt an encrypted message by a wrong password, we will get an incorrect output message.

To detect a wrong decryption password, we may add a **[message authentication code (MAC)](https://cryptobook.nakov.com/mac-and-key-derivation)** to the encrypted message.


### Warning: Insecure for Cryptographic Use

This code library is **cryptographically insecure**. Don't use in production!

Use this code **for educational purposes only**: to demonstrate some ideas about
how hashing and symetric encryption algorithms may be designed.

Note also that **32-bit ciphers are too weak** for real-world crypto hashing and encryption.
Real-world cryptosystems use blocks of size 256 or more bits to resist **[brute-force attacks](https://en.wikipedia.org/wiki/Brute-force_attack)**.

Designing your **own encryption algorithms** is risky and **prone to security flaws**. To design secure crypto algorithms, you need to have
deep specialized scientific knowledge, and your algorithms should pass peer reviews, thorough testing, and prove resistance against attacks.

Always use **proven crypto algorithms** (like SHA2, SHA3 and AES), designed by well-established experts in cryptography.
