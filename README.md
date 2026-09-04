# CodeBrix.Platform.Fonts.Fluent

A redistribution of the Fluent icon font (Windows 11 iconography) packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.Fluent supplies the default symbols (icon) font used by `SymbolIcon`, `FontIcon` and the `SymbolThemeFontFamily` theme resource in CodeBrix.Platform applications.
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.Fluent supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## Installation

```
dotnet add package CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever
```

Note that the NuGet package ID and the assembly name are different - there is no package named plain `CodeBrix.Platform.Fonts.Fluent`:

* NuGet package ID: `CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever`
* Assembly and namespace: `CodeBrix.Platform.Fonts.Fluent`

This is a content-files font package: it contributes no managed types, so there is nothing to `using`. It brings in no other NuGet package.

## CodeBrix.Platform.Fonts.Fluent supports:

* The Fluent icon font (`uno-fluentui-assets.ttf`) — the Windows 11 Fluent symbol set used for `SymbolIcon` / `FontIcon` glyphs.
* A `buildTransitive` MSBuild `.props` file that sets the `CodeBrixPlatformDefaultSymbolsFontFamily` property to this package's bundled font, so a CodeBrix.Platform app picks it up as the default symbols font automatically. Set `CodeBrixFontsFluentDisableImport` to any value in the consuming project to opt out of that default.
* The CodeBrix `.uprimarker` file so CodeBrix.Platform build pipelines discover the package as a UPRI-bearing font asset library and copy the font into the app at build time.

## Sample Code

### Reference the icon font from XAML (CodeBrix.Platform app)

```xml
<FontIcon FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf#Symbols"
          Glyph="&#xE713;" />
```

In a CodeBrix.Platform app, simply adding the package reference is usually enough — the bundled `.props` sets it as the default symbols font, so `<SymbolIcon Symbol="Setting" />` and the `SymbolThemeFontFamily` theme resource resolve to this font without any per-element `FontFamily`.

An app that overrides `SymbolThemeFontFamily` (or the default-symbols font family) by hand must point that override at the same `ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf` path shown above, or the hand-written value wins and the bundled font is never used.

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete reference and usage guide written for AI coding agents - point your agent at that file when it is writing code against this library.

Additional usage examples are available in the `CodeBrix.Platform.Fonts.Fluent.Tests` project:
https://github.com/ellisnet/CodeBrix.Platform.Fonts.Fluent/tree/main/tests/CodeBrix.Platform.Fonts.Fluent.Tests

## License

The library code, the `.props` file, and the package wrapper are licensed under the Apache License, Version 2.0 - see the
[LICENSE](https://github.com/ellisnet/CodeBrix.Platform.Fonts.Fluent/blob/main/LICENSE) file.

The bundled `uno-fluentui-assets.ttf` font is also distributed under the Apache License, Version 2.0. The combined NuGet package is published under the SPDX expression `Apache-2.0`.

For licensing and provenance information about the open source code and font included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.Platform.Fonts.Fluent/blob/main/THIRD-PARTY-NOTICES.txt).
