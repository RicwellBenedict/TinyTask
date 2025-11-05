using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MiniTappsk.Models;
using MiniTappsk.Services;

namespace MiniTappsk.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TodoTask> AllTasks { get; } = new();

        // Felder für den "Neuer Task" Screen
        private string _newTitle = string.Empty;
        private string? _newNotes;
        private DateTime? _newDueDate = DateTime.Today;
        private DateTime? _newReminderDate;
        private bool _newIsRecurring;
        private RecurrenceType _newRecurrenceType = RecurrenceType.None;

        public MainViewModel()
        {
            AllTasks.CollectionChanged += AllTasks_CollectionChanged;

            // Laden aus JSON
            var loaded = TaskStorage.Load();
            foreach (var task in loaded)
            {
                AttachTask(task);
                AllTasks.Add(task);
            }

            OnTasksChanged();
        }

        #region New Task Properties

        public string NewTitle
        {
            get => _newTitle;
            set { _newTitle = value; OnPropertyChanged(); }
        }

        public string? NewNotes
        {
            get => _newNotes;
            set { _newNotes = value; OnPropertyChanged(); }
        }

        public DateTime? NewDueDate
        {
            get => _newDueDate;
            set { _newDueDate = value; OnPropertyChanged(); }
        }

        public DateTime? NewReminderDate
        {
            get => _newReminderDate;
            set { _newReminderDate = value; OnPropertyChanged(); }
        }

        public bool NewIsRecurring
        {
            get => _newIsRecurring;
            set { _newIsRecurring = value; OnPropertyChanged(); }
        }

        public RecurrenceType NewRecurrenceType
        {
            get => _newRecurrenceType;
            set { _newRecurrenceType = value; OnPropertyChanged(); }
        }

        public Array RecurrenceTypes => Enum.GetValues(typeof(RecurrenceType));

        public void StartNewTask()
        {
            NewTitle = string.Empty;
            NewNotes = string.Empty;
            NewDueDate = DateTime.Today;
            NewReminderDate = null;
            NewIsRecurring = false;
            NewRecurrenceType = RecurrenceType.None;
        }

        public bool AddNewTask()
        {
            if (string.IsNullOrWhiteSpace(NewTitle))
                return false;

            var task = new TodoTask
            {
                Title = NewTitle,
                Notes = NewNotes,
                DueDate = NewDueDate ?? DateTime.Today,
                ReminderDate = NewReminderDate,
                IsRecurring = NewIsRecurring,
                Recurrence = NewRecurrenceType,
                IsDone = false
            };

            AllTasks.Add(task);
            StartNewTask(); // Felder resetten
            return true;
        }

        #endregion

        #region Gefilterte Views

        public ObservableCollection<TodoTask> TodayTasks =>
            new ObservableCollection<TodoTask>(
                AllTasks.Where(t => t.DueDate.Date == DateTime.Today));

        public ObservableCollection<TodoTask> TomorrowTasks =>
            new ObservableCollection<TodoTask>(
                AllTasks.Where(t => t.DueDate.Date == DateTime.Today.AddDays(1)));

        public ObservableCollection<TodoTask> ThisWeekTasks =>
            new ObservableCollection<TodoTask>(
                AllTasks.Where(t =>
                {
                    var d = t.DueDate.Date;
                    var today = DateTime.Today;
                    var end = today.AddDays(7);
                    return d > today.AddDays(1) && d <= end;
                }));

        #endregion

        #region intern: Änderungen beobachten & speichern

        private void AllTasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TodoTask t in e.NewItems)
                    AttachTask(t);
            }

            if (e.OldItems != null)
            {
                foreach (TodoTask t in e.OldItems)
                    DetachTask(t);
            }

            OnTasksChanged();
        }

        private void AttachTask(TodoTask task)
        {
            task.PropertyChanged += Task_PropertyChanged;
        }

        private void DetachTask(TodoTask task)
        {
            task.PropertyChanged -= Task_PropertyChanged;
        }

        private void Task_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnTasksChanged();
        }

        private void OnTasksChanged()
        {
            OnPropertyChanged(nameof(TodayTasks));
            OnPropertyChanged(nameof(TomorrowTasks));
            OnPropertyChanged(nameof(ThisWeekTasks));

            TaskStorage.Save(AllTasks);
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
