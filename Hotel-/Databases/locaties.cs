namespace Hotel_.Databases
{
    public class locaties
    {
        public int Id { get; set; }
        public string? Stad { get; set; }
        public string? Date { get; set; }
        public string? Kamers { get; set; }
       
    }

    public class makers
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public string? Informatie { get; internal set; }
        public string? Leeftijd { get; set; }

    }
}
