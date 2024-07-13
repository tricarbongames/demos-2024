using Xunit;
using Moq;

namespace DeckManager.Domain.Test;

public class DealerTest
{
    readonly Mock<ILogger<Dealer>> mockLogger = new();
    private const int ExpectedTotalCardCount = 52;

    [Fact]
    public void DealCard_DefaultSortOrder()
    {
        var uut = new Dealer(mockLogger.Object);

        var actualCard = uut.DealCard();

        Assert.Equal(default, actualCard.Suit);
        Assert.Equal(default, actualCard.Rank);
        AssertCardCounts(uut, 51, 1, 0);
    }

    [Fact]
    public void DealCard_ThenRebuild()
    {
        var uut = new Dealer(mockLogger.Object);

        uut.DealCard();
        uut.RebuildDeck();
        var actualTopCard = uut.Cheat();

        Assert.Equal(default, actualTopCard.Suit);
        Assert.Equal(default, actualTopCard.Rank);
        AssertCardCounts(uut, ExpectedTotalCardCount, 0, 0);
    }

    [Fact]
    public void DealCard_ThenSuffle_ThenOrder()
    {
        var uut = new Dealer(mockLogger.Object);

        uut.DealCard();
        uut.Shuffle();
        uut.Order();
        var actualTopCard = uut.Cheat();

        Assert.Equal(Suit.Spades, actualTopCard.Suit);
        Assert.Equal(Rank.Three, actualTopCard.Rank);
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
        AssertCardCounts(uut, 0, ExpectedTotalCardCount, 0);
    }

    [Fact]
    public void Shuffle_CompareToDefaultOrder()
    {
        var noShuffleDealer = new Dealer(mockLogger.Object);
        var shuffleDealer = new Dealer(mockLogger.Object);
        shuffleDealer.Shuffle();

        // If at least 10 cards are in a different placment after a shuffle, we can safely say a shuffle happened.
        const int differenceThreshold = 10;
        var differenceCount = 0;
        for (var i = 0; i < ExpectedTotalCardCount; i++)
        {
            if (noShuffleDealer.CardsInDeck[i].DisplayName != shuffleDealer.CardsInDeck[i].DisplayName)
            {
                differenceCount++;
            }
        }

        Assert.True(differenceCount > differenceThreshold);
        AssertCardCounts(shuffleDealer, ExpectedTotalCardCount, 0, 0);
    }

    [Fact]
    public void TryCut_SecondCardFromTop()
    {
        var uut = new Dealer(mockLogger.Object);

        uut.TryCut(1);
        var actualTopCard = uut.Cheat();

        Assert.Equal(Suit.Spades, actualTopCard.Suit);
        Assert.Equal(Rank.Four, actualTopCard.Rank);
        AssertCardCounts(uut, ExpectedTotalCardCount, 0, 0);
    }

    [Fact]
    public void TryCut_SplitIndexTooHigh()
    {
        var uut = new Dealer(mockLogger.Object);

        uut.TryCut(100);
        var actualTopCard = uut.Cheat();

        Assert.Equal(default, actualTopCard.Suit);
        Assert.Equal(default, actualTopCard.Rank);
        AssertCardCounts(uut, ExpectedTotalCardCount, 0, 0);
    }

    [Fact]
    public void Cheat_DefaultSortOrder()
    {
        var uut = new Dealer(mockLogger.Object);

        var actualCard = uut.Cheat();

        Assert.Equal(default, actualCard.Suit);
        Assert.Equal(default, actualCard.Rank);
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