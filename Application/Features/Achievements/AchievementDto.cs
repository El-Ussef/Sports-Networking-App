namespace Application.Features.Achievements;

public class AchievementDto
{

    public int Id { get; set; }
    
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
    
    public string Location { get; set; } = default!; // to be deleted 

    //public string CompetitionPlace { get; set; } = default!;

    public Guid RefId { get; set; } = default!;

    public DateTime DateAchieved { get; set; } = default!;

}
