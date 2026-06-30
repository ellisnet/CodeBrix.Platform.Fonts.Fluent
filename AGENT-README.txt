========================================================================
AGENT-README: CodeBrix.Platform.Fonts.Fluent
A Comprehensive Guide for AI Coding Agents
========================================================================


OVERVIEW
========================================================================

CodeBrix.Platform.Fonts.Fluent is a .NET 10 redistribution of the Uno
Platform Fluent icon font (Windows 11 iconography), packaged for the
CodeBrix family. It is a namespace-renamed equivalent of
`Uno.Fonts.Fluent` 2.8.1, intended as a drop-in replacement for that
package in CodeBrix.Platform-forked Uno applications. It supplies the
default symbols (icon) font used by SymbolIcon, FontIcon, and the
SymbolThemeFontFamily theme resource.

The library has effectively no managed code: the assembly is a metadata-
only .NET 10 DLL whose sole purpose is to host the bundled font content
file. The interesting payload lives in:

  - 1 `.ttf` font file (uno-fluentui-assets.ttf) under
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/ inside the nupkg.
  - A `.uprimarker` file that Uno-fork build pipelines use to discover
    UPRI-bearing font asset packages.
  - An MSBuild `.props` file under buildTransitive/net10.0/ that sets the
    `CodeBrixPlatformDefaultSymbolsFontFamily` property to this package's
    bundled font, so a CodeBrix.Platform app picks it up as the default
    symbols font automatically.

Unlike its sibling CodeBrix.Platform.Fonts.OpenSans (which ships 37
weight/style/stretch font files plus a `.ttf.manifest` and a font-pruning
`.targets`), Fluent is a single icon font with no manifest and no
static-vs-variable split, so the buildTransitive file is a `.props` that
sets a property rather than a `.targets` that prunes files.


INSTALLATION
========================================================================

NuGet package: CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever

  dotnet add package CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever

The library namespace inside the assembly is `CodeBrix.Platform.Fonts.Fluent`
(without the `.ApacheLicenseForever` suffix; that suffix exists only on
the NuGet PackageId for license-disambiguation across the CodeBrix family).

Target framework: .NET 10.0 or higher.


KEY NAMESPACE
========================================================================

The library exposes no public managed types in its first iteration —
the assembly is metadata-only, matching the shape of upstream
Uno.Fonts.Fluent. Consumers reference the bundled font content file via
an `ms-appx:///` URI rooted at the assembly content folder:

  ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf

Appended with `#Symbols` when used as a FontFamily for glyph rendering:

  ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf#Symbols


FONT INVENTORY
========================================================================

The package ships exactly 1 `.ttf` file and no manifest:

  uno-fluentui-assets.ttf  — the Uno Platform Fluent / Windows 11 symbol
                             icon font (~779 KB), redistributed bit-for-bit
                             from Uno.Fonts.Fluent 2.8.1.


CORE API REFERENCE
========================================================================

This library has no public managed API. Consumers interact with it only
through:

  1. The NuGet content path
     (`ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf`)
     used as a `FontFamily` value in XAML or in code that constructs XAML
     element trees, and as the default symbols font.

  2. The MSBuild `.props` file under buildTransitive/net10.0/
     `CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props`, whose
     on-disk filename matches the NuGet PackageId so that NuGet's
     auto-import convention (NU5129) picks it up in consumer builds.
     It sets:

       <CodeBrixPlatformDefaultSymbolsFontFamily>
         ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf
       </CodeBrixPlatformDefaultSymbolsFontFamily>

     guarded by `Condition="'$(CodeBrixFontsFluentDisableImport)'==''"` so a
     consumer can opt out by setting CodeBrixFontsFluentDisableImport.
     (CodeBrixPlatformDefaultSymbolsFontFamily is the property the
     CodeBrix.Platform XAML source generator actually reads; the upstream
     Uno names were UnoPlatformDefaultSymbolsFontFamily /
     UnoFontsFluentDisableImport, which the fork renamed.)

If a future iteration exposes a managed API (e.g. typed accessors that
return the font stream/path for non-Uno consumers), it will live under
the `CodeBrix.Platform.Fonts.Fluent` root namespace and be documented here.


ARCHITECTURE
========================================================================

Repository layout:

  CodeBrix.Platform.Fonts.Fluent/
    src/CodeBrix.Platform.Fonts.Fluent/
      CodeBrix.Platform.Fonts.Fluent.csproj
      InternalsVisibleTo.cs
      CodeBrix.Platform.Fonts.Fluent.uprimarker     (empty file)
      buildTransitive/
        net10.0/
          CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props
      Fonts/
        uno-fluentui-assets.ttf
    tests/CodeBrix.Platform.Fonts.Fluent.Tests/
      CodeBrix.Platform.Fonts.Fluent.Tests.csproj
      AssemblyMetadataTests.cs
      ContentFilePresenceTests.cs
      PropsFileTests.cs
      TestAssetPaths.cs
    AGENT-README.txt
    LICENSE                  (Apache-2.0)
    README.md
    THIRD-PARTY-NOTICES.txt

Inside the produced NuGet (.nupkg), the file layout is:
  buildTransitive/net10.0/CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props
  lib/net10.0/CodeBrix.Platform.Fonts.Fluent.dll
  lib/net10.0/CodeBrix.Platform.Fonts.Fluent.uprimarker
  lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf
  AGENT-README.txt
  README.md
  THIRD-PARTY-NOTICES.txt
  icon-codebrix-128.png

The `lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/` content layout is
load-bearing: the `ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/...`
URI that consumers reference resolves relative to that content folder, so
if the content folder is renamed the `.props` value (and any consumer
references) must be updated in lockstep.


RELATIONSHIP TO THE CodeBrix.Platform FORK
========================================================================

The CodeBrix.Platform fork hardcodes the Fluent symbols font path in a
few places (SymbolThemeFontFamily in SystemResources.xaml,
FeatureConfiguration.cs, and runtime tests). Upstream those point at
`ms-appx:///Uno.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf`. Because this
package deliberately registers its font under the CodeBrix content folder
(`ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/...`), a fork that
references THIS package instead of Uno.Fonts.Fluent must update those
hardcoded paths to the CodeBrix path. See THIRD-PARTY-NOTICES.txt and the
repo's CODEBRIX_REPOS.txt note for the exact list.


CODING CONVENTIONS (CodeBrix family)
========================================================================

This repository follows every CodeBrix family convention. Most are
inherited from the standard library scaffold; key points:

  * Target framework: net10.0 only. No multi-targeting.
  * Nullable reference types (NRT): OFF (do not set <Nullable>enable</Nullable>).
    No `?` annotations on reference types; no `!` null-forgiveness operator.
    Value-type nullables (`int?`, `DateOnly?`, etc.) are fine.
  * No global usings.
  * `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is on.
    Every public/protected member of a public type needs an XML doc
    comment. CS1591 is fixed at source, never suppressed. (In this
    library's first iteration there are no public types, so CS1591
    is trivially clean.)
  * Tests use xUnit v3 + SilverAssertions; coverlet.collector for
    coverage; `TestContext.Current.CancellationToken` is threaded through
    any cancellable call inside a test.
  * No project-level warning suppression (`<NoWarn>`, `<WarningLevel>0</>`,
    `<TreatWarningsAsErrors>false</>`, etc. are all forbidden).
  * Copyright string in the csproj records the upstream Uno Platform
    attribution alongside the standard CodeBrix copyright line, per the
    family's porting-guidance rule:
      Copyright (c) 2026 Jeremy Ellis and contributors. Fluent icon font
      (C) 2015-2023 Uno Platform Inc., distributed under Apache-2.0.

For the full list of family conventions see CODEBRIX_LIBRARY_OBSERVATIONS.txt
in the CodeBrix.Library.Dev-private repo.


TESTING
========================================================================

Tests live under tests/CodeBrix.Platform.Fonts.Fluent.Tests/. Run with:

  dotnet test CodeBrix.Platform.Fonts.Fluent.slnx

The test suite covers:

  * Content-file presence: that uno-fluentui-assets.ttf exists next to the
    test assembly's expected build-output font folder (resolved via
    `AppContext.BaseDirectory` + `TestAssets/Fonts/`, centralized in
    `TestAssetPaths`), that it is a non-trivial size, that it is the only
    `.ttf` shipped, and that the `.uprimarker` sibling file exists and is
    empty.
  * Assembly metadata: that the produced library assembly is named
    `CodeBrix.Platform.Fonts.Fluent`, targets .NET 10, can be loaded by
    name, and exports no public types (metadata-only).
  * .props file: that the buildTransitive .props file is present next to
    the test assembly, that it sets `CodeBrixPlatformDefaultSymbolsFontFamily`
    to the `ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf`
    path, that it no longer uses the upstream Uno property name, and that it
    carries no residual upstream `ms-appx:///Uno.Fonts.Fluent` path token
    (catches incomplete rename regressions).


PROVENANCE OF PORTED FILES
========================================================================

Files derived from upstream Uno.Fonts.Fluent 2.8.1 carry a
`was previously: <upstream-path>` provenance comment in their file header
(for files where comments are syntactically allowed). The binary `.ttf`
file cannot carry inline provenance, so its provenance is recorded in
THIRD-PARTY-NOTICES.txt instead.


KNOWN GOTCHAS
========================================================================

  * `ms-appx:///` URIs are resolved by Uno's runtime, not by .NET itself.
    Outside a CodeBrix.Platform-fork host, the URI won't resolve. Plain
    .NET 10 console / test apps that reference this package can still
    access the .ttf via the package's on-disk location
    (`<nuget-cache>/codebrix.platform.fonts.fluent.apachelicenseforever/<version>/lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf`),
    but they have to do that lookup themselves.

  * The `.props` sets the symbols font family only when
    `CodeBrixFontsFluentDisableImport` is empty. A consumer that sets that
    property opts out of the automatic default and must supply its own
    symbols font.

  * The property the package sets is `CodeBrixPlatformDefaultSymbolsFontFamily`
    — the name the CodeBrix.Platform XAML source generator reads. The
    upstream Uno package set `UnoPlatformDefaultSymbolsFontFamily`; the fork
    renamed the property, so a .props that set the old name would silently
    fail to register the default symbols font. Keep this in sync if the fork
    renames the property again.

  * Because this package uses the CodeBrix content path rather than the
    upstream `Uno.Fonts.Fluent` path, a CodeBrix.Platform fork that swaps
    to this package must update its hardcoded SymbolThemeFontFamily /
    default-symbols references to the CodeBrix path, or icon glyphs will
    fail to resolve. See "RELATIONSHIP TO THE CodeBrix.Platform FORK" above.
