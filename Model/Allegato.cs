namespace SFM.DBLite.Model
{
    public class Allegato
    {
        public int Progressivo { get; set; }

        [PrimaryKey]
        public int ProgressivoAllegato { get; set; }

        public string NomeFile { get; set; }
    }
}
