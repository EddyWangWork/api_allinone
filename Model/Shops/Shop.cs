﻿namespace demoAPI.Model
{
    public class Shop
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Types { get; set; }
        public string Location { get; set; }        
        public string? Remark { get; set; }
        public string? Comment { get; set; }
        public int Star { get; set; }
        public bool IsVisited { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}
