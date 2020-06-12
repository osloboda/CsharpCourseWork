using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace WindowsFormsApplication309
{
    public partial class MainForm : Form
    {
        private Game game;
        private PictureBox[,] pbs = new PictureBox[3,3];
        private Image Cross;
        private Image Nought;

        public MainForm()
        {
            InitializeComponent();

            Init();

            game = new Game();
            Build(game);
        }

        void Init()
        {
            Cross = Image.FromStream(new WebClient().OpenRead("https://icon-icons.com/downloadimage.php?id=106506&root=1524/PNG/128/&file=x_106506.png"));
            Nought = Image.FromStream(new WebClient().OpenRead("https://icon-icons.com/downloadimage.php?id=80174&root=1130/PNG/512/&file=circle_80174.png"));

            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                pbs[i, j] = new PictureBox { Parent = this, Size = new Size(106, 106), Top = i * 106, Left = j * 106, BorderStyle = BorderStyle.FixedSingle, Tag = new Point(i, j), Cursor = Cursors.Hand, SizeMode = PictureBoxSizeMode.StretchImage };
                pbs[i, j].Click += pb_Click;
            }

            new Button { Parent = this, Top = 335, Left = 122, Text = "Очистити" }.Click += delegate { game = new Game(); Build(game); };
        }

        private void Build(Game game)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    pbs[i, j].Image = game.Items[i, j] == FieldState.Cross ? Cross : (game.Items[i, j] == FieldState.Nought ? Nought : null);
        }

        void pb_Click(object sender, EventArgs e)
        {
            game.MakeMove((Point) (sender as Control).Tag);
            //Ходимо у відповідь
            if (!game.Winned)
            {
                var m = new AI().GetBestMove(game);
                if(m != null)
                    game.MakeMove(m.P);
            }

            Build(game);

            if(game.Winned)
                MessageBox.Show(string.Format("{0} - переможець!", game.CurrentPlayer == 0 ? "Гравець" : "Штучний інтелект"));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
    }
}