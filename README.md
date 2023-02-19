# Nakov's Simple Crypto Algorithms

Simple crypto algorithms:
  - 32-bit hash function
  - 32-bit symmetric block cypher

## Simple 32-Bit Hash + Symmetric Encryption Algorithm

This project demonstrates how to build a **simple 32-bit crypto hashing function** and 
a **symmetric 32-bit block cypher** (encrypt + decrypt).

### How Does It Work?

The algorithms are based on a simple **Merkle–Damgård construction** using 32-bit blocks:
https://en.wikipedia.org/wiki/Merkle%E2%80%93Damg%C3%A5rd_construction

### Designing Your Own Crypto Hash Function


### Warning: Insecure for Cryptographic Use

**Warning**: This library is **cryptographically insecure**. Don't use in production!
Use it for educational purposes only: to demonstrate some ideas about
how hashing and symetric encryption algorithms may be designed.

Always use proven crypto algorithms, designed by experts in cryptography (like SHA2, SHA3 and AES).
Designing your own encryption algorithm can be risky and prone to security flaws because it requires
specialized scientific knowledge, peer review, testing, and robustness against attacks.
