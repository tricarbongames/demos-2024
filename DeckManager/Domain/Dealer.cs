namespace DeckManager.Domain;

public interface IDealer
{
    public Card DealCard();
    public void Shuffle();
    public void TryDiscard(Card card);
    public void TryCut(int splitIndex);
    public void Order();
    public void RebuildDeck();
    public Card Cheat();
}

public class Dealer : IDealer
{
    private readonly ILogger<Dealer> _logger;
    public List<Card> CardsInDeck { get; private set; } = [];
    public List<Card> DealtCards { get; private set; } = [];
    public List<Card> DiscardedCards { get; private set; } = [];

    public Dealer(ILogger<Dealer> logger)
    {
        _logger = logger;
        RebuildDeck();
    }

    public Card DealCard()
    {
        var dealtCard = CardsInDeck.First();
        CardsInDeck.Remove(dealtCard);
        DealtCards.Add(dealtCard);

        return dealtCard;
    }

    // Shuffle assigns a random guid to each card in the deck, then sorts by the guids.
    public void Shuffle()
    {
        foreach (var card in CardsInDeck)
        {
            card.ShuffleOrder = Guid.NewGuid();
        }

        CardsInDeck = [.. CardsInDeck.OrderBy(c => c.ShuffleOrder)];
    }

    public void TryDiscard(Card card)
    {
        // We can treat Suit + Rank as a composite key, meaning only one card could be removed at a time.
        var discardCount = DealtCards.RemoveAll(c => c.Suit == card.Suit && c.Rank == card.Rank);

        if (discardCount == 0)
        {
            _logger.Log(LogLevel.Warning, $"Attempted to discard {card.DisplayName} that was not dealt");
            return;
        }

        DiscardedCards.Add(card);
    }

    public void TryCut(int splitIndex)
    {
        if (splitIndex < 0 || splitIndex >= CardsInDeck.Count - 1)
        {
            _logger.Log(LogLevel.Warning, $"Split index of {splitIndex} is invalid for a deck size of {CardsInDeck.Count}");
            return;
        }

        var topStack = CardsInDeck[..splitIndex];
        var bottomStack = CardsInDeck.Slice(splitIndex + 1, CardsInDeck.Count - 1);
        CardsInDeck = bottomStack;
        CardsInDeck.AddRange(topStack);
    }

    public void Order()
    {
        CardsInDeck = [.. CardsInDeck.OrderBy(c => c.SortOrderAfterRebuild)];
    }

    public void RebuildDeck()
    {
        CardsInDeck = [];
        DealtCards = [];
        DiscardedCards = [];
        var sortOrder = 0;

        foreach (var suit in Enum.GetValues<Suit>())
        {
            foreach (var rank in Enum.GetValues<Rank>())
            {
                CardsInDeck.Add(new Card(suit, rank, sortOrder));
                sortOrder++;
            }
        }
    }

    public Card Cheat()
    {
        return CardsInDeck.First();
    }
}