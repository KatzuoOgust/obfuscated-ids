# Test Naming Guidelines

Test method names follow the **`Method_Result_IfCondition`** pattern.

```
Method        – the public method or property under test
Result        – the expected outcome
IfCondition   – the state or input that leads to that outcome (omit if obvious)
```

## Rules

- Use **PascalCase** for each segment.
- Separate segments with a single underscore `_`.
- Be specific enough that the name reads as a mini specification.
- Avoid noise words: `Should`, `Test`, `Verify`, `Check`.
- For `[Theory]` tests the condition segment describes the varying input, not the data values.

## Examples

| ✅ Good | ❌ Bad |
|---------|--------|
| `FromExternal_ReturnsOriginalInt_IfEncodedWithSingleInt` | `SingleInt_RoundTrip` |
| `External_IsSame_IfSameValueEncodedTwice` | `SameValue_ProducesSameExternal` |
| `External_AllSameLength_IfPaddedBytesSet` | `PaddedBytes_AllTokensSameLength` |
| `Configure_ChangesExternalToken_IfKeyChanged` | `CustomKey_ChangesOutput` |
| `External_AlwaysSameToken_IfSingleIntEncodedRepeatedly` | `Single_EncodedMultipleTimes_AlwaysSameToken` |
| `External_IsUrlSafe_IfValueIsLongMax` | `External_IsUrlSafe` |

## Condition segment

Use `If` as a prefix for the condition to make the name read naturally:

```
FromExternal_ReturnsOriginalValue_IfPaddedBytesDisabledAtDecodeTime
                                   ^^
```

```
External_DiffersPerValue_IfValuesAreDifferent   ← explicit, preferred
External_DiffersPerValue                        ← acceptable when context is clear
```

## Namespace

Test classes live in the `ObfuscatedIds` namespace — the same root namespace as the library.
The test project sets `<RootNamespace>ObfuscatedIds</RootNamespace>` so new files default
to the correct namespace without a `.Tests` suffix.
