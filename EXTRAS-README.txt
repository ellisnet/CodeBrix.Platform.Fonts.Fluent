================================================================================
EXTRAS-README: CodeBrix.Platform.Fonts.Fluent
Samples, tools and other content in this repository that is not part of a NuGet package
================================================================================

This repository contains no samples, no tools and no demo applications. It
builds exactly one NuGet package and nothing else. Every file here is either
part of that package, documentation, or the test project.


TESTS (the only non-package content)
====================================

    tests/CodeBrix.Platform.Fonts.Fluent.Tests/

An xUnit v3 test project (SilverAssertions, coverlet.collector) that
verifies the three shipped files -- the font, the asset marker and the
buildTransitive .props -- are present, correctly shaped and correctly
renamed. It ships in the repository only; it is not packed.

Run it with:

    dotnet test CodeBrix.Platform.Fonts.Fluent.slnx

No opt-in environment variables, no test data to download, no device or
display required.

The tests double as the package's worked examples for a consuming agent;
AGENT-README.txt's WORKING EXAMPLES ON GITHUB section links to each file
and says what it proves.


NO SAMPLE APPLICATION
=====================

There is deliberately no sample app. This package registers a font for
CodeBrix.Platform applications, so a meaningful sample would be a whole
CodeBrix.Platform application head -- which belongs in the CodeBrix.Platform
samples, not here.

To see the font actually render, reference
`CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever` from any
CodeBrix.Platform application and drop in the three-line "MINIMUM VIABLE
PROJECT" page from AGENT-README.txt.


BUILD OUTPUT
============

    src/CodeBrix.Platform.Fonts.Fluent/bin/<Configuration>/

Because `GeneratePackageOnBuild` is true, every build leaves a .nupkg here.
The folder is gitignored; the packages accumulated in a working tree are
build residue, not release artifacts, and can be deleted at any time.
