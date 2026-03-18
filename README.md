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
// Single value — uses IdObfuscator.Default under the hood
var id = new ObfuscatedId<int>(42);
string token = id.External;                               // "SH4frQ"
int original = ObfuscatedId<int>.FromExternal(token).Value;  // 42

// Two-component composite
var id2 = new ObfuscatedId<int, long>(7, 123_456_789L);
string token2 = id2.External;                             // "QX4c4-JTmyHyZd4Fvw"
var back2 = ObfuscatedId<int, long>.FromExternal(token2);
// back2.Value1 == 7, back2.Value2 == 123_456_789L

// Up to five components
var id3 = new ObfuscatedId<int, int, Guid>(1, 2, Guid.NewGuid());
var id4 = new ObfuscatedId<int, long, Guid, string>(1, 2L, Guid.NewGuid(), "tag");
var id5 = new ObfuscatedId<int, int, int, int, int>(1, 2, 3, 4, 5);
```

Supported component types: `int`, `long`, `Guid`, `string`, and any type with a
standard `TypeConverter` (invariant-culture formatting is used).

### Custom obfuscator instance

```csharp
var options = new IdObfuscatorOptions();
options.SetXorKey("my-secret-key");
options.SetPermutationSeed("my-perm-seed");
options.PaddedBytes = 24;

var obfuscator = new IdObfuscator(options);

// Pass to ObfuscatedId via a typed wrapper
var typedOb = new IdObfuscator<int>(obfuscator);
var id = new ObfuscatedId<int>(42, typedOb);

// Or use the typed wrapper directly
string token = typedOb.Obfuscate(42);
int value = typedOb.Deobfuscate(token);  // 42
```

## Token length: padding

Without padding, token length varies with the value:

| Value | Token |
|-------|-------|
| `new ObfuscatedId<int>(42)` | `SH4frQ` (6 chars) |
| `new ObfuscatedId<int, long>(7, 123_456_789L)` | `QX4c4-JTmyHyZd4Fvw` (18 chars) |
| `new ObfuscatedId<string>("order-99")` | `Qn5E7bcE2jj-ag` (14 chars) |

Set `IdObfuscatorOptions.PaddedBytes` to make **all tokens the same length**
(`ceil(PaddedBytes / 3) × 4` base64url characters):

```csharp
var options = new IdObfuscatorOptions { PaddedBytes = 24 };  // every token → 32 characters
var obfuscator = new IdObfuscator(options);
```

| Value | Padded token (32 chars) |
|-------|------------------------|
| `new ObfuscatedId<int>(42)` | `SH4frdNhqBXHU-k9hvIOtEp-K5_TYagV` |
| `new ObfuscatedId<int, long>(7, 123_456_789L)` | `QX4c4-JTmyHyZd4Fv_IOtEp-K5_TYagV` |
| `new ObfuscatedId<string>("order-99")` | `Qn5E7bcE2jj-auk9hvIOtEp-K5_TYagV` |

The embedded length header means tokens encoded with padding decode correctly even
when `PaddedBytes` is later set to `0`. Maximum allowed value is
`IdObfuscatorOptions.MaxPaddedBytes` (257).

## Custom key

Configure via `IdObfuscatorOptions`:

```csharp
var options = new IdObfuscatorOptions();
options.XorKey = new byte[] { 0xAB, 0xCD, 0xEF, ... };
// or from a string
options.SetXorKey("my-secret-key");

var obfuscator = new IdObfuscator(options);
```

> Tokens encoded with one key cannot be decoded with another.

## Byte-position permutation

Optionally shuffle the encoded bytes before base64url encoding:

```csharp
var options = new IdObfuscatorOptions();
options.SetPermutationSeed("my-perm-seed");
// or options.PermutationSeed = new byte[] { ... };
// Set to null to disable: options.PermutationSeed = null;

var obfuscator = new IdObfuscator(options);
```

The permutation is **deterministic and length-dependent**: a distinct Fisher-Yates shuffle
is derived for each buffer length, so tokens of different sizes use different shuffles.

> Tokens produced with an active permutation **cannot** be decoded without the same seed,
> and are **not compatible** with tokens produced without a permutation (or with a different seed).

## Typed obfuscator interfaces

The library exposes two interfaces for dependency injection and testing:

- `IIdObfuscator` — string-level `Encode`/`Decode` (implemented by `IdObfuscator`)
- `IIdObfuscator<T>` — typed `Obfuscate`/`Deobfuscate` (implemented by `IdObfuscator<T>`, `IdObfuscator<T1,T2>`, …)

```csharp
// Register in DI
services.AddSingleton<IIdObfuscator>(new IdObfuscator(options));
services.AddSingleton<IIdObfuscator<int>>(sp =>
    new IdObfuscator<int>(sp.GetRequiredService<IIdObfuscator>()));

// Use typed wrapper directly
IIdObfuscator<int> ob = new IdObfuscator<int>(obfuscator);
string token = ob.Obfuscate(42);
int value = ob.Deobfuscate(token);

// Multi-component wrapper uses tuples
IIdObfuscator<(int, long)> ob2 = new IdObfuscator<int, long>(obfuscator);
string token2 = ob2.Obfuscate((7, 123_456_789L));
(int a, long b) = ob2.Deobfuscate(token2);
```

## Project structure

```
ObfuscatedIds.sln
├── src/
│   └── ObfuscatedIds/               # library
│       ├── Serialization/
│       │   ├── ObfuscatedIdJsonConverter.cs        # per-arity System.Text.Json converters
│       │   └── ObfuscatedIdJsonConverterFactory.cs # converter factory
│       ├── IIdObfuscator.cs         # IIdObfuscator (string-level) and IIdObfuscator<T> (typed)
│       ├── IdComponents.cs          # FormatValue, ParseValue, JoinComponents, SplitComponents
│       ├── IdObfuscator.cs          # XOR + base64url core, implements IIdObfuscator
│       ├── IdObfuscator.Error.cs    # exception factory (partial class)
│       ├── IdObfuscatorOptions.cs   # XorKey, PermutationSeed, PaddedBytes configuration
│       ├── IdObfuscatorTyped.cs     # IdObfuscator<T> … IdObfuscator<T1,T2,T3,T4,T5> wrappers
│       └── ObfuscatedId.cs          # ObfuscatedId<T…> value types (1–5 components)
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
