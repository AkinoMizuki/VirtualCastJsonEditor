using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;


/* ==== VirtualCastEditorフォーム ==== */
namespace VirtualCastJsonEditor
{



    class JsonSave
    {/* ==== Json読み書き ==== */

     /*============================================*/
     /*      オブジェクト定義                      */
     /*============================================*/
     /* ==== メインフォーム ==== */
     //VirtualCastJsonEditor VirtualCastJson = new VirtualCastJsonEditor();

        /*============================================*/
        /*      関数                                  */
        /*============================================*/

        public int TabIndex { get; private set; }
        public System.Windows.Forms.DataGridView[] TabData;


        public void SaveJsonConfig()
        {/* ==== Json書き出し ==== */

            //VirtualCastJson.TabData_init();
            /* ==== 全行数の取得 ==== */
            //int DataGridCount = TabData[TabIndex].Rows.Count;







        }/* ==== END_Json書き出し ==== */

        public void LoadJsonConfig()
        {/* ==== Json書き出し ==== */

        }/* ==== END_Json書き出し ==== */

    }/* ==== END_Json読み書き ==== */

}/* ==== END_VirtualCastEditorフォーム ==== */
