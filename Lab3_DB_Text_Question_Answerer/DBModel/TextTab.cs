﻿using System.Collections.ObjectModel;

namespace DBModel
{
    public class TextTab
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string LatestQuestion { get; set; }
        public string LatestAnswer { get; set; }
        public List<QuestionAndAnswer> QuestionAndAnswer { get; set; } = new List<QuestionAndAnswer>();
    }
}