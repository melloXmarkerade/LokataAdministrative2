namespace LokataAdministrative2.Models.Citation
{
    public class ViolationFeeDto : BaseDto
    {
        public string? Violation { get; set; }
        public double FirstOffense { get; set; }
        public double SecondOffense { get; set; }
        public double ThirdOffense { get; set; }
    }
}
