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

    // There is always a debate about if it is okay to test more than one use-case per test.
    // Since the setup is the same, I think the practical choice is to test all the empty deck use-cases in this one test. 
    [Fact]
    public void TryDeckActions_WhenDeckEmpty()
    {
        var uut = new Dealer(mockLogger.Object);

        var actualDealtCard = uut.DealCard();
        for (var i = 0; i < ExpectedTotalCardCount; i++)
        {
            actualDealtCard = uut.DealCard();
        }
        var actualCheatCard = uut.Cheat();
        // Call other methods that rely on the deck to make sure they don't blow up when the deck is empty.
        uut.Order();
        uut.TryCut(1);
        uut.Shuffle();

        Assert.Null(actualDealtCard);
        Assert.Null(actualCheatCard);
        AssertCardCounts(uut, 0, 52, 0);
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