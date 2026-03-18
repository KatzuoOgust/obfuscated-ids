# ObfuscatedIds

A small .NET 8 library for obfuscating internal database IDs before exposing them in APIs or URLs.
Supports single values and composite tuples of up to four components.

## How it works

```
internal value(s) → format to string → UTF-8 bytes → XOR with key → base64url
```

Decoding is the same pipeline in reverse (XOR is its own inverse).
The result is a URL-safe string with no `+`, `/`, or `=` padding characters.

## Quick start

```csharp
// Single value
var id = new ObfuscatedId<int>(42);
string token = id.External;                          // "TXoh_Rg" (example)
int original = ObfuscatedId<int>.FromExternal(token).Value;   // 42

// Two-component composite
var id2 = new ObfuscatedId<int, long>(7, 123_456_789L);
var back2 = ObfuscatedId<int, long>.FromExternal(id2.External);
// back2.Value1 == 7, back2.Value2 == 123_456_789L

// Three-component composite
var id3 = new ObfuscatedId<int, int, Guid>(1, 2, Guid.NewGuid());

// Four-component composite
var id4 = new ObfuscatedId<int, long, Guid, string>(1, 2L, Guid.NewGuid(), "tag");
```

Supported component types: `int`, `long`, `Guid`, `string`, and any type with a
standard `TypeConverter` (invariant-culture formatting is used).

## Custom key

Replace the default key once at application startup:

```csharp
IdObfuscator.Configure(new byte[] { 0xAB, 0xCD, 0xEF, ... });
```

> Tokens encoded with one key cannot be decoded with another.

## Project structure

```
ObfuscatedIds.sln
├── src/
│   └── ObfuscatedIds/          # library
│       ├── IdObfuscator.cs   # XOR + base64url core
│       └── ObfuscatedId.cs   # generic ObfuscatedId<T…> classes
└── tests/
    └── ObfuscatedIds.Tests/    # xunit + FluentAssertions
```

## Build & test

```sh
make build   # dotnet build
make test    # dotnet test
make clean   # remove bin/ and obj/
make help    # list all targets
```
