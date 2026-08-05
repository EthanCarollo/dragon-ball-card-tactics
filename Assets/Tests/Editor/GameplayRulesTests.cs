using NUnit.Framework;
using UnityEngine;

public class GameplayRulesTests
{
    [TestCase(0, 3)]
    [TestCase(1, 3)]
    [TestCase(3, 3)]
    [TestCase(4, 4)]
    [TestCase(7, 5)]
    [TestCase(10, 6)]
    [TestCase(100, 6)]
    public void RewardManaCostUnlocksProgressively(int completedRounds, int expectedMaximumManaCost)
    {
        Assert.That(
            CardRewardRules.GetMaximumRewardManaCost(completedRounds),
            Is.EqualTo(expectedMaximumManaCost));
    }

    [Test]
    public void TransformationIsOnlyAvailableAtItsFirstProgressionCost()
    {
        Assert.That(CardRewardRules.IsTransformationProgressionAvailable(3, 3), Is.True);
        Assert.That(CardRewardRules.IsTransformationProgressionAvailable(4, 3), Is.False);
        Assert.That(CardRewardRules.IsTransformationProgressionAvailable(3, int.MaxValue), Is.False);
    }

    [Test]
    public void PathfindingAvoidsOccupiedTiles()
    {
        var grid = new BoardObject[3, 3];
        grid[1, 1] = new TestBoardObject();

        var path = new AStarPathfinding(grid).FindPath(new Vector2Int(0, 1), new Vector2Int(2, 1));

        Assert.That(path, Is.Not.Null);
        CollectionAssert.DoesNotContain(path, new Vector2Int(1, 1));
        Assert.That(path.Count, Is.EqualTo(4));
    }

    [Test]
    public void PathfindingReturnsNullWhenTargetIsBlockedOff()
    {
        var grid = new BoardObject[3, 3];
        grid[1, 0] = new TestBoardObject();
        grid[1, 1] = new TestBoardObject();
        grid[1, 2] = new TestBoardObject();

        var path = new AStarPathfinding(grid).FindPath(new Vector2Int(0, 1), new Vector2Int(2, 1));

        Assert.That(path, Is.Null);
    }

    [Test]
    public void MovingCharacterReleasesItsPreviousTile()
    {
        var grid = new BoardObject[2, 1];
        var character = new TestBoardObject();
        grid[0, 0] = character;

        Assert.That(BoardUtils.MoveCharacter(grid, character, new Vector2Int(1, 0)), Is.True);
        Assert.That(grid[0, 0], Is.Null);
        Assert.That(grid[1, 0], Is.SameAs(character));
    }

    [Test]
    public void SwappingCharactersPreservesBothBoardOccupants()
    {
        var grid = new BoardObject[2, 1];
        var firstCharacter = new TestBoardObject();
        var secondCharacter = new TestBoardObject();
        grid[0, 0] = firstCharacter;
        grid[1, 0] = secondCharacter;

        Assert.That(BoardUtils.SwapCharacters(grid, firstCharacter, new Vector2Int(1, 0)), Is.True);
        Assert.That(grid[0, 0], Is.SameAs(secondCharacter));
        Assert.That(grid[1, 0], Is.SameAs(firstCharacter));
    }

    [Test]
    public void PathfindingRejectsOutOfBoundsPositions()
    {
        var grid = new BoardObject[2, 2];

        Assert.That(
            new AStarPathfinding(grid).FindPath(new Vector2Int(-1, 0), new Vector2Int(1, 1)),
            Is.Null);
    }

    private sealed class TestBoardObject : BoardObject
    {
        public override BoardObject Clone()
        {
            return new TestBoardObject();
        }

        public override void UpdateUi()
        {
        }

        public override void Update()
        {
        }
    }
}
