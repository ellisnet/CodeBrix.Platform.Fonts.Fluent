using System.IO;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.Fluent.Tests;

public class PropsFileTests
{
    [Fact]
    public void Props_file_is_present()
        => File.Exists(TestAssetPaths.PropsFilePath).Should().BeTrue();

    [Fact]
    public void Props_file_sets_default_symbols_font_family()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.PropsFilePath);

        //Assert
        content.Should().Contain("<CodeBrixPlatformDefaultSymbolsFontFamily>");
    }

    [Fact]
    public void Props_file_points_at_codebrix_font_path()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.PropsFilePath);

        //Assert
        content.Should().Contain("ms-appx:///CodeBrix.Platform.Fonts.Fluent/Fonts/uno-fluentui-assets.ttf");
    }

    [Fact]
    public void Props_file_contains_no_residual_uno_msappx_path()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.PropsFilePath);

        //Assert
        content.Should().NotContain("ms-appx:///Uno.Fonts.Fluent");
    }

    [Fact]
    public void Props_file_provides_disable_import_opt_out()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.PropsFilePath);

        //Assert
        content.Should().Contain("$(CodeBrixFontsFluentDisableImport)");
    }

    [Fact]
    public void Props_file_uses_the_codebrix_symbols_font_property()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.PropsFilePath);

        //Assert
        // The CodeBrix.Platform XAML source generator reads
        // CodeBrixPlatformDefaultSymbolsFontFamily, not the upstream Uno name.
        content.Should().NotContain("<UnoPlatformDefaultSymbolsFontFamily>");
    }
}
