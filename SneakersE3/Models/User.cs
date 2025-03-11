namespace Sneakers.API.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }
    public int CustomerNr { get; set; }
    public decimal Discount { get; set; }
    public string ApiKey { get; set; }
}