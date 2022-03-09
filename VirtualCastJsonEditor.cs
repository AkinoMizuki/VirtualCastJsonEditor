/* ==== 試験中モード ==== */

#undef DEBUG2
//#define DEBUG2

/* ==== 仕様の削除 ==== */
#undef DeleteSpecification

/* ==== END_試験中モード ==== */


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

/*フォームを読み込み*/
using VirtualCastJsonEditor;

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System.Net.Http;
using System.Net.Http.Headers;
//using Windows.Data.Pdf;

/* ==== VirtualCastEditorフォーム ==== */
namespace VirtualCastJsonEditor
{
    /* ==== VirtualCastEditor ==== */
    public partial class VirtualCastJsonEditor : Form
    {

        /* ==== 仕様の削除警告 ==== */
#if DeleteSpecification
#error DeleteSpecification is defined            

#endif
        /* ==== END_仕様の削除警告 ==== */

        public static VirtualCastJsonEditor MainWindow;

        /*===========================================================================*/
        /*      オブジェクト作成                                                     */
        /*===========================================================================*/
        /* ==== ヘルプフォーム作成 ==== */
        VCJE_Help VCEHelp = new VCJE_Help();

        /* ==== ダイアログ作成 ==== */
        DataGrigDialogs DataGrig_Dialogs = new DataGrigDialogs();

        /* ==== 設定フォーム作成 ==== */
        VCJE_Config VCJEConfg = new VCJE_Config();
        XmlSave XmlSave = new XmlSave();
        JsonSave JsonSave = new JsonSave();
        DataGridSelect GetSelect = new DataGridSelect();
        VCJE_NgHelpe VCJE_NgHelpe = new VCJE_NgHelpe();
        NowUpload NewWebView = new NowUpload();

        DataSet DataSet = new DataSet();

        /*=== アップデートチェック ===*/
        UpdateChecker.CheckProgram UpdateSequence = new UpdateChecker.CheckProgram();

        /*=== アップデートチェック ===*/
        ConnectedSlide4VR.CheckProgram ConnectedSlide4VR = new ConnectedSlide4VR.CheckProgram();

        /*===========================================================================*/
        /*      関数宣言                                                             */
        /*===========================================================================*/
        /*=== メインexeのディレクトリー取得用 ===*/
        public static string VCJE_ExePass = "";

        /* ==== Slide4Vrスライド参照用 ==== */
        public string[,] Slide4VrList = { };

       
#if DEBUG
        public bool Debug_Slide4Vr = true;  //デバックモードで接続(Slide4Vrのサーバーに詳細ログが出力されます。)
#else
        public bool Debug_Slide4Vr = false; //Slide4Vrのサーバーに最低限のログが出力されます。
#endif


        /* ====データセル参照用 ==== */
        public DataGridViewCell clickedCell;
        public object wbTemp;
        public string address;
        public System.Windows.Forms.DataGridView[] TabData;
        public DataTable[] DataTable;
        public int TabIndex = 0;
        public string ini_Json = "";

        /* ==== キーコントロール参照用 ==== */
        public const int KyeItem = 135;
        public string[,] KyeControl = new string[2, KyeItem];
        public const int ini_ForwardBox = 11;   //UpArrow
        public const int ini_BackwardBox = 12;  //Backwardタブ
        public const int ini_LeftBox = 14;      //Leftタブ
        public const int ini_RightBox = 13;     //Rightタブ
        public const int ini_UpBox = 35;        //Upタブ
        public const int ini_DownBox = 23;      //Downタブ
        public const int ini_Key1Box = 2;       //Key1タブ
        public const int ini_Key2Box = 3;       //Key2タブ
        public const int ini_Key3Box = 4;       //Key3タブ
        public const int ini_Key4Box = 5;       //Key4タブ

        /* === キーコントロールインデックス用 === */
        int KeyForward_Inbex = 0;   //UpArrow        
        int KeyBackward_Inbex = 0;  //Backwardタブ      
        int KeyLeft_Inbex = 0;      //Leftタブ        
        int KeyRight_Inbex = 0;     //Rightタブ 
        int KeyUp_Inbex = 0;        //Upタブ 
        int KeyDown_Inbex = 0;      //Downタブ    
        int KeyKey1_Inbex = 0;      //Key1タブ
        int KeyKey2_Inbex = 0;      //Key2タブ
        int KeyKey3_Inbex = 0;      //Key3タブ
        int KeyKey4_Inbex = 0;      //Key4タブ

        /* === キーコントロールタブ === */
        public ComboBox[] KeySelectorData;

        /* ==== ComboBox参照用 ==== */
        public DataGridViewComboBoxColumn[] ComboBoxIndex;


        /* ==== コンボセレクトインデックス用 ==== */
        int ImageFormat_Index = 0;              //キャプチャ撮影、ホワイトボードの保存形式用
        int ChromaCollar_Inbex = 0;             //背景クロマキー
        int capture_resolution_Index = 0;       //カメラ解像度 
        int ControllerComboBox_Index = 0;       //コントローラ種別

        /* ==== リンク用関数 ==== */
        public ArrayList BackDataLink { get; set; }     //視聴者用両面画像
        public ArrayList BackMyDataLink { get; set; }   //自分用両面画像

        /* ====リザルト結果 ==== */
        public int DvdCount;            //初期動画DVD
        public int AdminCommentCount;   //運営コメント
        public int ViewerCount;         //視聴者用固定画像
        public int MyDataCount;         //自分用固定画像
        public int ReversCount;         //視聴者用両面画像
        public int MyReversCount;       //自分用両面画像
        public int BackCount;           //背景画像
        public int BoardCount;          //ホワイトボード画像
        public int CampeCount;          //カンペ画像

        static bool loadingFlg = true;

        public string HomePage = "com.nicovideo.jp/community/co1666563";         //更新確認ページ

        /*===========================================================================*/
        /*      DataGridセレクター関数定義     　                                    */
        /*===========================================================================*/
        class DataSelectionClass
        {/* ==== DataGridセレクター ==== */
            public const int GridIndex = 11;        //DataGrid全体数
            public const int AdminComment = 0;      //運営コメント
            public const int Dvd = 1;               //初期動画DVD
            public const int Viewer = 2;            //視聴者用固定画像
            public const int MyData = 3;            //自分用固定画像
            public const int Revers = 4;            //視聴者用おもて面画像
            public const int BackRevers = 5;        //視聴者用うら面画像
            public const int MyRevers = 6;          //自分用おもて面画像
            public const int BackMyRevers = 7;      //自分用うら面画像
            public const int Back = 8;              //背景画像
            public const int Board = 9;             //ホワイトボード画像
            public const int Campe = 10;            //カンペ画像

        }/* ==== END_DataGridセレクター ==== */

        /*===========================================================================*/
        /*      Tabセレクター関数定義     　                                         */
        /*===========================================================================*/
        class TabSelectionClass
        {/* ==== Tabセレクター ==== */
            public const int TabIndex = 9;          //DataGrid全体数
            public const int AdminComment = 0;      //運営コメント
            public const int Dvd = 1;               //初期動画DVD
            public const int Viewer = 2;            //視聴者用固定画像
            public const int MyData = 3;            //自分用固定画像
            public const int Revers = 4;            //視聴者用両面画像
            public const int MyRevers = 5;          //自分用両面画像
            public const int Back = 6;              //背景画像
            public const int Board = 7;             //ホワイトボード画像
            public const int Campe = 8;             //カンペ画像

        }/* ==== END_Tabセレクター ==== */

        /*===========================================================================*/
        /*      コンボボックスセレクター関数定義     　                              */
        /*===========================================================================*/
        class ComboBoxSelectionClass
        {/* ==== コンボボックスセレクター ==== */

            public const int ComboBoxIndex = 2;    //コンボボックス全体数

        }/* ==== END_コンボボックスセレクター ==== */

        /*===========================================================================*/
        /*      キーコントロールタブ関数定義     　                              */
        /*===========================================================================*/
        class KeyControlSelectionClass
        {/* ==== キーコントロールタブセレクター ==== */

            public const int KeyIndex = 10;         //KeySelector全体数
            public const int ForwardIndex = 0;      //Forwardタブ
            public const int BackwardIndex = 1;     //Backwardタブ
            public const int LeftIndex = 2;         //Leftタブ
            public const int RightIndex = 3;        //Rightタブ
            public const int UpIndex = 4;           //Upタブ
            public const int DownIndex = 5;         //Downタブ
            public const int Key1Index = 6;         //Key1タブ
            public const int Key2Index = 7;         //Key2タブ
            public const int Key3Index = 8;         //Key3タブ
            public const int Key4Index = 9;         //Key4タブ

        }/* ==== END_キーコントロールタブセレクター ==== */


        /*===========================================================================*/
        /*      Jsonオブジェクトクラス     　                                        */
        /*===========================================================================*/
        /* ==== ver2_0_4c新Jsonデータ ==== */
        public class Rootobject
        {
            public Niconico niconico { get; set; }                          //ニコニコ
            public Import import { get; set; }                              //インポート
            public Comment comment { get; set; }                            //ニコニコ生放送184コメント
            public Humanoid humanoid { get; set; }                          //移動の設定
            public Studio studio { get; set; }                              //ダイレクトビューモードでの入室許可
            public Item item { get; set; }                                  //アイテム登録
            public Persistent_Object persistent_object { get; set; }        //画像や動画等の登録
            public Background background { get; set; }                      //背景にパノラマ画像を登録
            public Keyboard keyboard { get; set; }                          //キーコントロール
            public Embedded_Script embedded_script { get; set; }            //VCIアイテムのデバッグ設定
            public string mode { get; set; }                                //起動モードの変更
            public bool use_tcp { get; set; }                               //TCP接続を使用するかどうか (デフォルトはUDP)
            public string vr_input_controller_type { get; set; }            //使用中のVRコントローラの種別
            public bool enable_looking_glass { get; set; }                  //The Looking Glassに対応
            public bool enable_vivesranipal_eye { get; set; }               //VIVE Pro Eyeの眼球の動きを検出するかどうか 
            public float vivesranipal_eye_adjust_x { get; set; }            //左右の眼球の移動量 
            public float vivesranipal_eye_adjust_y { get; set; }            //上下の眼球の移動量 
            public bool enable_vivesranipal_blink { get; set; }             //VIVE Pro Eyeのまばたきの動きを検出するかどうか
            public bool enable_vivesranipal_lip { get; set; }               //VR リップシンク
            public bool enable_vivesranipal_eye_with_emotion { get; set; }  //表情変更中でも眼球操作を使用する
            public float vibration_power { get; set; }                      //振動の強弱を調整できる。0にすると無振動になってしまう。 

        }

        public class Niconico
        {
            public string[] broadcaster_comments { get; set; }              //運営コメントの登録
            public int ng_score_threshold { get; set; }                     //ニコ生コメント NGスコアしきい値
        }

        public class Import
        {
            public string[] video_content_uris { get; set; }                   //動画のURL

        }

        public class Comment
        {
            public bool display_anonymous_card { get; set; }                //184コメントからのコメント窓の生成を許可する
            public bool display_anonymous_flying { get; set; }              //184コメントからの落ちてくるコメント生成を許可する
        }

        public class Humanoid
        {
            public bool use_fast_spring_bone { get; set; }                  //SpringBone最適化
        }

        public class Studio
        {
            public int max_performers { get; set; }                         //スタジオに存在出来る演者の最大人数
            public int max_direct_viewers { get; set; }                     //スタジオに存在出来るダイレクトビューモード使用者の最大人数
        }

        public class Item
        {
            public Whiteboard whiteboard { get; set; }                      //ホワイトボードの画像登録
            public Cue_Card cue_card { get; set; }                          //カンペの画像登録
            public bool enable_displaycapture_chromakey { get; set; }       //ディスプレイアイテムのクロマキー合成
            public bool enable_nicovideo_chromakey { get; set; }            //ニコニコ動画プレイヤーのクロマキー合成
            public bool enable_videoboard_chromakey { get; set; }           //画面キャプチャークロマキー
            public Projectable_Item projectable_item { get; set; }          //画像のキャッシュを有効にする
            public bool hide_camera_from_viewers { get; set; }              //視聴者にカメラの表示設定
            public bool movie_audio_recording_enabled { get; set; }         //ムービー撮影時の録音を有効にする
            public string capture_resolution { get; set; }                  //カメラ解像度
            public string capture_format { get; set; }                      //キャプチャ撮影、ホワイトボードの保存形式
            public bool steam_screenshot { get; set; }                      //Steamスクリーンショット
            public bool disable_highlighting_item { get; set; }             //触れたアイテムをハイライトを無効
        }

        public class Whiteboard
        {
            public string[] source_urls { get; set; }                       //画像URLリスト
        }

        public class Cue_Card
        {
            public string[] source_urls { get; set; }                       //画像URLリスト
        }

        public class Projectable_Item
        {
            public bool enable_cache_all { get; set; }                      //画像のキャッシュを有効にする
        }

        public class Persistent_Object
        {
            public string[] image_urls { get; set; }                        //永続化画像の登録
            public string[] hidden_image_urls { get; set; }                 //永続化画像(放送非表示)の登録
            public string[][] double_sided_image_urls { get; set; }         //両面画像の登録
            public string[][] hidden_double_sided_image_urls { get; set; }  //両面画像(放送非表示)の登録

            /* === ニコニコ動画インポート用 ===*/
            public string[] nicovideo_ids { get; set; }                     //ニコニコ動画の再生DVDの登録
        }

        public class Background
        {
            public Panorama panorama { get; set; }                          //背景にパノラマ画像を登録
            public string chroma_key_background_color { get; set; }         //背景クロマキー
        }

        public class Keyboard
        {
            public string switch_rendering_to_desktop { get; set; }         //デスクトップへの描画内容を切り替えるキー
            public string keycode_vci_forward { get; set; }                 //VCI操作キー：Forward 
            public string keycode_vci_backward { get; set; }                //VCI操作キー：Backward
            public string keycode_vci_left { get; set; }                    //VCI操作キー：Left
            public string keycode_vci_right { get; set; }                   //VCI操作キー：Right
            public string keycode_vci_up { get; set; }                     //VCI操作キー：Up
            public string keycode_vci_down { get; set; }                    //VCI操作キー：Down
            public string keycode_vci_1 { get; set; }                       //VCI操作キー：Key1
            public string keycode_vci_2 { get; set; }                       //VCI操作キー：Key2
            public string keycode_vci_3 { get; set; }                       //VCI操作キー：Key3
            public string keycode_vci_4 { get; set; }                       //VCI操作キー：Key4


        }
            public class Panorama
        {
            public string[] source_urls { get; set; }                       //画像URLリスト
        }

        public class Embedded_Script
        {
            public int websocket_console_port { get; set; }                 //WebSocket Loggerポート番号
            public int moonsharp_debugger_port { get; set; }                //MoonSharpDebug のプラグイン

        }


        /*===========================================================================*/
        /*      イニシャライズ                                                       */
        /*===========================================================================*/
        public VirtualCastJsonEditor()
        {/* ==== FormLodeの初期化 ==== */

            InitializeComponent();

            /* ==== LocalPassの初期化 ==== */
            if (Properties.Settings.Default.LocalPass != null)
            {
                if (Properties.Settings.Default.LocalPass == "")
                {

                    Properties.Settings.Default.LocalPass = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\My Games\VirtualCast";
                    Properties.Settings.Default.Save();

                }
                
            }/* ==== LocalPassの初期化 ==== */

            /* ==== 更新確認用form ==== */
            //webBrowser1.ScriptErrorsSuppressed = true;



            /* ==== 別スレット ==== */
            var task1 = Task.Run(() =>
            {

                if (HomePage.Equals("about:blank")) return;
                if (!HomePage.StartsWith("http://") &&
                    !HomePage.StartsWith("https://"))
                {
                    HomePage = "http://" + HomePage;
                }
                try
                {
                    webView1.Navigate(new Uri(HomePage));
                }
                catch (System.UriFormatException)
                {
                    return;
                }

                //Slide4Vrサーバーアイドル開始
                var AppAccess = ConnectedSlide4VR.GetSlide4VrSlide(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, "AppAccess", 0, Debug_Slide4Vr);

            });/* ==== END_別スレット ==== */

            
            /* ==== 本スレット ==== */

            /*=== メインexeのディレクトリー取得 ===*/
            VCJE_ExePass = Environment.CurrentDirectory;
            /*=== アップデート確認 ===*/
            UploadCheck(VCJE_ExePass);


            /* ==== dllチェック ==== */
            string DllFile = System.IO.Path.Combine(@".\Newtonsoft.Json.dll");
            /* ==== 保存フォルダーの存在を確認 ==== */
            if (File.Exists(DllFile) == false)
            {/* ==== ファイルが既に存在する ==== */

                MessageBox.Show("「Newtonsoft.Json.dll」はフォルダー内に存在しませんでした"
                    + System.Environment.NewLine + "アプリケーションを終了します。",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                /*自分自身のフォームをCloseメソッドで閉じると、アプリケーションが終了する*/
                Close();
            }

            System.Reflection.Assembly mainAssembly = System.Reflection.Assembly.GetEntryAssembly();
            System.Reflection.AssemblyName mainAssemName = mainAssembly.GetName();
            // バージョン名（AssemblyVersion属性）を取得
            Version appVersion = mainAssemName.Version;
            this.Text = mainAssemName.Name + "：ver." + appVersion;

#if DEBUG
            this.Text += "　(Now DebugMode)";
#endif

            /* ==== 読み込み設定ファイルの初期化 ==== */
            SettingsRadioButton_ini();
            /* ==== タブ配列の初期化 ==== */
            TabData_init();
            DataTab_init();
            /* === キーコントロール配列の初期化 === */
            KeyControlBox_ini();
            KeyControl_ini();
            KeyControBox_Reset();
            /* ==== コンボボックス配列の初期化 ==== */
            ComboBox_ini();
            /* ==== アイトラ設定の初期化 ==== */
            EyeSettings_ini();
            /* ==== 設定タブの初期化 ==== */
            SettingsTab_ini();

            /* ==== 多重起動チェックフラグ初期化 ==== */
            Properties.Settings.Default.NgLevelCheck = false;
            Properties.Settings.Default.Save();



            /* ==== 新起動シーケンスメモ ==== */
            // === 初回起動時 ===
            //1.イニシャライズ
            //2.初起動フラグチェック
            //3.HTMLファイルが存在するか確認(HTMLがある場合はインポート)
            //4.Jsonが存在するか確認(Jsonがある場合はインポート)
            //5.新規画面
            //※基本はHTMLで保存をデフォルトとする
            /* ==== END_新起動シーケンスメモ ==== */


            /* ==== オープンチェックを開く ==== */
            string LodeFolder = Properties.Settings.Default.LocalPass;


            /* === 初回起動時 === */
            if (Properties.Settings.Default.FastVCJE == true)
            {
                /* === XML確認 === */
                string LodeFile = System.IO.Path.Combine(@".\"+ Properties.Settings.Default.VCJEDataSet + ".xml");
                string LodeFile2 = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEConfig + ".xml");

                /* ==== 画像保存フォルダーの存在を確認 ==== */
                if (Directory.Exists(LodeFolder))
                {/* ==== 保存フォルダは存在する ==== */

                    if (File.Exists(LodeFolder + LodeFile) && File.Exists(LodeFolder + LodeFile2))
                    {/* ==== ファイルが既に存在する ==== */

                        /* ==== XMLオープン ==== */

                        LoadXmlConfig(LodeFolder + LodeFile2);
                        VirtualDataSet.Clear();  //  ReadXMLは追記なので一旦消す
                        VirtualDataSet.ReadXml(LodeFolder + LodeFile);

                    }
                    else
                    {/* ==== Jsonを確認と開く ==== */

                        string Old_LodeFile = System.IO.Path.Combine(@".\config.json");
                        LodeFile = System.IO.Path.Combine(@".\default_config.json");

                        /* ==== 保存フォルダーの存在を確認 ==== */
                        if (Directory.Exists(LodeFolder))
                        {/* ==== 保存フォルダは存在する ==== */

                            if (File.Exists(LodeFolder + LodeFile))
                            {/* ==== ファイルが既に存在する ==== */

                                /* ==== Jsonオープン ==== */
                                JsonDeSerialize(LodeFile);

                            }
                            else if (File.Exists(LodeFolder + Old_LodeFile))
                            {

                                /* ==== Jsonオープン ==== */
                                JsonDeSerialize(Old_LodeFile);

                            }

                        }/* ==== END_保存フォルダは存在する ==== */

                    }
                }/* ==== END_保存フォルダは存在する ==== */

                Properties.Settings.Default.FastVCJE = false;
                Properties.Settings.Default.Save();

            }
            else
            {
                if (Properties.Settings.Default.OpenJson == true)
                {/* ==== Jsonを開く ==== */

                    string Old_LodeFile = System.IO.Path.Combine(@".\config.json");
                    string LodeFile = System.IO.Path.Combine(@".\default_config.json");

                    /* ==== 保存フォルダーの存在を確認 ==== */
                    if (Directory.Exists(LodeFolder))
                    {/* ==== 保存フォルダは存在する ==== */

                        if (File.Exists(LodeFolder + LodeFile))
                        {/* ==== ファイルが既に存在する ==== */

                            /* ==== Jsonオープン ==== */
                            JsonDeSerialize(LodeFile);

                        }
                        else if (File.Exists(LodeFolder + Old_LodeFile))
                        {
                            /* ==== Jsonオープン ==== */
                            JsonDeSerialize(Old_LodeFile);
                        }
                        else
                        {/* ==== ファイルがが無い ==== */

                            MessageBox.Show("「config.json」はフォルダー内に存在しませんでした", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }/* ==== END_保存フォルダは存在する ==== */
                    else
                    {/* ==== 保存フォルダはない ==== */

                        MessageBox.Show("設定されたフォルダーが存在しませんでした.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }/* ==== END_画像保存フォルダーの存在を確認 ==== */

                }/* ==== END_Jsonを開く ==== */


                else if (Properties.Settings.Default.OpenXML == true)
                {/* ==== XMLを開く ==== */

                    string LodeFile = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEDataSet + ".xml");
                    string LodeFile2 = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEConfig + ".xml");

                    /* ==== 画像保存フォルダーの存在を確認 ==== */
                    if (Directory.Exists(LodeFolder))
                    {/* ==== 保存フォルダは存在する ==== */

                        if (File.Exists(LodeFolder + LodeFile) && File.Exists(LodeFolder + LodeFile2))
                        {/* ==== ファイルが既に存在する ==== */

                            /* ==== XMLオープン ==== */
                            LoadXmlConfig(LodeFolder + LodeFile2);
                            VirtualDataSet.Clear();  //  ReadXMLは追記なので一旦消す
                            VirtualDataSet.ReadXml(LodeFolder + LodeFile);

                        }
                        else
                        {/* ==== ファイルがが無い ==== */

                            MessageBox.Show("「" + Properties.Settings.Default.VCJEDataSet + ".xml」と「" + Properties.Settings.Default.VCJEConfig + ".xml」はフォルダー内に存在しませんでした", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }/* ==== END_保存フォルダは存在する ==== */
                    else
                    {/* ==== 保存フォルダはない ==== */

                        MessageBox.Show("設定されたフォルダーが存在しませんでした.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }/* ==== END_画像保存フォルダーの存在を確認 ==== */

                }/* ==== END_Jsonを開く ==== */


            }/* === END_初回起動時 === */


            /* ==== DirectCheck表示の初期化 ==== */
            LookingGlassView_ini();

            /*=== タスクトレイに常駐設定 ===*/
            setComponents();

        }/* ==== END_FormLodeの初期化 ==== */


        /* === キーコントロール配列の初期化 === */
        public void KeyControl_ini()
        {
            /* === 表示用 === */
            KyeControl[0, 0] = "None";

            KyeControl[0, 1] = "0";
            KyeControl[0, 2] = "1";
            KyeControl[0, 3] = "2";
            KyeControl[0, 4] = "3";
            KyeControl[0, 5] = "4";
            KyeControl[0, 6] = "5";
            KyeControl[0, 7] = "6";
            KyeControl[0, 8] = "7";
            KyeControl[0, 9] = "8";
            KyeControl[0, 10] = "9";

            KyeControl[0, 11] = "UpArrow";
            KyeControl[0, 12] = "DownArrow";
            KyeControl[0, 13] = "RightArrow";
            KyeControl[0, 14] = "LeftArrow";

            KyeControl[0, 15] = "A";
            KyeControl[0, 16] = "B";
            KyeControl[0, 17] = "C";
            KyeControl[0, 18] = "D";
            KyeControl[0, 19] = "E";
            KyeControl[0, 20] = "F";
            KyeControl[0, 21] = "G";
            KyeControl[0, 22] = "H";
            KyeControl[0, 23] = "I";
            KyeControl[0, 24] = "J";
            KyeControl[0, 25] = "K";
            KyeControl[0, 26] = "L";
            KyeControl[0, 27] = "M";
            KyeControl[0, 28] = "N";
            KyeControl[0, 29] = "O";
            KyeControl[0, 30] = "P";
            KyeControl[0, 31] = "Q";
            KyeControl[0, 32] = "R";
            KyeControl[0, 33] = "S";
            KyeControl[0, 34] = "T";
            KyeControl[0, 35] = "U";
            KyeControl[0, 36] = "V";
            KyeControl[0, 37] = "W";
            KyeControl[0, 38] = "X";
            KyeControl[0, 39] = "Y";
            KyeControl[0, 40] = "Z";

            KyeControl[0, 41] = "Tab";
            KyeControl[0, 42] = "Exclaim(!)";
            KyeControl[0, 43] = "DoubleQuote(\")";
            KyeControl[0, 44] = "Hash(#)";
            KyeControl[0, 45] = "Dollar($)";
            KyeControl[0, 46] = "Percent(%)";
            KyeControl[0, 47] = "Ampersand(&)";
            KyeControl[0, 48] = "Quote(')";
            KyeControl[0, 49] = "Asterisk(*)";
            KyeControl[0, 50] = "Plus(+)";
            KyeControl[0, 51] = "Comma(,)";
            KyeControl[0, 52] = "Minus(-)";
            KyeControl[0, 53] = "Period(.)";
            KyeControl[0, 54] = "Slash(/)";
            KyeControl[0, 55] = "Tilde(~)";
            KyeControl[0, 56] = "Colon(:)";
            KyeControl[0, 57] = "Semicolon(;)";
            KyeControl[0, 58] = "Less(<)";
            KyeControl[0, 59] = "Equale(=)";
            KyeControl[0, 60] = "Greater(>)";
            KyeControl[0, 61] = "Question(?)";
            KyeControl[0, 62] = "At(@)";
            KyeControl[0, 63] = "Backslash(\")";
            KyeControl[0, 64] = "Caret(^)";
            KyeControl[0, 65] = "Underscore(_)";
            KyeControl[0, 66] = "BackQuote(`)";
            KyeControl[0, 67] = "LeftParen(()";
            KyeControl[0, 68] = "RightParen())";
            KyeControl[0, 69] = "LeftCurlyBracket({)";
            KyeControl[0, 70] = "RightCurlyBracket(})";
            KyeControl[0, 71] = "LeftBracket([)";
            KyeControl[0, 72] = "RightBracket(])";

            KyeControl[0, 73] = "Home";
            KyeControl[0, 74] = "PageUP";
            KyeControl[0, 75] = "PageDown";
            KyeControl[0, 76] = "RightShift";
            KyeControl[0, 77] = "LeftShift";
            KyeControl[0, 78] = "RightControl";
            KyeControl[0, 79] = "LeftControl";
            KyeControl[0, 80] = "RightAlt";
            KyeControl[0, 81] = "LeftAlt";

            KyeControl[0, 82] = "F1";
            KyeControl[0, 83] = "F2";
            KyeControl[0, 84] = "F3";
            KyeControl[0, 85] = "F4";
            KyeControl[0, 86] = "F5";
            KyeControl[0, 87] = "F6";
            KyeControl[0, 88] = "F7";
            KyeControl[0, 89] = "F8";
            KyeControl[0, 90] = "F9";
            KyeControl[0, 91] = "F10";
            KyeControl[0, 92] = "F11";
            KyeControl[0, 93] = "F12";
            KyeControl[0, 94] = "F13";
            KyeControl[0, 95] = "F14";
            KyeControl[0, 96] = "F15";

            KyeControl[0, 97] = "Keypad0";
            KyeControl[0, 98] = "Keypad1";
            KyeControl[0, 99] = "Keypad2";
            KyeControl[0, 100] = "Keypad3";
            KyeControl[0, 101] = "Keypad4";
            KyeControl[0, 102] = "Keypad5";
            KyeControl[0, 103] = "Keypad6";
            KyeControl[0, 104] = "Keypad7";
            KyeControl[0, 105] = "Keypad8";
            KyeControl[0, 106] = "Keypad9";
            KyeControl[0, 107] = "KeypadPeriod";
            KyeControl[0, 108] = "KeypadDivide";
            KyeControl[0, 109] = "KeypadMultiply";
            KyeControl[0, 110] = "KeypadMinus";
            KyeControl[0, 111] = "KeypadPlus";
            KyeControl[0, 112] = "KeypadEnter";
            KyeControl[0, 113] = "KeypadEquals";

            KyeControl[0, 114] = "Clear";

            KyeControl[0, 115] = "JoystickButton0";
            KyeControl[0, 116] = "JoystickButton1";
            KyeControl[0, 117] = "JoystickButton2";
            KyeControl[0, 118] = "JoystickButton3";
            KyeControl[0, 119] = "JoystickButton4";
            KyeControl[0, 120] = "JoystickButton5";
            KyeControl[0, 121] = "JoystickButton6";
            KyeControl[0, 122] = "JoystickButton7";
            KyeControl[0, 123] = "JoystickButton8";
            KyeControl[0, 124] = "JoystickButton9";
            KyeControl[0, 125] = "JoystickButton10";
            KyeControl[0, 126] = "JoystickButton11";
            KyeControl[0, 127] = "JoystickButton12";
            KyeControl[0, 128] = "JoystickButton13";
            KyeControl[0, 129] = "JoystickButton14";
            KyeControl[0, 130] = "JoystickButton15";
            KyeControl[0, 131] = "JoystickButton16";
            KyeControl[0, 132] = "JoystickButton17";
            KyeControl[0, 133] = "JoystickButton18";
            KyeControl[0, 134] = "JoystickButton19";


            /* === Json出力用 === */
            KyeControl[1, 0] = "None";

            KyeControl[1, 1] = "Alpha0";
            KyeControl[1, 2] = "Alpha1";
            KyeControl[1, 3] = "Alpha2";
            KyeControl[1, 4] = "Alpha3";
            KyeControl[1, 5] = "Alpha4";
            KyeControl[1, 6] = "Alpha5";
            KyeControl[1, 7] = "Alpha6";
            KyeControl[1, 8] = "Alpha7";
            KyeControl[1, 9] = "Alpha8";
            KyeControl[1, 10] = "Alpha9";

            KyeControl[1, 11] = "UpArrow";
            KyeControl[1, 12] = "DownArrow";
            KyeControl[1, 13] = "RightArrow";
            KyeControl[1, 14] = "LeftArrow";

            KyeControl[1, 15] = "A";
            KyeControl[1, 16] = "B";
            KyeControl[1, 17] = "C";
            KyeControl[1, 18] = "D";
            KyeControl[1, 19] = "E";
            KyeControl[1, 20] = "F";
            KyeControl[1, 21] = "G";
            KyeControl[1, 22] = "H";
            KyeControl[1, 23] = "I";
            KyeControl[1, 24] = "J";
            KyeControl[1, 25] = "K";
            KyeControl[1, 26] = "L";
            KyeControl[1, 27] = "M";
            KyeControl[1, 28] = "N";
            KyeControl[1, 29] = "O";
            KyeControl[1, 30] = "P";
            KyeControl[1, 31] = "Q";
            KyeControl[1, 32] = "R";
            KyeControl[1, 33] = "S";
            KyeControl[1, 34] = "T";
            KyeControl[1, 35] = "U";
            KyeControl[1, 36] = "V";
            KyeControl[1, 37] = "W";
            KyeControl[1, 38] = "X";
            KyeControl[1, 39] = "Y";
            KyeControl[1, 40] = "Z";

            KyeControl[1, 41] = "Tab";
            KyeControl[1, 42] = "Exclaim";
            KyeControl[1, 43] = "DoubleQuote";
            KyeControl[1, 44] = "Hash";
            KyeControl[1, 45] = "Dollar";
            KyeControl[1, 46] = "Percent";
            KyeControl[1, 47] = "Ampersand";
            KyeControl[1, 48] = "Quote";
            KyeControl[1, 49] = "Asterisk";
            KyeControl[1, 50] = "Plus";
            KyeControl[1, 51] = "Comma";
            KyeControl[1, 52] = "Minus";
            KyeControl[1, 53] = "Period";
            KyeControl[1, 54] = "Slash";
            KyeControl[1, 55] = "Tilde";
            KyeControl[1, 56] = "Colon";
            KyeControl[1, 57] = "Semicolon";
            KyeControl[1, 58] = "Less";
            KyeControl[1, 59] = "Equale";
            KyeControl[1, 60] = "Greater";
            KyeControl[1, 61] = "Question";
            KyeControl[1, 62] = "At";
            KyeControl[1, 63] = "Backslash";
            KyeControl[1, 64] = "Caret";
            KyeControl[1, 65] = "Underscore";
            KyeControl[1, 66] = "BackQuote";
            KyeControl[1, 67] = "LeftParen";
            KyeControl[1, 68] = "RightParen";
            KyeControl[1, 69] = "LeftCurlyBracket";
            KyeControl[1, 70] = "RightCurlyBracket";
            KyeControl[1, 71] = "LeftBracket";
            KyeControl[1, 72] = "RightBracket";

            KyeControl[1, 73] = "Home";
            KyeControl[1, 74] = "PageUP";
            KyeControl[1, 75] = "PageDown";
            KyeControl[1, 76] = "RightShift";
            KyeControl[1, 77] = "LeftShift";
            KyeControl[1, 78] = "RightControl";
            KyeControl[1, 79] = "LeftControl";
            KyeControl[1, 80] = "RightAlt";
            KyeControl[1, 81] = "LeftAlt";

            KyeControl[1, 82] = "F1";
            KyeControl[1, 83] = "F2";
            KyeControl[1, 84] = "F3";
            KyeControl[1, 85] = "F4";
            KyeControl[1, 86] = "F5";
            KyeControl[1, 87] = "F6";
            KyeControl[1, 88] = "F7";
            KyeControl[1, 89] = "F8";
            KyeControl[1, 90] = "F9";
            KyeControl[1, 91] = "F10";
            KyeControl[1, 92] = "F11";
            KyeControl[1, 93] = "F12";
            KyeControl[1, 94] = "F13";
            KyeControl[1, 95] = "F14";
            KyeControl[1, 96] = "F15";

            KyeControl[1, 97] = "Keypad0";
            KyeControl[1, 98] = "Keypad1";
            KyeControl[1, 99] = "Keypad2";
            KyeControl[1, 100] = "Keypad3";
            KyeControl[1, 101] = "Keypad4";
            KyeControl[1, 102] = "Keypad5";
            KyeControl[1, 103] = "Keypad6";
            KyeControl[1, 104] = "Keypad7";
            KyeControl[1, 105] = "Keypad8";
            KyeControl[1, 106] = "Keypad9";
            KyeControl[1, 107] = "KeypadPeriod";
            KyeControl[1, 108] = "KeypadDivide";
            KyeControl[1, 109] = "KeypadMultiply";
            KyeControl[1, 110] = "KeypadMinus";
            KyeControl[1, 111] = "KeypadPlus";
            KyeControl[1, 112] = "KeypadEnter";
            KyeControl[1, 113] = "KeypadEquals";

            KyeControl[1, 114] = "Clear";

            KyeControl[1, 115] = "JoystickButton0";
            KyeControl[1, 116] = "JoystickButton1";
            KyeControl[1, 117] = "JoystickButton2";
            KyeControl[1, 118] = "JoystickButton3";
            KyeControl[1, 119] = "JoystickButton4";
            KyeControl[1, 120] = "JoystickButton5";
            KyeControl[1, 121] = "JoystickButton6";
            KyeControl[1, 122] = "JoystickButton7";
            KyeControl[1, 123] = "JoystickButton8";
            KyeControl[1, 124] = "JoystickButton9";
            KyeControl[1, 125] = "JoystickButton10";
            KyeControl[1, 126] = "JoystickButton11";
            KyeControl[1, 127] = "JoystickButton12";
            KyeControl[1, 128] = "JoystickButton13";
            KyeControl[1, 129] = "JoystickButton14";
            KyeControl[1, 130] = "JoystickButton15";
            KyeControl[1, 131] = "JoystickButton16";
            KyeControl[1, 132] = "JoystickButton17";
            KyeControl[1, 133] = "JoystickButton18";
            KyeControl[1, 134] = "JoystickButton19";

            /* === キーコントロールカウンター === */
            for (int KeyBoxCounter = 0; KeyBoxCounter <= KeyControlSelectionClass.KeyIndex -1; ++KeyBoxCounter)
            {
                /* === ComBoxクリア === */
                KeySelectorData[KeyBoxCounter].Items.Clear();

                for (int KeyI = 0; KeyI <= KyeItem - 1; ++KeyI)
                {/* === 配列をセット === */

                    KeySelectorData[KeyBoxCounter].Items.Add(KyeControl[0, KeyI]);

                }/* === END_配列をセット === */

            }/* === END_キーコントロールカウンター === */

        }/* === END_キーコントロール配列の初期化 === */

        /* === キーコントロールボックスの初期化 === */
        public void KeyControlBox_ini()
        {
            KeySelectorData = new ComboBox[KeyControlSelectionClass.KeyIndex];          //KeySelector全体数

            KeySelectorData[KeyControlSelectionClass.ForwardIndex] = KeyForwardBox;     //Forwardタブ
            KeySelectorData[KeyControlSelectionClass.BackwardIndex] = KeyBackwardBox;   //Backwardタブ
            KeySelectorData[KeyControlSelectionClass.LeftIndex] = KeyLeftBox;           //Leftタブ
            KeySelectorData[KeyControlSelectionClass.RightIndex] = KeyRightBox;         //Rightタブ
            KeySelectorData[KeyControlSelectionClass.UpIndex] = KeyUpBox;               //Upタブ
            KeySelectorData[KeyControlSelectionClass.DownIndex] = KeyDownBox;           //Downタブ
            KeySelectorData[KeyControlSelectionClass.Key1Index] = KeyKey1Box;           //Key1タブ
            KeySelectorData[KeyControlSelectionClass.Key2Index] = KeyKey2Box;           //Key2タブ
            KeySelectorData[KeyControlSelectionClass.Key3Index] = KeyKey3Box;           //Key3タブ
            KeySelectorData[KeyControlSelectionClass.Key4Index] = KeyKey4Box;           //Key4タブ

        }/* === END_キーコントロールボックスの初期化 === */

        /* === キーコントロールボックスに配列をセット === */
        public void KeyControBox_Reset()
        {

            KeyForwardBox.SelectedIndex = ini_ForwardBox;   //UpArrow
            KeyBackwardBox.SelectedIndex = ini_BackwardBox; //Backwardタブ
            KeyLeftBox.SelectedIndex = ini_LeftBox;         //Leftタブ
            KeyRightBox.SelectedIndex = ini_RightBox;       //Rightタブ
            KeyUpBox.SelectedIndex = ini_UpBox;             //Upタブ
            KeyDownBox.SelectedIndex = ini_DownBox;         //Downタブ
            KeyKey1Box.SelectedIndex = ini_Key1Box;         //Key1タブ
            KeyKey2Box.SelectedIndex = ini_Key2Box;         //Key2タブ
            KeyKey3Box.SelectedIndex = ini_Key3Box;         //Key3タブ
            KeyKey4Box.SelectedIndex = ini_Key4Box;         //Key4タブ

        }/* === END_キーコントロールボックスに配列をセット === */



        /* ==== FormLode ==== */
        private void VirtualCastJsonEditor_Load(object sender, EventArgs e)
        {
            /* ==== DBの初期DVDコンバート ==== */
            ConverterFor(DataSelectionClass.Dvd);

            /* ==== タブの表示バグ対策 ==== */

            /* ==== アイトラ設定タブの反転 ==== */
            if (EyeTrackingCheckBox.Checked == false)
            {
                /* === アイトラ設定の非表示 ==== */
                SettingTabControl.TabPages.Remove(EyeTrackingTab);
                Properties.Settings.Default.EyeFlag = false;
            }
            else
            {
                /* ==== アイトラ設定タブの表示 ==== */
                SettingTabControl.TabPages.Insert(SettingTabControl.TabCount, EyeTrackingTab);
                Properties.Settings.Default.EyeFlag = true;
            }
            Properties.Settings.Default.Save();
            /* ==== END_タブの表示バグ対策 ==== */

#if DeleteSpecification

            /* ==== Edge対応につきIE11への以降の削除 ==== */
            /*
              IEのバージョンと値
              "FEATURE_BROWSER_EMULATION"キー内の値のデータとIEのversionの関係は以下になります。
                値(10進数)    値(16進数)    バージョンと意味
                11001           0x2AF9      Internet Explorer 11, Edgeモード(最新のバージョンでレンダリング)
                11000           0x2AF8      Internet Explorer 11
                10001           0x2711      Internet Explorer 10, Standardsモード
                10000           0x2710      Internet Explorer 10(!DOCTYPE で指定がある場合は、Standardsモードになります。)
                9999            0x270F      Internet Explorer 9, Standardsモード
                9000            0x2710      Internet Explorer 9(!DOCTYPE で指定がある場合は、Standardsモードになります。)
                8888            0x22B8      Internet Explorer 8, Standardsモード
                8000            0x1F40      Internet Explorer 8(!DOCTYPE で指定がある場合は、Standardsモードになります。)
                7000            0x1B58      Internet Explorer 7(!DOCTYPE で指定がある場合は、Standardsモードになります。)
            */

            var filename = Process.GetCurrentProcess().MainModule.FileName;
            filename = filename.Substring(filename.LastIndexOf('\\') + 1,
                filename.Length - filename.LastIndexOf('\\') - 1);
            if (filename.Contains("vhost"))
                filename = filename.Substring(0, filename.IndexOf('.') + 1) + "exe";

            Debug.Assert(Registry.CurrentUser != null, "Registry.CurrentUser != null");

            RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
            RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BEHAVIORS");
            key1?.SetValue(filename, 11001, RegistryValueKind.DWord);
            key2?.SetValue(filename, 11001, RegistryValueKind.DWord);
            key1?.Close();
            key2?.Close();

            /* ==== END_Edge対応につきIE11への以降の削除 ==== */

#endif // END_DeleteSpecificatione


        }/* ==== END_FormLode ==== */


        /* ==== タブ配列初期化 ==== */
        public void TabData_init()
        {

            this.TabData = new System.Windows.Forms.DataGridView[DataSelectionClass.GridIndex];
            /* ==== DataGridViewの呼び出し用配列 ==== */
            this.TabData[DataSelectionClass.AdminComment] = this.AdminCommentDataGridView;  //運営コメント文言  
            this.TabData[DataSelectionClass.Dvd] = this.DvdDataGridView;                     //初期動画DVD
            this.TabData[DataSelectionClass.Viewer] = this.ViewerDataGridView;               //視聴者用固定画像
            this.TabData[DataSelectionClass.MyData] = this.MyDataGridView;                   //自分用固定画像
            this.TabData[DataSelectionClass.Revers] = this.ReversDataGridView;               //視聴者用おもて面画像
            this.TabData[DataSelectionClass.BackRevers] = this.BackReversDataGridView;       //視聴者用うら面画像
            this.TabData[DataSelectionClass.MyRevers] = this.MyReversDataGridView;           //自分用おもて面画像
            this.TabData[DataSelectionClass.BackMyRevers] = this.BackMyReversDataGridView;   //自分用うら面画像

            this.TabData[DataSelectionClass.Back] = this.BackDataGridView;                   //背景画像
            this.TabData[DataSelectionClass.Board] = this.BoardDataGridView;                 //ホワイトボード画像
            this.TabData[DataSelectionClass.Campe] = this.CampeDataGridView;                 //カンペ画像

        }/* ==== END_タブ配列初期化 ==== */

        /* ==== タブ配列初期化 ==== */
        public void DataTab_init()
        {

            this.DataTable = new DataTable[DataSelectionClass.GridIndex];
            /* ==== DataTabの呼び出し用配列 ==== */
            this.DataTable[DataSelectionClass.AdminComment] = AdminCommentTalbe;   //運営コメント文言
            this.DataTable[DataSelectionClass.Dvd] = DvdDataTable;                 //初期動画DVD
            this.DataTable[DataSelectionClass.Viewer] = ViewerTable;               //視聴者用固定画像
            this.DataTable[DataSelectionClass.MyData] = MyTable;                   //自分用固定画像
            this.DataTable[DataSelectionClass.Revers] = ReversTable;               //視聴者用おもて面画像
            this.DataTable[DataSelectionClass.BackRevers] = BackReversTable;       //視聴者用うら面画像 
            this.DataTable[DataSelectionClass.MyRevers] = MyReversTable;           //自分用おもて面画像
            this.DataTable[DataSelectionClass.BackMyRevers] = BackMyReversTable;   //自分用うら面画像
            this.DataTable[DataSelectionClass.Back] = BackTable;                   //背景画像
            this.DataTable[DataSelectionClass.Board] = BoardTable;                 //ホワイトボード画像
            this.DataTable[DataSelectionClass.Campe] = CampeTable;                 //カンペ画像

        }/* ==== END_タブ配列初期化 ==== */


        /* ==== データグリッドの初期化 ==== */
        public void ClearDataGrid(int DataSelect)
        {
            DataTable[DataSelect].Clear();

        }/* ==== END_データグリッドの初期化 ==== */


        /* ==== コンボボックスの初期化 ==== */
        public void ComboBox_ini()
        {

            this.ComboBoxIndex = new DataGridViewComboBoxColumn[DataSelectionClass.GridIndex];
            this.ComboBoxIndex[DataSelectionClass.BackRevers] = LinkComboBox;            //視聴者用リスト
            this.ComboBoxIndex[DataSelectionClass.BackMyRevers] = MyLinkComboBox;        //自分用リスト

            BackDataLink = new ArrayList();
            BackMyDataLink = new ArrayList();

            LinkComboBox.Items.Add("未設定");
            LinkComboBox.Items.AddRange(BackDataLink.ToArray());

            MyLinkComboBox.Items.Add("未設定");
            MyLinkComboBox.Items.AddRange(BackMyDataLink.ToArray());

            Slide4VrComboBox.SelectedIndex = 0;                                         //Slide4Vrの初期化
            ImageFormatComboBox.SelectedIndex = 0;                                      //キャプチャ撮影、ホワイトボードの保存形式
            ChromaKeyColorComboBox.SelectedIndex = 0;                                   //スタジオ クロマキーの配色
            ResolutionComboBox.SelectedIndex = 1;                                       //カメラ解像度をFHDで初期化
            ControllerComboBox.SelectedIndex = 0;                                       //コントローラ種別

        }/* ==== END_コンボボックスの初期化 ==== */
        
        /* ==== 読み込み設定ファイルの初期化 ==== */
        public void SettingsRadioButton_ini()
        {

            switch (Properties.Settings.Default.DataSelect)
            {/* ==== プロファイルスイッチ ==== */
                
                case 0:
                    {//デフォルト

                        Default_RadioButton.Checked = true;         
                        break;

                    }//END_デフォル
                case 1:
                    {//プロファイル1

                        Preset1_RadioButton.Checked = true;
                        break;

                    }//END_プロファイル1
                case 2:
                    {//プロファイル2

                        Preset2_RadioButton.Checked = true;
                        break;

                    }//END_プロファイル1
                case 3:
                    {//プロファイル3

                        Preset3_RadioButton.Checked = true;
                        break;

                    }//END_プロファイル3
                case 4:
                    {//プロファイル4

                        Preset4_RadioButton.Checked = true;
                        break;

                    }//END_プロファイル4


            }/* ==== END_プロファイルスイッチ ==== */
            
            //Slide4vrAPIトークンの復帰
            if (Properties.Settings.Default.Slide4vrAPIToken != "" || Properties.Settings.Default.Slide4vrAPIToken != " " || Properties.Settings.Default.Slide4vrAPIToken != "　" || Properties.Settings.Default.Slide4vrAPIToken != null)
            {
                ApiTextBox.Text = Properties.Settings.Default.Slide4vrAPIToken.ToString();         //APIトークンの復帰
                ApiRockCheckBox.Checked = Properties.Settings.Default.LockSlide4vrAPI;  //API編集の有無                                                     // 文字の入力位置（キャレット）を末尾に設定する
                ApiTextBox.SelectionStart = ApiTextBox.Text.Length;
            }

        } /* ==== END_読み込み設定ファイルの初期化 ==== */


        /* ==== データの初期化 ==== */
        public void AllClear()
        {

            ApiRockCheckBox.Checked = false;                     //API編集有無

            DirectCheckBox.Checked = false;                     //ダイレクトビューモードの初期化
            LookingGlassCheckBox.Checked = false;               //DirectCheckの初期化
            LookingGlassCheckBox.Visible = false;               //DirectCheck表示設定の初期化
            CameraCheckBox.Checked = false;                     //カメラ非表示の初期化の初期化
            VideoRecBox.Checked = false;                        //ムービー撮影時の録音
            ImageCacheBox.Checked = false;                      //画像のキャッシュ
            DisplayChromakeyBox.Checked = false;                //ディスプレイのクロマキーの初期化
            NicovideoChromakeyBox.Checked = false;              //動画プレイヤーのクロマキーの初期化
            CaptureCheckBox.Checked = false;                    //動画キャプチャーのクロマキーの初期化
            ItemHighlightCheckBox.Checked = false;              //アイテムのハイライトを非表示
            NgCommentWindowBox.Checked = false;                 //184のコメント窓の生成を許可
            NgFallingCommentsBox.Checked = false;               //184の落ちてくるコメント生成
            SpringBoneCheckBox.Checked = false;                 //SpringBone最適化
            TcpCheckBox.Checked = false;                        //TCPの接続を有効にする
            EyeTrackingCheckBox.Checked = false;                //アイトラの初期化
            LipSynchCheckBox.Checked = false;                   //リップシンク 
            SteamCheckBox.Checked = false;                      //Steamライブラリの初期化

            ImageFormatComboBox.SelectedIndex = 0;              //画像形式の初期化
            ChromaKeyColorComboBox.SelectedIndex = 0;           //スタジオ クロマキーの配色
            ResolutionComboBox.SelectedIndex = 1;               //カメラ解像度をFHDで初期化
            ControllerComboBox.SelectedIndex = 0;               //コントローラ種別
            VibeNumericUpDown.Value = (decimal)0.5;             //コントローラ振動の強さ

            MaxStudioNumericUpDown.Value = 4;                   //スタジオ最大人数
            MaxDirectViewNumericUpDown.Value = 0;               //ダイレクトビュー最大人数

            ThresholdUpDown.Value = -10000;                     //NGコメントしきい値の初期化        
            EyeSettings_ini();                                  //アイトラ設定の初期化

            ClearDataGrid(DataSelectionClass.AdminComment);     //運営コメント文言の初期化
            ClearDataGrid(DataSelectionClass.Dvd);              //初期動画DVDの初期化
            ClearDataGrid(DataSelectionClass.Viewer);           //視聴者用固定画像の初期化
            ClearDataGrid(DataSelectionClass.MyData);           //自分用固定画像の初期化
            ClearDataGrid(DataSelectionClass.Revers);           //視聴者用表面画像
            ClearDataGrid(DataSelectionClass.BackRevers);       //視聴者用裏面画像
            ClearDataGrid(DataSelectionClass.MyRevers);         //自分用表面画像
            ClearDataGrid(DataSelectionClass.BackMyRevers);     //自分用裏面画像
            ClearDataGrid(DataSelectionClass.Back);             //背景画像の初期化
            ClearDataGrid(DataSelectionClass.Board);            //ホワイトボード画像の初期化
            ClearDataGrid(DataSelectionClass.Campe);            //カンペ画像の初期化


            //デスクトップへ描画停止を切り替えるキーの初期化
            StopDrawingTextBox.Text = "X";

            //キーコントロールの初期化
            KeyControBox_Reset();

            //視聴者用両面画像リストの初期化
            BackDataLink.Clear();
            LinkComboBox.Items.Clear();
            LinkComboBox.Items.Add("未設定");
            LinkComboBox.Items.AddRange(BackDataLink.ToArray());
            //自分用両面画像リストの初期化
            BackMyDataLink.Clear();
            MyLinkComboBox.Items.Clear();
            MyLinkComboBox.Items.Add("未設定");
            MyLinkComboBox.Items.AddRange(BackMyDataLink.ToArray());

        }/* ==== END_データの初期化の初期化 ==== */


        /* ==== DirectCheck表示設定 ==== */
        private void LookingGlassView_ini()
        {
            /* ==== DirectCheck表示設定＆ダイレクトモード＆ViewTalkの確認 ==== */
            if (DirectCheckBox.Checked == true)
            {

                /* ==== LookingGlassの表示 ==== */
                LookingGlassCheckBox.Visible = true;

            }/* ==== END_LookingGlassの確認 ==== */
            else
            {
                /* ==== LookingGlassの非表示 ==== */
                LookingGlassCheckBox.Checked = false;
                LookingGlassCheckBox.Visible = false;

            }/* ==== END_LookingGlassの確認 ==== */

        }/* ==== END_DirectCheck表示設定 ==== */


        /* ==== アイトラ設定の初期化 ==== */
        private void EyeSettings_ini()
        {

            EyeDetectionCheckBox.Checked = false;               //眼球検出の初期化
            BlinkDetectionCheckBox.Checked = false;             //まばたき検出
            EmotionCheckBox.Checked = false;                    //表情変更中でも眼球操作を使用するの初期化
            Move_X_NumericUpDown.Value = 1;                     //上下の目移動量の初期化
            Move_Y_NumericUpDown.Value = 1;                     //左右の目移動量の初期化

        }/* ==== END_VCIデバック設定の初期化=== */


        /* ==== アイトラ設定タブの反転 ==== */
        private void EyeTabSettingChange()
        {
            /* ==== アイトラ設定タブの反転 ==== */
            if (Properties.Settings.Default.EyeFlag == true)
            {
                /* === アイトラ設定の非表示 ==== */
                SettingTabControl.TabPages.Remove(EyeTrackingTab);
                Properties.Settings.Default.EyeFlag = false;
            }
            else
            {
                /* ==== アイトラ設定タブの表示 ==== */
                SettingTabControl.TabPages.Insert(SettingTabControl.TabCount, EyeTrackingTab);
                Properties.Settings.Default.EyeFlag = true;
            }
            Properties.Settings.Default.Save();

        }/* ==== END_アイトラ設定タブの反転 ==== */


        /* ==== 設定タブの初期化 ==== */
        private void SettingsTab_ini()
        {

            /* ====アイトラの非表示 ==== */
            SettingTabControl.TabPages.Remove(EyeTrackingTab);
            Properties.Settings.Default.EyeFlag = false;

            Properties.Settings.Default.Save();

        }/* ==== END_設定タブの初期化 ==== */



        /* ==== ソフトウェア位置の保存 ==== */
        private void VirtualCastJsonEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            /*=== タスクトレイにアイコンを残さないようにする ===*/
            VCJE_NotifyIcon.Icon = null;

        }/* ==== END_ソフトウェア位置の保存 ==== */


        /* ==== DataGridの補正処理 ==== */
        public int DataGrigCorrection(int DataGridIndex)
        {
            switch (DataGridIndex)
            {/* ==== 補正処理スイッチ ==== */
                case TabSelectionClass.AdminComment:
                    {/* ==== 運営コメント ==== */
                        DataGridIndex = DataSelectionClass.AdminComment;
                        break;
                    }
                case TabSelectionClass.Dvd:
                    {/* ==== 初期動画DVD ==== */
                        DataGridIndex = DataSelectionClass.Dvd;
                        break;
                    }
                case TabSelectionClass.Viewer:
                    {/* ==== 視聴者用固定画像 ==== */
                        DataGridIndex = DataSelectionClass.Viewer;
                        break;
                    }
                case TabSelectionClass.MyData:
                    {/* ==== 自分用固定画像 ==== */
                        DataGridIndex = DataSelectionClass.MyData;
                        break;
                    }
                case TabSelectionClass.Revers:
                    {/* ==== 視聴者用両面画像 ==== */
                        DataGridIndex = DataSelectionClass.Revers;
                        break;
                    }
                case TabSelectionClass.MyRevers:
                    {/* ==== 自分用両面画像 ==== */
                        DataGridIndex = DataSelectionClass.MyRevers;
                        break;
                    }
                case TabSelectionClass.Back:
                    {/* ==== 背景画像 ==== */
                        DataGridIndex = DataSelectionClass.Back;
                        break;
                    }
                case TabSelectionClass.Board:
                    {/* ==== ホワイトボード画像 ==== */
                        DataGridIndex = DataSelectionClass.Board;
                        break;
                    }
                case TabSelectionClass.Campe:
                    {/* ==== カンペ画像 ==== */
                        DataGridIndex = DataSelectionClass.Campe;
                        break;
                    }
                default:
                    {/* ==== その他 == == */
                        break;
                    }

            }/* ==== END_補正処理スイッチ ==== */

            return DataGridIndex;

        }/* ==== END_DataGridの補正処理 ==== */


        /*********************************************************************
        *   アップデートチェック
        *********************************************************************/
        public void UploadCheck(string UpExe)
        {/*=== アップデートチェック ===*/

            bool UpResult = false;

            /* ==== 別スレット ==== */
            var UpResultTask = Task.Run(() =>
            {
                UpResult =UpdateSequence.UpdateCheck(UpExe);
            });/* ==== END_別スレット ==== */

            UpResultTask.Wait();      


            /*=== アップデート確認 ===*/
            if (UpResult == true)
            {

                // 親フォームを作成
                using (Form f = new Form())
                {
                    f.TopMost = true; // 親フォームを常に最前面に表示する
                                      /*=== メッセージボックスを表示する ===*/
                    DialogResult result = MessageBox.Show("現在の「Virtual Cast Json Editor」は最新ではありません。" + System.Environment.NewLine
                + "ダウンロードページに移動しますか？",
                "アップデートのお知らせ",
                MessageBoxButtons.YesNo);

                /*=== 何が選択されたか調べる ===*/
                if (result == DialogResult.Yes)
                {
                    /*=== 「はい」が選択された時 ===*/
                    //ブラウザで開く
                    System.Diagnostics.Process.Start("https://hyoudoukan.booth.pm/items/1168800");
                }
                else if (result == DialogResult.No)
                {
                    /*=== 「いいえ」が選択された時 ===*/
                }

                    f.TopMost = false;
                }
            }/*=== END_アップデート確認 ===*/

        }/*=== END_アップデートチェック ===*/


        /*===========================================================================*/
        /*      メニューバー                                                         */
        /*===========================================================================*/
        /* ==== 保存ボタン ==== */
        private void Save_Click(object sender, EventArgs e)
        {
            /* ==== 保存 ==== */
            SaveSequence();

        }/* ==== END_保存ボタン ==== */


        /* ==== 更新してVirtualCast起動したら閉じる ==== */
        private void UpdateVirtualCastToolStripMenuItem_Click(object sender, EventArgs e)
        {

            /* ==== DL版とsteam版のSelect ==== */
            if (Properties.Settings.Default.SteamSelect == true)
            {/* ==== steam版 ==== */

                Properties.Settings.Default.VCexePass = @"C:\Program Files (x86)\Steam\steamapps\common\VirtualCast";

            }
            else
            {/* ==== DL版 ==== */

                Properties.Settings.Default.VCexePass = Properties.Settings.Default.DLPass;

            }/* ==== END_DL版とsteam版のSelect ==== */
            Properties.Settings.Default.Save();

            /* ==== ファイルディレクトリーチェック ==== */
            if (Properties.Settings.Default.LocalPass == "")
            {
                /* ==== 設定フォーム生成 ==== */
                using (VCJE_Config VCEConf = new VCJE_Config())
                {
                    MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                        + System.Environment.NewLine + "再度保存操作を行ってください。",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_Config VCEConfDlg = new VCJE_Config
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                    VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                    VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                }/* ==== END_設定フォーム生成 ==== */
            }
            else
            {
                /* ==== 保存 ==== */
                SaveSequence();

                try
                {
                    System.Diagnostics.Process VC_Process;
                    if (Process.GetProcessesByName("VirtualCast").Length > 0)
                    {

                        //notepadのプロセスを取得
                        System.Diagnostics.Process[] ps =
                            System.Diagnostics.Process.GetProcessesByName("VirtualCast");

                        foreach (System.Diagnostics.Process p in ps)
                        {
                            //クローズメッセージを送信する
                            p.CloseMainWindow();

                            //プロセスが終了するまで最大10秒待機する
                            p.WaitForExit(10000);
                            //プロセスが終了したか確認する
                            if (p.HasExited)
                            {

                                /* ==== VirtualCast起動 ==== */
                                if (File.Exists(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe"))
                                {

                                    VC_Process = System.Diagnostics.Process.Start(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe");
                                    /* それでもファイルが既に存在する */

                                    Properties.Settings.Default.Save();

                                    /* VC_Loadフォーム定義 */
                                    VC_Load dig = new VC_Load();
                                    /* マルチタスク */
                                    Task task = new Task(() =>
                                    {
                                        //5秒間（5000ミリ秒）停止する
                                        System.Threading.Thread.Sleep(5000);
                                        dig.DialogResult = DialogResult.OK;
                                    });
                                    task.Start();

                                    /* VC_Load画面表示 */
                                    Invoke((MethodInvoker)delegate ()
                                    {

                                        dig.ShowDialog(this);

                                    });

                                    /*自分自身のフォームをCloseメソッドで閉じると、アプリケーションが終了する*/
                                    Application.Exit();

                                }
                                else
                                {

                                    MessageBox.Show("「VirtualCast.exe」が存在しませんでした。");

                                }



                            }
                            else
                            {

                                Console.WriteLine("「VirtualCast」を終了出来ませんでした。");

                                MessageBox.Show("「config.json」は更新しましたが、「VirtualCast」を終了出来ませんでした。" +
                                System.Environment.NewLine +
                                "「VirtualCast」を終了してから再度操作をしてください。",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            }
                        }
                    }
                    else
                    {

                        /* ==== VirtualCast起動 ==== */
                        if (File.Exists(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe"))
                        {
                            VC_Process = System.Diagnostics.Process.Start(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe");
                        }
                        else
                        {
                            MessageBox.Show("「VirtualCast.exe」が存在しませんでした。");

                        }
                        Properties.Settings.Default.Save();
                        /*自分自身のフォームをCloseメソッドで閉じると、アプリケーションが終了する*/
                        Application.Exit();

                    }
                }
                catch
                {/* === 例外処理 === */
                    MessageBox.Show("「VirtualCast.exe」からアクセス権を得られませんでした。" + System.Environment.NewLine
                        + "リトライを試してください。");
                }/* === END_例外処理 === */
            }
        }/* ==== END_更新してVirtualCast起動したら閉じる ==== */


        /* ==== 更新してVirtualCast起動をしてVCJEはそのまま ==== */
        private void UpdateKeepToolStripMenuItem_Click(object sender, EventArgs e)
        {

            /* ==== DL版とsteam版のSelect ==== */
            if (Properties.Settings.Default.SteamSelect == true)
            {/* ==== steam版 ==== */

                Properties.Settings.Default.VCexePass = @"C:\Program Files (x86)\Steam\steamapps\common\VirtualCast";

            }
            else
            {/* ==== DL版 ==== */

                Properties.Settings.Default.VCexePass = Properties.Settings.Default.DLPass;

            }/* ==== END_DL版とsteam版のSelect ==== */
            Properties.Settings.Default.Save();


            /* ==== ファイルディレクトリーチェック ==== */
            if (Properties.Settings.Default.LocalPass == "")
            {
                /* ==== 設定フォーム生成 ==== */
                using (VCJE_Config VCEConf = new VCJE_Config())
                {
                    MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                        + System.Environment.NewLine + "再度保存操作を行ってください。",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_Config VCEConfDlg = new VCJE_Config
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                    VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                    VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                }/* ==== END_設定フォーム生成 ==== */
            }
            else
            {

                /* ==== 保存 ==== */
                SaveSequence();
                try
                {
                    System.Diagnostics.Process VC_Process;
                    if (Process.GetProcessesByName("VirtualCast").Length > 0)
                    {



                        //notepadのプロセスを取得
                        System.Diagnostics.Process[] ps =
                            System.Diagnostics.Process.GetProcessesByName("VirtualCast");

                        foreach (System.Diagnostics.Process p in ps)
                        {
                            //クローズメッセージを送信する
                            p.CloseMainWindow();

                            //プロセスが終了するまで最大10秒待機する
                            p.WaitForExit(10000);
                            //プロセスが終了したか確認する
                            if (p.HasExited)
                            {

                                /* ==== VirtualCast起動 ==== */
                                if (File.Exists(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe"))
                                {

                                    VC_Process = System.Diagnostics.Process.Start(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe");
                                    /* それでもファイルが既に存在する */

                                    Properties.Settings.Default.Save();

                                    /* VC_Loadフォーム定義 */
                                    VC_Load dig = new VC_Load();
                                    /* マルチタスク */
                                    Task task = new Task(() =>
                                    {
                                        //5秒間（5000ミリ秒）停止する
                                        System.Threading.Thread.Sleep(5000);
                                        dig.DialogResult = DialogResult.OK;
                                    });
                                    task.Start();

                                    /* VC_Load画面表示 */
                                    Invoke((MethodInvoker)delegate ()
                                    {

                                        dig.ShowDialog(this);

                                    });
                                }
                                else
                                {

                                    MessageBox.Show("「VirtualCast.exe」が存在しませんでした。");

                                }

                            }
                            else
                            {

                                Console.WriteLine("VirtualCastJsonEditorが終了しませんでした。");

                                MessageBox.Show("「config.json」は更新しましたが、「VirtualCast」を終了出来ませんでした。。" +
                                System.Environment.NewLine +
                                "「VirtualCast」を終了してから再度操作をしてください。",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            }
                        }
                    }
                    else
                    {
                        /* ==== VirtualCast起動 ==== */
                        if (File.Exists(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe"))
                        {
                            VC_Process = System.Diagnostics.Process.Start(Properties.Settings.Default.VCexePass + "\\VirtualCast.exe");
                        }
                        else
                        {
                            MessageBox.Show("「VirtualCast.exe」が存在しませんでした。");
                        }

                        Properties.Settings.Default.Save();

                    }
                }
                catch
                {/* === 例外処理 === */
                    MessageBox.Show("「VirtualCast.exe」からアクセス権を得られませんでした。" + System.Environment.NewLine
                        + "リトライを試してください。");
                }/* === END_例外処理 === */

            }

        }/* ==== END_更新してVirtualCast起動をしてVCJEはそのまま ==== */
        

        /* ==== 保存シーケンス ==== */
        private void SaveSequence()
        {

            /* ==== Json保存 ==== */
            JsonSerialize();
            /* ==== Xml保存 ==== */
            SaveConfig(Properties.Settings.Default.LocalPass + System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEConfig + ".xml"));
            VirtualDataSet.WriteXml(Properties.Settings.Default.LocalPass + System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEDataSet + ".xml"));

            /* ==== セーブ結果表示 ==== */
            MessageBox.Show(
                 "運営コメント数：" + AdminCommentCount + System.Environment.NewLine
                + "有効初期動画：" + DvdCount + System.Environment.NewLine
                + "有効視聴者用画像：" + ViewerCount + System.Environment.NewLine
                + "有効生主用画像：" + MyDataCount + System.Environment.NewLine
                + "有効視聴者用両面画像：" + ReversCount + System.Environment.NewLine
                + "有効生主用両面画像：" + MyReversCount + System.Environment.NewLine
                + "有効背景画像：" + BackCount + System.Environment.NewLine
                + "有効ホワイトボード画像：" + BoardCount + System.Environment.NewLine
                + "有効カンペ画像：" + CampeCount + System.Environment.NewLine,
                "Json Save Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk
                );
            /* ==== END?セーブ結果表示 ==== */

        }/* ==== END_保存シーケンス ==== */


        /* ==== Jsonオープン ==== */
        private void OpenJson_Click(object sender, EventArgs e)
        {

            /* ==== ファイルディレクトリーチェック ==== */
            if (Properties.Settings.Default.LocalPass == "")
            {
                /* ==== 設定フォーム生成 ==== */
                using (VCJE_Config VCEConf = new VCJE_Config())
                {
                    MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                        + System.Environment.NewLine + "再度保存操作を行ってください。",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_Config VCEConfDlg = new VCJE_Config
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                    VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                    VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                }/* ==== END_設定フォーム生成 ==== */
            }
            else
            {/* ==== Jsonを開く ==== */
                /* ==== オープンチェックを開く ==== */
                string Old_LodeFile = System.IO.Path.Combine(@".\config.json");
                string LodeFile = System.IO.Path.Combine(@".\default_config.json");

                if (File.Exists(Properties.Settings.Default.LocalPass + LodeFile))
                {/* ==== ファイルが既に存在する ==== */

                    /* ==== Jsonオープン ==== */
                    JsonDeSerialize(LodeFile);

                    /* ==== オープン結果表示 ==== */
                    MessageBox.Show(
                        "Jsonファイルのロードが終了しました。",
                        "Json Load Result",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk
                        );
                    /* ==== END?セーブ結果表示 ==== */

                }
                else if(File.Exists(Properties.Settings.Default.LocalPass + Old_LodeFile))
                {/* ==== ファイルが既に存在する ==== */

                    /* ==== Jsonオープン ==== */
                    JsonDeSerialize(Old_LodeFile);

                    /* ==== オープン結果表示 ==== */
                    MessageBox.Show(
                        "Jsonファイルのロードが終了しました。",
                        "Json Load Result",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk
                        );
                    /* ==== END?セーブ結果表示 ==== */

                }
                else
                {/* ==== ファイルがが無い ==== */

                    MessageBox.Show("「config.json」はフォルダー内に存在しませんでした", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }/* ==== END_Jsonを開く ==== */

        }/* ==== END_Jsonオープン ==== */


        /* ==== Xmlオープン ==== */
        private void OpenXml_Click(object sender, EventArgs e)
        {
            /* ==== ファイルディレクトリーチェック ==== */
            if (Properties.Settings.Default.LocalPass == "")
            {
                /* ==== 設定フォーム生成 ==== */
                using (VCJE_Config VCEConf = new VCJE_Config())
                {
                    MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                        + System.Environment.NewLine + "再度保存操作を行ってください。",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_Config VCEConfDlg = new VCJE_Config
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                    VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                    VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                }/* ==== END_設定フォーム生成 ==== */
            }
            else
            {

                /* ==== ファイルディレクトリーチェック ==== */
                if (Properties.Settings.Default.LocalPass == "")
                {
                    /* ==== 設定フォーム生成 ==== */
                    using (VCJE_Config VCEConf = new VCJE_Config())
                    {
                        MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                            + System.Environment.NewLine + "再度保存操作を行ってください。",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        /* ==== マニュアルモードで生成 ==== */
                        VCJE_Config VCEConfDlg = new VCJE_Config
                        {

                            StartPosition = FormStartPosition.Manual

                        };/* ==== END_マニュアルモードで生成 ==== */

                        VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                        VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                        VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                        VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                    }/* ==== END_設定フォーム生成 ==== */
                }
                else
                {/* ==== XMLを開く ==== */
                    /* ==== オープンチェックを開く ==== */
                    string LodeFile = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEDataSet + ".xml");
                    string LodeFile2 = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEConfig + ".xml");

                    if (File.Exists(Properties.Settings.Default.LocalPass + LodeFile) && File.Exists(Properties.Settings.Default.LocalPass + LodeFile2))
                    {/* ==== ファイルが既に存在する ==== */

                        /* ==== XMLオープン ==== */
                        LoadXmlConfig(Properties.Settings.Default.LocalPass + LodeFile2);
                        VirtualDataSet.Clear();  //  ReadXMLは追記なので一旦消す
                        VirtualDataSet.ReadXml(Properties.Settings.Default.LocalPass + LodeFile);

                        /* ==== オープン結果表示 ==== */
                        MessageBox.Show(
                            "Xmlファイルのロードが終了しました",
                            "Xml Load Result",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Asterisk
                            );
                        /* ==== END?セーブ結果表示 ==== */
                    }
                    else
                    {/* ==== ファイルがが無い ==== */

                        MessageBox.Show("「VCJEDataSet.xml」と「VCJEConfig.xml」はフォルダー内に存在しませんでした", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }/* ==== END_ファイルディレクトリーチェック ==== */

        }/* ==== END_Xmlオープン ==== */


        /*===========================================================================*/
        /*      Read & Write                                                         */
        /*===========================================================================*/
        private void JsonSerialize()
        {/* ==== Jsonシリアライズ ==== */

            ini_Json = "";
            string file_path;
            /* ==== ファイルディレクトリーチェック ==== */
            if (Properties.Settings.Default.LocalPass == "")
            {
                /* ==== 設定フォーム生成 ==== */
                using (VCJE_Config VCEConf = new VCJE_Config())
                {
                    MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                        + System.Environment.NewLine + "再度保存操作を行ってください。",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_Config VCEConfDlg = new VCJE_Config
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                    VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                    VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                }/* ==== END_設定フォーム生成 ==== */
            }
            else
            {/* ==== Json保存開始 ==== */

                file_path = Properties.Settings.Default.LocalPass + System.IO.Path.Combine(@".\default_config.json");

                // ファイルへテキストデータを書き込む

                /* ==== Jsonスタート ==== */
                ini_Json += "{" + System.Environment.NewLine;
                /* ==== ニコニコ ==== */
                ini_Json += "  \"niconico\": {" + System.Environment.NewLine;

                /* ==== 任意の運営コメント文言を投稿する機能 ==== */
                ini_Json += "    \"broadcaster_comments\": [" + System.Environment.NewLine;
                AdminCommentCount = JsonFor(DataSelectionClass.AdminComment);
                ini_Json += "    ]," + System.Environment.NewLine;
                /* ==== END_任意の運営コメント文言を投稿する機能 ==== */

                /* ==== ニコ生のコメントNGスコアのしきい値の設定 ==== */
                ini_Json += "    \"ng_score_threshold\": " + ThresholdUpDown.Value + "," + System.Environment.NewLine;
                /* ==== END_ニコニコ ==== */

                ini_Json += "  }," + System.Environment.NewLine;


                /* ==== ニコニコ生放送184コメント ==== */
                ini_Json += "  \"comment\": {" + System.Environment.NewLine;
                //184のコメント窓の生成を許可
                ini_Json += "    \"display_anonymous_card\": ";
                //bool
                if (NgCommentWindowBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                //184の落ちてくるコメント生成
                ini_Json += "    \"display_anonymous_flying\": ";
                //bool
                if (NgFallingCommentsBox.Checked == true)
                {
                    ini_Json += "true" + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false" + System.Environment.NewLine;
                }
                
                ini_Json += "  }," + System.Environment.NewLine;
                /* ==== END_ニコニコ生放送184コメント ==== */


                /* ==== SpringBone最適化を行う機能をコンフィグ ==== */
                ini_Json += "  \"humanoid\": {" + System.Environment.NewLine;
                ini_Json += "    \"use_fast_spring_bone\": ";
                if (SpringBoneCheckBox.Checked == true)
                {
                    ini_Json += "true" + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false" + System.Environment.NewLine;
                }
                ini_Json += "  }," + System.Environment.NewLine;
                /* ==== END_SpringBone最適化を行う機能をコンフィグ ==== */


                /* ==== ダイレクトビューモード ==== */
                ini_Json += "  \"studio\": {" + System.Environment.NewLine;

                //スタジオに存在出来る演者の最大人数
                ini_Json += "   \"max_performers\": " + MaxStudioNumericUpDown.Value;
                ini_Json += "," + System.Environment.NewLine;

                //スタジオに存在出来るダイレクトビューモード使用者の最大人数
                ini_Json += "   \"max_direct_viewers\": " + MaxDirectViewNumericUpDown.Value;
                ini_Json += System.Environment.NewLine;

                ini_Json += "  }," + System.Environment.NewLine;
                /* ==== END_ダイレクトビューモード ==== */


                /* ==== アイテム ==== */
                ini_Json += "  \"item\": {" + System.Environment.NewLine;
                /* ==== ホワイトボードで使用する画像のURLを指定します ==== */
                ini_Json += "    \"whiteboard\": {" + System.Environment.NewLine;
                ini_Json += "      \"source_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                BoardCount = JsonFor(DataSelectionClass.Board);
                ini_Json += "      ]" + System.Environment.NewLine;
                ini_Json += "    }," + System.Environment.NewLine;


                /* ==== カンペで使用する画像のURLを指定します ==== */
                ini_Json += "    \"cue_card\": {" + System.Environment.NewLine;
                ini_Json += "      \"source_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                CampeCount = JsonFor(DataSelectionClass.Campe);
                ini_Json += "      ]" + System.Environment.NewLine;
                ini_Json += "    }," + System.Environment.NewLine;


                /* ==== ディスプレイキャプチャーをクロマキー ==== */
                ini_Json += "    \"enable_displaycapture_chromakey\": ";
                //bool
                if (DisplayChromakeyBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine; 
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine; 
                }


                /* ==== ニコニコ動画プレイヤーをクロマキー ==== */
                ini_Json += "    \"enable_nicovideo_chromakey\": ";
                //bool
                if (NicovideoChromakeyBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine; 
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine; 
                }

                /* ==== ニコニコ動画キャプチャーのクロマキー ==== */
                ini_Json += "    \"enable_videoboard_chromakey\": ";
                //bool
                if (CaptureCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                /* ==== 画像のキャッシュを有効にする ==== */
                ini_Json += "    \"projectable_item\": {" + System.Environment.NewLine;
                ini_Json += "      \"enable_cache_all\": ";
                if (ImageCacheBox.Checked == true)
                {
                    ini_Json += "true" + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false" + System.Environment.NewLine;
                }
                ini_Json += "    }," + System.Environment.NewLine;


                /* ==== カメラ解像度 ==== */
                ini_Json += "    \"capture_resolution\": \"" + ResolutionComboBox.SelectedItem + "\",";
                ini_Json += System.Environment.NewLine;


                /* ==== ムービー撮影時の録音を有効にする ==== */
                ini_Json += "    \"movie_audio_recording_enabled\": ";
                //bool
                if (VideoRecBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                /* ==== キャプチャ撮影、ホワイトボードの保存形式 ==== */
                ini_Json += "    \"capture_format\": \"" + ImageFormatComboBox.SelectedItem + "\",";
                ini_Json += System.Environment.NewLine;

                /*=== Steamスクリーンショット ===*/
                ini_Json += "    \"steam_screenshot\": ";
                //bool
                if (SteamCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                /* ==== カメラアイテムを視聴者に見せるかどうか（デフォルトは見せる） ==== */
                ini_Json += "    \"hide_camera_from_viewers\": ";
                //bool
                if (CameraCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                /*=== 触れたアイテムをハイライトを無効 ===*/
                ini_Json += "    \"disable_highlighting_item\": ";
                //bool
                if (ItemHighlightCheckBox.Checked == true)
                {
                    ini_Json += "true" + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false" + System.Environment.NewLine;
                }
                ini_Json += "  }," + System.Environment.NewLine + System.Environment.NewLine;
                /* ==== END_アイテム ==== */



                /* ==== 画像動画 ==== */
                ini_Json += "  \"persistent_object\": {" + System.Environment.NewLine;

                /* ==== 初期表示を行う画像を指定します ==== */
                ini_Json += "    \"image_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                ViewerCount = JsonFor(DataSelectionClass.Viewer);
                ini_Json += "    ]," + System.Environment.NewLine;

                /* ==== 初期表示でなおかつ視聴者に見えない画像を指定します ==== */
                ini_Json += "    \"hidden_image_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                MyDataCount = JsonFor(DataSelectionClass.MyData);
                ini_Json += "    ]," + System.Environment.NewLine;

                /* ==== 永続画像の両面の画像を指定します ==== */
                ini_Json += "    \"double_sided_image_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                ReversCount = JsonFor(DataSelectionClass.Revers);
                ini_Json += "    ]," + System.Environment.NewLine;

                /* ==== 視聴者には見えない永続画像両面の画像を指定します ==== */
                ini_Json += "    \"hidden_double_sided_image_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                MyReversCount = JsonFor(DataSelectionClass.MyRevers);
                ini_Json += "    ]" + System.Environment.NewLine;
                ini_Json += "  }," + System.Environment.NewLine;
                /* ==== END_画像動画 ==== */


                /* ==== 初期表示の動画URLを指定します ==== */
                ini_Json += "  \"import\": {" + System.Environment.NewLine;
                ini_Json += "    \"video_content_uris\": [" + System.Environment.NewLine;
                //吐き出しfor文
                DvdCount = JsonFor(DataSelectionClass.Dvd);
                ini_Json += "    ]" + System.Environment.NewLine;
                ini_Json += "  }," + System.Environment.NewLine;

                


                /* ==== Equirectangular形式のパノラマ画像 ==== */
                ini_Json += "  \"background\": {" + System.Environment.NewLine;
                ini_Json += "    \"panorama\": {" + System.Environment.NewLine;
                ini_Json += "      \"source_urls\": [" + System.Environment.NewLine;
                //吐き出しfor文
                BackCount = JsonFor(DataSelectionClass.Back);
                ini_Json += "      ]" + System.Environment.NewLine;
                ini_Json += "    }," + System.Environment.NewLine;

                /* ==== クロマキー ==== */
                ini_Json += "    \"chroma_key_background_color\": \"" + ChromaKeyColorComboBox.SelectedItem + "\"" + System.Environment.NewLine;
                ini_Json += "  }," + System.Environment.NewLine;
                /* ==== END_テレポート移動の設定 ==== */


                /*=== VCIキーコントロール ===*/
                ini_Json += "  \"keyboard\": {" + System.Environment.NewLine;

                //デスクトップへの描画内容を切り替えるキー
                if (StopDrawingTextBox.Text == "" || StopDrawingTextBox.Text == " " || StopDrawingTextBox.Text == "　")
                {
                    ini_Json += "    \"switch_rendering_to_desktop\": \"" + "X" + "\",";
                    ini_Json += System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "    \"switch_rendering_to_desktop\": \"" + StopDrawingTextBox.Text + "\",";
                    ini_Json += System.Environment.NewLine;
                }// END_デスクトップへの描画内容を切り替えるキー



                ini_Json += "    \"keycode_vci_forward\": \"" + KyeControl[1,KeyForwardBox.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_backward\": \"" + KyeControl[1, KeyBackwardBox.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_left\": \"" + KyeControl[1, KeyLeftBox.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_right\": \"" + KyeControl[1, KeyRightBox.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_up\": \"" + KyeControl[1, KeyUpBox.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_down\": \"" + KyeControl[1, KeyDownBox.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_1\": \"" + KyeControl[1, KeyKey1Box.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_2\": \"" + KyeControl[1, KeyKey2Box.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_3\": \"" + KyeControl[1, KeyKey3Box.SelectedIndex] + "\",";
                ini_Json += System.Environment.NewLine;
                ini_Json += "    \"keycode_vci_4\": \"" + KyeControl[1, KeyKey4Box.SelectedIndex] + "\"";
                ini_Json += System.Environment.NewLine;
                ini_Json += "  }," + System.Environment.NewLine;
                /*=== END_VCIキーコントロール ===*/


                /* ==== VCIデバックモード ==== */
                ini_Json += "  \"embedded_script\": {" + System.Environment.NewLine;

                /* ==== ポート番号 ==== */
                ini_Json += "    \"websocket_console_port\": " + PortUpDown.Value + "," + System.Environment.NewLine;
                
                /* ==== MoonSharpDebug のプラグイン ==== */
                ini_Json += "    \"moonsharp_debugger_port\": " + MSPortUpDown.Value + System.Environment.NewLine;
              
                ini_Json += "  }," + System.Environment.NewLine;
                /* ==== END_VCIデバックモード ==== */


                /* ==== ダイレクトビューモードで起動します ==== */
                ini_Json += "  \"mode\": ";
                if (DirectCheckBox.Checked == true)
                {
                    ini_Json += "\"direct-view\"," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "\"default\"," + System.Environment.NewLine;
                }

                /* ==== TCP接続を使用するかどうか (デフォルトはUDP) ==== */
                ini_Json += "  \"use_tcp\": ";
                //bool
                if (TcpCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                /* ==== 使用中のVRコントローラの種別 ==== */
                ini_Json += "  \"vr_input_controller_type\": \"" + ControllerComboBox.SelectedItem + "\",";
                ini_Json += System.Environment.NewLine;
                //"vr_input_controller_type": "AutoDetect",

                /* ==== ダイレクトビューモードでのみLooking Glassへ投影可能です。 ==== */
                ini_Json += "  \"enable_looking_glass\": ";
                //bool
                if (LookingGlassCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }


                /* ==== VIVE Pro Eyeの設定 ==== */
                //VIVE Pro Eyeの眼球の動きを検出するかどうか 
                ini_Json += "  \"enable_vivesranipal_eye\": ";
                if (EyeDetectionCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," +  System.Environment.NewLine;
                }

                //上下の眼球の移動量
                ini_Json += "  \"vivesranipal_eye_adjust_x\": " + Move_X_NumericUpDown.Value;
                ini_Json += "," + System.Environment.NewLine;

                //左右の眼球の移動量
                ini_Json += "  \"vivesranipal_eye_adjust_y\": " + Move_Y_NumericUpDown.Value;
                ini_Json += "," + System.Environment.NewLine;

                //VIVE Pro Eyeのまばたきの動きを検出するかどうか
                ini_Json += "  \"enable_vivesranipal_blink\": ";
                if (BlinkDetectionCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                //VR リップシンク
                ini_Json += "  \"enable_vivesranipal_lip\": ";
                if (LipSynchCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }

                //表情変更中でも眼球操作を使用する
                ini_Json += "  \"enable_vivesranipal_eye_with_emotion\": ";
                if (EmotionCheckBox.Checked == true)
                {
                    ini_Json += "true," + System.Environment.NewLine;
                }
                else
                {
                    ini_Json += "false," + System.Environment.NewLine;
                }
                /* ==== END_VIVE Pro Eyeの設定 ==== */

                /* ==== 振動の強弱を調整できる。0にすると無振動になってしまう。 ==== */
                ini_Json += "  \"vibration_power\": " + VibeNumericUpDown.Value + System.Environment.NewLine;

                ini_Json += "}";
                /* ==== END_Jsonスタート ==== */
                string file_data = ini_Json;    // ファイルのデータ
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file_path))   // UTF-8のテキスト用


                {
                    sw.Write(file_data); // ファイルへテキストデータを出力する
                }
                /* ==== END_Json保存開始 ==== */
            }/* ==== END_ファイルディレクトリーチェック ==== */

        } /* ==== END_Jsonシリアライズ ==== */

        /* ==== DataGridJsonシリアライズ ==== */
        public int JsonFor(int GridJsonIndex)
        {
            int MaxDataGridCount;       //データグリッドMax値取得
            int DataGridCount;          //データグリッドカウンター
            int MaxCheckCount;          //アクティブMax値取得
            int CheckCount;             //アクティブ数
            int CCount;                 //アクティブカウンター
            int TotalResultCount;       //合計リザルトカウンター
            int ResultCount;            //リザルトカウンター

            int SearchCount;            //検索カウンター
            int MaxSearchCount;         //検索カウンターMax値取得
            int SearchIndex;            //検索項目

            int LinkCount;            //Linkカウンター
            int MaxLinkCount;         //LinkカウンターMax値取得
            int LinkIndexCount;       //Link項目数

            /* ==== null回避用 ==== */
            bool FastCheck = false;
            bool FastCountCheck = false;

            /* ==== データ受け取り用 ==== */
            object ActiveCheck;
            string ActiveCheck2;

            /* ==== ActiveCount用 ==== */
            MaxCheckCount = 0;
            CheckCount = 0;
            CCount = 0;
            ResultCount = 0;

            /* ==== LinkCount用 ==== */
            LinkIndexCount = 0;

            /* ==== Max行取得 ==== */
            MaxDataGridCount = TabData[GridJsonIndex].Rows.Count;
            MaxCheckCount = MaxDataGridCount;

            /* ==== ActiveCount ==== */
            for (CCount = 0; MaxCheckCount >= CCount; CCount++)
            {
                if (FastCountCheck == false)
                {/* ==== 帳尻合わせ ==== */
                    FastCountCheck = true;
                }
                else
                {
                    ActiveCheck = TabData[GridJsonIndex].Rows[CCount - 1].Cells[1].Value;
                    ActiveCheck2 = "" + TabData[GridJsonIndex].Rows[CCount - 1].Cells[2].Value;

                    if (ActiveCheck2.Length == 0) ;
                    else
                    {
                        if (ActiveCheck == DBNull.Value) ;
                        else if (Convert.ToBoolean(ActiveCheck) == true)
                        {
                            /* ==== Active数 ==== */
                            CheckCount++;
                        }
                    }
                }/* ==== END_帳尻合わせ ==== */

            }/* ==== END_ActiveCount ==== */
            
            /* ==== Dataリンクチェック ==== */
            switch (GridJsonIndex)
            {
                case DataSelectionClass.Revers://視聴者用おもて面画像
                case DataSelectionClass.MyRevers://自分用おもて面画像
                    {

                        /* ==== null回避用 ==== */
                        bool LinkFastCheck = false;
                        bool LinkFastCountCheck = false;

                        /* ==== データ受け取り用 ==== */
                        string LinkActiveCheck;
                        string LinkActiveCheck2;

                        LinkIndexCount = 0;
                            
                        /* ==== Max行取得 ==== */
                        MaxLinkCount = TabData[GridJsonIndex].Rows.Count;

                        /* ==== LinkCount ==== */
                        for (LinkCount = 0; MaxLinkCount >= LinkCount; LinkCount++)
                        {
                            if (LinkFastCountCheck == false)
                            {/* ==== 帳尻合わせ ==== */
                                LinkFastCountCheck = true;
                            }
                            else
                            {
                                LinkActiveCheck = "" +TabData[GridJsonIndex].Rows[LinkCount - 1].Cells[1].Value;
                                LinkActiveCheck2 = "" + TabData[GridJsonIndex].Rows[LinkCount - 1].Cells[3].Value;

                                if (LinkActiveCheck2.Length == 0) ;
                                else
                                {
                                    if (LinkActiveCheck == "") ;
                                    else if (Convert.ToBoolean(LinkActiveCheck) == true)
                                    {
                                        /* ==== Active数 ==== */
                                        LinkIndexCount++;
                                    }
                                }
                            }/* ==== END_帳尻合わせ ==== */
                            
                        }/* ==== END_LinkCount ==== */

                        break;
                    }
            }/* ==== END_Dataリンクチェック ==== */




            /* ==== DataGridシリアライズ ==== */
            for (DataGridCount = 0; MaxDataGridCount >= DataGridCount; DataGridCount++)
            {

                if (FastCheck == false)
                {/* ==== 帳尻合わせ ==== */
                    FastCheck = true;
                }
                else
                {/* ==== シリアライズ ==== */

                    ActiveCheck = TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[1].Value;
                    ActiveCheck2 = "" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value;
                    if (ActiveCheck2.Length == 0) ;
                    else
                    {/* ==== 空白チェック ==== */
                        if (ActiveCheck == DBNull.Value) ;
                        else if (Convert.ToBoolean(ActiveCheck) == true)
                        {/* ==== アクティブ ==== */

                            if (CheckCount <= 1)
                            {/* ==== 最終行 ==== */

                                /* ==== URL切り替え ==== */
                                switch (GridJsonIndex)
                                {

                                    case DataSelectionClass.AdminComment://運営コメント文言を投稿する
                                    case DataSelectionClass.Dvd://初期動画DVD
                                    case DataSelectionClass.Viewer://視聴者用固定画像
                                    case DataSelectionClass.MyData://自分用固定画像
                                    case DataSelectionClass.Back://背景画像
                                        {
                                            ini_Json += "      \"" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value + "\"" + System.Environment.NewLine;
                                            /* ==== リザルト結果の総数をカウント ==== */
                                            ResultCount += 1;
                                            break;
                                        }
                                    case DataSelectionClass.Board://ホワイトボード画像
                                    case DataSelectionClass.Campe://カンペ画像
                                        {
                                            ini_Json += "        \"" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value + "\"" + System.Environment.NewLine;
                                            /* ==== リザルト結果の総数をカウント ==== */
                                            ResultCount += 1;
                                            break;
                                        }

                                    case DataSelectionClass.Revers://視聴者用おもて面画像
                                    case DataSelectionClass.MyRevers://自分用おもて面画像
                                        {
                                            
                                            //うら画像処理

                                            /* ==== null回避用 ==== */
                                            bool SearchFastCheck = false;
                                            bool SearchFastCountCheck = false;

                                            /* ==== データ受け取り用 ==== */
                                            string SearchActiveCheck;
                                            string SearchActiveCheck2;

                                            /* ==== 検索ワード ==== */
                                            SearchActiveCheck = "" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[3].Value;
                                            
                                            /* ==== Max行取得 ==== */
                                            MaxSearchCount = TabData[GridJsonIndex + 1].Rows.Count;

                                            /* ==== FastResult ==== */
                                            for (SearchCount = 0; MaxSearchCount >= SearchCount; SearchCount++)
                                            {
                                                if (SearchFastCountCheck == false)
                                                {/* ==== 帳尻合わせ ==== */
                                                    SearchFastCountCheck = true;
                                                }
                                                else
                                                {
                                                    
                                                    SearchActiveCheck2 = "" + TabData[GridJsonIndex + 1].Rows[SearchCount - 1].Cells[1].Value;

                                                    if (SearchActiveCheck2.Length == 0) ;
                                                    else
                                                    {
                                                        if (SearchActiveCheck == SearchActiveCheck2)
                                                        {/* ==== 検索一致 ==== */
                                                            ini_Json += "      [" + System.Environment.NewLine;
                                                            ini_Json += "        \"" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value + "\"," + System.Environment.NewLine;
                                                            ini_Json += "        \"" + TabData[GridJsonIndex + 1].Rows[SearchCount - 1].Cells[2].Value + "\"" + System.Environment.NewLine;
                                                            ini_Json += "      ]" + System.Environment.NewLine + System.Environment.NewLine;
                                                            
                                                            /* ==== リザルト結果の総数をカウント ==== */
                                                            ResultCount += 1;

                                                            SearchCount = MaxSearchCount;
                                                        }
                                                    }
                                                }/* ==== END_帳尻合わせ ==== */

                                            }/* ==== END_FastResult ==== */

                                            
                                            break;
                                        }
                                }/* ==== END_URL切り替え ==== */

                            }/* ==== END_最終行 ==== */
                            else
                            {/* ==== 行吐き出し ==== */

                                /* ==== URL切り替え ==== */
                                switch (GridJsonIndex)
                                {

                                    case DataSelectionClass.AdminComment://運営コメント文言を投稿する
                                    case DataSelectionClass.Dvd://初期動画DVD
                                    case DataSelectionClass.Viewer://視聴者用固定画像
                                    case DataSelectionClass.MyData://自分用固定画像
                                    case DataSelectionClass.Back://背景画像
                                        {
                                            ini_Json += "      \"" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value + "\"," + System.Environment.NewLine;
                                            /* ==== リザルト結果の総数をカウント ==== */
                                            ResultCount += 1;
                                            break;
                                        }
                                    case DataSelectionClass.Board://ホワイトボード画像
                                    case DataSelectionClass.Campe://カンペ画像
                                        {
                                            ini_Json += "        \"" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value + "\"," + System.Environment.NewLine;
                                            /* ==== リザルト結果の総数をカウント ==== */
                                            ResultCount += 1;
                                            break;
                                        }

                                    case DataSelectionClass.Revers://視聴者用おもて面画像
                                    case DataSelectionClass.MyRevers://自分用おもて面画像
                                        {
                                            
                                            //うら画像処理

                                            /* ==== null回避用 ==== */
                                            bool SearchFastCheck = false;
                                            bool SearchFastCountCheck = false;

                                            /* ==== データ受け取り用 ==== */
                                            string SearchActiveCheck;
                                            string SearchActiveCheck2;

                                            /* ==== 検索ワード ==== */
                                            SearchActiveCheck = "" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[3].Value;

                                            /* ==== Max行取得 ==== */
                                            MaxSearchCount = TabData[GridJsonIndex + 1].Rows.Count;

                                            /* ==== FastResult ==== */
                                            for (SearchCount = 0; MaxSearchCount >= SearchCount; SearchCount++)
                                            {
                                                if (SearchFastCountCheck == false)
                                                {/* ==== 帳尻合わせ ==== */
                                                    SearchFastCountCheck = true;
                                                }
                                                else
                                                {

                                                    SearchActiveCheck2 = "" + TabData[GridJsonIndex + 1].Rows[SearchCount - 1].Cells[1].Value;

                                                    if (SearchActiveCheck2.Length == 0) ;
                                                    else
                                                    {

                                                        if (SearchActiveCheck == SearchActiveCheck2)
                                                        {/* ==== 検索一致 ==== */
                                                            ini_Json += "      [" + System.Environment.NewLine;
                                                            ini_Json += "        \"" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value + "\"," + System.Environment.NewLine;
                                                            ini_Json += "        \"" + TabData[GridJsonIndex + 1].Rows[SearchCount - 1].Cells[2].Value + "\"" + System.Environment.NewLine;

                                                            if (LinkIndexCount <= 1)
                                                            {
                                                                ini_Json += "      ]" + System.Environment.NewLine + System.Environment.NewLine;
                                                                
                                                            }
                                                            else
                                                            {
                                                                ini_Json += "      ]," + System.Environment.NewLine + System.Environment.NewLine;
                                                                LinkIndexCount--;
                                                            }
                                                            /* ==== リザルト結果の総数をカウント ==== */
                                                            ResultCount += 1;

                                                            SearchCount = MaxSearchCount;
                                                        }
                                                    }
                                                }/* ==== END_帳尻合わせ ==== */

                                            }/* ==== END_FastResult ==== */
                                            
                                            break;
                                        }

                                }/* ==== END_URL切り替え ==== */

                                CheckCount--;

                            }/* ==== END_行吐き出し ==== */


                        }/* ==== END_アクティブ ==== */

                    }/* ==== END_空白チェック ==== */

                }/* ==== END_帳尻合わせ ==== */
            }/* ==== END_DataGridシリアライズ ==== */

            return (ResultCount);

        }/* ==== END_DataGridJsonシリアライズ ==== */




        /* ==== DataGridDVDコンバート ==== */
        public void ConverterFor(int GridJsonIndex)
        {
            int MaxDataGridCount;       //データグリッドMax値取得
            int DataGridCount;          //データグリッドカウンター
            int MaxCheckCount;          //アクティブMax値取得
            int CheckCount;             //アクティブ数
            int CCount;                 //アクティブカウンター
            int TotalResultCount;       //合計リザルトカウンター
            int ResultCount;            //リザルトカウンター

            int SearchCount;            //検索カウンター
            int MaxSearchCount;         //検索カウンターMax値取得
            int SearchIndex;            //検索項目

            int LinkCount;            //Linkカウンター
            int MaxLinkCount;         //LinkカウンターMax値取得
            int LinkIndexCount;       //Link項目数

            /* ==== null回避用 ==== */
            bool FastCheck = false;
            bool FastCountCheck = false;

            /* ==== データ受け取り用 ==== */
            object ActiveCheck;
            string ActiveCheck2;

            /* ==== ActiveCount用 ==== */
            MaxCheckCount = 0;
            CheckCount = 0;
            CCount = 0;
            ResultCount = 0;

            /* ==== LinkCount用 ==== */
            LinkIndexCount = 0;

            /* ==== Max行取得 ==== */
            MaxDataGridCount = TabData[GridJsonIndex].Rows.Count;
            MaxCheckCount = MaxDataGridCount;


            /* ==== DataGridシリアライズ ==== */
            for (DataGridCount = 0; MaxDataGridCount >= DataGridCount; DataGridCount++)
            {

                if (FastCheck == false)
                {/* ==== 帳尻合わせ ==== */
                    FastCheck = true;
                }
                else
                {/* ==== シリアライズ ==== */

                    ActiveCheck = TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[1].Value;
                    ActiveCheck2 = "" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value;
                    if (ActiveCheck2.Length == 0) ;
                    else
                    {/* ==== 空白チェック ==== */

                        /* ==== URL切り替え ==== */
                        switch (GridJsonIndex)
                        {

                            case DataSelectionClass.Dvd://初期動画DVD
                                {
                                    int index2 = TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value.ToString().IndexOf("sm");

                                    if (index2 == 0)
                                    {
                                        TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value = "https://www.nicovideo.jp/watch/" + TabData[GridJsonIndex].Rows[DataGridCount - 1].Cells[2].Value;
                                    }
                                    
                                    /* ==== リザルト結果の総数をカウント ==== */
                                    ResultCount += 1;
                                    break;
                                }

                        }/* ==== END_URL切り替え ==== */

                        CheckCount--;

                    }/* ==== END_空白チェック ==== */

                }/* ==== END_帳尻合わせ ==== */
            }/* ==== END_DataGridシリアライズ ==== */

        }/* ==== END_DataGridDVDコンバート ==== */


        public void CompatibilityErr()
        {/* ==== Json互換性エラー ==== */
            MessageBox.Show("Jsonファイルの取得に失敗しました。"
            + System.Environment.NewLine + "現在その他ツール、textファイルからの自作は互換性がありません。"
            + System.Environment.NewLine + "お手数ですが、他からのインポートは手動でお願いいたします。",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }/* ==== END_Json互換性エラー ==== */


        private void JsonDeSerialize(string JsonName)
        {/* ==== Json読み込み ==== */

            /* ==== フォームのデータを初期化 ==== */
            AllClear();

            var json = string.Empty;

            var fileName = Properties.Settings.Default.LocalPass + JsonName;

            if (System.IO.File.Exists(fileName) == true)
            {
                using (var reader = new StreamReader(fileName, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    json = reader.ReadToEnd();
                }

                /* ==== Jsonインポート ==== */
                Rootobject rootobject = new Rootobject();
                JsonConvert.PopulateObject(json, rootobject);

                /* ==== ニコニコ ==== */
                Console.WriteLine(rootobject.niconico);                                             //ニコニコ
                if (rootobject.niconico != null)
                {/* ==== Json要素確認 ==== */       
                    Console.WriteLine(rootobject.niconico.broadcaster_comments);                    //運営コメントの登録
                    Console.WriteLine(rootobject.niconico.ng_score_threshold);                      //ニコ生コメント NGスコアしきい値

                }

                /* ==== インポート ==== */
                Console.WriteLine(rootobject.import);                                               //ニコニコ
                if (rootobject.import != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.import);                                           //動画URLインポート
                }

                /* ==== 184コメント ==== */
                Console.WriteLine(rootobject.comment);                                              //184コメント
                if (rootobject.comment != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.comment.display_anonymous_card);                   //184コメントからのコメント窓の生成を許可する
                    Console.WriteLine(rootobject.comment.display_anonymous_flying);                 //184コメントからの落ちてくるコメント生成を許可する
                }

                /* ==== SpringBone最適化 ==== */
                Console.WriteLine(rootobject.humanoid);                                             //SpringBone最適化
                if (rootobject.humanoid != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.humanoid.use_fast_spring_bone);                    //SpringBone最適化
                }

                /* ==== ダイレクトビューモードでの入室許可 ==== */
                Console.WriteLine(rootobject.studio);                                               //ダイレクトビューモードでの入室許可
                if (rootobject.studio != null)
                {/* ==== Json要素確認 ==== */

                    //スタジオに存在出来る演者の最大人数
                    Console.WriteLine(rootobject.studio.max_performers);
                    //スタジオに存在出来るダイレクトビューモード使用者の最大人数
                    Console.WriteLine(rootobject.studio.max_direct_viewers);
                }


                /* ==== アイテム登録 ==== */
                Console.WriteLine(rootobject.item);                                                 //アイテム登録
                if (rootobject.item != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.item.whiteboard);                                  //ホワイトボードの画像登録
                    if (rootobject.item.whiteboard != null)
                    {/* ==== Json要素確認 ==== */
                        Console.WriteLine(rootobject.item.whiteboard.source_urls);                  //画像URLリスト
                    }

                    Console.WriteLine(rootobject.item.cue_card);                                    //カンペの画像登録
                    if (rootobject.item.cue_card != null)
                    {/* ==== Json要素確認 ==== */
                        Console.WriteLine(rootobject.item.cue_card.source_urls);                    //画像URLリスト
                    }

                    Console.WriteLine(rootobject.item.enable_displaycapture_chromakey);             //ディスプレイアイテムのクロマキー合成
                    Console.WriteLine(rootobject.item.enable_nicovideo_chromakey);                  //ニコニコ動画プレイヤーのクロマキー合成
                    Console.WriteLine(rootobject.item.enable_videoboard_chromakey);                 //画面キャプチャークロマキー合成

                    Console.WriteLine(rootobject.item.projectable_item);                            //画像のキャッシュを有効にする
                    if (rootobject.item.projectable_item != null)
                    {/* ==== Json要素確認 ==== */
                        Console.WriteLine(rootobject.item.projectable_item.enable_cache_all);       //画像のキャッシュを有効にする
                    }

                    Console.WriteLine(rootobject.item.capture_resolution);                          //カメラ解像度
                    Console.WriteLine(rootobject.item.movie_audio_recording_enabled);               //ムービー撮影時の録音を有効にする
                    Console.WriteLine(rootobject.item.capture_format);                              //キャプチャ撮影、ホワイトボードの保存形式
                    Console.WriteLine(rootobject.item.steam_screenshot);                            //Steamスクリーンショット
                    Console.WriteLine(rootobject.item.hide_camera_from_viewers);                    //視聴者にカメラの表示設定
                    Console.WriteLine(rootobject.item.disable_highlighting_item);                   //触れたアイテムをハイライトを無効
                }


                /* ==== 画像や動画等の登録 ==== */
                Console.WriteLine(rootobject.persistent_object);                                    //画像や動画等の登録
                if (rootobject.persistent_object != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.persistent_object.image_urls);                     //永続化画像の登録
                    Console.WriteLine(rootobject.persistent_object.hidden_image_urls);              //永続化画像(放送非表示)の登録
                    Console.WriteLine(rootobject.persistent_object.double_sided_image_urls);        //両面画像の登録
                    Console.WriteLine(rootobject.persistent_object.hidden_double_sided_image_urls); //両面画像(放送非表示)の登録
                    Console.WriteLine(rootobject.persistent_object.nicovideo_ids);                  //ニコニコ動画の再生DVDの登録⇒廃止コンバート用
                }


                /* ==== 背景にパノラマ画像を登録 ==== */
                Console.WriteLine(rootobject.background);                                           //背景にパノラマ画像を登録
                if (rootobject.background != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.background.panorama);                              //背景にパノラマ画像を登録
                    Console.WriteLine(rootobject.background.panorama.source_urls);                  //画像URLリスト
                    Console.WriteLine(rootobject.background.chroma_key_background_color);           //背景クロマキー
                }


                /* ==== キーコントロール ==== */
                Console.WriteLine(rootobject.keyboard);                                             //キーコントロール
                if (rootobject.keyboard != null)
                {/* ==== Json要素確認 ==== */

                    Console.WriteLine(rootobject.keyboard.switch_rendering_to_desktop);             //デスクトップへの描画内容を切り替えるキー

                    Console.WriteLine(rootobject.keyboard.keycode_vci_forward);                     //VCI操作キー：Forward 
                    Console.WriteLine(rootobject.keyboard.keycode_vci_backward);                    //VCI操作キー：Backward
                    Console.WriteLine(rootobject.keyboard.keycode_vci_left);                        //VCI操作キー：Left
                    Console.WriteLine(rootobject.keyboard.keycode_vci_right);                       //VCI操作キー：Right
                    Console.WriteLine(rootobject.keyboard.keycode_vci_up);                         //VCI操作キー：Up
                    Console.WriteLine(rootobject.keyboard.keycode_vci_down);                        //VCI操作キー：Down
                    Console.WriteLine(rootobject.keyboard.keycode_vci_1);                           //VCI操作キー：Key1
                    Console.WriteLine(rootobject.keyboard.keycode_vci_2);                           //VCI操作キー：Key2
                    Console.WriteLine(rootobject.keyboard.keycode_vci_3);                           //VCI操作キー：Key3
                    Console.WriteLine(rootobject.keyboard.keycode_vci_4);                           //VCI操作キー：Key4
                }


                /* ==== VCIアイテムのデバッグ設定 ==== */
                Console.WriteLine(rootobject.embedded_script);                                      //VCIアイテムのデバッグ設定
                if (rootobject.embedded_script != null)
                {/* ==== Json要素確認 ==== */
                    Console.WriteLine(rootobject.embedded_script.websocket_console_port);           //WebSocket Loggerポート番号
                    Console.WriteLine(rootobject.embedded_script.moonsharp_debugger_port);          //MoonSharpDebug のプラグイン
                }


                /* ==== 起動モードの変更 ==== */
                Console.WriteLine(rootobject.mode);                                                 //起動モードの変更

                /* ==== TCP接続を使用するかどうか (デフォルトはUDP) ==== */
                Console.WriteLine(rootobject.use_tcp);
                
                /* ==== 使用中のVRコントローラの種別 ==== */
                Console.WriteLine(rootobject.vr_input_controller_type);

                /* ==== The Looking Glassに対応 ==== */
                Console.WriteLine(rootobject.enable_looking_glass);                                 //The Looking Glassに対応

                /* ==== VIVE Pro Eyeの設定 ==== */
                Console.WriteLine(rootobject.enable_vivesranipal_eye);                              //VIVE Pro Eyeの眼球の動きを検出するかどうか
                Console.WriteLine(rootobject.vivesranipal_eye_adjust_x);                            //左右の眼球の移動量
                Console.WriteLine(rootobject.vivesranipal_eye_adjust_y);                            //上下の眼球の移動量
                Console.WriteLine(rootobject.enable_vivesranipal_blink);                            //VIVE Pro Eyeのまばたきの動きを検出するかどうか
                Console.WriteLine(rootobject.enable_vivesranipal_lip);                              //VR リップシンク
                Console.WriteLine(rootobject.enable_vivesranipal_eye_with_emotion);                 //表情変更中でも眼球操作を使用する

                /* ==== 振動の強弱を調整できる。0にすると無振動になってしまう。 === */
                Console.WriteLine(rootobject.vibration_power);

                /* ==== DataTable反映 ==== */
                if (rootobject.niconico != null)
                {/* ==== ニコニコ反映確認 ==== */

                    if (rootobject.niconico.broadcaster_comments != null)
                    {/* ====運営コメントの登録確認 ==== */
                        if (rootobject.niconico.broadcaster_comments.Length >= 0)
                        {
                            /* ====  運営コメントの登録総数取得 ==== */
                            int MaxCharacter = rootobject.niconico.broadcaster_comments.Length - 1;
                            int CountCharacter;

                            for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                            {/* ====  運営コメントの登録を選択 ==== */

                                DataTable[DataSelectionClass.AdminComment].Rows.Add(CountCharacter + 1, true, rootobject.niconico.broadcaster_comments[CountCharacter].ToString());

                            }
                        }/* ==== END_運営コメントの登録の登録 ==== */

                    }/* ==== END_運営コメントの登録確認 ==== */


                    if (rootobject.niconico.ng_score_threshold != null)
                    {/* ====ニコ生コメント NGスコアしきい値確認 ==== */

                        ThresholdUpDown.Value = (Int32)rootobject.niconico.ng_score_threshold;

                    }/* ==== END_ニコ生コメント NGスコアしきい値の登録確認 ==== */

                }/* ==== END_ニコニコ反映確認 ==== */


                if (rootobject.import != null)
                {/* ==== 初期動画URL ==== */

                    if (rootobject.import.video_content_uris.Length >= 0)
                    {
                        /* ==== 動画のURLの登録総数取得 ==== */
                        int MaxCharacter = rootobject.import.video_content_uris.Length - 1;
                        int CountCharacter;

                        for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                        {/* ====  動画のURLの登録を選択 ==== */

                            DataTable[DataSelectionClass.Dvd].Rows.Add(CountCharacter + 1, true, rootobject.import.video_content_uris[CountCharacter].ToString(), "", "");

                        }
                    }/* ==== END_動画のURLの登録 ==== */



                }/* ==== END_初期動画URL ==== */


                if (rootobject.comment != null)
                {/* ==== 184コメント ==== */

                    if (rootobject.comment.display_anonymous_card != null)
                    {/* ==== 184のコメント窓の生成を許可 ==== */

                        NgCommentWindowBox.Checked = rootobject.comment.display_anonymous_card;

                    }/* ==== END_184のコメント窓の生成を許可 ==== */

                    if (rootobject.comment.display_anonymous_flying != null)
                    {/* ==== 184の落ちてくるコメント生成 ==== */

                        NgFallingCommentsBox.Checked = rootobject.comment.display_anonymous_flying;

                    }/* ==== END_184の落ちてくるコメント生成 ==== */

                }/* ==== END_184コメント ==== */


                if (rootobject.humanoid != null)
                {/* ==== 移動の設定確認 ==== */

                    if (rootobject.humanoid.use_fast_spring_bone != null)
                    {/* ==== SpringBone最適化確認 ==== */

                        SpringBoneCheckBox.Checked = rootobject.humanoid.use_fast_spring_bone;

                    }/* ==== END_SpringBone最適化確認 ==== */

                }/* ==== END_移動の設定確認 ==== */


                if (rootobject.studio != null)
                {/* ==== ダイレクトビューモードでの入室許可 ==== */

                    if (rootobject.studio.max_performers != null)
                    {/* ==== スタジオに存在出来る演者の最大人数 ==== */

                        MaxStudioNumericUpDown.Value = rootobject.studio.max_performers;

                    }/* ==== END_スタジオに存在出来る演者の最大人数 ==== */


                    if (rootobject.studio.max_direct_viewers != null)
                    {/* ==== スタジオに存在出来るダイレクトビューモード使用者の最大人数 ==== */

                        MaxDirectViewNumericUpDown.Value = rootobject.studio.max_direct_viewers;

                    }/* ==== END_スタジオに存在出来るダイレクトビューモード使用者の最大人数 ==== */

                }/* ==== END_ダイレクトビューモードでの入室許可確認 ==== */


                if (rootobject.item != null)
                {/* ==== アイテム登録 ==== */

                    if (rootobject.item.whiteboard != null)
                    {/* ==== ホワイトボードの画像登録 ==== */

                        if (rootobject.item.whiteboard.source_urls != null)
                        {/* ==== 画像URLリスト ==== */

                            if (rootobject.item.whiteboard.source_urls.Length >= 0)
                            {
                                /* ====  画像URLリスト総数取得 ==== */
                                int MaxCharacter = rootobject.item.whiteboard.source_urls.Length - 1;
                                int CountCharacter;

                                for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                                {/* ====  画像URLリストを選択 ==== */

                                    DataTable[DataSelectionClass.Board].Rows.Add(CountCharacter + 1, true, rootobject.item.whiteboard.source_urls[CountCharacter].ToString(), "", "");

                                }
                            }/* ==== END_画像URLリスト ==== */

                        }/* ==== 画像URLリスト ==== */

                    }/* ==== ホワイトボードの画像登録 ==== */


                    if (rootobject.item.cue_card != null)
                    {/* ==== カンペの画像登録 ==== */

                        if (rootobject.item.cue_card.source_urls != null)
                        {/* ==== 画像URLリスト ==== */

                            if (rootobject.item.cue_card.source_urls.Length >= 0)
                            {
                                /* ====  画像URLリスト総数取得 ==== */
                                int MaxCharacter = rootobject.item.cue_card.source_urls.Length - 1;
                                int CountCharacter;

                                for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                                {/* ====  画像URLリストを選択 ==== */

                                    DataTable[DataSelectionClass.Campe].Rows.Add(CountCharacter + 1, true, rootobject.item.cue_card.source_urls[CountCharacter].ToString(), "", "");

                                }
                            }/* ==== END_画像URLリスト ==== */

                        }/* ==== 画像URLリスト ==== */

                    }/* ==== カンペの画像登録登録 ==== */


                    if (rootobject.item.enable_displaycapture_chromakey != null)
                    {/* ==== ディスプレイアイテムのクロマキー合成確認 ==== */

                        DisplayChromakeyBox.Checked = rootobject.item.enable_displaycapture_chromakey;

                    }/* ==== END_ディスプレイアイテムのクロマキー合成確認 ==== */


                    if (rootobject.item.enable_nicovideo_chromakey != null)
                    {/* ==== ニコニコ動画プレイヤーのクロマキー合成確認 ==== */

                        NicovideoChromakeyBox.Checked = rootobject.item.enable_nicovideo_chromakey;

                    }/* ==== END_ニコニコ動画プレイヤーのクロマキー合成確認 ==== */



                    if (rootobject.item.enable_videoboard_chromakey != null)
                    {/* ==== 画面キャプチャークロマキー合成確認 ==== */

                        CaptureCheckBox.Checked = rootobject.item.enable_videoboard_chromakey;

                    }/* ==== END_/画面キャプチャークロマキー合成確認 ==== */


                    if (rootobject.item.projectable_item != null)
                    {/* ==== 画像のキャッシュを有効にする ==== */

                        if (rootobject.item.projectable_item.enable_cache_all != null)
                        {/* ==== キャッシュを有効にする ==== */

                            ImageCacheBox.Checked = rootobject.item.projectable_item.enable_cache_all;

                        }/* ==== END_キャッシュを有効にする ==== */

                    }/* ==== END_画像のキャッシュを有効にする ==== */


                    if (rootobject.item.capture_resolution != null)
                    {/* ==== カメラ解像度の設定 ==== */

                        capture_resolution_Index = ResolutionComboBox.FindStringExact(rootobject.item.capture_resolution.ToString());

                        if (capture_resolution_Index == -1)
                        {
                            ++capture_resolution_Index;
                        }

                        ResolutionComboBox.SelectedIndex = capture_resolution_Index;

                    }/* ==== END_カメラ解像度の設定 ==== */


                    if (rootobject.item.movie_audio_recording_enabled != null)
                    {/* ==== ムービー撮影時の録音を有効にする ==== */

                        VideoRecBox.Checked = rootobject.item.movie_audio_recording_enabled;

                    }/* ==== END_ムービー撮影時の録音を有効にする ==== */


                    if (rootobject.item.capture_format != null)
                    {/* ==== キャプチャ撮影、ホワイトボードの保存形式 ==== */

                        ImageFormat_Index = ImageFormatComboBox.FindStringExact(rootobject.item.capture_format.ToString());

                        if (ImageFormat_Index == -1)
                        {
                            ++ImageFormat_Index;
                        }

                        ImageFormatComboBox.SelectedIndex = ImageFormat_Index;

                    }/* ==== END_キャプチャ撮影、ホワイトボードの保存形式 ==== */


                    if (rootobject.item.steam_screenshot != null)
                    {/* ==== Steamスクリーンショット ==== */

                        SteamCheckBox.Checked = rootobject.item.steam_screenshot;

                    }/* ==== END_Steamスクリーンショット ==== */


                    if (rootobject.item.hide_camera_from_viewers != null)
                    {/* ==== 視聴者にカメラの表示設定確認 ==== */

                        CameraCheckBox.Checked = rootobject.item.hide_camera_from_viewers;

                    }/* ==== END_視聴者にカメラの表示設定確認 ==== */


                    if (rootobject.item.disable_highlighting_item != null)
                    {/* ==== 触れたアイテムをハイライトを無効 ==== */

                        ItemHighlightCheckBox.Checked = rootobject.item.disable_highlighting_item;

                    }/* ==== END_触れたアイテムをハイライトを無効 ==== */


                }/* ==== END_アイテム登録 ==== */



                if (rootobject.persistent_object != null)
                {/* ==== 画像や動画等の登録 ==== */

                    if (rootobject.persistent_object.image_urls != null)
                    {/* ====永続化画像の登録確認 ==== */
                        if (rootobject.persistent_object.image_urls.Length >= 0)
                        {
                            /* ====  永続化画像の登録総数取得 ==== */
                            int MaxCharacter = rootobject.persistent_object.image_urls.Length - 1;
                            int CountCharacter;

                            for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                            {/* ====  永続化画像の登録を選択 ==== */

                                DataTable[DataSelectionClass.Viewer].Rows.Add(CountCharacter + 1, true, rootobject.persistent_object.image_urls[CountCharacter].ToString(), "", "");

                            }
                        }/* ==== END_ 永続化画像の登録 ==== */

                    }/* ==== END_ 永続化画像の登録確認 ==== */


                    if (rootobject.persistent_object.hidden_image_urls != null)
                    {/* ====永続化画像(放送非表示)の登録確認 ==== */
                        if (rootobject.persistent_object.hidden_image_urls.Length >= 0)
                        {
                            /* ====  永続化画像(放送非表示)の登録総数取得 ==== */
                            int MaxCharacter = rootobject.persistent_object.hidden_image_urls.Length - 1;
                            int CountCharacter;

                            for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                            {/* ====  永続化画像(放送非表示)の登録を選択 ==== */

                                DataTable[DataSelectionClass.MyData].Rows.Add(CountCharacter + 1, true, rootobject.persistent_object.hidden_image_urls[CountCharacter].ToString(), "", "");

                            }
                        }/* ==== END_永続化画像(放送非表示)の登録 ==== */

                    }/* ==== END_永続化画像(放送非表示)の登録確認 ==== */


                    if (rootobject.persistent_object.double_sided_image_urls != null)
                    {/* ====両面画像の登録確認 ==== */
                        if (rootobject.persistent_object.double_sided_image_urls.Length >= 0)
                        {
                            /* ====  両面画像の登録総数取得 ==== */
                            int MaxCharacter = rootobject.persistent_object.double_sided_image_urls.Length - 1;
                            int CountCharacter;

                            for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                            {/* ====  両面画像の登録を選択 ==== */

                                DataTable[DataSelectionClass.Revers].Rows.Add(CountCharacter + 1, true, rootobject.persistent_object.double_sided_image_urls[CountCharacter][0].ToString(), "リンク画像" + (CountCharacter + 1), "");
                                DataTable[DataSelectionClass.BackRevers].Rows.Add(CountCharacter + 1, "リンク画像" + (CountCharacter + 1), rootobject.persistent_object.double_sided_image_urls[CountCharacter][1].ToString());
                                BackDataLink.Add("リンク画像" + (CountCharacter + 1));

                            }

                            ComboBoxIndex[DataSelectionClass.BackRevers].Items.AddRange(BackDataLink.ToArray());

                        }/* ==== END_両面画像の登録 ==== */

                    }/* ==== END_両面画像の登録確認 ==== */


                    if (rootobject.persistent_object.hidden_double_sided_image_urls != null)
                    {/* ====両面画像(放送非表示)の登録確認 ==== */
                        if (rootobject.persistent_object.hidden_double_sided_image_urls.Length >= 0)
                        {
                            /* ====  両面画像(放送非表示)の登録総数取得 ==== */
                            int MaxCharacter = rootobject.persistent_object.hidden_double_sided_image_urls.Length - 1;
                            int CountCharacter;

                            for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                            {/* ====  両面画像(放送非表示)の登録を選択 ==== */

                                DataTable[DataSelectionClass.MyRevers].Rows.Add(CountCharacter + 1, true, rootobject.persistent_object.hidden_double_sided_image_urls[CountCharacter][0].ToString(), "リンク画像" + (CountCharacter + 1), "");
                                DataTable[DataSelectionClass.BackMyRevers].Rows.Add(CountCharacter + 1, "リンク画像" + (CountCharacter + 1), rootobject.persistent_object.hidden_double_sided_image_urls[CountCharacter][1].ToString());
                                BackMyDataLink.Add("リンク画像" + (CountCharacter + 1));

                            }

                            ComboBoxIndex[DataSelectionClass.BackMyRevers].Items.AddRange(BackDataLink.ToArray());

                        }/* ==== END_両面画像(放送非表示)の登録 ==== */

                    }/* ==== END_ 両面画像(放送非表示)の登録確認 ==== */


                    if (rootobject.persistent_object.nicovideo_ids != null)
                    {/* ====ニコニコ動画の再生DVDの登録確認 ==== */
                        if (rootobject.persistent_object.nicovideo_ids.Length >= 0)
                        {
                            /* ====  ニコニコ動画の再生DVDの登録総数取得 ==== */
                            int MaxCharacter = rootobject.persistent_object.nicovideo_ids.Length - 1;
                            int CountCharacter;

                            for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                            {/* ====  ニコニコ動画の再生DVDの登録を選択 ==== */

                                int index2 = rootobject.persistent_object.nicovideo_ids[CountCharacter].ToString().IndexOf("sm");

                                if (index2 == 0)
                                {
                                    rootobject.persistent_object.nicovideo_ids[CountCharacter] = "https://www.nicovideo.jp/watch/" + rootobject.persistent_object.nicovideo_ids[CountCharacter].ToString();
                                }

                                DataTable[DataSelectionClass.Dvd].Rows.Add(CountCharacter + 1, true, rootobject.persistent_object.nicovideo_ids[CountCharacter].ToString(), "", "");

                            }
                        }/* ==== END_ニコニコ動画の再生DVDの登録 ==== */

                    }/* ==== END_ニコニコ動画の再生DVDの登録確認 ==== */

                }/* ==== 画像や動画等の登録 ==== */



                if (rootobject.background != null)
                {/* ==== 背景にパノラマ画像を登録 ==== */

                    if (rootobject.background.panorama != null)
                    {/* ==== 背景にパノラマ画像を登録 ==== */

                        if (rootobject.background.panorama.source_urls != null)
                        {/* ====画像URLリスト確認 ==== */
                            if (rootobject.background.panorama.source_urls.Length >= 0)
                            {
                                /* ====  画像URLリスト総数取得 ==== */
                                int MaxCharacter = rootobject.background.panorama.source_urls.Length - 1;
                                int CountCharacter;

                                for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                                {/* ====  画像URLリストを選択 ==== */

                                    DataTable[DataSelectionClass.Back].Rows.Add(CountCharacter + 1, true, rootobject.background.panorama.source_urls[CountCharacter].ToString(), "", "");

                                }
                            }/* ==== END_画像URLリスト ==== */

                        }/* ==== END_画像URLリスト確認 ==== */

                    }/* ==== 背景にパノラマ画像を登録 ==== */


                    if (rootobject.background.chroma_key_background_color != null)
                    {/* ==== 背景クロマキー ==== */
                        ChromaCollar_Inbex = ChromaKeyColorComboBox.FindStringExact(rootobject.background.chroma_key_background_color.ToString());

                        if (ChromaCollar_Inbex == -1)
                        {
                            ++ChromaCollar_Inbex;
                        }

                        ChromaKeyColorComboBox.SelectedIndex = ChromaCollar_Inbex;
                    }/* ==== END_背景クロマキー ==== */

                }/* ==== 背景にパノラマ画像を登録 ==== */


                if (rootobject.keyboard != null)
                {/* === キーコントロール === */
                    //配列に変換する

                    var IndexKyeControl = KyeControl.Cast<string>();


                    if (rootobject.keyboard.switch_rendering_to_desktop != null)
                    {/* === デスクトップへの描画内容を切り替えるキー === */


                        StopDrawingTextBox.Text = rootobject.keyboard.switch_rendering_to_desktop.ToString();

                    }
                    else
                    {
                        StopDrawingTextBox.Text = "X";
                    }/* === END デスクトップへの描画内容を切り替えるキー === */


                    if (rootobject.keyboard.keycode_vci_forward != null)
                    {/* === UpArrowタブ === */
                        KeyForward_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_forward.ToString());

                        KeyForward_Inbex -= KyeItem;

                        if (KeyForward_Inbex == -1)
                        {
                            ++KeyForward_Inbex;
                        }

                        KeyForwardBox.SelectedIndex = KeyForward_Inbex;
                    }
                    else
                    {
                        KeyForwardBox.SelectedIndex = ini_ForwardBox;
                    }/* === END UpArrowタブ === */


                    if (rootobject.keyboard.keycode_vci_backward != null)
                    {/* === Backwardタブ === */
                        KeyBackward_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_backward.ToString());

                        KeyBackward_Inbex -= KyeItem;

                        if (KeyBackward_Inbex == -1)
                        {
                            ++KeyBackward_Inbex;
                        }

                        KeyBackwardBox.SelectedIndex = KeyBackward_Inbex;
                    }
                    else
                    {
                        KeyBackwardBox.SelectedIndex = ini_BackwardBox;
                    }/* === END_Backwardタブ === */


                    if (rootobject.keyboard.keycode_vci_left != null)
                    {/* === Leftタブ === */

                        KeyLeft_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_left.ToString());

                        KeyLeft_Inbex -= KyeItem;

                        if (KeyLeft_Inbex == -1)
                        {
                            ++KeyLeft_Inbex;
                        }

                        KeyLeftBox.SelectedIndex = KeyLeft_Inbex;
                    }
                    else
                    {
                        KeyLeftBox.SelectedIndex = ini_LeftBox;
                    }/* === END_Leftタブ === */


                    if (rootobject.keyboard.keycode_vci_left != null)
                    {/* === Rightタブ === */

                        KeyRight_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_right.ToString());

                        KeyRight_Inbex -= KyeItem;

                        if (KeyRight_Inbex == -1)
                        {
                            ++KeyRight_Inbex;
                        }

                        KeyRightBox.SelectedIndex = KeyRight_Inbex;
                    }
                    else
                    {
                        KeyRightBox.SelectedIndex = ini_RightBox;
                    }/* === END_Rightタブ === */


                    if (rootobject.keyboard.keycode_vci_up != null)
                    {/* === Upタブ === */

                        KeyUp_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_up.ToString());

                        KeyUp_Inbex -= KyeItem;

                        if (KeyUp_Inbex == -1)
                        {
                            ++KeyUp_Inbex;
                        }

                        KeyUpBox.SelectedIndex = KeyUp_Inbex;
                    }
                    else
                    {
                        KeyUpBox.SelectedIndex = ini_UpBox;
                    }/* === END_Upタブ === */


                    if (rootobject.keyboard.keycode_vci_down != null)
                    {/* === Downタブ === */

                        KeyDown_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_down.ToString());

                        KeyDown_Inbex -= KyeItem;

                        if (KeyDown_Inbex == -1)
                        {
                            ++KeyDown_Inbex;
                        }

                        KeyDownBox.SelectedIndex = KeyDown_Inbex;
                    }
                    else
                    {
                        KeyDownBox.SelectedIndex = ini_DownBox;
                    }/* === END_Downタブ === */


                    if (rootobject.keyboard.keycode_vci_1 != null)
                    {/* === Key1タブ === */

                        KeyKey1_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_1.ToString());

                        KeyKey1_Inbex -= KyeItem;

                        if (KeyKey1_Inbex == -1)
                        {
                            ++KeyKey1_Inbex;
                        }

                        KeyKey1Box.SelectedIndex = KeyKey1_Inbex;
                    }
                    else
                    {
                        KeyKey1Box.SelectedIndex = ini_Key1Box;
                    }/* === END_Key1タブ === */


                    if (rootobject.keyboard.keycode_vci_2 != null)
                    {/* === Key2タブ === */

                        KeyKey2_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_2.ToString());

                        KeyKey2_Inbex -= KyeItem;

                        if (KeyKey2_Inbex == -1)
                        {
                            ++KeyKey2_Inbex;
                        }

                        KeyKey2Box.SelectedIndex = KeyKey2_Inbex;
                    }
                    else
                    {
                        KeyKey2Box.SelectedIndex = ini_Key2Box;
                    }/* === END_Key2タブ === */


                    if (rootobject.keyboard.keycode_vci_3 != null)
                    {/* === Key3タブ === */

                        KeyKey3_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_3.ToString());

                        KeyKey3_Inbex -= KyeItem;

                        if (KeyKey3_Inbex == -1)
                        {
                            ++KeyKey3_Inbex;
                        }

                        KeyKey3Box.SelectedIndex = KeyKey3_Inbex;
                    }
                    else
                    {
                        KeyKey3Box.SelectedIndex = ini_Key3Box;
                    }/* === END_Key3タブ === */


                    if (rootobject.keyboard.keycode_vci_4 != null)
                    {/* === Key4タブ === */

                        KeyKey4_Inbex = Array.LastIndexOf(IndexKyeControl.ToArray(), rootobject.keyboard.keycode_vci_4.ToString());

                        KeyKey4_Inbex -= KyeItem;

                        if (KeyKey4_Inbex == -1)
                        {
                            ++KeyKey4_Inbex;
                        }

                        KeyKey4Box.SelectedIndex = KeyKey4_Inbex;
                    }
                    else
                    {
                        KeyKey4Box.SelectedIndex = ini_Key4Box;
                    }/* === Key4タブ === */

                }/* === キーコントロール === */


                if (rootobject.embedded_script != null)
                {/* ==== VCIアイテムのデバッグ設定確認 ==== */

                    if (rootobject.embedded_script.websocket_console_port != null)
                    {/* ==== デバッグ情報コンソールポート確認 ==== */

                        PortUpDown.Value = rootobject.embedded_script.websocket_console_port;

                    }/* ==== END_デバッグ情報コンソールポート確認 ==== */


                    if (rootobject.embedded_script.moonsharp_debugger_port != null)
                    {/* ==== デバッグ情報コンソール画面 ==== */

                        MSPortUpDown.Value = rootobject.embedded_script.moonsharp_debugger_port;

                    }/* ==== END_デバッグ情報コンソール画面 ==== */

                }/* ==== END_VCIアイテムのデバッグ設定確認 ==== */


                if (rootobject.mode != null)
                {/* ==== 起動モードの変更確認 ==== */

                    if (rootobject.mode == "direct-view")
                    {/* ==== 起動モードの変更確認 ==== */

                        DirectCheckBox.Checked = true;
                    }

                }/* ==== END_起動モードの変更確認 ==== */


                if (rootobject.use_tcp != null)
                {/* ==== TCP接続を使用するかどうか (デフォルトはUDP) ==== */

                    TcpCheckBox.Checked = rootobject.use_tcp;

                }/* ==== END_TCP接続を使用するかどうか (デフォルトはUDP) ==== */


                if (rootobject.vr_input_controller_type != null)
                {/* ==== 使用中のVRコントローラの種別 ==== */

                    ControllerComboBox_Index = ControllerComboBox.FindStringExact(rootobject.vr_input_controller_type.ToString());

                    if (ControllerComboBox_Index == -1)
                    {
                        ++ControllerComboBox_Index;
                    }

                    ControllerComboBox.SelectedIndex = ControllerComboBox_Index;

                }/* ==== END_使用中のVRコントローラの種別 ==== */

                if (rootobject.enable_looking_glass != null)
                {/* ==== The Looking Glassに対応確認 ==== */

                    LookingGlassCheckBox.Checked = rootobject.enable_looking_glass;

                }/* ==== END_The Looking Glassに対応確認 ==== */


                if (rootobject.enable_vivesranipal_eye != null)
                {/* ==== VIVE Pro Eyeの眼球の動きを検出するかどうか ==== */

                    EyeDetectionCheckBox.Checked = rootobject.enable_vivesranipal_eye;

                }/* ==== END_VIVE Pro Eyeの眼球の動きを検出するかどうか ==== */


                if (rootobject.vivesranipal_eye_adjust_x != null)
                {/* ==== 左右の眼球の移動量 ==== */

                    Move_X_NumericUpDown.Value = (decimal)rootobject.vivesranipal_eye_adjust_x;

                }
                else 
                {
                    Move_X_NumericUpDown.Value = (decimal)1;
                }
                /* ==== END_左右の眼球の移動量 ==== */


                if (rootobject.vivesranipal_eye_adjust_y != null)
                {/* ==== 上下の眼球の移動量 ==== */

                    Move_Y_NumericUpDown.Value = (decimal)rootobject.vivesranipal_eye_adjust_y;

                }
                else
                {
                    Move_Y_NumericUpDown.Value = (decimal)1;
                }
                /* ==== END_上下の眼球の移動量 ==== */


                if (rootobject.enable_vivesranipal_blink != null)
                {/* ==== VIVE Pro Eyeのまばたきの動きを検出するかどうか ==== */

                    BlinkDetectionCheckBox.Checked = rootobject.enable_vivesranipal_blink;

                }/* ==== END_VIVE Pro Eyeのまばたきの動きを検出するかどうか ==== */


                if (rootobject.enable_vivesranipal_lip != null)
                {/* ==== VR リップシンク ==== */

                    LipSynchCheckBox.Checked = rootobject.enable_vivesranipal_lip;

                }/* ==== END_VR リップシンク ==== */

                if (rootobject.enable_vivesranipal_eye_with_emotion != null)
                {/* ==== 表情変更中でも眼球操作を使用する ==== */

                    EmotionCheckBox.Checked = rootobject.enable_vivesranipal_eye_with_emotion;

                }/* ==== END_表情変更中でも眼球操作を使用する ==== */

                if ((EyeDetectionCheckBox.Checked == true) || (BlinkDetectionCheckBox.Checked == true))
                {/* === EyeTrackingCheckBox の制御 === */

                    EyeTrackingCheckBox.Checked = true;

                }/* === END_EyeTrackingCheckBox の制御 === */



                if (rootobject.vibration_power != null)
                {/* ==== 振動の強弱を調整できる。0にすると無振動になってしまう ==== */

                    VibeNumericUpDown.Value = (decimal)rootobject.vibration_power;

                }
                else 
                {
                    VibeNumericUpDown.Value = (decimal)0.5;
                }
                /* ==== END_振動の強弱を調整できる。0にすると無振動になってしまう ==== */


            }/* ==== END_DataTable反映 ==== */
            else
                {/* ==== Jsonフォイルが無い ==== */

                    Console.WriteLine("ファイルがまだ作成されていないようです");

                }/* ==== END_Jsonフォイルが無い ==== */

            }/* ==== END_Json読み込み ==== */
        

        private void LoadXmlConfig(string fileName)
        {/* ==== XML読み込み ==== */

            /* XmlSerializerオブジェクトを作成 */
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
            /* 読み込むファイルを開く */
            StreamReader sr = new StreamReader(
                fileName, new System.Text.UTF8Encoding(false));
            /* XMLファイルから読み込み、逆シリアル化する */
            SampleClass obj = (SampleClass)serializer.Deserialize(sr);

            /* データ復元 */
            Frontmost_ToolStripMenuItem.Checked = obj.FrontmostCheck;   //前面表示
            if (obj.FrontmostCheck == true)
            {
                TopMost = !TopMost;
            }
            DirectCheckBox.Checked = obj.DirectCheckBox;                        //ダイレクトビューモード


            LookingGlassCheckBox.Checked = obj.LookingGlassCheckBox;                        //LookingGlass表示設定

            CameraCheckBox.Checked = obj.CameraCheckBox;                                    //カメラ非表示
            VideoRecBox.Checked = obj.VideoRecBox;                                          //ムービー撮影時の録音
            ImageCacheBox.Checked = obj.ImageCacheBox;                                      //画像のキャッシュ
            DisplayChromakeyBox.Checked = obj.DisplayChromakeyBox;                          //ディスプレイクロマキー
            NicovideoChromakeyBox.Checked = obj.NicovideoChromakeyBox;                      //動画プレイヤークロマキー
            CaptureCheckBox.Checked = obj.CaptureCheckBox;                                  //動画キャプチャーのクロマキー
            ItemHighlightCheckBox.Checked = obj.ItemHighlightCheckBox;                      //アイテムのハイライトを非表示
            NgCommentWindowBox.Checked = obj.NgCommentWindowBox;                            //184のコメント窓の生成を許可
            NgFallingCommentsBox.Checked = obj.NgFallingCommentsBox;                        //184の落ちてくるコメント生成
            SpringBoneCheckBox.Checked = obj.SpringBoneCheckBox;                            //SpringBone最適化
            TcpCheckBox.Checked = obj.TcpCheckBox;                                          //TCPで接続する
            EyeTrackingCheckBox.Checked = obj.EyeTrackingCheckBox;                          //EyeTrackingMode
            LipSynchCheckBox.Checked = obj.LipSynchCheckBox;                                //リップシンク
            SteamCheckBox.Checked = obj.SteamCheckBox;                                      //Steamライブラリの追加
            EyeDetectionCheckBox.Checked = obj.EyeDetectionCheckBox;                        //VIVE Pro Eyeの眼球の動きを検出するかどうか 
            BlinkDetectionCheckBox.Checked = obj.BlinkDetectionCheckBox;                    //VIVE Pro Eyeのまばたきの動きを検出するかどうか  
            EmotionCheckBox.Checked = obj.EmotionCheckBox;                                  //表情変更中でも眼球操作を使用する
            ThresholdUpDown.Value = obj.ThresholdUpDown;                                    //NGコメントしきい値
            PortUpDown.Value = obj.PortUpDown;                                              //WebSocket Loggerポート番号
            MSPortUpDown.Value = obj.MSPortUpDown;                                          //MoonSharpDebug のプラグイン
            MaxDirectViewNumericUpDown.Value = (decimal)obj.MaxDirectViewNumericUpDown;     //ダイレクトビュー最大人数
            VibeNumericUpDown.Value = (decimal)obj.VibeNumericUpDown;                       //コントローラ振動の強さ
            Move_X_NumericUpDown.Value = (decimal)obj.Move_X_NumericUpDown;                 //上下の眼球の移動量
            Move_Y_NumericUpDown.Value = (decimal)obj.Move_Y_NumericUpDown;                 //左右の眼球の移動量


            //デスクトップへ描画停止を切り替えるキー
            if (obj.StopDrawingTextBox == null || obj.StopDrawingTextBox == "" || obj.StopDrawingTextBox == " " || obj.StopDrawingTextBox == "　")
            {
                StopDrawingTextBox.Text = "X";
            }
            else 
            {
                StopDrawingTextBox.Text = obj.StopDrawingTextBox;
            }

            //最大スタジオ人数
            if ((Int32)obj.MaxStudioNumericUpDown <= 1)
            {
                MaxStudioNumericUpDown.Value = (decimal)4;
            }
            else
            {
                MaxStudioNumericUpDown.Value = (decimal)obj.MaxStudioNumericUpDown;
            }


            //キャプチャ撮影、ホワイトボードの保存形式
            ImageFormat_Index = ImageFormatComboBox.FindStringExact(obj.ImageFormatComboBox);

            if(ImageFormat_Index == -1)
            {
                ++ ImageFormat_Index;
            }

            ImageFormatComboBox.SelectedIndex = ImageFormat_Index;

            //カメラ解像度
            capture_resolution_Index = ResolutionComboBox.FindStringExact(obj.ResolutionComboBox);

            if (capture_resolution_Index == -1)
            {
                ++capture_resolution_Index;
            }

            ResolutionComboBox.SelectedIndex = capture_resolution_Index;


            //スタジオ クロマキーの配色
            ChromaCollar_Inbex = ChromaKeyColorComboBox.FindStringExact(obj.ChromaKeyColorComboBox);

            if (ChromaCollar_Inbex == -1)
            {
                ++ChromaCollar_Inbex;
            }

            ChromaKeyColorComboBox.SelectedIndex = ChromaCollar_Inbex;

            //コントローラ種別
            ControllerComboBox_Index = ControllerComboBox.FindStringExact(obj.ControllerComboBox);

            if (ControllerComboBox_Index == -1)
            {
                ++ControllerComboBox_Index;
            }

            ControllerComboBox.SelectedIndex = ControllerComboBox_Index;


            if (obj.KeyForwardBox != null)
            {/* === UpArrow === */
                KeyForward_Inbex = KeyForwardBox.FindStringExact(obj.KeyForwardBox);

                if (KeyForward_Inbex == -1)
                {
                    ++KeyForward_Inbex;
                }

                KeyForwardBox.SelectedIndex = KeyForward_Inbex;
            }
            else
            {
                    KeyForwardBox.SelectedIndex = ini_ForwardBox;
            }/* === END_UpArrow === */


            if (obj.KeyBackwardBox != null)
            {/* === Backward === */
                
                KeyBackward_Inbex = KeyBackwardBox.FindStringExact(obj.KeyBackwardBox);

                if (KeyBackward_Inbex == -1)
                {
                    ++KeyBackward_Inbex;
                }

                KeyBackwardBox.SelectedIndex = KeyBackward_Inbex;
            }
            else
            {
                KeyBackwardBox.SelectedIndex = ini_BackwardBox;
            }/* === END_Backward === */


            if (obj.KeyLeftBox != null)
            {/* === Left === */
                
                KeyLeft_Inbex = KeyLeftBox.FindStringExact(obj.KeyLeftBox);

                if (KeyLeft_Inbex == -1)
                {
                    ++KeyLeft_Inbex;
                }

                KeyLeftBox.SelectedIndex = KeyLeft_Inbex;
            }
            else
            {
                KeyLeftBox.SelectedIndex = ini_LeftBox;
            }/* === END_Left === */


            if (obj.KeyRightBox != null)
            {/* === Right === */

                KeyRight_Inbex = KeyRightBox.FindStringExact(obj.KeyRightBox);

                if (KeyRight_Inbex == -1)
                {
                    ++KeyRight_Inbex;
                }

                KeyRightBox.SelectedIndex = KeyRight_Inbex;
            }
            else
            {
                KeyRightBox.SelectedIndex = ini_RightBox;
            }/* === END_Right === */

            if (obj.KeyUpBox != null)
            {/* === Up === */

                KeyUp_Inbex = KeyUpBox.FindStringExact(obj.KeyUpBox);

                if (KeyUp_Inbex == -1)
                {
                    ++KeyUp_Inbex;
                }

                KeyUpBox.SelectedIndex = KeyUp_Inbex;
            }
            else
            {
                KeyUpBox.SelectedIndex = ini_UpBox;
            }/* === END_Up === */


            if (obj.KeyDownBox != null)
            {/* === Down === */

                KeyDown_Inbex = KeyDownBox.FindStringExact(obj.KeyDownBox);

                if (KeyDown_Inbex == -1)
                {
                    ++KeyDown_Inbex;
                }

                KeyDownBox.SelectedIndex = KeyDown_Inbex;
            }
            else
            {
                KeyDownBox.SelectedIndex = ini_DownBox;
            }/* === END_Down === */


            if (obj.KeyKey1Box != null)
            {/* === Key1 === */

                KeyKey1_Inbex = KeyKey1Box.FindStringExact(obj.KeyKey1Box);

                if (KeyKey1_Inbex == -1)
                {
                    ++KeyKey1_Inbex;
                }

                KeyKey1Box.SelectedIndex = KeyKey1_Inbex;
            }
            else
            {
                KeyKey1Box.SelectedIndex = ini_Key1Box;
            }/* === END_Key1 === */

            if (obj.KeyKey2Box != null)
            {/* === Key2 === */

                KeyKey2_Inbex = KeyKey2Box.FindStringExact(obj.KeyKey2Box);

                if (KeyKey2_Inbex == -1)
                {
                    ++KeyKey2_Inbex;
                }

                KeyKey2Box.SelectedIndex = KeyKey2_Inbex;
            }
            else
            {
                KeyKey2Box.SelectedIndex = ini_Key2Box;
            }/* === END_Key2 === */


            if (obj.KeyKey3Box != null)
            {/* === Key3 === */
             
                KeyKey3_Inbex = KeyKey3Box.FindStringExact(obj.KeyKey3Box);

                if (KeyKey3_Inbex == -1)
                {
                    ++KeyKey3_Inbex;
                }

                KeyKey3Box.SelectedIndex = KeyKey3_Inbex;
            }
            else
            {
                KeyKey3Box.SelectedIndex = ini_Key3Box;
            }/* === END_Key3 === */


            if (obj.KeyKey4Box != null)
            {/* === Key4 === */
             
                KeyKey4_Inbex = KeyKey4Box.FindStringExact(obj.KeyKey4Box);

                if (KeyKey4_Inbex == -1)
                {
                    ++KeyKey4_Inbex;
                }

                KeyKey4Box.SelectedIndex = KeyKey4_Inbex;
            }
            else
            {
                KeyKey4Box.SelectedIndex = ini_Key4Box;
            }/* === END_Key4 === */

            //視聴者用両面画像
            BackDataLink = obj.BackDataLink;                            
            LinkComboBox.Items.Clear();
            LinkComboBox.Items.Add("未設定");
            LinkComboBox.Items.AddRange(BackDataLink.ToArray());
            
            //自分用両面画像
            BackMyDataLink = obj.BackMyDataLink;                        
            MyLinkComboBox.Items.Clear();
            MyLinkComboBox.Items.Add("未設定");
            MyLinkComboBox.Items.AddRange(BackMyDataLink.ToArray());

            /* ファイルを閉じる */
            sr.Close();

            /* ==== DirectCheck表示の初期化 ==== */
            LookingGlassView_ini();

        }/* ==== END_XML書き出し ==== */

        /* SampleClassオブジェクトをXMLファイルに保存する */
        public void SaveConfig(string fileName)
        {

            /* 保存するクラス(SampleClass)のインスタンスを作成 */
            SampleClass obj = new SampleClass
            {

                /* テキストボックス保存用 */
                FrontmostCheck = Frontmost_ToolStripMenuItem.Checked,                       //最前面表示
                DirectCheckBox = DirectCheckBox.Checked,                                    //ダイレクトビューモード
                LookingGlassCheckBox = LookingGlassCheckBox.Checked,                        //LookingGlass表示設定

                CameraCheckBox = CameraCheckBox.Checked,                                    //カメラ非表示
                VideoRecBox = VideoRecBox.Checked,                                          //ムービー撮影時の録音
                ImageCacheBox = ImageCacheBox.Checked,                                      //画像のキャッシュ
                DisplayChromakeyBox = DisplayChromakeyBox.Checked,                          //ディスプレイクロマキー
                NicovideoChromakeyBox = NicovideoChromakeyBox.Checked,                      //動画プレイヤークロマキー
                CaptureCheckBox = CaptureCheckBox.Checked,                                  //動画キャプチャーのクロマキー
                ItemHighlightCheckBox = ItemHighlightCheckBox.Checked,                      //アイテムのハイライトを非表示
                NgCommentWindowBox = NgCommentWindowBox.Checked,                            //184のコメント窓の生成を許可
                NgFallingCommentsBox = NgFallingCommentsBox.Checked,                        //184の落ちてくるコメント生成
                SpringBoneCheckBox = SpringBoneCheckBox.Checked,                            //SpringBone最適化    
                TcpCheckBox = TcpCheckBox.Checked,                                          //TCP接続を使用する
                EyeTrackingCheckBox = EyeTrackingCheckBox.Checked,                          //EyeTrackingMode
                LipSynchCheckBox = LipSynchCheckBox.Checked,                                //リップシンク
                SteamCheckBox = SteamCheckBox.Checked,                                      //Steamライブラリの追加
                EyeDetectionCheckBox = EyeDetectionCheckBox.Checked,                        //VIVE Pro Eyeの眼球の動きを検出するかどうか 
                BlinkDetectionCheckBox = BlinkDetectionCheckBox.Checked,                    //VIVE Pro Eyeのまばたきの動きを検出するかどうか
                EmotionCheckBox = EmotionCheckBox.Checked,                                  //表情変更中でも眼球操作を使用する
                ThresholdUpDown = (Int32)ThresholdUpDown.Value,                             //NGコメントしきい値
                PortUpDown = (Int32)PortUpDown.Value,                                       //WebSocket Loggerポート番号
                MSPortUpDown = (Int32)MSPortUpDown.Value,                                   //MoonSharpDebug のポート番号
                MaxStudioNumericUpDown = (Int32)MaxStudioNumericUpDown.Value,               //最大スタジオ人数
                MaxDirectViewNumericUpDown = (Int32)MaxDirectViewNumericUpDown.Value,       //ダイレクトビュー最大人数
                VibeNumericUpDown = (float)VibeNumericUpDown.Value,                         //コントローラ振動の強さ
                Move_X_NumericUpDown = (float)Move_X_NumericUpDown.Value,                   //上下の眼球の移動量 
                Move_Y_NumericUpDown = (float)Move_Y_NumericUpDown.Value,                   //左右の眼球の移動量
                ImageFormatComboBox = ImageFormatComboBox.SelectedItem.ToString(),          //キャプチャ撮影、ホワイトボードの保存形式
                ResolutionComboBox = ResolutionComboBox.SelectedItem.ToString(),                         //カメラ解像度
                ChromaKeyColorComboBox = ChromaKeyColorComboBox.SelectedItem.ToString(),    //スタジオ クロマキーの配色
                ControllerComboBox = ControllerComboBox.SelectedItem.ToString(),                         //コントローラ種別
                StopDrawingTextBox = StopDrawingTextBox.Text,                         //デスクトップへ描画停止を切り替えるキー
                KeyForwardBox = KeyForwardBox.SelectedItem.ToString(),                      //UpArrow
                KeyBackwardBox = KeyBackwardBox.SelectedItem.ToString(),                    //Backwardタブ
                KeyLeftBox = KeyLeftBox.SelectedItem.ToString(),                            //Leftタブ
                KeyRightBox = KeyRightBox.SelectedItem.ToString(),                          //Rightタブ
                KeyUpBox = KeyUpBox.SelectedItem.ToString(),                                //Upタブ
                KeyDownBox = KeyDownBox.SelectedItem.ToString(),                            //Downタブ
                KeyKey1Box = KeyKey1Box.SelectedItem.ToString(),                            //Key1タブ
                KeyKey2Box = KeyKey2Box.SelectedItem.ToString(),                            //Key2タブ
                KeyKey3Box = KeyKey3Box.SelectedItem.ToString(),                            //Key3タブ
                KeyKey4Box = KeyKey4Box.SelectedItem.ToString(),                            //Key4タブ
                BackDataLink = BackDataLink,                                                //視聴者用両面画像
                BackMyDataLink = BackMyDataLink                                             //自分用両面画像

            };


            /* XmlSerializerオブジェクトを作成 */
            /* オブジェクトの型を指定する */
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
            /* 書き込むファイルを開く（UTF-8 BOM無し）*/
            StreamWriter sw = new StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            /* シリアル化し、XMLファイルに保存する */
            serializer.Serialize(sw, obj);
            /* ファイルを閉じる */
            sw.Close();
        }/* END_SampleClassオブジェクトをXMLファイルに保存する */

        public class SampleClass
        {
            /*データ保存用*/
            public bool FrontmostCheck;             //前面表示
            public bool DirectCheckBox;             //ダイレクトビューモード
            public bool CameraCheckBox;             //カメラ非表示
            public bool VideoRecBox;                //ムービー撮影時の録音
            public bool ImageCacheBox;              //画像のキャッシュ
            public bool DisplayChromakeyBox;        //ディスプレイクロマキー
            public bool NicovideoChromakeyBox;      //動画プレイヤークロマキー
            public bool CaptureCheckBox;            //動画キャプチャーのクロマキー
            public bool ItemHighlightCheckBox;      //アイテムのハイライトを非表示
            public bool NgCommentWindowBox;         //184のコメント窓の生成を許可
            public bool NgFallingCommentsBox;       //184の落ちてくるコメント生成
            public bool LookingGlassCheckBox;       //Looking Glass表示設定
            public bool SpringBoneCheckBox;         //SpringBone最適化
            public bool TcpCheckBox;                //TCP接続を使用する
            public bool EyeTrackingCheckBox;        //EyeTrackingMode
            public bool LipSynchCheckBox;           //リップシンク
            public bool SteamCheckBox;              //Steamライブラリの追加
            public bool EyeDetectionCheckBox;       //VIVE Pro Eyeの眼球の動きを検出するかどうか 
            public bool BlinkDetectionCheckBox;     //VIVE Pro Eyeのまばたきの動きを検出するかどうか
            public bool EmotionCheckBox;            //表情変更中でも眼球操作を使用する
            public Int32 ThresholdUpDown;           //NGコメントしきい値
            public Int32 PortUpDown;                //WebSocket Loggerポート番号
            public Int32 MSPortUpDown;              //MoonSharpDebugポート番号
            public Int32 MaxStudioNumericUpDown;    //最大スタジオ人数
            public Int32 MaxDirectViewNumericUpDown;//ダイレクトビュー最大人数

            public float VibeNumericUpDown;         //コントローラ振動の強さ
            public float Move_X_NumericUpDown;      //上下の眼球の移動量 
            public float Move_Y_NumericUpDown;      //左右の眼球の移動量

            public string ImageFormatComboBox;      //キャプチャ撮影、ホワイトボードの保存形式
            public string ResolutionComboBox;       //カメラ解像度
            public string ChromaKeyColorComboBox;   //スタジオ クロマキーの配色
            public string ControllerComboBox;       //コントローラ種別

            public string StopDrawingTextBox;       //デスクトップへ描画停止を切り替えるキー
            public string KeyForwardBox;            //UpArrow
            public string KeyBackwardBox;           //Backwardタブ
            public string KeyLeftBox;               //Leftタブ
            public string KeyRightBox;              //Rightタブ
            public string KeyUpBox;                 //Upタブ
            public string KeyDownBox;               //Downタブ
            public string KeyKey1Box;               //Key1タブ
            public string KeyKey2Box;               //Key2タブ
            public string KeyKey3Box;               //Key3タブ
            public string KeyKey4Box;               //Key4タブ

            public ArrayList BackDataLink;          //視聴者用両面画像
            public ArrayList BackMyDataLink;        //自分用両面画像

        }

        /* ==== 終了ボタン ==== */
        private void END_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            /*自分自身のフォームをCloseメソッドで閉じると、アプリケーションが終了する*/
            Application.Exit();
            // Close();

        }/* ==== END_終了ボタン ==== */


        /* ==== 最前面表示 ==== */
        private void Frontmost_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            //チェク状態を反転させる
            item.Checked = !item.Checked;
            TopMost = !TopMost;

        }/* ==== END_最前面表示 ==== */


        /* ==== 設定表示 ==== */
        private void VCEConfig_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            /* ==== 設定フォーム生成 ==== */
            using (VCJE_Config VCEConf = new VCJE_Config())
            {
                /* ==== マニュアルモードで生成 ==== */
                VCJE_Config VCEConfDlg = new VCJE_Config
                {

                    StartPosition = FormStartPosition.Manual

                };/* ==== END_マニュアルモードで生成 ==== */

                VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

            }/* ==== END_設定フォーム生成 ==== */

        }/* ==== END_設定表示 ==== */


        /* ==== 更新履歴表示 ==== */
        private void UpdateConfig_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            /* ==== 更新履歴フォーム生成 ==== */
            using (VCJE_UpdateConfig VCJE_UpdateConfig = new VCJE_UpdateConfig())
            {
                /* ==== マニュアルモードで生成 ==== */
                VCJE_UpdateConfig VCJE_UpdateConfigDlg = new VCJE_UpdateConfig
                {

                    StartPosition = FormStartPosition.Manual

                };/* ==== END_マニュアルモードで生成 ==== */

                VCJE_UpdateConfigDlg.Left = Left + (Width - VCJE_UpdateConfigDlg.Width) / 2;
                VCJE_UpdateConfigDlg.Top = Top + (Height - VCJE_UpdateConfigDlg.Height) / 2;
                VCJE_UpdateConfigDlg.Owner = this; // 常に親ウィンドウの手前に表示
                VCJE_UpdateConfigDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

            }/* ==== END_更新履歴フォーム生成 ==== */

        }/* ==== END_更新履歴表示 ==== */


        /* ==== Help表示 ==== */
        private void Help_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            /* ==== ヘルプフォーム生成 ==== */
            using (VCJE_Help VCEHelp = new VCJE_Help())
            {
                /* ==== マニュアルモードで生成 ==== */
                VCJE_Help VCEHelpDlg = new VCJE_Help
                {

                    StartPosition = FormStartPosition.Manual

                };/* ==== END_マニュアルモードで生成 ==== */

                VCEHelpDlg.Left = Left + (Width - VCEHelpDlg.Width) / 2;
                VCEHelpDlg.Top = Top + (Height - VCEHelpDlg.Height) / 2;
                VCEHelpDlg.Owner = this; // 常に親ウィンドウの手前に表示
                VCEHelpDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

            }/* ==== END_ヘルプフォーム生成 ==== */

        }/* ==== END_Help表示 ==== */


        /*===========================================================================*/
        /*      メインフォーム                                                       */
        /*===========================================================================*/

        /* ==== APIトークン編集ロック ==== */
        private void ApiRockCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            ApiTextBox.Enabled = !ApiRockCheckBox.Checked;                          //編集有無
            Properties.Settings.Default.LockSlide4vrAPI = ApiRockCheckBox.Checked;  //有無の設定保存
            Properties.Settings.Default.Save();//トークンを保存する

        }//END_APIトークン編集ロック ==== */

        /* ==== APIトークン手動貼り付け用 ==== */
        private void ApiTextBox_Leave(object sender, EventArgs e)
        {
            // 文字の入力位置（キャレット）を末尾に設定する
            ApiTextBox.SelectionStart = ApiTextBox.Text.Length;
            Properties.Settings.Default.Slide4vrAPIToken = ApiTextBox.Text;
            Properties.Settings.Default.Save();//トークンを保存する

            /* ==== 別スレット ==== */
            var task1 = Task.Run(() =>
            {

                //Slide4Vrサーバーアイドル開始
                var AppAccess = ConnectedSlide4VR.GetSlide4VrSlide(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, "AppAccess",0 , Debug_Slide4Vr);

            });/* ==== END_別スレット ==== */



        }//END_APIトークン手動貼り付け用 ==== */


        /* ==== APIトークン貼り付け ==== */
        private void PastingButton_Click(object sender, EventArgs e)
        {

            if (ApiTextBox.Enabled == true)
            {//編集有無確認
                string InputText = "";
                IDataObject ClipboardData = Clipboard.GetDataObject(); // クリップボードからオブジェクトを取得する。
                if (ClipboardData.GetDataPresent(DataFormats.Text)) // テキストデータかどうか確認する。
                {
                    InputText = (string)ClipboardData.GetData(DataFormats.Text); // オブジェクトからテキストを取得する。
                    Properties.Settings.Default.Slide4vrAPIToken = InputText;
                    Properties.Settings.Default.Save();//トークンを保存する
                    ApiTextBox.Enabled = false; //編集を無効にする
                    ApiRockCheckBox.Checked = true; //編集を無効にする

                }
                else
                {

                    MessageBox.Show("コピーしたデータが文字列ではありません。");

                }

                ApiTextBox.Text = InputText; // TextBoxに表示する。
                // 文字の入力位置（キャレット）を末尾に設定する
                ApiTextBox.SelectionStart = ApiTextBox.Text.Length;

            }
            else
            {
                MessageBox.Show("APIトークンの入力がロックされています。");

            }// END_編集有無確認

            /* ==== 別スレット ==== */
            var task1 = Task.Run(() =>
            {

                //Slide4Vrサーバーアイドル開始
                var AppAccess = ConnectedSlide4VR.GetSlide4VrSlide(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, "AppAccess", 0, Debug_Slide4Vr);

            });/* ==== END_別スレット ==== */


        }/* ==== APIトークン貼り付け ==== */


        /* ==== PdfFileボタンクリックイベント ==== */
        private void PdfFileButton_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "SlideData.pdf"; // 既定のファイル名
            openFileDialog1.DefaultExt = ".pdf"; // 既定のファイル拡張子
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "PDF files (*.pdf;*.pptx)|*.pdf;*.pptx";

            // ダイアログボックスの表示
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたファイルをテキストボックスに表示する
                PdfFileBox.Text = openFileDialog1.FileName;
                // 文字の入力位置（キャレット）を末尾に設定する
                PdfFileBox.SelectionStart = PdfFileBox.Text.Length;
            }

        }/* ==== END_PdfFileFileボタンクリックイベント ==== */


        /* ==== アップロードボタン ==== */
        private void SlideUploadButtonn_ClickAsync(object sender, EventArgs e)
        {

            /* ==== 誤送信防止 === */
            if (GetSlide4VrListButton.Enabled == false || GetSlide4VrSlideButton.Enabled == false || Slide4VrDeleteButton.Enabled == false)
            {

                DialogResult dr = MessageBox.Show("現在「Slide4VR」サーバーと通信中です。" +
                    System.Environment.NewLine + "通信完了後操作してください。",
                    "注意", MessageBoxButtons.OK);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {

                    return;

                }


            }/* ==== END_誤送信防止 === */


            /* ==== 誤送信防止 === */
            if (SlideNameTextBox.Text.Length == 0 || PdfFileBox.Text.Length == 0)
            {

                DialogResult dr = MessageBox.Show("「スライド名」と「スライドのファイル」を指定してください。",
                    "注意", MessageBoxButtons.OK);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {

                    return;

                }


            }/* ==== END_誤送信防止 === */

            string FilePdf = PdfFileBox.Text.Substring(PdfFileBox.Text.LastIndexOf(".") + 1);

            /* ==== 誤送信防止 === */
            if (FilePdf == "pdf" || FilePdf == "pptx") { }
            else
            {

                DialogResult dr = MessageBox.Show("「PDF」を指定してください。",
                    "注意", MessageBoxButtons.OK);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {

                    return;

                }


            }/* ==== END_誤送信防止 === */

            /* ==== APIトークン確認 === */
            if (ApiTextBox.Text.Length != 0)
            {

                DialogResult dr = MessageBox.Show( "スライドのアップロード容量は3MB, 80枚以下程度を目安にしてください。"+
                   System.Environment.NewLine + "アップロードしますか？",
                    "参考", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {

                    SlideUploadButton.Enabled = false;

                    string[] Slide4VrSlideUpload;
                    string[,] Slide4VrUploadResult;

                    /* ==== 別スレット ==== */
                    var UpResultTask = Task.Run(async () =>
                    {
                        Slide4VrSlideUpload = await ConnectedSlide4VR.SlideUpload_Async(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, PdfFileBox.Text, SlideNameTextBox.Text, 4, Debug_Slide4Vr);

                        //// Listの内容を配列にコピーします。
                        //GetUploadResultList[0] = ListToArray[0].title;                              //スライドタイトル
                        //GetUploadResultList[1] = ListToArray[0].path;                               //確認用URL
                        //GetUploadResultList[2] = System.Convert.ToString(ListToArray[0].length);    //容量
                        //GetUploadResultList[3] = ListToArray[0].message;                            //エラーメッセージ
                        //GetUploadResultList[4] = ListToArray[0].error;                              //エラー

                        if (Slide4VrSlideUpload[4] == null)
                        {

                            this.Invoke((Action)(() =>
                            {//別のスレットでUI制御します

                                InfoBalloonMesage("アップロードのお知らせ。","タイトル：" +
                                    Slide4VrSlideUpload[0] +
                                    System.Environment.NewLine + "容量：" +
                                    Slide4VrSlideUpload[2] + "Byte", 500);
                                SlideUploadButton.Enabled = true;

                            }));// END_別のスレットでUI制御します


                            //URLからスライドKyeを抽出
                            Slide4VrSlideUpload[1] = Slide4VrSlideUpload[1].Substring(Slide4VrSlideUpload[1].LastIndexOf("/") + 1);
                            

                            /* ==== アップロード反映確認 ==== */
                            //15秒に1回チェックタスク
                           
                            //No_GetSlideID　⇒　IDが無いからUpload中もしくは失敗かも？リトライ数回かな
                            /* マルチタスク */
                            Task task = new Task(() =>
                            {
                                string SlideName = Slide4VrSlideUpload[0];
                                int RetryInc = 0;
                                bool BreakStatus = false;
                                bool BreakCheck = false;

                                while (BreakCheck == false)
                                {/*=== 定期チャック ===*/
                                    //15秒間（15000ミリ秒）停止する
                                    System.Threading.Thread.Sleep(15000);

                                    Slide4VrUploadResult = ConnectedSlide4VR.GetSlide4VrUploadCheckReque(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, Slide4VrSlideUpload[1], 5, Debug_Slide4Vr);

                                    if (Slide4VrUploadResult[0, 0] == "True" || Slide4VrUploadResult[0, 0] == "true")
                                    {
                                        BreakCheck = true;
                                        BreakStatus = true;
                                    }
                                    else if (Slide4VrUploadResult[0, 0] == "No_GetSlideID" || Slide4VrUploadResult[0, 0] == "false" || Slide4VrUploadResult[0, 0] == "False")
                                    {
                                        RetryInc++;
                                        BreakStatus = false;

                                        if (RetryInc >= 20)
                                        {
                                            BreakCheck = true;
                                        }
                                    }
                                    //確認用
#if DEBUG
                                    InfoBalloonMesage("アップロードのお知らせ。", RetryInc + Slide4VrUploadResult[0, 0], 500);
#endif
                                }/*=== END_定期チャック ===*/

                                if (BreakStatus == true)
                                {
                                    this.Invoke((Action)(() =>
                                    {//別のスレットでUI制御します

                                        //ボタンの無効
                                        Slide4VrDeleteButton.Enabled = true;
                                        //Clickイベントを発生させる
                                        GetSlide4VrListButton.PerformClick();


                                    }));// END_別のスレットでUI制御します

                                    InfoBalloonMesage("アップロードのお知らせ。", "「" + SlideName + "」の画像変換に成功しました。", 500);
                                    
                                }
                                else
                                {
                                    InfoBalloonMesage("アップロードのお知らせ。", "「" + SlideName + "」の画像変換が失敗しました。" +
                                        System.Environment.NewLine + "容量や枚数を見直して再チャレンジしてください。", 500);
                                }

                                SlideUploadButton.Enabled = true;

                            });
                            task.Start();

                        }
                        else
                        {

                            this.Invoke((Action)(() =>
                            {//別のスレットでUI制御します

                                InfoBalloonMesage("アップロードのお知らせ。", "アップロードは失敗しました。" + 
                                    System.Environment.NewLine + "APIトークンが間違っている又は、" +
                                    System.Environment.NewLine + "選択したファイルをお確かめください", 500);
                                SlideUploadButton.Enabled = true;

                            }));// END_別のスレットでUI制御します

                        }


                    });/* ==== END_別スレット ==== */

                }


            }
            else
            {
                DialogResult dr = MessageBox.Show("APIトークンの入力がされていません。" +
                    System.Environment.NewLine + "Slide4VrでログインしAPIをAPIトークンにご入力ください。" +
                    System.Environment.NewLine + "ログインページに行きますか？", "確認", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    //ブラウザで開く
                    System.Diagnostics.Process.Start("https://slide4vr.nklab.dev/login/");
                }

            }/* ==== END_APIトークン確認 === */

        }/* ==== END_アップロードボタン ==== */


        /* ==== スライド一覧の取得 ====*/
        private void GetSlide4VrListButton_Click(object sender, EventArgs e)
        {

            /* ==== 誤送信防止 === */
            if (SlideUploadButton.Enabled == false || GetSlide4VrSlideButton.Enabled == false || Slide4VrDeleteButton.Enabled == false)
            {

                DialogResult dr = MessageBox.Show("現在「Slide4VR」サーバーと通信中です。"+
                    System.Environment.NewLine + "通信完了後操作してください。",
                    "注意", MessageBoxButtons.OK);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {

                    return;

                }


            }/* ==== END_誤送信防止 === */

            /* ==== APIトークン確認 === */
            if (ApiTextBox.Text.Length != 0)
            {
                //ボタン無効
                GetSlide4VrListButton.Enabled = false;

                /* ==== 別スレット ==== */
                var UpResultTask = Task.Run(() =>
                {
                    Slide4VrList = ConnectedSlide4VR.GetSlide4VrSlide(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, "SlideList", 1, Debug_Slide4Vr);

                    this.Invoke((Action)(() =>
                    {//別のスレットでUI制御します


                        try
                        {

                            if (Slide4VrList[0, 0].ToString() != "Not" || Slide4VrList[0, 0].ToString() != null)
                            {/* ==== スライド一覧登録 ==== */


                                /* === ComBoxクリア === */
                                Slide4VrComboBox.Items.Clear();

                                /* ====  画像URLリスト総数取得 ==== */
                                int MaxCharacter = Slide4VrList.GetLength(0) - 1;
                                int CountCharacter;

                                for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                                {/* ====  画像URLリストを選択 ==== */

                                    if (Slide4VrList[CountCharacter, 0] != null)
                                    {
                                        Slide4VrComboBox.Items.Add(Slide4VrList[CountCharacter, 0]);
                                    }
                                }

                            }
                            
                            /* ==== END_スライド一覧登録 ==== */

                            Slide4VrComboBox.SelectedIndex = 0;

                            //インデックス0のサムネ画像が画像の表示確認
                            if (System.Convert.ToBoolean(Slide4VrList[0, 1]) != false)
                            {
                                if (Slide4VrList[0, 2] != "")
                                {
                                    Slide4VrSelectPictureBox.ImageLocation = Slide4VrList[0, 2];    //画像の表示
                                }
                                else
                                {
                                    Slide4VrSelectPictureBox.Image = Properties.Resources.NoImage;
                                }

                            }
                            else
                            {
                                Slide4VrSelectPictureBox.Image = Properties.Resources.NoImage;
                            }// END_インデックス0のサムネ画像が画像の表示確認

                             //ボタン有効
                            GetSlide4VrListButton.Enabled = true;

                        }
                        catch
                        {

                            /* === ComBoxクリア === */
                            Slide4VrComboBox.Items.Clear();
                            Slide4VrComboBox.Items.Add("未取得");
                            Slide4VrComboBox.SelectedIndex = 0;
                            /* === 画像初期化 === */
                            Slide4VrSelectPictureBox.Image = Properties.Resources.NoImage;

                            //ボタン有効
                            GetSlide4VrListButton.Enabled = true;
                            return;

                        }

                    }));// END_別のスレットでUI制御します

                });/* ==== END_別スレット ==== */
            }
            else
            {

                DialogResult dr = MessageBox.Show("APIトークンの入力がされていません。" +
                    System.Environment.NewLine + "Slide4VrでログインしAPIをAPIトークンにご入力ください。" +
                    System.Environment.NewLine + "ログインページに行きますか？", "確認", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    //ブラウザで開く
                    System.Diagnostics.Process.Start("https://slide4vr.nklab.dev/login/");
                }

            }/* ==== END_APIトークン確認 === */

        }/* ==== END_スライド一覧の取得 ====*/


        /* ==== スライド選択の変更 ====*/
        private void Slide4VrComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Slide4VrList.Length != 0)
            {
                if (System.Convert.ToBoolean(Slide4VrList[Slide4VrComboBox.SelectedIndex, 1]) != false)
                {
                    if (Slide4VrList[Slide4VrComboBox.SelectedIndex, 2] != "")
                    {

                        Slide4VrSelectPictureBox.ImageLocation = Slide4VrList[Slide4VrComboBox.SelectedIndex, 2];    //画像の表示

                    }
                    else
                    {
                        Slide4VrSelectPictureBox.Image = Properties.Resources.NoImage;
                    }

                }
                else
                {
                    Slide4VrSelectPictureBox.Image = Properties.Resources.NoImage;
                }// END_インデックス0のサムネ画像が画像の表示確認

            }

        }/* ==== END_スライド選択の変更 ====*/


        /* ==== スライドの消去 ==== */
        private void Slide4VrDeleteButton_Click(object sender, EventArgs e)
        {

            /* ==== 誤送信防止 === */
            if (SlideUploadButton.Enabled == false || SlideUploadButton.Enabled == false || GetSlide4VrSlideButton.Enabled == false)
            {
            
                DialogResult dr = MessageBox.Show("現在「Slide4VR」サーバーと通信中です。" +
                    System.Environment.NewLine + "通信完了後操作してください。",
                    "注意", MessageBoxButtons.OK);
            
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
            
                    return;
            
                }
            
            
            }/* ==== END_誤送信防止 === */

            if (Slide4VrComboBox.Text != "未取得" && ApiTextBox.Text.Length != 0)
            {/* ==== 未設定確認 ==== */

                //ボタンの無効
                Slide4VrDeleteButton.Enabled = false;
                int Slide4VrIndex = Slide4VrComboBox.SelectedIndex;

                string DeleteResult = "";
                /* ==== 別スレット ==== */
                var UpResultTask = Task.Run(async () =>
                {

                    try
                    {

                        DeleteResult = await ConnectedSlide4VR.SlideDelete_Async(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, Slide4VrList[Slide4VrIndex, 3], 6, Debug_Slide4Vr);
                        this.Invoke((Action)(() =>
                        {//別のスレットでUI制御します

                            //ボタンの無効
                            Slide4VrDeleteButton.Enabled = true;

                            if (DeleteResult == "Delete_OK")
                            {
                                
                                //Clickイベントを発生させる
                                GetSlide4VrListButton.PerformClick();

                            }

                        }));// END_別のスレットでUI制御します
                    }
                    catch
                    {

                        DialogResult dr = MessageBox.Show("「Slide4VR」サーバーと通信中に失敗しました。" +
                        System.Environment.NewLine + "時間を開けてトライしてください。",
                        "お知らせ", MessageBoxButtons.OK);

                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                        
                            //ボタン有効
                            Slide4VrDeleteButton.Enabled = true;
                            return;
                        }

                    }

                });/* ==== END_別スレット ==== */


            }/* ==== END_未設定確認 ==== */

        }/* ==== END_スライドの消去 ==== */


        /* ==== スライド名引用 ====*/
        private void TitleCopyButton_Click(object sender, EventArgs e)
        {

            if (Slide4VrList.Length != 0)
            {
                if (System.Convert.ToBoolean(Slide4VrList[Slide4VrComboBox.SelectedIndex, 1]) != false)
                {
                    if (Slide4VrList[Slide4VrComboBox.SelectedIndex, 2] != "")
                    {

                        SlideNameTextBox.Text = Slide4VrList[Slide4VrComboBox.SelectedIndex, 0].ToString();    //スライドタイトル

                    }
                    else
                    {
                        SlideNameTextBox.Text = "タイトル";
                    }

                }
                else
                {
                    SlideNameTextBox.Text = "タイトル";
                }

            }
            else
            {
                SlideNameTextBox.Text = "タイトル";
            }

        } /* ==== END_スライド名引用 ====*/


        /* ==== スライドOGPを表示 ====*/
        private void Slide4VrSelectPictureBox_Click(object sender, EventArgs e)
        {
            int Slide4VrIndex = Slide4VrComboBox.SelectedIndex;
            string[,] Slide4VrID = { };

            if (Slide4VrComboBox.Text != "未取得")
            {/* ==== 未設定確認 ==== */

                var UpResultTask = Task.Run(() =>
                {

                    Slide4VrID = ConnectedSlide4VR.GetSlide4VrSlide(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, "User_ID", 2, Debug_Slide4Vr);

                    this.Invoke((Action)(() =>
                    {//別のスレットでUI制御します

                        if (Slide4VrID[0, 0].ToString() != "Not" || Slide4VrID[0, 0].ToString() != null)
                        {/* ==== スライド一覧登録 ==== */

                            address = @"https://slide4vr.nklab.dev/slide/" + Slide4VrID[0, 0] + "/" + Slide4VrList[Slide4VrIndex, 3];

                            if (String.IsNullOrEmpty(address)) return;
                            if (address.Equals("about:blank")) return;
                            if (!address.StartsWith("http://") &&
                                !address.StartsWith("https://"))
                            {
                                address = "https://" + address;
                            }
                            try
                            {

                                webView1.Navigate(new Uri(address));

                            }
                            catch (System.UriFormatException)
                            {
                                return;
                            }

                        }

                    }));// END_別のスレットでUI制御します

                });/* ==== END_別スレット ==== */

            }/* ==== END_未設定確認 ==== */

        }/* ==== END_スライドOGPを表示 ====*/


        /* ==== Slide4Vr取り込み ==== */
        private void GetSlide4VrSlideButton_Click(object sender, EventArgs e)
        {


            /* ==== 誤送信防止 === */
            if (SlideUploadButton.Enabled == false || SlideUploadButton.Enabled == false || Slide4VrDeleteButton.Enabled == false)
            {

                DialogResult dr = MessageBox.Show("現在「Slide4VR」サーバーと通信中です。" +
                    System.Environment.NewLine + "通信完了後操作してください。",
                    "注意", MessageBoxButtons.OK);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {

                    return;

                }


            }/* ==== END_誤送信防止 === */


            /* ==== APIトークン確認 === */
            if (ApiTextBox.Text.Length != 0)
            {


                try
                {
                    //ボタン無効
                    GetSlide4VrSlideButton.Enabled = false;

                    int Slide4VrIndex = Slide4VrComboBox.SelectedIndex;

                    string[,] Slide4Vrwhiteboard = { };
                    /* ==== 別スレット ==== */
                    var UpResultTask = Task.Run(() =>
                    {

                        Slide4Vrwhiteboard = ConnectedSlide4VR.GetSlide4VrSlide(Properties.Settings.Default.Slide4vrAPIToken, VCJE_ExePass, Slide4VrList[Slide4VrIndex, 3], 3, Debug_Slide4Vr);

                        this.Invoke((Action)(() =>
                        {//別のスレットでUI制御します

                            if (Slide4Vrwhiteboard[0, 0].ToString() != "Not" || Slide4Vrwhiteboard[0, 0].ToString() != null)
                            {/* ==== ホワイトボードの画像登録 ==== */

                                ClearDataGrid(DataSelectionClass.Board);            //ホワイトボード画像の初期化

                                /* ====  画像URLリスト総数取得 ==== */
                                int MaxCharacter = Slide4Vrwhiteboard.Length - 1;
                                int CountCharacter;

                                for (CountCharacter = 0; MaxCharacter >= CountCharacter; CountCharacter++)
                                {/* ====  画像URLリストを選択 ==== */

                                    DataTable[DataSelectionClass.Board].Rows.Add(CountCharacter + 1, true, Slide4Vrwhiteboard[CountCharacter, 0].ToString(), "", "");

                                }

                            }/* ==== ホワイトボードの画像登録 ==== */

                            //ボタン有効
                            GetSlide4VrSlideButton.Enabled = true;

                        }));// END_別のスレットでUI制御します

                    });/* ==== END_別スレット ==== */
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("「Slide4VR」サーバーと通信中に失敗しました。" +
                    System.Environment.NewLine + "時間を開けてトライしてください。",
                    "お知らせ", MessageBoxButtons.OK);

                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {

                        return;

                    }


                } 

            }
            else
            {

                DialogResult dr = MessageBox.Show("APIトークンの入力がされていません。" +
                    System.Environment.NewLine + "Slide4VrでログインしAPIをAPIトークンにご入力ください。" +
                    System.Environment.NewLine + "ログインページに行きますか？", "確認", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    //ブラウザで開く
                    System.Diagnostics.Process.Start("https://slide4vr.nklab.dev/login/");
                }

            }/* ==== END_APIトークン確認 === */

        }/* ==== END_Slide4Vr取り込み ==== */


        /*　==== プロファイル設定の保存 ==== */
        private void ProfileSettingSaveButton_Click(object sender, EventArgs e)
        {

            //プリセット設定の保存
            if (Default_RadioButton.Checked == true)
            {

                Properties.Settings.Default.DataSelect = 0;                 //プリセットID
                Properties.Settings.Default.VCJEConfig = "VCJEConfig";      //設定データ
                Properties.Settings.Default.VCJEDataSet = "VCJEDataSet";    //DBデータ

            }
            else if (Preset1_RadioButton.Checked == true)
            {

                Properties.Settings.Default.DataSelect = 1;                 //プリセットID
                Properties.Settings.Default.VCJEConfig = "VCJEConfig_1";      //設定データ
                Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_1";    //DBデータ

            }
            else if (Preset2_RadioButton.Checked == true)
            {

                Properties.Settings.Default.DataSelect = 2;                 //プリセットID
                Properties.Settings.Default.VCJEConfig = "VCJEConfig_2";      //設定データ
                Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_2";    //DBデータ

            }
            else if (Preset3_RadioButton.Checked == true)
            {

                Properties.Settings.Default.DataSelect = 3;                 //プリセットID
                Properties.Settings.Default.VCJEConfig = "VCJEConfig_3";      //設定データ
                Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_3";    //DBデータ

            }
            else if (Preset4_RadioButton.Checked == true)
            {

                Properties.Settings.Default.DataSelect = 4;                 //プリセットID
                Properties.Settings.Default.VCJEConfig = "VCJEConfig_4";      //設定データ
                Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_4";    //DBデータ

            }

            Properties.Settings.Default.Save();

            //END_プリセット設定の保存


            /* ==== Xml保存 ==== */
            SaveConfig(Properties.Settings.Default.LocalPass + System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEConfig + ".xml"));
            VirtualDataSet.WriteXml(Properties.Settings.Default.LocalPass + System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEDataSet + ".xml"));

        }/*　==== END_プロファイル設定の保存 ==== */


        /*　==== プロファイル設定の読み込み ==== */
        private void ProfileSettingReadButton_Click(object sender, EventArgs e)
        {


            /* ==== ファイルディレクトリーチェック ==== */
            if (Properties.Settings.Default.LocalPass == "")
            {
                /* ==== 設定フォーム生成 ==== */
                using (VCJE_Config VCEConf = new VCJE_Config())
                {
                    MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                        + System.Environment.NewLine + "再度保存操作を行ってください。",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_Config VCEConfDlg = new VCJE_Config
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                    VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                    VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                }/* ==== END_設定フォーム生成 ==== */
            }
            else
            {

                /* ==== ファイルディレクトリーチェック ==== */
                if (Properties.Settings.Default.LocalPass == "")
                {
                    /* ==== 設定フォーム生成 ==== */
                    using (VCJE_Config VCEConf = new VCJE_Config())
                    {
                        MessageBox.Show("ConfigFormでVirtualCastのディレクトリーフォルダーを設定し、"
                            + System.Environment.NewLine + "再度保存操作を行ってください。",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        /* ==== マニュアルモードで生成 ==== */
                        VCJE_Config VCEConfDlg = new VCJE_Config
                        {

                            StartPosition = FormStartPosition.Manual

                        };/* ==== END_マニュアルモードで生成 ==== */

                        VCEConfDlg.Left = Left + (Width - VCEConfDlg.Width) / 2;
                        VCEConfDlg.Top = Top + (Height - VCEConfDlg.Height) / 2;
                        VCEConfDlg.Owner = this; // 常に親ウィンドウの手前に表示
                        VCEConfDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                    }/* ==== END_設定フォーム生成 ==== */
                }
                else
                {/* ==== XMLを開く ==== */

                    //データの一時バックアップ
                    int BakDataSelect = Properties.Settings.Default.DataSelect;         //プリセットID
                    string BakVCJEConfig = Properties.Settings.Default.VCJEConfig;      //設定データ
                    string BakVCJEDataSet = Properties.Settings.Default.VCJEDataSet;    //DBデータ
                    string SavePresetName = "";                                         //プリセット名

                    //プリセット設定の保存
                    if (Default_RadioButton.Checked == true)
                    {

                        SavePresetName = "Default";                                 //プリセット名
                        Properties.Settings.Default.DataSelect = 0;                 //プリセットID
                        Properties.Settings.Default.VCJEConfig = "VCJEConfig";      //設定データ
                        Properties.Settings.Default.VCJEDataSet = "VCJEDataSet";    //DBデータ

                    }
                    else if (Preset1_RadioButton.Checked == true)
                    {

                        SavePresetName = "Preset1";                                 //プリセット名
                        Properties.Settings.Default.DataSelect = 1;                 //プリセットID
                        Properties.Settings.Default.VCJEConfig = "VCJEConfig_1";      //設定データ
                        Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_1";    //DBデータ

                    }
                    else if (Preset2_RadioButton.Checked == true)
                    {

                        SavePresetName = "Preset2";                                 //プリセット名
                        Properties.Settings.Default.DataSelect = 2;                 //プリセットID
                        Properties.Settings.Default.VCJEConfig = "VCJEConfig_2";      //設定データ
                        Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_2";    //DBデータ

                    }
                    else if (Preset3_RadioButton.Checked == true)
                    {
                        SavePresetName = "Preset3";                                 //プリセット名
                        Properties.Settings.Default.DataSelect = 3;                 //プリセットID
                        Properties.Settings.Default.VCJEConfig = "VCJEConfig_3";      //設定データ
                        Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_3";    //DBデータ

                    }
                    else if (Preset4_RadioButton.Checked == true)
                    {
                        SavePresetName = "Preset4";                                 //プリセット名
                        Properties.Settings.Default.DataSelect = 4;                 //プリセットID
                        Properties.Settings.Default.VCJEConfig = "VCJEConfig_4";      //設定データ
                        Properties.Settings.Default.VCJEDataSet = "VCJEDataSet_4";    //DBデータ

                    }

                    Properties.Settings.Default.Save();

                    //END_プリセット設定の保存


                    /* ==== オープンチェックを開く ==== */
                    string LodeFile = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEDataSet + ".xml");
                    string LodeFile2 = System.IO.Path.Combine(@".\" + Properties.Settings.Default.VCJEConfig + ".xml");

                    if (File.Exists(Properties.Settings.Default.LocalPass + LodeFile) && File.Exists(Properties.Settings.Default.LocalPass + LodeFile2))
                    {/* ==== ファイルが既に存在する ==== */

                        /* ==== XMLオープン ==== */
                        LoadXmlConfig(Properties.Settings.Default.LocalPass + LodeFile2);
                        VirtualDataSet.Clear();  //  ReadXMLは追記なので一旦消す
                        VirtualDataSet.ReadXml(Properties.Settings.Default.LocalPass + LodeFile);

                        /* ==== オープン結果表示 ==== */
                        MessageBox.Show(
                            "「" + SavePresetName + "」のロードが終了しました",
                            "Xml Load Result",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Asterisk
                            );
                        /* ==== END?セーブ結果表示 ==== */
                    }
                    else
                    {/* ==== ファイルがが無い ==== */

                        //データの一時バックアップから復帰
                        Properties.Settings.Default.DataSelect = BakDataSelect;        //プリセットID
                        Properties.Settings.Default.VCJEConfig = BakVCJEConfig;      //設定データ
                        Properties.Settings.Default.VCJEDataSet = BakVCJEDataSet;    //DBデータ

                        Properties.Settings.Default.Save();

                        MessageBox.Show("「" + SavePresetName + "」はフォルダー内に存在しませんでした", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }/* ==== END_ファイルディレクトリーチェック ==== */

        }/*　==== END_プロファイル設定の読み込み ==== */


        /* ==== LookingGlass_&_ViewTalk表示設定 ==== */
        private void DirectCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            /* ==== LookingGlassの反転 ==== */
            LookingGlassCheckBox.Visible =! LookingGlassCheckBox.Visible;

            /* ==== LookingGlass_&_ViewTalkの無効 ==== */
            if (DirectCheckBox.Checked == false)
            {
                /* ==== LookingGlassの非表示 ==== */
                LookingGlassCheckBox.Checked = false;

            }/* ==== END_LookingGlasss_&_ViewTalkの無効 ==== */


        }/* ==== END_LookingGlass_&_ViewTalk表示設定 ==== */


        /* ==== NGレベルしきい値目安表示 ==== */
        private void NgLevelButton_Click(object sender, EventArgs e)
        {
            /* ==== 多重起動チェック ==== */
            if (Properties.Settings.Default.NgLevelCheck == false)
            {
                /* ==== 多重起動チェックフラグ ==== */
                Properties.Settings.Default.NgLevelCheck = true;
                Properties.Settings.Default.Save();

                /* ====  NGレベルしきいフォーム生成 ==== */
                using (VCJE_NgHelpe = new VCJE_NgHelpe())
                {
                    /* ==== マニュアルモードで生成 ==== */
                    VCJE_NgHelpe NG_HelpeDlg = new VCJE_NgHelpe
                    {

                        StartPosition = FormStartPosition.Manual

                    };/* ==== END_マニュアルモードで生成 ==== */

                    NG_HelpeDlg.Left = Left + (Width - NG_HelpeDlg.Width) / 2;
                    NG_HelpeDlg.Top = Top + (Height - NG_HelpeDlg.Height) / 2;
                    NG_HelpeDlg.Owner = this; // 常に親ウィンドウの手前に表示
                    NG_HelpeDlg.Show(this);// モードログとして表示 

                }/* ==== END_ NGレベルしきいフォーム生成 ==== */
            }/* ==== END_多重起動チェック ==== */

        }/* ==== END_NGレベルしきい値目安表示 ==== */


        /* ==== アイトラ設定 ==== */
        private void EyeTrackingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            /* ==== アイトラ設定タブの反転 ==== */
            EyeTabSettingChange();
            
            /* ==== アイトラ設定設定タブの初期化 ==== */
            if (EyeTrackingCheckBox.Checked == false)
            {
                /* ==== アイトラ設定設定タブの非表示 ==== */
                EyeSettings_ini();
            
            }
            /* ==== END_VCIデバック設定タブの無効 ==== */

        }/* ==== END_アイトラ設定 ==== */


        /* ==== メイン側DataGrid右クリック処理 ==== */
        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);

            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = TabData[TabIndex].HitTest(e.X,e.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    /* ==== IDとURL取得 ==== */
                    if (hit.RowIndex >= 0)
                    {
                        clickedCell = TabData[TabIndex].Rows[hit.RowIndex].Cells[2];
                    }
                    else
                    {
                        clickedCell = null;
                    }   

                }

                /* ==== 右クリックメニュー表示 ==== */
                RightMenuStri();

            }

        }/* ==== END_メイン側DataGrid右クリック処理 ==== */


        /* ==== サブ側DataGrid右クリック処理 ==== */
        private void SubDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);
            /* ==== タブインデックスSub切り替え ==== */
            TabIndex += 1; 

            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = TabData[TabIndex].HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    /* ==== IDとURL取得 ==== */
                    if (hit.RowIndex >= 0)
                    {
                        clickedCell = TabData[TabIndex].Rows[hit.RowIndex].Cells[2];
                    }
                    else
                    {
                        clickedCell = null;
                    }

                }

                /* ==== 右クリックメニュー表示 ==== */
                RightMenuStri();

            }

        }/* ==== END_サブ側DataGrid右クリック処理 ==== */


        /* ==== 右クリックメニュー表示 ==== */
        private void RightMenuStri()
        {

            //コンテキストメニューを表示する座標
            System.Drawing.Point p = System.Windows.Forms.Cursor.Position;

            //指定した画面上の座標位置にコンテキストメニューを表示する
            this.contextMenuStrip1.Show(p);

        }/* ==== END_右クリックメニュー表示 ==== */



        /* ==== メイン側コミット ==== */
        private void DataGridView_CurrentCellDirtyStateChanged(
        object sender, EventArgs e)
        {
            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);

            if (TabData[TabIndex].IsCurrentCellDirty)
            {
                //コミットする
                TabData[TabIndex].CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }/* ==== END_メイン側コミット ==== */

        /* ==== サブ側コミット ==== */
        private void SubDataGridView_CurrentCellDirtyStateChanged(
        object sender, EventArgs e)
        {
            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);
            /* ==== タブインデックスSub切り替え ==== */
            TabIndex += 1;

            if (TabData[TabIndex].IsCurrentCellDirty)
            {
                //コミットする
                TabData[TabIndex].CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }/* ==== END_サブ側コミット ==== */

        /* ==== Main_DataErrorイベントハンドラ ==== */
        private void Main_DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);

            if (e.Exception != null)
            {
                DataGridViewComboBoxColumn column = TabData[TabIndex].Columns["未設定"] as DataGridViewComboBoxColumn;
            }

        }/* ==== END_DataErrorイベントハンドラ ==== */

        /* ==== ドラック追加 ==== */
        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UniformResourceLocator") == true)
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;

                /* ==== ファイル追加用 ==== */
                //e.Effect = DragDropEffects.All;
            }
        }
        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {

            /* ==== 変数 ==== */
            int nicovideo;      //動画用
            int nico3d;         //立体用
            int mylist;         //マイリスト
            int mylist1;        //他人のマイリスト
            string UrlCheck;    //確認用

            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);


//            /* ==== ファイル操作 ==== */
//            if (e.Data.GetDataPresent(DataFormats.FileDrop))
//            {
//
//                // ドラッグ中のファイルやディレクトリの取得
//                string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);
//
//                foreach (string d in drags)
//                {
//
//                    if (!System.IO.File.Exists(d))
//                    {
//                        // ファイル以外であればイベント・ハンドラを抜ける
//                        return;
//                    }
//                }
//                e.Effect = DragDropEffects.Copy;
//            }/* ==== ファイル操作 ==== */


            string url = e.Data.GetData(DataFormats.Text).ToString();

            /* ==== URL切り替え ==== */
            switch (TabIndex)
            {

                case DataSelectionClass.Dvd://初期動画DVD
                case DataSelectionClass.Viewer://視聴者用固定画像
                case DataSelectionClass.MyData://自分用固定画像
                case DataSelectionClass.Back://背景画像
                case DataSelectionClass.Board://ホワイトボード画像
                case DataSelectionClass.Campe://カンペ画像
                    {

                        UrlCheck = url; 
                        nicovideo = UrlCheck.LastIndexOf("www.nicovideo.jp/watch/");
                        nico3d = UrlCheck.LastIndexOf("3d.nicovideo.jp/works/");
                        mylist = UrlCheck.LastIndexOf("www.nicovideo.jp/my/mylist/");
                        mylist1 = UrlCheck.LastIndexOf("www.nicovideo.jp/mylist/");
                        nicovideo = nicovideo * nico3d * mylist * mylist1;

                        if (nicovideo <= 0)
                        {
                            break;
                        }
                        else
                        {

                            DataTable[TabIndex].Rows.Add("", true, url, "", "");
                            break;

                        }
                    }

                case DataSelectionClass.Revers://視聴者用両面画像
                case DataSelectionClass.MyRevers:////自分用両面画像
                    {

                        UrlCheck = url;
                        nicovideo = UrlCheck.LastIndexOf("www.nicovideo.jp/watch/");
                        nico3d = UrlCheck.LastIndexOf("3d.nicovideo.jp/works/");
                        mylist = UrlCheck.LastIndexOf("www.nicovideo.jp/my/mylist/");
                        mylist1 = UrlCheck.LastIndexOf("www.nicovideo.jp/mylist/");

                        nicovideo = nicovideo * nico3d * mylist * mylist1;
                        if (nicovideo <= 0)
                        {
                            break;
                        }
                        else
                        {
                            /* ==== ダイアログ生成 ==== */
                            using (DataGrigDialogs DataGrig_Dialogs = new DataGrigDialogs())
                            {
                                /* ==== マニュアルモードで生成 ==== */
                                DataGrigDialogs DataGrigDialogsDlg = new DataGrigDialogs
                                {

                                    StartPosition = FormStartPosition.Manual

                                };/* ==== END_マニュアルモードで生成 ==== */

                                DataGrigDialogsDlg.Left = Left + (Width - DataGrigDialogsDlg.Width) / 2;
                                DataGrigDialogsDlg.Top = Top + (Height - DataGrigDialogsDlg.Height) / 2;
                                DataGrigDialogsDlg.Owner = this; // 常に親ウィンドウの手前に表示
                                DataGrigDialogsDlg.ShowDialog(this);// モードレス・ダイアログとして表示 

                            }

                            /* ==== おもてroうら確認 ==== */
                            if (Properties.Settings.Default.DataGridSelecter == true)
                            {/* ==== うら ==== */
                                TabIndex += 1;

                                DataTable[TabIndex].Rows.Add("", "", url, "");
                                break;
                            }
                            else
                            {/* ==== おもて ==== */
                                DataTable[TabIndex].Rows.Add("", true, url, "","");
                                break;
                            }/* ==== END_おもてroうら確認 ==== */

                        }

                    }

            }/* ==== END_URL切り替え ==== */

        }/* ==== END_ドラック追加 ==== */

        /* ==== ヘッダー行番号表示 ==== */
        private void DataGridView_CellPainting(object sender,
            DataGridViewCellPaintingEventArgs e)
        {

            //列ヘッダーかどうか調べる
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                //セルを描画する
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                //行番号を描画する範囲を決定する
                //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);
                //行番号を描画する
                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                //描画が完了したことを知らせる
                e.Handled = true;
            }/* ==== END_本スレット ==== */


        }/* ==== END_ヘッダー行番号表示 ==== */


        /* ==== KeyControlClear ==== */
        private void KeyControlAllButton_Click(object sender, EventArgs e)
        {
            //キーコントロールの初期化
            KeyControBox_Reset();
        }/* ==== END_KeyControlClear ==== */

        /* ==== データ消去 ==== */
        private void AllClearButton_Click(object sender, EventArgs e)
        {

            AllClear();

        }/* ==== END_データ消去 ==== */


        /* ==== URL反映 ==== */
        private void webView1_DOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs e)
        {
            UrlTextBox.Text = e.Uri.ToString();
        }/* ==== END_URL反映 ==== */


        /* ==== 戻る ==== */
        private void ReturnButton_Click(object sender, EventArgs e)
        {
            // ★★★前ページの履歴があるか？★★★
            if (webView1.CanGoBack == true)
            {
                // ★★★前ページに戻る★★★
                webView1.GoBack();
            }
        }/* ==== END_戻る ==== */


        /* ==== 進む ==== */
        private void MoveButton_Click(object sender, EventArgs e)
        {

            // ★★★次ページの履歴があるか？★★★
            if (webView1.CanGoForward == true)
            {
                // ★★★次ページに進む★★★
                webView1.GoForward();
            }

        }/* ==== END_進む ==== */

        /* ==== Googleホーム画面 ==== */
        private void GoogleButton_Click(object sender, EventArgs e)
        {
            //webView1.ScriptErrorsSuppressed = true;

            address = "www.google.com";

            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "https://" + address;
            }
            try
            {

                webView1.Navigate(new Uri(address));

            }
            catch (System.UriFormatException)
            {
                return;
            }
        }/* ==== END_Googleホーム画面 ==== */

        /* ==== 新しいウィンドウ ==== */
        private void webView1_NewWindow(Object sender, CancelEventArgs e)
        {

            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.Cancel);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "NewWindow Event");

        }/* ==== END_新しいウィンドウ ==== */


        /* ==== ブラウザ再読み込み ==== */
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            // ★★★再読み込みする★★★
            webView1.Refresh();

        }/* ==== END_ブラウザ再読み込み ==== */


        /* ==== エンターURL検索 ==== */
        private void UrlTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                //webBrowser1.ScriptErrorsSuppressed = true;

                string address = UrlTextBox.Text;         //更新確認ページ

                if (String.IsNullOrEmpty(address)) return;
                if (address.Equals("about:blank")) return;
                if (!address.StartsWith("http://") &&
                    !address.StartsWith("https://"))
                {
                    address = "http://" + address;
                }
                try
                {

                    webView1.Navigate(new Uri(address));
                }
                catch (System.UriFormatException)
                {
                    return;
                }
                //webBrowser1.Url = new Uri(address);
            }

            webView1.Focus();

        }/* ==== エンターURL検索 ==== */

        /* ==== URL検索 ==== */
        private void OpenButton_Click(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;

            string address = UrlTextBox.Text;         //更新確認ページ

            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {

                webView1.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
            //webBrowser1.Url = new Uri(address);

        }/* ==== END_URL検索 ==== */


        /* ==== Twitterのマイページ ==== */
        private void TwitterMoveLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //webBrowser1.ScriptErrorsSuppressed = true;

            address = "twitter.com/home";

            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "https://" + address;
            }
            try
            {
                webView1.Navigate(new Uri(address));

            }
            catch (System.UriFormatException)
            {
                return;
            }

        }/* ==== END_Twitterのマイページ ==== */


        /* ==== Discord ==== */
        private void DiscordMoveLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //webBrowser1.ScriptErrorsSuppressed = true;

            address = "https://discordapp.com/channels/@me";

            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "https://" + address;
            }
            try
            {
                webView1.Navigate(new Uri(address));

            }
            catch (System.UriFormatException)
            {
                return;
            }

        }/* ==== END_Discord ==== */


        /* ==== Slide4vr ==== */
        private void Slide4vrMoveLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //webBrowser1.ScriptErrorsSuppressed = true;

            address = "https://slide4vr.nklab.dev/login";

            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "https://" + address;
            }
            try
            {

                webView1.Navigate(new Uri(address));

            }
            catch (System.UriFormatException)
            {
                return;
            }

        }/* ==== END_Slide4vr ==== */



        /* ==== IE切り替え確認 ==== */
        private void AgentButton_Click(object sender, EventArgs e)
        {
            //webBrowser1.DocumentText = "<script>document.write(navigator.userAgent);</script>";
            webView1.Navigate("<script>document.write(navigator.userAgent);</script>");
        }/* ==== END_IE切り替え確認 ==== */


        /* ==== web画面のReset ==== */
        private void ResetButton_Click(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;

            if (String.IsNullOrEmpty(HomePage)) return;
            if (HomePage.Equals("about:blank")) return;
            if (!HomePage.StartsWith("http://") &&
                !HomePage.StartsWith("https://"))
            {
                HomePage = "http://" + HomePage;
            }
            try
            {
                webView1.Navigate(new Uri(HomePage));
            }
            catch (System.UriFormatException)
            {
                return;
            }

        }/* ==== END_web画面のReset ==== */


        /* ==== アバター検索ページ ==== */
        private void VRMLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //リンク先に移動したことにする
            VRMLinkLabel.LinkVisited = true;
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://slide4vr.nklab.dev/login");
        }/* ==== END_アバター検索ページ ==== */


        /* ==== Twitterページ ==== */
        private void TwitterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //リンク先に移動したことにする
            TwitterLinkLabel.LinkVisited = true;
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://twitter.com");

        }/* ==== END_Twitterページ ==== */

        /* ==== TheSeedOnlineページ ==== */
        private void TSOLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //リンク先に移動したことにする
            TSOLinkLabel.LinkVisited = true;
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://seed.online");

        }/* ==== END_GoogleDriveページ ==== */

        /* ==== Discordページ ==== */
        private void DiscordLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //リンク先に移動したことにする
            DiscordLinkLabel.LinkVisited = true;
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://discordapp.com/channels/@me");

        }/* ==== END_DiscordDriveページ ==== */


        /*===========================================================================*/
        /*      ComboBoxイベント                                                     */
        /*===========================================================================*/
        /* ==== 視聴者用うら画像データーリンク ==== */
        private void LinkButton_Click(object sender, EventArgs e)
        {
            ComboBoxLink(BackDataLink);

        }/* ==== END_視聴者用うら画像データーリンク ==== */

        /* ==== 自分用うら画像データーリンク ==== */
        private void MyLinkButton_Click(object sender, EventArgs e)
        {
            ComboBoxLink(BackMyDataLink);

        }/* ==== END_視聴者用うら画像データーリンク ==== */


        /* ====コンボボックスのデーターリンク ==== */
        void ComboBoxLink(ArrayList DataLink)
        {
            /* ==== タブインデックス取得 ==== */
            TabIndex = TabControl.SelectedIndex;
            /* ==== タブインデックス補正 ==== */
            TabIndex = DataGrigCorrection(TabIndex);

            TabIndex += 1;

            var tbl = DataTable[TabIndex];


            DataLink.Clear();

            for (int i = 0; i < tbl.Rows.Count; ++i)
            {
                var item = tbl.Rows[i];
                DataLink.Add(item.ItemArray[1].ToString());

            }

            ComboBoxIndex[TabIndex].Items.Clear();
            ComboBoxIndex[TabIndex].Items.Add("未設定");
            ComboBoxIndex[TabIndex].Items.AddRange(DataLink.ToArray());

        }/* ====END_コンボボックスのデーターリンク ==== */


        /*===========================================================================*/
        /*      右クリックイベント                                                   */
        /*===========================================================================*/
        /* ==== web確認 ==== */
        private void GoWeb_ToolStripMenuItem_Click(object sender, EventArgs e)
        {/* ==== webをセレクト ==== */

            GotoWeb(true);

        }/* ==== END_web確認 ==== */


        /* ==== ブラウザで確認 ==== */
        private void Web_ToolStripMenuItem_Click(object sender, EventArgs e)
        {/* ==== ブラウザ確認 ==== */

            GotoWeb(false);

        }/* ==== END_ブラウザで確認 ==== */


        /* ==== Web確認セレクター ==== */
        public void GotoWeb(bool SelectWeb)
        {
            /* ==== 空セル確認 ==== */
            if (clickedCell == null)
            {
                MessageBox.Show("空のオブジェクト");
            }
            else
            {
                object _object = clickedCell.Value;
                if (_object == null)
                {
                    MessageBox.Show("空のオブジェクト");
                }
                else
                {/* ==== webサイトへ ==== */
                    string objtext = _object.ToString();

                    //webBrowser1.ScriptErrorsSuppressed = true;

                    /* ==== URL切り替え ==== */
                    switch (TabIndex)
                    {

                        //ニコニコマイリスト

                        case DataSelectionClass.Dvd://初期動画DVD
                        case DataSelectionClass.Viewer://視聴者用固定画像
                        case DataSelectionClass.MyData://自分用固定画像
                        case DataSelectionClass.Revers://視聴者用おもて両画像
                        case DataSelectionClass.BackRevers://視聴者用うら両画像
                        case DataSelectionClass.MyRevers://自分用おもて両画像
                        case DataSelectionClass.BackMyRevers://自分用うら両画像
                        case DataSelectionClass.Back://背景画像
                        case DataSelectionClass.Board://ホワイトボード画像
                        case DataSelectionClass.Campe://カンペ画像
                            {

                                address = objtext;//userId取得
                                break;
                            }

                    }/* ==== END_URL切り替え ==== */

                    if (String.IsNullOrEmpty(address)) return;
                    if (address.Equals("about:blank")) return;
                    if (!address.StartsWith("http://") &&
                        !address.StartsWith("https://"))
                    {
                        address = "http://" + address;
                    }
                    try
                    {
                        if (SelectWeb == true)
                        {/* ==== web確認 ==== */
                            webView1.Navigate(new Uri(address));
                        }
                        else
                        {/* ==== ブラウザ確認 ==== */

                            //ブラウザで開く
                            System.Diagnostics.Process.Start(address);

                        }

                    }
                    catch (System.UriFormatException)
                    {
                        return;
                    }
                }/* ==== END_webサイトへ ==== */

            }/* ==== END_空セル確認 ==== */

        }/* ==== END_Web確認セレクター ==== */


        /* ==== セル消去 ==== */
        private void DeleteExcel_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            /* ==== 全行数の取得 ==== */
            int DataGridCount = TabData[TabIndex].Rows.Count;
            /* ==== セレクト行を取得 ==== */
            int SelectGrid = clickedCell.RowIndex + 1;

            /* ==== セル消去 ==== */
            if (DataGridCount >= 2)
            {
                /* ==== 追加セルか確認 ==== */
                if (DataGridCount <= SelectGrid) { }
                else
                {/* ==== セル消去 ==== */

                    DataTable[TabIndex].Rows.RemoveAt(TabData[TabIndex].CurrentCell.RowIndex);

                }/* ==== END_セル消去 ==== */

            }/* ==== セル消去 ==== */

        }/* ==== END_セル消去 ==== */

        /* ==== 全セル消去 ==== */
        private void AllExcel_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClearDataGrid(TabIndex);

        }/* ==== 全セル消去 ==== */



        /*********************************************************************
        *   バルーンメッセージ
        *********************************************************************/
        private void setComponents()
        {/*=== タスクトレイに常駐設定 ===*/
            NotifyIcon icon = new NotifyIcon();
            icon.Visible = true;
        }/*=== END_ タスクトレイに常駐設定 ===*/

        public void InfoBalloonMesage(string Item, string Content, int DisplayTime)
        {/*=== インフォメーション_バルーンメッセージ ===*/

            VCJE_NotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            VCJE_NotifyIcon.BalloonTipTitle = Item;
            VCJE_NotifyIcon.BalloonTipText = Content;
            VCJE_NotifyIcon.ShowBalloonTip(DisplayTime);

        }/*=== END_インフォメーション_バルーンメッセージ ===*/


        public void ErrorBalloonMesage(string Item, string Content, int DisplayTime)
        {/*=== Error_バルーンメッセージ ===*/

            VCJE_NotifyIcon.BalloonTipIcon = ToolTipIcon.Error;
            VCJE_NotifyIcon.BalloonTipTitle = Item;
            VCJE_NotifyIcon.BalloonTipText = Content;
            VCJE_NotifyIcon.ShowBalloonTip(DisplayTime);

        }/*=== END_Error_バルーンメッセージ ===*/




        /*********************************************************************
        *   デバッグモード
        *********************************************************************/

        /* ==== デバッグモード ==== */
#if DEBUG
        //LinkColumn
        private void MyReversDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //MyReversDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

            var MyReversDataGridView = sender as DataGridView;
            if (MyReversDataGridView == null || MyReversDataGridView.CurrentCell == null)
                return;
            var isComboBox = MyReversDataGridView.CurrentCell is DataGridViewComboBoxCell;
            if ((isComboBox || MyReversDataGridView.CurrentCell is DataGridViewCheckBoxCell)
            && MyReversDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
            && isComboBox && MyReversDataGridView.EndEdit())
                MyReversDataGridView.BindingContext[MyReversDataGridView.DataSource].EndCurrentEdit();

        }



        //CellErrorTextNeededイベントハンドラ
        private void DataGridView_CellErrorTextNeeded(object sender,
            DataGridViewCellErrorTextNeededEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //セルの値が負の整数であれば、エラーアイコンを表示する
            object cellVal = dgv[e.ColumnIndex, e.RowIndex].Value;
            if (cellVal is int && ((int)cellVal) < 0)
            {
                e.ErrorText = "再リンクし正選択をしてください。";
            }
        }



#endif
        // ソースコード冒頭に「using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;」が必要

        // ナビゲーション開始時に発生するイベント
        private void WebView1_NavigationStarting(object sender,
                       WebViewControlNavigationStartingEventArgs e)
        {
            // 例えば、e.Uriが、これから表示するURI
            // また、e.Cancelプロパティにfalseを設定することで、ナビゲーションをキャンセル可能

            //MessageBox.Show("[NewWindow Event]" + e.Uri);

        }

        // ナビゲーション完了時に発生するイベント
        private void WebView1_NavigationCompleted(object sender,
                       WebViewControlNavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                //webView1.Navigate(e.Uri);
                //MessageBox.Show("表示に成功した" + e.Uri);
                // 表示に成功した
                // 例えば、e.Uriが、表示したWebページのURI
                // WebView1.DocumentTitleが、Webページのタイトルc
            }
            else
            {
                //MessageBox.Show("表示に失敗した");
                // 表示に失敗した
                // e.WebErrorStatusでエラーが分かる
            }
        }

        /* ==== 新しいウィンドウ ==== */
        private void webView1_NewWindowRequested(object sender, WebViewControlNewWindowRequestedEventArgs e)
        {
            //
            ///* ==== 設定フォーム生成 ==== */
            //using (NewWebView NewWebView = new NewWebView())
            //{
            //    /* ==== マニュアルモードで生成 ==== */
            //    NewWebView VCEConfDlg = new NewWebView
            //    {
            //
            //        StartPosition = FormStartPosition.Manual
            //
            //    };/* ==== END_マニュアルモードで生成 ==== */
            //
            //    NewWebView.Left = Left + (Width - NewWebView.Width) / 2;
            //    NewWebView.Top = Top + (Height - VCEConfDlg.Height) / 2;
            //    //NewWebView.Owner = this; // 常に親ウィンドウの手前に表示
            //    NewWebView.ShowDialog(this);// モードレス・ダイアログとして表示 
            //
            //}/* ==== END_設定フォーム生成 ==== */



            //System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            //messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.Cancel);
            //messageBoxCS.AppendLine();
            //webView1.Navigate(e.Uri);
            //UrlTextBox.Text = e.Uri.ToString();
            //webView1.Navigate("https://slide2vr.firebaseapp.com/__/auth/handler?apiKey=AIzaSyD2hSZPJHerbT2lrrbCrGcEQlCpvUnDiaQ&appName=%5BDEFAULT%5D&authType=signInViaPopup&providerId=twitter.com&eventId=956313178&v=8.0.1");

        }/* ==== END_新しいウィンドウ ==== */


        /* ==== END_デバッグモード ==== */

    }/* ==== END_VirtualCastJsonEditor ==== */

}/* ==== END_VirtualCastJsonEditorフォーム ==== */