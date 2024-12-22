using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModelsMP;

internal class NotesViewModelMP : IQueryAttributable
{
    public ObservableCollection<NoteViewModelMP> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public NotesViewModelMP()
    {
        AllNotes = new ObservableCollection<NoteViewModelMP>(NoteMP.LoadAll().Select(n => new NoteViewModelMP(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModelMP>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(NoteViewModelMP note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModelMP matchedNote = AllNotes.FirstOrDefault(n => n.Identifier == noteId);

            // Si la nota existe, elimínala
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            NoteViewModelMP matchedNote = AllNotes.FirstOrDefault(n => n.Identifier == noteId);

            // Si la nota se encuentra, actualízala
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // Si la nota no se encuentra, agrégala como nueva al inicio
            else
                AllNotes.Insert(0, new NoteViewModelMP(Models.NoteMP.Load(noteId)));
        }
    }

}
