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
    /* ==== VCJE_Help ==== */
    public partial class VCJE_Help : Form
    {

        /*===========================================================================*/
        /*      イニシャライズ                                                       */
        /*===========================================================================*/
        public VCJE_Help()
        {
            InitializeComponent();
            System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
            System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();
            // バージョン名（AssemblyVersion属性）を取得
            Version appVersion = mainAssemName.Version;
            SoftNeme.Text = "SoftNeme：" + mainAssemName.Name;
            VerNo.Text = "Version " + appVersion;
        }

        /*===========================================================================*/
        /*      メインフォーム                                                       */
        /*===========================================================================*/
        /* ==== OKボタン ==== */
        private void CancelButton_Click(object sender, EventArgs e)
        {

            Close();

        }/* ==== END_OKボタン ==== */

        /* ==== コミュニティリンク ==== */
        private void CommuURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            // Specify that the link was visited.
            this.CommuURL.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://com.nicovideo.jp/community/co1666563");

        }/* ==== END_コミュニティリンク ==== */


        /* ==== Twitterリンク ==== */
        private void TwitterURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            // Specify that the link was visited.
            this.TwitterURL.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://twitter.com/fairychirno");

        }/* ==== END_Twitterリンク ==== */

        private void MailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //クリップボードに文字列をコピーする
            Clipboard.SetText("mizukiakino@hyoudoukan.com");
            MessageBox.Show("クリップボードにコピーしました。", "メールアドレス");

        }
    }/* ==== END_VCJE_Help ==== */

}/* ==== END_VirtualCastJsonEditorフォーム ==== */
