# CodeBrix.Platform.Fonts.Fluent

A redistribution of the Uno Platform Fluent icon font (Windows 11 iconography) packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.Fluent is a namespace-renamed, .NET 10 equivalent of `Uno.Fonts.Fluent` 2.8.1 — intended as a drop-in replacement for that package in CodeBrix.Platform-forked Uno applications, supplying the default symbols (icon) font used by `SymbolIcon`, `FontIcon`, and the `SymbolThemeFontFamily` theme resource.
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.Fluent supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.Platform.Fonts.Fluent supports:

* The Uno Platform Fluent icon font (`uno-fluentui-assets.ttf`) — the Windows 11 Fluent symbol set used for `SymbolIcon` / `FontIcon` glyphs.
* A `buildTransitive` MSBuild `.props` file that sets the `CodeBrixPlatformDefaultSymbolsFontFamily` property to this package's bundled font, so a CodeBrix.Platform app picks it up as the default symbols font automatically.
* The Uno `.uprimarker` file (renamed) so Uno-fork build pipelines discover the package as a UPRI-bearing font asset library and copy the font into the app at build time.

## Sample Code

### Reference the icon font from XAML (Uno-fork app)

```xml
<FontIcon FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf#Symbols"
          Glyph="&#xE713;" />
```

In a CodeBrix.Platform-forked app, simply adding the package reference is usually enough — the bundled `.props` sets it as the default symbols font, so `<SymbolIcon Symbol="Setting" />` and the `SymbolThemeFontFamily` theme resource resolve to this font without any per-element `FontFamily`.

### Migrating from Uno.Fonts.Fluent

Swap the NuGet reference from `Uno.Fonts.Fluent` to `CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever`. This package registers its font at `ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf` (rather than the upstream `ms-appx:///Uno.Fonts.Fluent/Fonts/...` path), so in a CodeBrix.Platform fork the hardcoded `SymbolThemeFontFamily` / default-symbols references must point at the CodeBrix path. The font file and glyph set are bit-for-bit identical to those Uno.Fonts.Fluent 2.8.1 shipped.

## License

The library code, the `.props` file, and the package wrapper are licensed under the Apache License, Version 2.0. see: https://en.wikipedia.org/wiki/Apache_License

The bundled `uno-fluentui-assets.ttf` font is redistributed from Uno.Fonts.Fluent 2.8.1, which Uno Platform publishes under the Apache License, Version 2.0. The combined NuGet package is published under the SPDX expression `Apache-2.0`.
