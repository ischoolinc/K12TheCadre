using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace K12.Behavior.TheCadre
{
    //幹部與敘獎 - 物件
    //            List<ClassCadreNameObj> ClassCadreNameList = 
    //            accessHelper.Select<ClassCadreNameObj>(string.Format("NameType = '{0}'", "班級幹部"));
    [TableName("Behavior.TheCadre.CadreType")]
    class ClassCadreNameObj : ActiveRecord
    {
        /// <summary>
        /// 幹部名稱類型(班級幹部/學校幹部/社團幹部/空字串為所有類型)
        /// </summary>
        [Field(Field = "NameType", Indexed = true)]
        public string NameType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Field(Field = "Index", Indexed = true)]
        public int Index { get; set; }

        /// <summary>
        /// 幹部名稱
        /// </summary>
        [Field(Field = "CadreName", Indexed = false)]
        public string CadreName { get; set; }

        /// <summary>
        /// 擔任人數
        /// </summary>
        [Field(Field = "Number", Indexed = false)]
        public int Number { get; set; }

        /// <summary>
        /// 敘獎 - 大功
        /// </summary>
        [Field(Field = "MeritA", Indexed = false)]
        public int MeritA { get; set; }

        /// <summary>
        /// 敘獎 - 小功
        /// </summary>
        [Field(Field = "MeritB", Indexed = false)]
        public int MeritB { get; set; }

        /// <summary>
        /// 敘獎 - 嘉獎
        /// </summary>
        [Field(Field = "MeritC", Indexed = false)]
        public int MeritC { get; set; }

        /// <summary>
        /// 敘獎 - 事由
        /// </summary>
        [Field(Field = "Reason", Indexed = false)]
        public string Reason { get; set; }
    }
}
