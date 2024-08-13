using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace K12.Behavior.TheCadre.Plus
{
    public class SendMessage
    {
        List<string> _tagIDList { get; set; }

        string _title { get; set; }

        string _message { get; set; }

        string _msgTitle { get; set; }
        

        /// <summary>
        /// 傳送推播通知
        /// </summary>
        public SendMessage(List<string> tagIDList, string title, string message)
        {
            _tagIDList = tagIDList;
            _message = message;
            _title = title;
        }

        /// <summary>
        /// 開始傳送
        /// </summary>
        public void Run()
        {
            BatchPushNotice();
        }

        private void BatchPushNotice()
        {
            //必須要使用greening帳號登入才能用
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            XmlElement root = doc.CreateElement("Request");

            //標題
            var eleTitle = doc.CreateElement("Title");
            eleTitle.InnerText = _title;
            root.AppendChild(eleTitle);

            //發送人員
            var eleDisplaySender = doc.CreateElement("DisplaySender");
            eleDisplaySender.InnerText = "系統通知";
            root.AppendChild(eleDisplaySender);

            //背景
            var eleMessage = doc.CreateElement("Message");
            eleMessage.InnerText = _message;
            root.AppendChild(eleMessage);

            foreach (string each in _tagIDList)
            {
                //發送對象
                var eleTarget = doc.CreateElement("TargetTeacher");
                eleTarget.InnerText = each;
                root.AppendChild(eleTarget);
            }

            //送出
            FISCA.DSAClient.XmlHelper xmlHelper = new FISCA.DSAClient.XmlHelper(root);
            var conn = new FISCA.DSAClient.Connection();
            conn.Connect(FISCA.Authentication.DSAServices.AccessPoint, "1campus.notice.admin.v17", FISCA.Authentication.DSAServices.PassportToken);
            var resp = conn.SendRequest("PostNotice", xmlHelper);


            //處理Log
            StringBuilder sb_log = new StringBuilder();
            sb_log.AppendLine(string.Format("發送對象「{0}」", "班導師"));

            sb_log.AppendLine(string.Format("發送標題「{0}」", _title));
            sb_log.AppendLine(string.Format("發送單位「{0}」", "系統通知"));
            sb_log.AppendLine(string.Format("發送內容「{0}」", _message));
            sb_log.AppendLine(string.Format("對象清單「{0}」", string.Join(",", _tagIDList)));

            FISCA.LogAgent.ApplicationLog.Log("智慧簡訊", "發送", sb_log.ToString());

        }
    }
}
