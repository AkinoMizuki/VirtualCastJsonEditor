using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* ==== VirtualCastJsonEditorフォーム ==== */
namespace VirtualCastJsonEditor
{
    /* ==== VCJE_NgHelpe ==== */
    public partial class VCJE_NgHelpe : Form
    {
        /*===========================================================================*/
        /*      イニシャライズ                                                       */
        /*===========================================================================*/
        public VCJE_NgHelpe()
        {
            InitializeComponent();
        }

        /*===========================================================================*/
        /*      メインフォーム                                                       */
        /*===========================================================================*/

        /* ==== Cancelボタン ==== */
        private void CancelButton_Click(object sender, EventArgs e)
        {
            /* ==== 多重起動チェックフラグ ==== */
            Properties.Settings.Default.NgLevelCheck = false;
            Properties.Settings.Default.Save();

            /* ==== 終了 ==== */
            Close();

        }/* ==== END_Cancelボタン ==== */

    }/* ==== VCJE_NgHelpe ==== */

}/* ==== END_VirtualCastJsonEditorフォーム ==== */
