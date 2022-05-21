using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    public partial class FormRecordsViewer : Form
    {
        private readonly Game.ReadOnlyRecord[] _records;

        public event Action<Game.ReadOnlyRecord> Selected;

        public FormRecordsViewer()
        {
            InitializeComponent();

            _records = Save.DeserializeDirectory(Game.RecordsDirectoryName).Cast<Game.ReadOnlyRecord>().ToArray();
            listBoxRecords.Items.AddRange(_records.Select(r => r.CreateShort()).ToArray());
        }

        private void ListBoxRecords_SelectedIndexChanged(object sender, EventArgs e) =>
            Selected?.Invoke(listBoxRecords.SelectedIndex == ListBox.NoMatches 
                ? null 
                : _records[listBoxRecords.SelectedIndex]);

        private void FormRecordsViewer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: 
                    Close(); 
                    return;

                case Keys.Delete:
                    if (listBoxRecords.SelectedIndex == ListBox.NoMatches) return;
                    Save.DeleteFileInDirectory(Game.RecordsDirectoryName, listBoxRecords.SelectedIndex);
                    listBoxRecords.Items.RemoveAt(listBoxRecords.SelectedIndex);
                    return;
            }
        }
    }
}
