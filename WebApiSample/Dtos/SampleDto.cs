namespace WebApiSample.Dtos
{
    public record SampleDto(string Constr1, string Constr2)
    {
        public required string Req1 { get; set; }
    }
}
