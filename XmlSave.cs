using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

/* ==== VirtualCastEditorフォーム ==== */
namespace VirtualCastJsonEditor
{

    /*============================================*/
    /*      オブジェクト定義                      */
    /*============================================*/


    /*============================================*/
    /*      関数                                  */
    /*============================================*/


    class XmlSave
    {/* ==== XML読み書き ==== */
        private readonly object AvatarDataGridView;

        public class SampleClass
        {
            /*テキストボックス保存用*/
            public string[] AvatarData;          //アバターモデル
            public string[] BackModelData;       //背景モデル
            public string[] DvdData;             //初期動画DVD
            public string[] ViewerData;          //視聴者用固定画像
            public string[] MyData;              //自分用固定画像 
            public string[] BackData;            //背景画像
            public string[] BoardData;           //ホワイトボード画像
            public string[] CampeData;           //カンペ画像

        }

        public void SaveXmlConfig()
        {/* ==== XML書き出し ==== */

         /* 保存先のファイル名 */
            string fileName = @"./Config.xml";

            /* 保存するクラス(SampleClass)のインスタンスを作成 */
            SampleClass obj = new SampleClass
            {

                /* テキストボックス保存用 */
                //ProjectSaveFile = ProjectFileBox.Text,
                //SequenceSaveFile1 = SequenceFileBox1.Text,
                //SequenceSaveFile2 = SequenceFileBox2.Text,
                //SequenceSaveFile3 = SequenceFileBox3.Text,
                //SequenceSaveFile4 = SequenceFileBox4.Text

            };


            /* XmlSerializerオブジェクトを作成 */
            /* オブジェクトの型を指定する */
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
            /* 書き込むファイルを開く（UTF-8 BOM無し）*/
            StreamWriter sw = new StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            /* シリアル化し、XMLファイルに保存する */
            //serializer.Serialize(sw, obj);
            serializer.Serialize(sw, this.AvatarDataGridView);
            /* ファイルを閉じる */
            sw.Close();

        }/* ==== END_XML書き出し ==== */

        public void LoadXmlConfig()
        {/* ==== XML読み込み ==== */
         /* 保存元のファイル名 */
            string fileName = @"./Config.xml";

            /* XmlSerializerオブジェクトを作成 */
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
            /* 読み込むファイルを開く */
            StreamReader sr = new StreamReader(
                fileName, new System.Text.UTF8Encoding(false));
            /* XMLファイルから読み込み、逆シリアル化する */
            SampleClass obj = (SampleClass)serializer.Deserialize(sr);

            /* テキストボックス復元 */
            //ProjectFileBox.Text = obj.ProjectSaveFile;
            //SequenceFileBox1.Text = obj.SequenceSaveFile1;
            //SequenceFileBox2.Text = obj.SequenceSaveFile2;
            //SequenceFileBox3.Text = obj.SequenceSaveFile3;
            //SequenceFileBox4.Text = obj.SequenceSaveFile4;


            /* ファイルを閉じる */
            sr.Close();



        }/* ==== END_XML読み込み ==== */


    }/* ==== XML読み書き ==== */

}/* ==== END_VirtualCastEditorフォーム ==== */
