namespace GUI.Files.Explorer
{
    public partial class FileExplorerForm : Form
    {
        public FileExplorerForm()
        {
            InitializeComponent();
        }

        private void btn_OpenExplorer_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog()
            {
                Description = "Select path.."
            })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    webbrowser.Url = new Uri(fbd.SelectedPath);
                    txtb_Path.Text = fbd.SelectedPath;
                }
            }
        }

        private void btn_GoBack_Click(object sender, EventArgs e)
        {
            if (webbrowser.CanGoBack)
            {
                webbrowser.GoBack();
            }
        }

        private void btn_GoForward_Click(object sender, EventArgs e)
        {
            if (webbrowser.CanGoForward)
            {
                webbrowser.GoForward();
            }
        }

        private void FileExplorerForm_Load(object sender, EventArgs e)
        {

        }
    }
}