# Portable source regressions

## Fix
Normalize nullable/empty stream and download URLs before comparing stored values. Clearing a populated download previously threw, a cleared stream became a parameter-only string, and assigning the same raw URL repeatedly emitted false change notifications. Preserve the existing client parameter and ordinary URL format.

## Run
With .NET SDK 10.0.401 installed:

    dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release

This is a dependency-free net10.0 console harness with C# 5 syntax checking. Its project links the actual production files, without copying their logic. NuGet sources are cleared. The SDK runs the tests only; the historical application is not retargeted.

## Coverage
Actual ItemViewModel, no SoundCloud calls: clear populated URLs, repeat raw assignments, preserve ordinary URL format and keep empty URLs empty.

## Original failure
The same tests were run against original source at [d6dfb695d491c529dc6647e82e236b8bb1647e45](https://github.com/m0rg0t/Malukah-songs-WP7/tree/d6dfb695d491c529dc6647e82e236b8bb1647e45) and exited nonzero; the repaired source exits zero.

    FAIL: clear populated download: Object reference not set to an instance of an object.
    FAIL: clear populated stream: Assertion failed
    FAIL: repeated raw stream URL does not notify: Assertion failed
    FAIL: repeated raw download URL does not notify: Assertion failed
    FAIL: empty URLs stay empty: Assertion failed

## Boundaries
The stand-ins in Portable.Tests/Stubs.cs only supply unavailable platform types; they do not prove platform compatibility or simulate the app UI. No real service/user data, credentials, notifications, payments, publishing or deployment are used.

SoundCloud API availability, authentication, ads, playback and offline cache are unverified. Existing client parameter, platform and packages remain unchanged.

The full historical Windows/Phone build and device runtime remain unverified. Original project targets, package manifests, vendor binaries and assets are unchanged. No compatible modern replacement for the old platform-specific dependencies has been established, so no speculative package upgrade is included.
