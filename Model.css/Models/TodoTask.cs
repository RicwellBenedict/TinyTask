using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiniTappsk.Models
{
    public enum RecurrenceType
    {
        None,
        Daily,
        Weekly,
        Monthly
    }

    public class TodoTask : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string? _notes;
        private DateTime _dueDate = DateTime.Today;
        private bool _isDone;
        private DateTime? _reminderDate;
        private bool _isRecurring;
        private RecurrenceType _recurrence;

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string? Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(); }
        }

        public bool IsDone
        {
            get => _isDone;
            set { _isDone = value; OnPropertyChanged(); }
        }

        public DateTime? ReminderDate
        {
            get => _reminderDate;
            set { _reminderDate = value; OnPropertyChanged(); }
        }

        public bool IsRecurring
        {
            get => _isRecurring;
            set { _isRecurring = value; OnPropertyChanged(); }
        }

        public RecurrenceType Recurrence
        {
            get => _recurrence;
            set { _recurrence = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
