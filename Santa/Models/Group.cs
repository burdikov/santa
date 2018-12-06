using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Group
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Pass { get; set; }

    public bool Joinable { get; set; }

    public Person Owner { get; set; }

    public ICollection<Membership> Members { get; set; }
}