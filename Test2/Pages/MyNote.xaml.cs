using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Test2.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyNote : Page
    {
        public ObservableCollection<string> notes = new ObservableCollection<string>();
        IReadOnlyList<StorageFile> files = new List<StorageFile>();
        public MyNote()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetNote();
            NoteList.ItemsSource = notes;

        }
        public void GetNote()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            files = storageFolder.GetFilesAsync().GetAwaiter().GetResult();

            foreach (var file in files)
            {
                notes.Add(file.Name);
            }
        }

        private void NoteList_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedIndex = NoteList.SelectedIndex;
            var fileName = notes[selectedIndex];
            foreach (var file in files)
            {
                if (file.Name == fileName)
                {
                    var content = Windows.Storage.FileIO.ReadTextAsync(file).GetAwaiter().GetResult();
                    Note.Text = content;
                }
            }
        }

        private void CreateNoteFrame_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Note));
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedIndex = NoteList.SelectedIndex;
            var fileName = notes[selectedIndex];
            foreach (var file in files)
            {
                if (file.Name == fileName)
                {
                    Windows.Storage.FileIO.WriteLinesAsync(file, new[] { Note.Text }).GetAwaiter().GetResult();
                }

            }

        }
    }
}
