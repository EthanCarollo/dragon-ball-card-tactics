using NUnit.Framework;

public class PlayerLevelTests
{
    [Test]
    public void AddExperienceLevelsUpAtExactThreshold()
    {
        var level = new PlayerLevel();

        level.AddExperience(4);

        Assert.That(level.CurrentLevel, Is.EqualTo(2));
        Assert.That(level.CurrentExperience, Is.EqualTo(0));
        Assert.That(level.maxUnit, Is.EqualTo(5));
    }

    [Test]
    public void AddExperienceCarriesOverflowToNextLevel()
    {
        var level = new PlayerLevel();

        level.AddExperience(9);

        Assert.That(level.CurrentLevel, Is.EqualTo(2));
        Assert.That(level.CurrentExperience, Is.EqualTo(5));
    }

    [Test]
    public void AddExperienceIgnoresNonPositiveValues()
    {
        var level = new PlayerLevel();

        level.AddExperience(-1);

        Assert.That(level.CurrentLevel, Is.EqualTo(1));
        Assert.That(level.CurrentExperience, Is.EqualTo(0));
    }

    [Test]
    public void AddExperienceCapsAtMaximumLevel()
    {
        var level = new PlayerLevel();

        level.AddExperience(10000);

        Assert.That(level.CurrentLevel, Is.EqualTo(level.MaxLevel));
        Assert.That(level.CurrentExperience, Is.EqualTo(100));
        Assert.That(level.MaxExperience, Is.EqualTo(100));
    }
}
