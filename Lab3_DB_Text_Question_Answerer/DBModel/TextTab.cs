using System.Collections.ObjectModel;

namespace DBModel
{
    public class TextTab
    {
        public int TabId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public virtual ICollection<QuestionAndAnswer> QuestionAndAnswer { get; set; } = new ObservableCollection<QuestionAndAnswer>();
    }
}