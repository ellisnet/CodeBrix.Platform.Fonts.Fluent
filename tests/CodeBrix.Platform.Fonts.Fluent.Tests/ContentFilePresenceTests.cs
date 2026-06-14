using System.IO;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.Fluent.Tests;

public class ContentFilePresenceTests
{
    [Fact]
    public void Fluent_font_is_present()
        => File.Exists(TestAssetPaths.FontPath).Should().BeTrue();

    [Fact]
    public void Exactly_one_ttf_is_shipped()
    {
        //Arrange/Act
        var ttfFiles = Directory.GetFiles(TestAssetPaths.FontsFolder, "*.ttf");

        //Assert
        ttfFiles.Length.Should().Be(1);
    }

    [Fact]
    public void Fluent_font_is_non_trivial_size()
    {
        //Arrange
        var info = new FileInfo(TestAssetPaths.FontPath);

        //Assert
        info.Length.Should().BeGreaterThan(100_000L);
    }

    [Fact]
    public void Uprimarker_file_is_present()
        => File.Exists(TestAssetPaths.UprimarkerPath).Should().BeTrue();

    [Fact]
    public void Uprimarker_file_is_empty()
    {
        //Arrange
        var info = new FileInfo(TestAssetPaths.UprimarkerPath);

        //Assert
        info.Length.Should().Be(0L);
    }
}
