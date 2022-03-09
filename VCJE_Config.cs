using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

/* ==== VirtualCastEditorフォーム ==== */
namespace VirtualCastJsonEditor
{
    /* ==== VCJE_Config ==== */
    public partial class VCJE_Config : Form
    {
        /*===========================================================================*/
        /*      イニシャライズ                                                       */
        /*===========================================================================*/
        public VCJE_Config()
        {
            InitializeComponent();

            /* ==== Steam選択ラジオボタン初期化 ==== */
            SteamRadioButton.Checked = Properties.Settings.Default.SteamSelect;
            /* ==== VirtualCastのパス ==== */
            ProjectFileBox.Text = Properties.Settings.Default.DLPass;
            /* ==== XMLのラジオボタン初期化 ==== */
            XML_RadioButton.Checked = Properties.Settings.Default.OpenXML;
            /* ==== Jsonのラジオボタン初期化 ==== */
            Json_RadioButton.Checked = Properties.Settings.Default.OpenJson;
            /* ==== NoneFileのラジオボタン初期化 ==== */
            NoneFile_RadioButton.Checked = Properties.Settings.Default.NoneFile;


            if (SteamRadioButton.Checked != true)
            {
                DLRadioButton.Checked = true;
            }

            /* ==== DL版VirtualCastのパスコントロール ==== */
            DLPassSelect();

        }

        /*===========================================================================*/
        /*      メインフォーム                                                       */
        /*===========================================================================*/
        
        /* ==== DL版VirtualCastのパスコントロール ==== */
        private void DLPassSelect ()
        {

            ProjectFileBox.Enabled = !SteamRadioButton.Checked;

            button1.Enabled = ProjectFileBox.Enabled;

        }/* ==== END_DL版VirtualCastのパスコントロール ==== */


        /* ==== OKボタン ==== */
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }/* ==== END_OKボタン ==== */

        /* ==== ProjectFileボタンクリックイベント ==== */
        private void SelectProjectFile_MouseUp(object sender, EventArgs e)
        {

            /* FolderBrowserDialogを表示 */
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                /* ユーザーが選択したフォルダをテキストボックスに表示 */
                ProjectFileBox.Text = folderBrowserDialog1.SelectedPath;

            } /* END_FolderBrowserDialogを表示 */

        }/* ==== END_ProjectFileボタンクリックイベント ==== */

        /* ==== 適応ボタンイベント ==== */
        private void ApplyButton_Click(object sender, EventArgs e)
        {

            /* ==== Steam選択ラジオボタン初期化 ==== */
            Properties.Settings.Default.SteamSelect = SteamRadioButton.Checked;
            /* ==== DL版VirtualCastのパス ==== */
            Properties.Settings.Default.DLPass = ProjectFileBox.Text;
            /* ==== XMLのラジオボタン適応 ==== */
            Properties.Settings.Default.OpenXML = XML_RadioButton.Checked;
            /* ==== Jsonのラジオボタン適応 ====*/
            Properties.Settings.Default.OpenJson = Json_RadioButton.Checked;
            /* ==== NoneFileのラジオボタン適応 ==== */
            Properties.Settings.Default.NoneFile = NoneFile_RadioButton.Checked;
            /* ==== 設定をアプリケーションに保存します。 ==== */
            Properties.Settings.Default.Save();

            /* ==== VirtualCast保存先セレクター ==== */
            if (SteamRadioButton.Checked == true)
            {/* ==== Steamをセレクト ==== */
                
                Properties.Settings.Default.LocalPass = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\My Games\VirtualCast";
            }
            else
            {/* ==== DL版をセレクト ==== */
                
                Properties.Settings.Default.LocalPass = Properties.Settings.Default.DLPass;

            }/* ==== END_VirtualCast保存先セレクター ==== */

            /* ==== 設定をアプリケーションに保存します。 ==== */
            Properties.Settings.Default.Save();

        }/* ==== 適応ボタンイベント ==== */

        /* ==== 初期化イベント ==== */
        private void InitializeButton_Click(object sender, EventArgs e)
        {

            /* ==== Steam選択ラジオボタン初期化 ==== */
            SteamRadioButton.Checked = true;
            /* ==== VirtualCastのパス ==== */
            ProjectFileBox.Text = "";
            /* ==== XMLのラジオボタン初期化 ==== */
            XML_RadioButton.Checked = true;
            /* ==== Jsonのラジオボタン初期化 ====*/
            Json_RadioButton.Checked = false;
            /* ==== NoneFileのラジオボタン初期化 ==== */
            NoneFile_RadioButton.Checked = false;

        }/* ==== END_初期化イベント ==== */

        /* ==== SteamLDSelectイベント ==== */
        private void SteamRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            DLPassSelect();

        }/* ==== SteamLDSelectイベント ==== */
    }/* ==== END_VCJE_Config ==== */

}/* ==== END_VirtualCastJsonEditorフォーム ==== */