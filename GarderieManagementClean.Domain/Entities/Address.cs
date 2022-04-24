namespace GarderieManagementClean.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }



        public string Ville { get; set; }

        public string Rue { get; set; }

        public string Province { get; set; }

        public string CodePostal { get; set; }

        public string Telephone { get; set; }



        public int GarderieId { get; set; }

        public virtual Garderie Garderie { get; set; }
    }
}
