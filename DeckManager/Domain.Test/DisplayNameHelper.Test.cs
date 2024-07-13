using Xunit;

namespace DeckManager.Domain.Test;

public class DisplayNameHelperTest
{
    [Theory]
    [InlineData(Suit.Spades)]
    [InlineData(Suit.Hearts)]
    [InlineData(Suit.Clubs)]
    [InlineData(Suit.Diamonds)]
    public static void AssertDisplayNames(Suit suit)
    {
        Assert.Equal($"2 of {suit}", new Card(suit, Rank.Two, default).DisplayName);
        Assert.Equal($"3 of {suit}", new Card(suit, Rank.Three, default).DisplayName);
        Assert.Equal($"4 of {suit}", new Card(suit, Rank.Four, default).DisplayName);
        Assert.Equal($"5 of {suit}", new Card(suit, Rank.Five, default).DisplayName);
        Assert.Equal($"6 of {suit}", new Card(suit, Rank.Six, default).DisplayName);
        Assert.Equal($"7 of {suit}", new Card(suit, Rank.Seven, default).DisplayName);
        Assert.Equal($"8 of {suit}", new Card(suit, Rank.Eight, default).DisplayName);
        Assert.Equal($"9 of {suit}", new Card(suit, Rank.Nine, default).DisplayName);
        Assert.Equal($"10 of {suit}", new Card(suit, Rank.Ten, default).DisplayName);
        Assert.Equal($"Jack of {suit}", new Card(suit, Rank.Jack, default).DisplayName);
        Assert.Equal($"Queen of {suit}", new Card(suit, Rank.Queen, default).DisplayName);
        Assert.Equal($"King of {suit}", new Card(suit, Rank.King, default).DisplayName);
        Assert.Equal($"Ace of {suit}", new Card(suit, Rank.Ace, default).DisplayName);
    }
}