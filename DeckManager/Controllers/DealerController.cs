using Microsoft.AspNetCore.Mvc;
using DeckManager.Domain;

namespace DeckManager.Controllers;

[ApiController]
[Route("[controller]")]
public class DealerController : ControllerBase
{
    private readonly IDealer _dealer;

    public DealerController(IDealer dealer)
    {
        _dealer = dealer;
    }

    [HttpGet("DealCard")]
    public Card DealCard()
    {
        /* 
         I went back and forth about what to do when the deck is empty.
         The current implementation will result in a 204 no content when that is the case.
         This might not be optimal depending on front end use-cases and implementation.
         If this were a real production service, I would follow-up with stakeholders to further clarify the requirements.
        */
        return _dealer.DealCard();
    }

    [HttpGet("Shuffle")]
    public void Shuffle()
    {
        _dealer.Shuffle();
    }

    [HttpPost("Discard")]
    public void TryDiscard([FromBody] Card card)
    {
        _dealer.TryDiscard(card);
    }

    [HttpGet("Cut")]
    public void TryCut([FromQuery] int splitIndex)
    {
        _dealer.TryCut(splitIndex);
    }

    [HttpGet("Order")]
    public void Order()
    {
        _dealer.Order();
    }

    [HttpGet("RebuildDeck")]
    public void RebuildDeck()
    {
        _dealer.RebuildDeck();
    }

    [HttpGet("Cheat")]
    public Card Cheat()
    {
        return _dealer.Cheat();
    }
}