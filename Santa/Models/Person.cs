using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    public string Name { get; set; }

    public ICollection<Membership> Groups { get; set; }
    public ICollection<Group> OwnedGroups { get; set; }
}