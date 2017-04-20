namespace badmintonDataBase.Models
{
    public class Judge
    {
        public int JudgeId { get; set; }
        public string JudgeName { get; set; }
        public string JudgeSurName { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }
        public int YearOfBirth { get; set; }
    }
}