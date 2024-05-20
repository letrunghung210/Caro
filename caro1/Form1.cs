using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace caro1
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        SocketManager socket;
        #endregion
        public Form1()
        {
            InitializeComponent();
            ChessBoard = new ChessBoardManager(panel1, textBox1, pictureBox2);
            ChessBoard.EndedUGame += ChessBoard_EndedUGame; ;
            ChessBoard.PlayerUMarked += ChessBoard_PlayerUMarked;
            progressBar1.Step = Cons.COOL_DOWN_STEP;
            progressBar1.Maximum = Cons.COOL_DOWN_TIME;
            progressBar1.Value = 0;
            timer1.Interval = Cons.COOL_DOWN_INTERVAL;
            socket = new SocketManager();
            NewGame();
        }
        #region Methods
        void EndGame()
        {
            timer1.Stop();
            panel1.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            MessageBox.Show("Kết thúc");
        }
        void NewGame()
        {
            progressBar1.Value = 0;
            timer1.Stop();
            undoToolStripMenuItem.Enabled = true;
            ChessBoard.DrawChessBoard();
        }
        void Quit()
        {
            Application.Exit();
        }
        void Undo()
        {
           ChessBoard.Undo();
        }

        private void ChessBoard_PlayerUMarked(object sender, EventArgs e)
        {
            timer1.Start();
            progressBar1.Value = 0;
        }

        private void ChessBoard_EndedUGame(object sender, EventArgs e)
        {
            EndGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            if (progressBar1.Value >= progressBar1.Maximum)
            {

                EndGame();

            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)

                e.Cancel = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            socket.IP = textBox2.Text;
            if (socket.ConnectServer())
            {
                socket.CreateServer();
                Thread listenThread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            Listen();
                            break;
                        }
                        catch
                        {

                        }
                    }
                });
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            else
            {
                Thread listenThread = new Thread(() =>
                {
                    Listen();
                });
                listenThread.IsBackground = true;
                listenThread.Start();
                socket.Send("Thông tin từ Client");
            }

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox2.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }
        void Listen()
        {
            string data = (string)socket.Receive();
            MessageBox.Show(data);
        }
        #endregion

    
    }
}
            
    

