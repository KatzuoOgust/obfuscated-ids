# ObfuscatedIds

[![CI/CD](https://github.com/KatzuoOgust/obfuscated-ids/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/KatzuoOgust/obfuscated-ids/actions/workflows/ci.yml)

A small .NET 8 library for obfuscating internal database IDs before exposing them in APIs or URLs.
Supports single values and composite tuples of up to five components.

## How it works

```
internal value(s) → format to string → UTF-8 bytes → XOR with key → [permute bytes] → base64url
```

Decoding is the same pipeline in reverse. XOR is its own inverse; permutation uses the matching
inverse shuffle derived from the same seed.
The result is a URL-safe string with no `+`, `/`, or `=` padding characters.

## Quick start

```csharp
// Single value
var id = new ObfuscatedId<int>(42);
string token = id.External;                          // "SH4frQ"
int original = ObfuscatedId<int>.FromExternal(token).Value;   // 42

// Two-component composite
var id2 = new ObfuscatedId<int, long>(7, 123_456_789L);
string token2 = id2.External;                        // "QX4c4-JTmyHyZd4Fvw"
var back2 = ObfuscatedId<int, long>.FromExternal(token2);
// back2.Value1 == 7, back2.Value2 == 123_456_789L

// Three-component composite
var id3 = new ObfuscatedId<int, int, Guid>(1, 2, Guid.NewGuid());

// Four-component composite
var id4 = new ObfuscatedId<int, long, Guid, string>(1, 2L, Guid.NewGuid(), "tag");
```

Supported component types: `int`, `long`, `Guid`, `string`, and any type with a
standard `TypeConverter` (invariant-culture formatting is used).

## Token length: padding

Without padding, token length varies with the value:

| Value | Token |
|-------|-------|
| `new ObfuscatedId<int>(42)` | `SH4frQ` (6 chars) |
| `new ObfuscatedId<int, long>(7, 123_456_789L)` | `QX4c4-JTmyHyZd4Fvw` (18 chars) |
| `new ObfuscatedId<string>("order-99")` | `Qn5E7bcE2jj-ag` (14 chars) |

Set `IdObfuscator.PaddedBytes` once at startup to make **all tokens the same length**
(`ceil(PaddedBytes / 3) × 4` base64url characters):

```csharp
IdObfuscator.PaddedBytes = 24;  // every token → 32 characters
```

| Value | Padded token (32 chars) |
|-------|------------------------|
| `new ObfuscatedId<int>(42)` | `SH4frdNhqBXHU-k9hvIOtEp-K5_TYagV` |
| `new ObfuscatedId<int, long>(7, 123_456_789L)` | `QX4c4-JTmyHyZd4Fv_IOtEp-K5_TYagV` |
| `new ObfuscatedId<string>("order-99")` | `Qn5E7bcE2jj-auk9hvIOtEp-K5_TYagV` |

The embedded length header means tokens encoded with padding decode correctly even
when `PaddedBytes` is later set to `0`. Maximum allowed value is
`IdObfuscator.MaxPaddedBytes` (257).

## Custom key

Replace the default key once at application startup:

```csharp
IdObfuscator.ConfigureXorKey(new byte[] { 0xAB, 0xCD, 0xEF, ... });
// or from a string
IdObfuscator.ConfigureXorKey("my-secret-key");
```

> Tokens encoded with one key cannot be decoded with another.

## Byte-position permutation

Optionally shuffle the encoded bytes before base64url encoding. This makes tokens look
more random and harder to spot patterns in, even when the XOR key is guessed.

```csharp
// Configure once at startup (byte[] or string overload)
IdObfuscator.ConfigurePermutation("my-perm-seed");

// Disable again if needed
IdObfuscator.ResetPermutation();
```

The permutation is **deterministic and length-dependent**: a distinct Fisher-Yates shuffle
is derived for each buffer length, so tokens of different sizes use different shuffles.

> Tokens produced with an active permutation **cannot** be decoded without the same seed,
> and are **not compatible** with tokens produced without a permutation (or with a different seed).

## Project structure

```
ObfuscatedIds.sln
├── src/
│   └── ObfuscatedIds/               # library
│       ├── Serialization/
│       │   └── ObfuscatedIdJsonConverter.cs   # System.Text.Json factory + converters
│       ├── IdObfuscator.cs          # XOR + base64url core
│       ├── IdObfuscator.Error.cs    # exception factory (partial class)
│       └── ObfuscatedId.cs          # generic ObfuscatedId<T…> classes
└── tests/
    └── ObfuscatedIds.Tests/         # xunit + FluentAssertions
        ├── Serialization/
        │   └── ObfuscatedIdJsonConverterTests.cs
        └── ObfuscatedIdTests.cs
```

## Build & test

```sh
make build   # dotnet build
make test    # dotnet test
make clean   # remove bin/ and obj/
make help    # list all targets
```
