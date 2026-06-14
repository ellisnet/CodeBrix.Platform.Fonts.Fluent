using System;
using System.IO;

namespace CodeBrix.Platform.Fonts.Fluent.Tests;

internal static class TestAssetPaths
{
    public static string TestAssetsRoot { get; } =
        Path.Combine(AppContext.BaseDirectory, "TestAssets");

    public static string FontsFolder { get; } =
        Path.Combine(TestAssetsRoot, "Fonts");

    public static string FontPath { get; } =
        Path.Combine(FontsFolder, "uno-fluentui-assets.ttf");

    public static string UprimarkerPath { get; } =
        Path.Combine(TestAssetsRoot, "CodeBrix.Platform.Fonts.Fluent.uprimarker");

    public static string PropsFilePath { get; } =
        Path.Combine(TestAssetsRoot, "buildTransitive", "net10.0", "CodeBrix.Platform.Fonts.Fluent.ApacheLicenseForever.props");
}
