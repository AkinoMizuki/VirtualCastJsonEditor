using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualCastJsonEditor
{
    static class Program
    {

        // アプリケーション固定名
        private static string strAppConstName = "VirtualCastJsonEditor";

        // 多重起動を禁止するミューテックス
        private static Mutex mutexObject;
        

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // ミューテックスを生成する
            mutexObject = new Mutex(false, strAppConstName);

            // ミューテックスを取得する
            if (mutexObject.WaitOne(0, false))
            {
                // アプリケーションを実行
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new VirtualCastJsonEditor());

                // ミューテックスを解放する
                mutexObject.ReleaseMutex();
            }
            else
            {
                //  警告を表示して終了
                MessageBox.Show("すでに起動しています。多重起動はできません。", "多重起動禁止");
            }

            // ミューテックスを破棄する
            mutexObject.Close();
        }
    }
}

