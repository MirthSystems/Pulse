namespace MirthSystems.Pulse.Core.Entities
{
    /// <summary>
    /// Holds the results of address standardization
    /// </summary>
    public class StandardizedAddress
    {
        public string? Building { get; set; }
        public string? HouseNum { get; set; }
        public string? PreDir { get; set; }
        public string? Qual { get; set; }
        public string? PreType { get; set; }
        public string? Name { get; set; }
        public string? SufType { get; set; }
        public string? SufDir { get; set; }
        public string? RuralRoute { get; set; }
        public string? Extra { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Postcode { get; set; }
        public string? Box { get; set; }
        public string? Unit { get; set; }
    }
}
