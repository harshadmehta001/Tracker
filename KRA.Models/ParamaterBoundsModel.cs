﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
    public class ParamaterBoundsModel
    {
        [Key]
        public int DefinitionID { get; set; }
        public int AccountParamID { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public int Result { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
