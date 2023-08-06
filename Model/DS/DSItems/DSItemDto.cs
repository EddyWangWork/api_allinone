namespace demoAPI.Model.DS
{
    public class DSItemWithSubDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DSItemSubDto> DSItemSubDtos { get; set; }
        public DSItemWithSubDto()
        {
            DSItemSubDtos = new List<DSItemSubDto>();
        }
    }

    public class DSItemWithSubDtoV2
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public int SubID { get; set; }
    }

    public class DSItemWithSubDtoV3
    {
        public int itemID { get; set; }
        public int itemSubID { get; set; }
        public string Name { get; set; }

    }

    public class DSItemDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
