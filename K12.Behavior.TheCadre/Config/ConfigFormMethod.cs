using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.DSAUtil;
using Framework.Feature;
using System.Xml;

namespace K12.Behavior.TheCadre
{
    class ConfigFormMethod
    {
        private Dictionary<string, string> CodeDic = new Dictionary<string, string>();

        /// <summary>
        /// 取得獎勵事由代碼表
        /// </summary>
        public  Dictionary<string, string> GetDisciplineReason()
        {
            CodeDic.Clear();
            DSResponse dsrsp = Config.GetDisciplineReasonList();
            foreach (XmlElement var in dsrsp.GetContent().GetElements("Reason"))
            {
                string type = var.GetAttribute("Type");
                string code = var.GetAttribute("Code");
                string desc = var.GetAttribute("Description");

                if (type == "獎勵")
                {
                    if (!CodeDic.ContainsKey(code))
                    {
                        CodeDic.Add(code, desc);
                    }
                }
            }
            return CodeDic;
        }
    }
}
