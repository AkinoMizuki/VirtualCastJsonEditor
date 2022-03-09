using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualCastJsonEditor
{/* ==== VirtualCastEditorフォーム ==== */
    public partial class DataGrigDialogs : Form
    {/* ==== DataGridDialogsフォーム ==== */
        /*===========================================================================*/
        /*      イニシャライズ                                                       */
        /*===========================================================================*/
        public DataGrigDialogs()
        {
            InitializeComponent();
        }
        /*===========================================================================*/
        /*      メインフォーム                                                       */
        /*===========================================================================*/
        private void TableButton_Click(object sender, EventArgs e)
        {/* ==== おもて画像を選択 ==== */

            /* ==== DataGridSelecterに"false"を設定に保存する ==== */
            Properties.Settings.Default.DataGridSelecter = false;
            Properties.Settings.Default.Save();
            /* ==== ダイアログを閉じる ==== */
            Close();

        }/* ==== END_おもて画像を選択 ==== */

        private void BackButton_Click(object sender, EventArgs e)
        {/* ==== うら画像を選択 ==== */

            /* ==== DataGridSelecterに"true"を設定に保存する ==== */
            Properties.Settings.Default.DataGridSelecter = true;
            Properties.Settings.Default.Save();
            /* ==== ダイアログを閉じる ==== */
            Close();

        }/* ==== うら画像を選択 ==== */

    }/* ==== END_DataGridDialogsフォーム ==== */

}/* ==== END_VirtualCastEditorフォーム ==== */
