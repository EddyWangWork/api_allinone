﻿namespace demoAPI.Model
{
    public class TodolistDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class TodolistDtoTemp : TodolistDto
    {
        public DateTime DoneDate { get; set; }
        public int TodolistDoneID { get; set; }
    }
}