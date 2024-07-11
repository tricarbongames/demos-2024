namespace DeckManager.Domain;

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }
    public string DisplayName { get; }
    public int SortOrderAfterRebuild { get; }
    public Guid ShuffleOrder { get; set; }

    public Card(Suit suit, Rank rank, int sortOrderAfterRebuild)
    {
        Suit = suit;
        Rank = rank;
        SortOrderAfterRebuild = sortOrderAfterRebuild;
        DisplayName = DisplayNameHelper.GetDisplayName(suit, rank);
    }
}