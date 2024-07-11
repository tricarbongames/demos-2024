using Xunit;
using Moq;

namespace DeckManager.Domain.Test;

// Trying to give you an idea of how I would test, not going for 100% coverage here.
public class DealerTest
{
    readonly Mock<ILogger<Dealer>> mockLogger = new();
    private const int ExpectedTotalCardCount = 52;

    [Fact]
    public void DealCard_NotShuffled()
    {
        var uut = new Dealer(mockLogger.Object);
        var expectedCard = uut.CardsInDeck[0];

        var actualCard = uut.DealCard();

        Assert.Equal(expectedCard, actualCard);
        AssertCardCounts(uut, 51, 1, 0);
    }

    [Fact]
    public void DealCard_ThenDiscardCard()
    {
        var uut = new Dealer(mockLogger.Object);

        var toBeDiscarded = uut.DealCard();
        uut.TryDiscard(toBeDiscarded);

        Assert.Equal(uut.DiscardedCards[0], toBeDiscarded);
        AssertCardCounts(uut, 51, 0, 1);
    }

    [Fact]
    public void Cheat_NotShuffled()
    {
        var uut = new Dealer(mockLogger.Object);
        var expectedCard = uut.CardsInDeck[0];

        var actualCard = uut.Cheat();

        Assert.Equal(expectedCard, actualCard);
        AssertCardCounts(uut, ExpectedTotalCardCount, 0, 0);
    }

    private static void AssertCardCounts(Dealer dealer, int inDeck, int dealt, int discarded)
    {
        Assert.Equal(inDeck, dealer.CardsInDeck.Count);
        Assert.Equal(dealt, dealer.DealtCards.Count);
        Assert.Equal(discarded, dealer.DiscardedCards.Count);

        var actualTotalCardCount = inDeck + dealt + discarded;
        Assert.Equal(ExpectedTotalCardCount, actualTotalCardCount);
    }
}