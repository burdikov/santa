using System.ComponentModel.DataAnnotations.Schema;

public class Membership
{
    public int GroupId { get; set; }
    public long PersonId { get; set; }
    public Group Group { get; set; }
    public Person Person { get; set; }
    public long? GifteeId { get; set; }
    public Person Giftee { get; set; }
    public string Message { get; set; }
}