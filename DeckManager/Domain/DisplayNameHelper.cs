namespace DeckManager.Domain;

public static class DisplayNameHelper
{
    public static Dictionary<Rank, string> RankDisplay = new Dictionary<Rank, string>
    {
        {Rank.Two, "2"},
        {Rank.Three, "3"},
        {Rank.Four, "4"},
        {Rank.Five, "5"},
        {Rank.Six, "6"},
        {Rank.Seven, "7"},
        {Rank.Eight, "8"},
        {Rank.Nine, "9"},
        {Rank.Ten, "10"},
        {Rank.Jack, Rank.Jack.ToString()},
        {Rank.Queen, Rank.Queen.ToString()},
        {Rank.King, Rank.King.ToString()},
        {Rank.Ace, Rank.Ace.ToString()},
    };

    public static string GetDisplayName(Suit suit, Rank rank)
    {
        return $"{RankDisplay[rank]} of {suit}";
    }
}