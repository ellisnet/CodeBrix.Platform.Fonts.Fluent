================================================================================
AGENT-README: CodeBrix.Platform.Fonts.Fluent
A Guide for AI Coding Agents — CONSUMING the
CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever NuGet package
================================================================================


OVERVIEW
========

CodeBrix.Platform.Fonts.Fluent is an ASSET package. It ships one icon font
-- a Windows 11 style Fluent symbol set -- plus the MSBuild glue that makes
that font the default symbols (icon) font of a CodeBrix.Platform
application. It is the font behind `SymbolIcon`, behind `FontIcon` when no
`FontFamily` is set on it, and behind the `SymbolThemeFontFamily` theme
resource.

The package contains no managed API. Its assembly is metadata-only: it
exports zero public types (this repository's test suite asserts that). The
payload is the font file, an asset-marker file, and a `.props` file. An
agent consuming this package writes no `using` for it and calls nothing in
it -- it adds a `PackageReference` and then uses a string (the font URI)
and, optionally, one MSBuild property.

Target framework: .NET 10 or later.

Provenance: the `uno-fluentui-assets.ttf` binary and the packaging shape
(the `.props` logic and the asset-marker convention) are redistributed from
the upstream Fluent icon-font package, version 2.8.1, under Apache-2.0. The
font file deliberately keeps its original upstream filename; every path in
this document quotes it verbatim. THIRD-PARTY-NOTICES.txt in this
repository carries the full attribution.

WHO NEEDS THIS PACKAGE
----------------------
  * Any CodeBrix.Platform application that renders `SymbolIcon`,
    `FontIcon`, or any built-in control whose template draws an icon
    (they resolve `SymbolThemeFontFamily`).
  * Any application that wants Fluent iconography in ordinary text
    (`TextBlock`, `Button` content) by naming the font explicitly.

Applications generated from the CodeBrix.Platform application template
already reference it. Referencing it a second time is harmless but
unnecessary.


INSTALLATION
============

PackageId: CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever

    dotnet add package CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever

NuGet dependencies: none. The package brings in nothing but itself.

License: Apache-2.0 (the package's SPDX `PackageLicenseExpression`). Both
the wrapper/packaging and the redistributed font binary are Apache-2.0, so
there is no dual-license expression to reason about. The package sets
`PackageRequireLicenseAcceptance`, so a restore in an interactive tool asks
for acceptance.

Requirements and limits:
  * .NET 10.0 or later. No multi-targeting, no netstandard target.
  * No native libraries, no OS restrictions -- it is a font and two
    build-time files.
  * The `ms-appx:///` URI this package registers is resolved by the
    CodeBrix.Platform runtime. Outside a CodeBrix.Platform application the
    URI means nothing (see WHAT THIS PACKAGE DOES NOT DO).

Assembly identity: the assembly and its root namespace are
`CodeBrix.Platform.Fonts.Fluent`, WITHOUT the `.ApacheLicenseForever`
suffix. That suffix exists only on the NuGet PackageId, where the CodeBrix
family uses it to disambiguate licensing.


KEY NAMESPACES / USINGS
=======================

This package contributes NO namespace and NO type, so there is nothing to
`using` from it. The types in every example below come from CodeBrix.Platform
itself (package `CodeBrix.Platform.UI...`, referenced by any application
head), not from this package:

    using Microsoft.UI.Xaml.Controls;   // FontIcon, SymbolIcon, Symbol
    using Microsoft.UI.Xaml.Media;      // FontFamily
    using CodeBrix.Platform.UI;         // FeatureConfiguration

The one identifier this package really contributes is a STRING -- the font
content URI:

    ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf

Treat that string as the package's public API. It is stable, it is what the
shipped `.props` writes, and it is what CodeBrix.Platform's own default
symbols font is set to.


HOW THE PACKAGE REGISTERS ITSELF (THE FULL CHAIN)
=================================================

Referencing the package is normally the ONLY thing a consumer does. This is
what that reference sets in motion, end to end:

  1. NuGet auto-imports
     `buildTransitive/net10.0/CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props`
     into the consuming project. (The `.props` filename matches the
     PackageId, which is what makes NuGet's auto-import convention apply.)

  2. That `.props` sets the MSBuild property
     `CodeBrixPlatformDefaultSymbolsFontFamily` to the font content URI,
     guarded by `Condition="'$(CodeBrixFontsFluentDisableImport)'==''"`.

  3. CodeBrix.Platform's source-generator package marks
     `CodeBrixPlatformDefaultSymbolsFontFamily` as a compiler-visible
     property, so the XAML source generator can read it.

  4. When the property is non-empty, the XAML generator appends one line to
     the end of the generated `App.xaml` constructor:

         global::CodeBrix.Platform.UI.FeatureConfiguration.Font.SymbolsFont
             = "<the URI from step 2>";

  5. Assigning `FeatureConfiguration.Font.SymbolsFont` pushes a new
     `FontFamily` into the `SymbolThemeFontFamily` key of the Default,
     Light and HighContrast theme dictionaries. `SymbolIcon` and
     `FontIcon` both default their `FontFamily` to
     `FeatureConfiguration.Font.SymbolsFont`.

Separately, at build time the head project's asset step looks beside each
referenced assembly for a file named `<AssemblyName>.uprimarker`. This
package ships `lib/net10.0/CodeBrix.Platform.Fonts.Fluent.uprimarker` right
next to `CodeBrix.Platform.Fonts.Fluent.dll`, which is how the font under
`lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/` is collected into the
application's assets and becomes reachable through `ms-appx:///`.

CONSEQUENCE FOR AGENTS: in a normal CodeBrix.Platform application you do
NOT need to write the URI anywhere. `<SymbolIcon Symbol="Setting" />` and a
bare `<FontIcon Glyph="&#xE713;" />` already resolve to this font. Write the
URI explicitly only when you are overriding something or rendering glyphs
in ordinary text.


CORE API REFERENCE
==================

There is no managed API. The complete consumer-visible contract is these
six items.

1. FONT CONTENT URI (string)
----------------------------
    ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf

  Usable anywhere a `FontFamily` is accepted: XAML attribute, the
  `FontFamily(string)` constructor, `FeatureConfiguration.Font.SymbolsFont`.
  The `CodeBrix.Platform.Fonts.Fluent/Fonts/` segment is the package's
  content folder inside the nupkg and is load-bearing -- it is not the
  assembly name being echoed, it is a real directory.

2. MSBuild PROPERTY `CodeBrixPlatformDefaultSymbolsFontFamily`
--------------------------------------------------------------
  Set by the shipped `.props` to the URI above. This is the property the
  CodeBrix.Platform XAML source generator reads. A consumer may set it in
  its own project file to point the default symbols font somewhere else
  entirely:

      <PropertyGroup>
        <CodeBrixPlatformDefaultSymbolsFontFamily>ms-appx:///MyApp/Assets/Fonts/my-icons.ttf</CodeBrixPlatformDefaultSymbolsFontFamily>
      </PropertyGroup>

  MSBuild evaluation order decides the winner; put your own value in a
  `Directory.Build.targets` or an `<PropertyGroup>` that is evaluated after
  NuGet's imported `.props` if you find it is being overwritten.

3. MSBuild PROPERTY `CodeBrixFontsFluentDisableImport` (opt-out)
----------------------------------------------------------------
  Set it to any non-empty value BEFORE the package's `.props` is evaluated
  (a `Directory.Build.props`, or the top of the project file) and the
  package stops registering itself as the default symbols font:

      <PropertyGroup>
        <CodeBrixFontsFluentDisableImport>true</CodeBrixFontsFluentDisableImport>
      </PropertyGroup>

  The font file still ships and is still reachable by URI; only the
  automatic default is suppressed. If you opt out, you own supplying a
  symbols font, or `SymbolIcon` renders with whatever the runtime default
  is.

4. RUNTIME PROPERTY `CodeBrix.Platform.UI.FeatureConfiguration.Font.SymbolsFont`
--------------------------------------------------------------------------------
      public static string SymbolsFont { get; set; }

  A `string` (a font URI), not a `FontFamily`. Its stock default in
  CodeBrix.Platform is already this package's URI. Assigning it re-writes
  the `SymbolThemeFontFamily` theme resource in all three theme
  dictionaries. Per its own documentation it must be assigned AFTER
  `App.InitializeComponent()` to take effect -- which is exactly where the
  generated assignment from the `.props` lands.

5. THEME RESOURCE KEY `SymbolThemeFontFamily`
----------------------------------------------
  A `FontFamily` resource present in the Default, Light and HighContrast
  theme dictionaries. Built-in control templates resolve icons through it.
  Override it in application resources (see COMPLETE EXAMPLES) to change
  every templated icon at once.

6. NUPKG LAYOUT (what a consumer can rely on being on disk)
------------------------------------------------------------
    buildTransitive/net10.0/CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent.dll
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent.xml
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent.uprimarker            (0 bytes)
    lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf
    AGENT-README.txt
    README.md
    THIRD-PARTY-NOTICES.txt
    icon-codebrix-128.png

  A non-CodeBrix.Platform consumer that just wants the .ttf bytes can read
  the last font path above out of the restored package folder. Nothing else
  in the package is meaningful to a plain .NET application.


FONT INVENTORY
==============

The package ships EXACTLY ONE font file and no manifest:

    uno-fluentui-assets.ttf     ~779 KB

Facts read directly from that file's own tables:

    Family name (`name` IDs 1/3/4/6)   Symbols
    Subfamily (`name` ID 2)            Regular
    Version string (`name` ID 5)       Version 1.0
    Producer note (`name` ID 10)       Font generated by IcoMoon.
    Units per em (`head`)              2048
    Glyph count (`maxp`)               1410
    Glyph names (`post`)               none -- `post` is format 3.0
    `cmap` subtables                   format 4, for (0,3), (1,3) and (3,1)

Consequences of that inventory:

  * ONE face only. There is no bold, no italic, no weight axis and no
    variable font. Setting `FontWeight` or `FontStyle` on a `FontIcon`
    changes nothing about which outline is drawn.
  * No `.ttf.manifest` ships with this package, because there is no
    weight/style/stretch family to resolve. The sibling TEXT-font packages
    in the CodeBrix.Platform.Fonts.* group do ship manifests; this one does
    not, and none of their manifest-related rules apply here.
  * No `CODEBRIX-DEVELOP.json` descriptor and no `buildTransitive`
    `.targets`: this package registers a default and prunes nothing.
  * `post` format 3.0 means the font carries NO glyph names. You cannot
    discover a glyph by name from the file; you address glyphs by
    codepoint. The names in the table further down come from
    CodeBrix.Platform's `Symbol` enum, not from the font.


GLYPH COVERAGE
==============

The `cmap` maps 1,409 codepoints. Three of them are not icons:

    U+0000, U+0001    control slots
    U+0020            space

The remaining 1,406 are all in the Private Use Area, spread from U+E001 to
U+F8AE. Because they are PUA, these codepoints mean NOTHING in any other
font: text carrying them renders as missing-glyph boxes (or as unrelated
icons) unless the run is drawn in this font.

Coverage by 256-codepoint block (count of mapped codepoints):

    U+E000-U+E0FF    11        U+EC00-U+ECFF   107
    U+E100-U+E1FF   192        U+ED00-U+EDFF    90
    U+E200-U+E2FF     5        U+EE00-U+EEFF    21
    U+E600-U+E6FF     3        U+EF00-U+EFFF    24
    U+E700-U+E7FF   189        U+F000-U+F0FF    53
    U+E800-U+E8FF   238        U+F100-U+F1FF    82
    U+E900-U+E9FF   158        U+F200-U+F2FF    18
    U+EA00-U+EAFF    77        U+F300-U+F3FF    10
    U+EB00-U+EBFF   111        U+F400-U+F4FF     7
                               U+F500-U+F5FF     2
                               U+F700-U+F7FF     5
                               U+F800-U+F8FF     3

    (U+E300-U+E5FF, U+F600-U+F6FF: nothing mapped.)

Coverage is dense but NOT contiguous -- the mapped codepoints form 274
separate runs, so a codepoint inside a covered block is not guaranteed to
exist. Do not compute a glyph by adding an offset to a known one. Every
codepoint in the table below was checked against this font's `cmap`.

Two ranges matter in practice:

  * U+E100-U+E1FF -- the LEGACY range. This is where the values of the
    `Symbol` enum live. All 192 mapped codepoints in it are present.
  * U+E700 upward -- the MODERN range. This is where `SymbolIcon` actually
    draws from (see the next section) and where most glyphs that exist only
    in the modern set live.


THE `Symbol` ENUM, AND THE GLYPH IT ACTUALLY DRAWS
==================================================

CodeBrix.Platform's `Microsoft.UI.Xaml.Controls.Symbol` enum has 197
members. Every one of them has a codepoint that exists in this font --
verified member by member against the shipped `cmap`.

But `SymbolIcon` does NOT draw the enum's own value. Before drawing, it
runs the value through a translation table that moves the legacy U+E1xx
codepoints to modern equivalents, so the icon matches current Fluent
iconography. 196 of the 197 members have a table entry, and for 191 of them
the drawn codepoint DIFFERS from the enum value. (The one member with no
entry is `Target`, U+E1D2. Five more -- `GlobalNavigationButton`,
`Placeholder`, `Print`, `Share` and `XboxOneConsole` -- have an entry that
maps them to themselves.) Any value the table does not recognise falls
through and is drawn as-is.

WHY YOU CARE: these two lines do NOT produce the same picture.

    <SymbolIcon Symbol="Setting" />                  <!-- draws U+E713 -->
    <FontIcon Glyph="&#xE115;" />                    <!-- draws U+E115 -->

`Symbol.Setting` is 57621 = U+E115, but `SymbolIcon` draws U+E713. To match
a `SymbolIcon` with a raw `FontIcon`, use the RIGHT-HAND column below.

    Symbol member            enum value     glyph actually drawn
    ----------------------   ------------   --------------------
    Accept                   U+E10B         U+E8FB
    Add                      U+E109         U+E710
    Back                     U+E112         U+E72B
    Bold                     U+E19B         U+E8DD
    Calendar                 U+E163         U+E787
    Camera                   U+E114         U+E722
    Cancel                   U+E10A         U+E711
    Clear                    U+E106         U+E894
    Clock                    U+E121         U+E823
    Contact                  U+E13D         U+E77B
    Copy                     U+E16F         U+E8C8
    Cut                      U+E16B         U+E8C6
    Delete                   U+E107         U+E74D
    Document                 U+E130         U+E8A5
    Download                 U+E118         U+E896
    Edit                     U+E104         U+E70F
    Favorite                 U+E113         U+E734
    Filter                   U+E16E         U+E71C
    Find                     U+E11A         U+E721
    Folder                   U+E188         U+E8B7
    Forward                  U+E111         U+E72A
    FullScreen               U+E1D9         U+E740
    GlobalNavigationButton   U+E700         U+E700
    Help                     U+E11B         U+E897
    Home                     U+E10F         U+E80F
    Italic                   U+E199         U+E8DB
    Keyboard                 U+E144         U+E765
    Link                     U+E167         U+E71B
    List                     U+E14C         U+EA37
    Mail                     U+E119         U+E715
    More                     U+E10C         U+E712
    Mute                     U+E198         U+E74F
    Next                     U+E101         U+E893
    OpenFile                 U+E1A5         U+E8E5
    Paste                    U+E16D         U+E77F
    Pause                    U+E103         U+E769
    People                   U+E125         U+E716
    Phone                    U+E13A         U+E717
    Play                     U+E102         U+E768
    Previous                 U+E100         U+E892
    Print                    U+E749         U+E749
    Redo                     U+E10D         U+E7A6
    Refresh                  U+E149         U+E72C
    Remove                   U+E108         U+E738
    Rename                   U+E13E         U+E8AC
    Save                     U+E105         U+E74E
    Send                     U+E122         U+E724
    Setting                  U+E115         U+E713
    Share                    U+E72D         U+E72D
    SolidStar                U+E1CF         U+E735
    Sort                     U+E174         U+E8CB
    Stop                     U+E15B         U+E71A
    Sync                     U+E117         U+E895
    Undo                     U+E10E         U+E7A7
    Upload                   U+E11C         U+E898
    Video                    U+E116         U+E714
    Volume                   U+E15D         U+E767
    ZoomIn                   U+E12E         U+E8A3
    ZoomOut                  U+E1A4         U+E71F

That is 59 of the 197 members. For any member not listed, read its value
out of `Symbol` and, if you need the raw codepoint, prefer `SymbolIcon` and
let the framework do the translation for you.

SIX GLYPHS THE FRAMEWORK ITSELF USES BY RAW CODEPOINT (no enum member):

    ArrowUp        U+E110        ArrowLeft      U+E112
    ArrowDown      U+E74B        ArrowRight     U+E111
    LockLocked     U+E72E        LockUnlocked   U+E785

  CodeBrix.Platform paints these in its own on-screen keyboard and file
  picker. They are useful precisely because they are known-good: plain
  Unicode arrows (U+2191 and friends) are absent from the text fonts a
  CodeBrix.Platform application usually ships, whereas these are in this
  font, which the framework always has.

FINDING A GLYPH NOT LISTED HERE
-------------------------------
  The font has no glyph names, so there is no lookup inside the package.
  Practical options, in order:
    1. If a `Symbol` member names what you want, use `SymbolIcon` -- that
       is the only name-based path that is guaranteed correct.
    2. Otherwise use the published Fluent/Segoe MDL2 icon codepoint you
       want and CHECK it renders. The modern U+E7xx-U+F8xx ranges above
       are what this font covers.
    3. Never invent a codepoint by arithmetic; coverage is not contiguous.


THE "#Symbols" FRAGMENT
=======================

You will see the font URI written two ways. Both work, and both are in use
inside CodeBrix.Platform:

    ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf
    ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf#Symbols

The fragment after `#` names a font FAMILY inside the file. This file
declares exactly one family, and its name -- read from the font's own
`name` table -- is literally `Symbols`. So `#Symbols` selects the same and
only face that the bare path selects. It is redundant, not wrong.

What is verifiable from this repository: the `.props` this package ships
sets the BARE path, with no fragment -- and `PropsFileTests` in this
repository asserts that the `.props` carries exactly that path token. That
bare value is what reaches `FeatureConfiguration.Font.SymbolsFont`. In
CodeBrix.Platform's own generic theme dictionary, meanwhile, the
`SymbolThemeFontFamily` entry for the Skia heads is written WITH
`#Symbols`; both forms therefore coexist in a running application and
resolve to the same face.

RECONCILIATION WITH THE SIBLING FONT PACKAGES: the sibling TEXT-font
packages tell you not to append a fragment. That rule is about fonts whose
weight/style variants are resolved through a `.ttf.manifest` keyed on the
bare `.ttf` path. This package ships NO manifest and ONE face (see FONT
INVENTORY), so there is nothing here for a fragment to interfere with.

RULE OF THUMB: write the bare path. It is what this package registers, it
is shorter, and it can never be out of step with a family rename.


COMPLETE EXAMPLES
=================

1. THE DEFAULT PATH -- WRITE NOTHING
------------------------------------
With the package referenced and nothing else done, both of these draw a
Fluent icon in the correct font:

    <SymbolIcon Symbol="Setting" />
    <FontIcon Glyph="&#xE713;" />

`SymbolIcon` always uses the symbols font. `FontIcon` uses it because its
`FontFamily` dependency property defaults to
`FeatureConfiguration.Font.SymbolsFont`.

2. FontIcon WITH AN EXPLICIT FontFamily
---------------------------------------
Spell the family out when you want the code to be independent of whatever
the application's default symbols font happens to be:

    <FontIcon
        FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf"
        Glyph="&#xE713;"
        FontSize="20" />

The XAML character-entity form is `&#x` + the hex codepoint + `;`. All
codepoints in this font are in the BMP, so one entity is one glyph -- no
surrogate pairs are ever needed.

3. SymbolIcon, RELYING ON THE DEFAULT
-------------------------------------
    <StackPanel Orientation="Horizontal" Spacing="8">
        <SymbolIcon Symbol="Save" />
        <SymbolIcon Symbol="Delete" />
        <SymbolIcon Symbol="Refresh" />
    </StackPanel>

Never set `FontFamily` on a `SymbolIcon` -- it has no such property, and it
resolves its font itself.

4. AN ICON INSIDE A Button
--------------------------
    <Button>
        <StackPanel Orientation="Horizontal" Spacing="6">
            <SymbolIcon Symbol="Save" />
            <TextBlock Text="Save" />
        </StackPanel>
    </Button>

    <!-- or, glyph-only, with no icon element at all -->
    <Button
        FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf"
        Content="&#xE74E;" />

5. A GLYPH IN ORDINARY TEXT
---------------------------
A `TextBlock` uses the application's TEXT font, which does not have these
codepoints. Put the icon in its own `Run` (or its own `TextBlock`) and give
that run the symbols font:

    <TextBlock>
        <Run Text="Press " />
        <Run FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf"
             Text="&#xE713;" />
        <Run Text=" to open settings." />
    </TextBlock>

6. OVERRIDING SymbolThemeFontFamily FOR THE WHOLE APPLICATION
-------------------------------------------------------------
Three ways, in increasing order of how much the framework helps you.

  (a) BUILD TIME -- set the MSBuild property the generator reads. This is
      what the shipped `.props` does, so overriding it keeps you on the
      framework's own path:

          <PropertyGroup>
            <CodeBrixPlatformDefaultSymbolsFontFamily>ms-appx:///MyApp/Assets/Fonts/my-icons.ttf</CodeBrixPlatformDefaultSymbolsFontFamily>
          </PropertyGroup>

  (b) RUNTIME -- assign the property yourself, after
      `App.InitializeComponent()`. This rewrites `SymbolThemeFontFamily` in
      the Default, Light and HighContrast theme dictionaries:

          public App()
          {
              this.InitializeComponent();
              CodeBrix.Platform.UI.FeatureConfiguration.Font.SymbolsFont =
                  "ms-appx:///MyApp/Assets/Fonts/my-icons.ttf";
          }

  (c) DECLARATIVE -- put the key in application resources. Mirror the three
      theme dictionaries the framework maintains, or a theme switch will
      lose your value:

          <Application.Resources>
              <ResourceDictionary>
                  <ResourceDictionary.ThemeDictionaries>
                      <ResourceDictionary x:Key="Default">
                          <FontFamily x:Key="SymbolThemeFontFamily">ms-appx:///MyApp/Assets/Fonts/my-icons.ttf</FontFamily>
                      </ResourceDictionary>
                      <ResourceDictionary x:Key="Light">
                          <FontFamily x:Key="SymbolThemeFontFamily">ms-appx:///MyApp/Assets/Fonts/my-icons.ttf</FontFamily>
                      </ResourceDictionary>
                      <ResourceDictionary x:Key="HighContrast">
                          <FontFamily x:Key="SymbolThemeFontFamily">ms-appx:///MyApp/Assets/Fonts/my-icons.ttf</FontFamily>
                      </ResourceDictionary>
                  </ResourceDictionary.ThemeDictionaries>
              </ResourceDictionary>
          </Application.Resources>

      Prefer (a) or (b): assigning `FeatureConfiguration.Font.SymbolsFont`
      is what the framework itself does, and it also becomes the default
      for newly created `FontIcon`/`SymbolIcon` instances, which a resource
      override does not.

7. THE SAME THINGS IN C#
------------------------
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using CodeBrix.Platform.UI;

    private const string FluentSymbols =
        "ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf";

    // FontIcon with an explicit family and a glyph by codepoint
    var gear = new FontIcon
    {
        FontFamily = new FontFamily(FluentSymbols),
        Glyph = "\uE713",
        FontSize = 20
    };

    // FontIcon relying on the registered default
    var gearDefault = new FontIcon { Glyph = "\uE713" };

    // SymbolIcon by enum member (the framework picks the modern glyph)
    var saveIcon = new SymbolIcon(Symbol.Save);

    // ...or via the property, which is what XAML sets
    var deleteIcon = new SymbolIcon { Symbol = Symbol.Delete };

    // A glyph in ordinary text
    var line = new TextBlock
    {
        FontFamily = new FontFamily(FluentSymbols),
        Text = "\uE713"
    };

    // Change the application-wide symbols font (after InitializeComponent)
    FeatureConfiguration.Font.SymbolsFont = FluentSymbols;

Note `Glyph` is a `string`, not a `char`: `Glyph = "\uE713"`, never
`Glyph = '\uE713'`.

8. OPTING OUT
-------------
In `Directory.Build.props`, or at the top of the project file, before the
package's `.props` is evaluated:

    <PropertyGroup>
      <CodeBrixFontsFluentDisableImport>true</CodeBrixFontsFluentDisableImport>
    </PropertyGroup>

The font still ships and can still be named by URI; only the automatic
registration as the default symbols font is suppressed.

9. GETTING AT THE .ttf FROM A NON-CodeBrix.Platform PROGRAM
------------------------------------------------------------
`ms-appx:///` is not a .NET URI scheme -- nothing outside CodeBrix.Platform
resolves it. A plain .NET program that references this package can still
read the file out of the restored package folder:

    lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf

relative to the package directory in the NuGet cache. You do that lookup
yourself; the package offers no helper.


MINIMUM VIABLE PROJECT
======================

The smallest thing that proves the font is wired up, inside an existing
CodeBrix.Platform application.

    <!-- MyApp.csproj -- add the package reference.
         (Version deliberately omitted here; pin the current published
         version in your own project.) -->
    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
      </PropertyGroup>
      <ItemGroup>
        <PackageReference Include="CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever" />
      </ItemGroup>
    </Project>

    <!-- MainPage.xaml -- three ways of drawing the same gear -->
    <Page x:Class="MyApp.MainPage"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
      <StackPanel Spacing="12" Padding="24">

        <!-- 1. the enum, translated to the modern glyph by the framework -->
        <SymbolIcon Symbol="Setting" />

        <!-- 2. the same glyph, by codepoint, on the default font -->
        <FontIcon Glyph="&#xE713;" />

        <!-- 3. the same glyph, with the font named explicitly -->
        <FontIcon
            FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf"
            Glyph="&#xE713;" />

      </StackPanel>
    </Page>

All three should render an identical gear. If #1 and #2 render and #3 does
not, the URI is wrong. If #3 renders and #1/#2 do not, the default
registration did not happen -- check `CodeBrixFontsFluentDisableImport` and
that the reference reaches the head project.


PERFORMANCE TIPS
================

There is nothing to tune. The package is a static asset carrier: one ~779 KB
font file that the runtime loads once and caches, plus two build-time files
that cost nothing at runtime. Two small habits:

  * Reuse one `FontFamily` instance (a `static readonly` field, or a XAML
    resource) instead of constructing `new FontFamily(uri)` per icon.
  * The whole font is one file; there is no way to ship a subset of it, so
    an application that uses six glyphs still carries all 1,410. If that
    matters, the answer is a smaller font of your own, plus the opt-out
    property -- not a setting on this package.


COMMON PITFALLS TO AVOID
========================

  * DO NOT ASSUME `Symbol.X` AND CODEPOINT `X` ARE THE SAME PICTURE.
    `SymbolIcon` remaps almost every `Symbol` member from its legacy
    U+E1xx value to a modern one -- 191 of the 197 draw a codepoint other
    than their own value. `Symbol.Setting` is U+E115 but draws U+E713.
    Mixing `SymbolIcon` and hand-written `FontIcon` glyphs in one toolbar
    is the usual way to end up with one odd-looking icon.

  * THE PROPERTY NAME IS `CodeBrixPlatformDefaultSymbolsFontFamily`.
    A `.props` or project file that sets the upstream project's old
    property name silently does nothing -- there is no warning, the icons
    just fall back to whatever the runtime default is.

  * THE OPT-OUT MUST BE SET EARLY. `CodeBrixFontsFluentDisableImport` is
    read as a condition on the package's imported `.props`. Setting it
    inside a target, or after that import has been evaluated, is too late.

  * PUA CODEPOINTS ARE MEANINGLESS IN A TEXT FONT. A raw PUA codepoint in a
    `TextBlock` that uses the application's text font gives you a
    missing-glyph box, not an icon. The run carrying the glyph must carry
    the symbols font too.

  * `Glyph` IS A STRING. `new FontIcon { Glyph = "\uE713" }` compiles;
    a `char` does not.

  * WEIGHT AND STYLE DO NOTHING. The file has one face. `FontWeight="Bold"`
    or `FontStyle="Italic"` on a `FontIcon` will not produce a bold or
    slanted icon; the same outline is drawn. Use `FontSize` and
    `Foreground` for visual variation.

  * COVERAGE IS NOT CONTIGUOUS. 1,406 icon codepoints spread over 274
    separate runs between U+E001 and U+F8AE. A codepoint that "should"
    exist between two that do may not. Verify by rendering.

  * CHANGING THE SYMBOLS FONT LATE ONLY PARTLY TAKES. Assigning
    `FeatureConfiguration.Font.SymbolsFont` rewrites the
    `SymbolThemeFontFamily` theme resource, but `FontIcon` captures its
    default `FontFamily` in dependency-property metadata and `SymbolIcon`
    caches its font family in a static field -- both are established the
    first time those types are used. Set the font during application
    startup (or, better, through the build property), not after icons are
    already on screen.

  * DO NOT RENAME THE CONTENT FOLDER. The `ms-appx:///` URI resolves
    against the package's `lib/net10.0/CodeBrix.Platform.Fonts.Fluent/Fonts/`
    directory. It is a real path, not a decorative namespace.

  * THE ASSET MARKER MUST SIT BESIDE THE ASSEMBLY. Asset discovery looks
    for `<AssemblyName>.uprimarker` next to each referenced assembly. This
    matters if you ever vendor the font by hand instead of referencing the
    package -- copying only the `.ttf` will not make it reachable by URI.


WHAT THIS PACKAGE DOES NOT DO
=============================

  * IT IS NOT A TEXT FACE. There are no letters, digits or punctuation in
    it beyond a space. It cannot be an application's `DefaultTextFontFamily`
    and it is not a fallback for text.

  * IT EXPOSES NO MANAGED API. Zero public types. No helper that returns
    the font stream, the font path, or a glyph by name. If you want the
    bytes, read the file (COMPLETE EXAMPLES #9).

  * `ms-appx:///` DOES NOT RESOLVE OUTSIDE CodeBrix.Platform. Console
    apps, ASP.NET, test hosts and non-CodeBrix.Platform UI frameworks get
    nothing from the URI.

  * NO GLYPH NAMES, NO GLYPH INDEX. The font's `post` table is format 3.0,
    so nothing in the package can turn "printer icon" into a codepoint.
    The `Symbol` enum is the only name-based path, and it belongs to
    CodeBrix.Platform, not to this package.

  * NO WEIGHTS, NO ITALICS, NO VARIABLE AXES, NO STRETCH. One `Regular`
    face.

  * NO `.ttf.manifest`, NO `CODEBRIX-DEVELOP.json`, NO `buildTransitive`
    `.targets`. It does not participate in weight/style manifest
    resolution, it does not appear in application-template font pickers,
    and it prunes nothing at build time.

  * IT DOES NOT FALL BACK TO A SYSTEM ICON FONT. If a codepoint is not in
    this file you get that font's missing-glyph, by design.

  * IT DOES NOT DRAW ANYTHING BY ITSELF. Rendering, layout and theming all
    belong to CodeBrix.Platform.


WORKING EXAMPLES ON GITHUB
==========================

The test project is the executable specification of everything this
package promises:

  https://github.com/ellisnet/CodeBrix.Platform.Fonts.Fluent/tree/main/tests/CodeBrix.Platform.Fonts.Fluent.Tests

  ContentFilePresenceTests.cs
      The font is present, is the ONLY `.ttf` in the package, is of
      non-trivial size, and the `.uprimarker` asset marker exists and is
      empty.

  PropsFileTests.cs
      The `.props` exists, sets `CodeBrixPlatformDefaultSymbolsFontFamily`,
      points at
      `ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf`,
      offers the `$(CodeBrixFontsFluentDisableImport)` opt-out, and carries
      no residual upstream property name or path. Read this one first if
      you are unsure what the package registers.

  AssemblyMetadataTests.cs
      The assembly is named `CodeBrix.Platform.Fonts.Fluent`, targets
      .NET 10, loads by name, and exports no public types.

  TestAssetPaths.cs
      Where the three shipped files land relative to a build output --
      useful if you are writing your own asset-presence check.

A rendered end-to-end example lives in this repository's README.md, on
GitHub at:

  https://github.com/ellisnet/CodeBrix.Platform.Fonts.Fluent/blob/main/README.md


QUICK REFERENCE CARD
====================

    PackageId       CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever
    Assembly / ns   CodeBrix.Platform.Fonts.Fluent
    Target          .NET 10 or later
    License         Apache-2.0
    Dependencies    none
    Public types    none

    FONT URI (the package's real public API)
      ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf
      (`#Symbols` may be appended; same single family, redundant)

    ONE FACE        Symbols / Regular, 1410 glyphs, 2048 upem
    COVERAGE        1,406 icon codepoints, U+E001..U+F8AE (PUA), 274 runs
                    plus U+0020 space; nothing outside the PUA

    MSBUILD
      CodeBrixPlatformDefaultSymbolsFontFamily   set by the shipped .props
      CodeBrixFontsFluentDisableImport           set non-empty to opt out

    RUNTIME
      CodeBrix.Platform.UI.FeatureConfiguration.Font.SymbolsFont  (string)
      SymbolThemeFontFamily                       theme resource key

    XAML
      <SymbolIcon Symbol="Setting" />                     default font
      <FontIcon Glyph="&#xE713;" />                       default font
      <FontIcon FontFamily="ms-appx:///..." Glyph="&#xE713;" />
      <Run FontFamily="ms-appx:///..." Text="&#xE713;" /> glyph in text

    C#
      new SymbolIcon(Symbol.Save)
      new FontIcon { Glyph = "\uE713" }
      new FontIcon { FontFamily = new FontFamily(uri), Glyph = "\uE713" }

    GOTCHA OF RECORD
      Symbol.Setting == U+E115, but SymbolIcon draws U+E713.
      191 of the 197 Symbol members are remapped like this.
