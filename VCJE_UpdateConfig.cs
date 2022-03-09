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

namespace VirtualCastJsonEditor
{
    public partial class VCJE_UpdateConfig : Form
    {
        public VCJE_UpdateConfig()
        {
            InitializeComponent();
        }

        private void VCJE_UpdateConfig_Load(object sender, EventArgs e)
        {


            UpdateConfigBox.ReadOnly = true;
            UpdateConfigBox.BackColor = Color.White;
            UpdateConfigBox.TabStop = false;

            string LodeFile = System.IO.Path.Combine(@".\ReadMe.txt");

                if (File.Exists(LodeFile))
                {/* ==== ファイルが既に存在する ==== */

                /* ==== Jsonオープン ==== */
                //UpdateConfigBox.Show = UpdateConfigBox

                using (System.IO.StreamReader sr = new System.IO.StreamReader(LodeFile, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    UpdateConfigBox.Text = sr.ReadToEnd();
                }

            }
                else
                {/* ==== ファイルがが無い ==== */

                    MessageBox.Show("「ReadMe.txt」はフォルダー内に存在しませんでした", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            
        }
    }
}
