================================================================================
MAINTAINER-README: CodeBrix.Platform.Fonts.Fluent
Notes for people and agents MAINTAINING this repository — not for package consumers
================================================================================

If you are CONSUMING the NuGet package, read AGENT-README.txt instead. This
file is about building, testing, packaging and provenance.


PURPOSE AND SCOPE
=================

This repository produces exactly one NuGet package:

    PackageId       CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever
    Project         src/CodeBrix.Platform.Fonts.Fluent/
    AGENT-README    AGENT-README.txt (repo root)

The package is an asset carrier. Its assembly is metadata-only -- there is
no C# in the library beyond InternalsVisibleTo.cs, and the assembly exports
zero public types on purpose. The deliverables are:

  * Fonts/uno-fluentui-assets.ttf         the icon font
  * CodeBrix.Platform.Fonts.Fluent.uprimarker    empty asset marker
  * buildTransitive/net10.0/CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props

Everything a consumer can rely on is one of those three files plus the
package metadata. Changing any of their names or locations is a breaking
change even though there is no API to break.


REPOSITORY LAYOUT
=================

    CodeBrix.Platform.Fonts.Fluent/
      CodeBrix.Platform.Fonts.Fluent.slnx
      AGENT-README.txt              consumer documentation (packed)
      MAINTAINER-README.txt         this file (not packed)
      EXTRAS-README.txt             non-package content (not packed)
      README-INDEX.txt              map of the README files (not packed)
      README.md                     human-facing overview (packed)
      THIRD-PARTY-NOTICES.txt       upstream attribution (packed)
      LICENSE                       Apache-2.0 (not packed; see PACKAGING)
      icon-codebrix-128.png         package icon (packed)
      src/CodeBrix.Platform.Fonts.Fluent/
        CodeBrix.Platform.Fonts.Fluent.csproj
        InternalsVisibleTo.cs
        CodeBrix.Platform.Fonts.Fluent.uprimarker      (0 bytes, on purpose)
        Fonts/uno-fluentui-assets.ttf
        buildTransitive/net10.0/
          CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props
      tests/CodeBrix.Platform.Fonts.Fluent.Tests/
        CodeBrix.Platform.Fonts.Fluent.Tests.csproj
        AssemblyMetadataTests.cs
        ContentFilePresenceTests.cs
        PropsFileTests.cs
        TestAssetPaths.cs

The eight AI-agent pointer files (AGENTS.md, CLAUDE.md, .clinerules,
.cursorrules, .cursor/rules/agent-readme.mdc, .windsurfrules,
.github/copilot-instructions.md, .junie/guidelines.md) are maintained
centrally across the CodeBrix family. Do not hand-edit them here.

The .slnx carries a "Solution Items" folder (.gitignore, AGENT-README.txt,
EXTRAS-README.txt, global.json, icon-codebrix-128.png, LICENSE,
MAINTAINER-README.txt, README-INDEX.txt, README.md,
THIRD-PARTY-NOTICES.txt), a "Solution Items/src" folder holding the .props so
it is editable from the IDE, a "Tests" folder holding the test project, and
the library project at the root.

global.json at the repository root selects the Microsoft.Testing.Platform
test runner. It does NOT pin an SDK version. See TESTING below.


BUILDING
========

    dotnet restore CodeBrix.Platform.Fonts.Fluent.slnx
    dotnet build   CodeBrix.Platform.Fonts.Fluent.slnx

Target framework: net10.0 only, no multi-targeting.
`GeneratePackageOnBuild` is true, so every build also produces a .nupkg
under src/CodeBrix.Platform.Fonts.Fluent/bin/<Configuration>/ (that path is
gitignored).

There is nothing to compile except InternalsVisibleTo.cs; a build failure
here is almost always a packaging or item-group problem, not a code
problem.


TESTING
=======

    dotnet test CodeBrix.Platform.Fonts.Fluent.slnx

The test runner is Microsoft.Testing.Platform, selected by global.json at the
repository root:

    { "test": { "runner": "Microsoft.Testing.Platform" } }

That file pins no SDK version, so the newest installed .NET 10 SDK is still
used. Keep it committed -- without it, `dotnet test` falls back to the older
VSTest bridge.

The test project is xUnit v3 with SilverAssertions.
No opt-in environment variables, no special prep, no network access, no
device. The suite is fast and is the repository's only guard rail, since
there is no API surface for a compiler to check.

The tests do not read src/ directly. The test csproj copies the three
shipped files into the test output as linked content:

    Fonts/*.ttf                 -> TestAssets/Fonts/
    *.uprimarker                -> TestAssets/
    buildTransitive/net10.0/*.props -> TestAssets/buildTransitive/net10.0/

TestAssetPaths.cs is the single place those output paths are spelled out.
If you move a shipped file, fix TestAssetPaths.cs and the csproj item group
together, or the tests will pass against a stale copy.

What the suite asserts, and why each assertion exists:

  ContentFilePresenceTests
      Font present; EXACTLY ONE .ttf (a second font would silently change
      what "the symbols font" means); font larger than 100 KB (catches an
      LFS pointer or a truncated copy); marker file present and zero bytes.

  PropsFileTests
      .props present; sets CodeBrixPlatformDefaultSymbolsFontFamily; points
      at the ms-appx path with THIS package's content folder; offers the
      $(CodeBrixFontsFluentDisableImport) opt-out; contains no residual
      upstream property name and no residual upstream ms-appx path. Those
      last two are the rename-regression guards -- they are the reason the
      package works at all after the fork renamed the property.

  AssemblyMetadataTests
      Assembly name, .NET 10 target framework, loadable by name, and zero
      exported types. If someone ever adds a public helper type, this test
      fails on purpose: decide deliberately, then update the test AND
      AGENT-README.txt (which currently promises no public API).


PACKAGING AND PUBLISHING
========================

Driver: `GeneratePackageOnBuild=true` in the library csproj. There is no
separate pack script.

Versioning: date-stamped and auto-incrementing, computed in the csproj from
System.DateTime.UtcNow as `1.<x>.<y>.<z>`:

    1   major     pinned to 1 for this library
    x   minor     whole years since the `_VersionBaseYear` property
    y   build     UTC day of year, 1-based (Jan 1 = 1)
    z   revision  UTC minute of day, 0..1439

The value is strictly increasing over time, but it is NOT SemVer: major and
minor say nothing about API compatibility. Two builds inside the same UTC
minute produce the same version, so never publish twice within a minute. To
re-baseline the minor number, change `_VersionBaseYear`.

What ships in the .nupkg:

    buildTransitive/net10.0/CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent.dll
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent.xml
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent.uprimarker
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf
    AGENT-README.txt
    README.md
    THIRD-PARTY-NOTICES.txt
    icon-codebrix-128.png

AGENT-README.txt is the ONLY README that is packed. MAINTAINER-README.txt,
EXTRAS-README.txt and README-INDEX.txt stay in the repository.

Licensing metadata: `PackageLicenseExpression` is `Apache-2.0`, so the
LICENSE file itself is not packed (NuGet renders the expression). The
repository still keeps LICENSE at the root for GitHub.
`PackageRequireLicenseAcceptance` is true.

Two packaging rules that are easy to break:

  1. THE .props FILENAME MUST EQUAL THE PackageId. NuGet only auto-imports
     `buildTransitive/<tfm>/<PackageId>.props`. Renaming the package
     without renaming the file (or the reverse) produces a package that
     restores cleanly, warns NU5129, and quietly registers nothing.

  2. THE CONTENT FOLDER NAME IS PART OF THE CONTRACT. The font packs to
     `lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/`, and the
     `ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/...` URI resolves
     against exactly that folder. If it is renamed, the .props value, the
     tests, README.md, AGENT-README.txt and CodeBrix.Platform's own
     defaults all have to move in lockstep.

The `.uprimarker` must remain directly beside the .dll in `lib/net10.0/`
(not inside the content folder). Asset discovery looks for
`<AssemblyName>.uprimarker` as a sibling of each referenced assembly.


PROVENANCE AND VENDORED SOURCES
===============================

This package repackages an upstream Fluent icon-font NuGet package,
version 2.8.1, published under Apache-2.0. THIRD-PARTY-NOTICES.txt is the
authoritative record; keep it in step with any change here.

  * `Fonts/uno-fluentui-assets.ttf` is redistributed byte-for-byte. It is
    NOT re-generated, subset, or re-hinted by this repository -- there is
    no regeneration step and no font toolchain in the build. "Updating the
    font" means taking a newer upstream binary, replacing the file,
    re-running the tests, and updating THIRD-PARTY-NOTICES.txt. Do not
    subset it: consumers address glyphs by codepoint and any subsetting
    silently breaks an unknown set of them.
  * The `.props` and the `.uprimarker` are adapted from upstream. Both
    carry a `was previously: <upstream-path>` provenance comment where the
    file format allows comments; the binary font cannot, so its provenance
    lives in THIRD-PARTY-NOTICES.txt.
  * Renames applied during repackaging: target framework netstandard1.0 ->
    net10.0; assembly / root namespace / content folder ->
    CodeBrix.Platform.Fonts.Fluent; the MSBuild property and the opt-out
    property -> CodeBrixPlatformDefaultSymbolsFontFamily and
    CodeBrixFontsFluentDisableImport; the ms-appx path -> this package's
    content folder. The font FILE keeps its original upstream name.

Facts about the shipped font, for identification (read from its own tables,
not from documentation): family `Symbols`, subfamily `Regular`, name-table
version string `Version 1.0`, producer note `Font generated by IcoMoon.`,
2048 units per em, 1410 glyphs, `post` table format 3.0 (no glyph names),
`cmap` format 4 subtables for platform/encoding (0,3), (1,3) and (3,1),
1409 mapped codepoints of which 1406 are Private Use Area. If a font swap
ever changes those numbers, AGENT-README.txt's GLYPH COVERAGE section has
to be regenerated -- it quotes them.

RELATIONSHIP TO CodeBrix.Platform
---------------------------------
CodeBrix.Platform already names THIS package's URI in its own defaults: the
`SymbolThemeFontFamily` entries for the Skia and Android heads in its
generic theme dictionary, and the stock value of
`FeatureConfiguration.Font.SymbolsFont`. Its XAML source generator reads
the `CodeBrixPlatformDefaultSymbolsFontFamily` MSBuild property this package
sets and emits an assignment to `FeatureConfiguration.Font.SymbolsFont` at
the end of the generated App.xaml constructor. So this repository and
CodeBrix.Platform are coupled through three strings -- the URI, the MSBuild
property name, and the opt-out property name. Changing any of them here
without changing CodeBrix.Platform produces a build that succeeds and an
application whose icons silently do not render.


CODING CONVENTIONS
==================

Standard CodeBrix family conventions apply. The ones that actually bite in
this repository:

  * net10.0 only. No multi-targeting.
  * Nullable reference types OFF. No `?` on reference types, no `!`
    null-forgiveness. Value-type nullables are fine.
  * No global usings.
  * `GenerateDocumentationFile` is true. Every public/protected member of a
    public type needs an XML doc comment; CS1591 is fixed at source, never
    suppressed. Trivially satisfied today, since there are no public types.
  * No project-level warning suppression (`NoWarn`, `WarningLevel 0`,
    `TreatWarningsAsErrors false`).
  * Tests: `<Class>Tests.cs` file names, snake_case test method names,
    `//Arrange` / `//Act` / `//Assert` comments, SilverAssertions for
    assertions, and `TestContext.Current.CancellationToken` threaded
    through any cancellable call.
  * `InternalsVisibleTo.cs` exposes internals to
    `CodeBrix.Platform.Fonts.Fluent.Tests`.
  * The csproj `Copyright` records the upstream font attribution alongside
    the CodeBrix copyright line, as the family's porting guidance requires.
    Keep it in step with THIRD-PARTY-NOTICES.txt.
  * When editing a file adapted from upstream, keep its
    `was previously: <path>` comment. Comment things out rather than
    deleting them when the history is useful.

For the full family convention list see CODEBRIX_LIBRARY_OBSERVATIONS.txt
in the CodeBrix.Library.Dev-private repository.


NOTES
=====

  * The word for the upstream project is not used in this repository's
    documentation; write "upstream" or "the upstream project". The font
    FILENAME is the single exception and is always quoted verbatim.
  * There is no sample application here. Verifying that icons really render
    means referencing the package from a CodeBrix.Platform application; see
    EXTRAS-README.txt.
  * AGENT-README.txt deliberately contains no package version numbers.
    Version facts belong in this file.
